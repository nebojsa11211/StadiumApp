// =====================================================================================================
//  Stadium ngrok proxy — one HTTPS entry point for all six Stadium apps.
//
//  The free ngrok plan allows ONE agent / ONE static domain per account, so the apps cannot each get
//  their own tunnel (the second one dies with ERR_NGROK_334). Instead ngrok tunnels this proxy, and
//  the proxy routes by the first path segment:
//
//      /backend   API                 https://localhost:7010   (Swagger at /backend/swagger)
//      /customer  Customer            https://localhost:7020
//      /admin     Admin               https://localhost:7030
//      /bar       Bar                 https://localhost:7040
//      /sim       TicketingSimulator  https://localhost:7050
//      /runner    Runner (WASM)       https://localhost:7060
//
//  The prefix is FORWARDED, not stripped. Each app is told its prefix (Proxy__Prefix in its "ngrok"
//  launch profile) and removes it itself with UsePathBase, so every link, <base href>, redirect and
//  Blazor circuit URL it generates keeps the prefix. Strip it here and the apps would believe they live
//  at the root of the public hostname, which belongs to nobody.
//
//  HTTPS only (project rule): the proxy listens on https://localhost:7000 with an exported copy of the
//  trusted ASP.NET Core dev certificate, and talks to the apps over HTTPS too. `secure: false` skips
//  verifying the apps' certificates — the hop is loopback, so nothing is lost.
//
//  See Ngrok.md in the repository root for the whole picture.
// =====================================================================================================

'use strict';

const fs = require('node:fs');
const path = require('node:path');
const os = require('node:os');
const https = require('node:https');
const http = require('node:http');
const { execFileSync } = require('node:child_process');
const express = require('express');
const { createProxyMiddleware } = require('http-proxy-middleware');

const PORT = Number(process.env.PORT || 7000);
const NGROK_API = 'http://127.0.0.1:4040/api/tunnels'; // the ngrok agent's own loopback control API

// Prefix -> app. Keep in sync with Proxy__Prefix + launchUrl in each app's "ngrok" launch profile.
const apps = {
    '/backend':  { name: 'API',                 port: 7010, landing: '/backend/swagger' },
    '/customer': { name: 'Customer',            port: 7020, landing: '/customer/' },
    '/admin':    { name: 'Admin',               port: 7030, landing: '/admin/' },
    '/bar':      { name: 'Bar',                 port: 7040, landing: '/bar/' },
    '/sim':      { name: 'Ticketing Simulator', port: 7050, landing: '/sim/' },
    '/runner':   { name: 'Runner',              port: 7060, landing: '/runner/' },
};
const prefixes = Object.keys(apps);

/** Returns the prefix owning `pathname` ("/admin" for "/admin" and "/admin/x"), or null. */
function prefixOf(pathname) {
    if (!pathname) return null;
    return prefixes.find(p => pathname === p || pathname.startsWith(p + '/')) || null;
}

function firstHeaderValue(value) {
    if (!value) return undefined;
    return String(Array.isArray(value) ? value[0] : value).split(',')[0].trim() || undefined;
}

// --- forwarded headers ---------------------------------------------------------------------------
// changeOrigin rewrites Host to localhost:70x0, so without these the app would compose absolute URLs
// (login redirects, QR codes, cookie challenges) out of localhost. ngrok already sends the public
// host/proto; when the proxy is opened directly we fall back to what this socket saw. Single values
// only, so ASP.NET's ForwardedHeadersMiddleware (ForwardLimit = 1) takes them as-is.
function setForwardedHeaders(proxyReq, req) {
    const proto = firstHeaderValue(req.headers['x-forwarded-proto']) || 'https';
    const host = firstHeaderValue(req.headers['x-forwarded-host']) || req.headers.host;
    const client = firstHeaderValue(req.headers['x-forwarded-for']) || req.socket.remoteAddress;
    proxyReq.setHeader('X-Forwarded-Proto', proto);
    if (host) proxyReq.setHeader('X-Forwarded-Host', host);
    if (client) proxyReq.setHeader('X-Forwarded-For', client);
}

function badGatewayPage(app, err) {
    return `<!doctype html><meta charset="utf-8"><title>502 — ${app.name} unreachable</title>
<body style="font-family:system-ui;max-width:40rem;margin:3rem auto;padding:0 1rem">
<h1>502 — the proxy cannot reach ${app.name}</h1>
<p>Nothing answered on <code>https://localhost:${app.port}</code>. The app is not running, is still
starting, or was started on its <code>https</code> profile instead of <code>ngrok</code>.</p>
<p><small>${escapeHtml(err && err.message || '')}</small></p></body>`;
}

function escapeHtml(s) {
    return String(s).replace(/[&<>"']/g, c => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c]));
}

// One proxy middleware per app, each owning its own prefix via pathFilter (NOT app.use('/admin', ...),
// which would make Express strip the mount path before the proxy sees it). No pathRewrite.
// ws stays false: WebSocket upgrades are routed by hand below, so hpm does not also subscribe to the
// server's upgrade event and handle the same socket twice.
function makeProxy(prefix, app, pathFilter) {
    return createProxyMiddleware({
        target: `https://localhost:${app.port}`,
        changeOrigin: true,
        secure: false,
        pathFilter,
        on: {
            proxyReq: (proxyReq, req) => setForwardedHeaders(proxyReq, req),
            proxyReqWs: (proxyReq, req) => setForwardedHeaders(proxyReq, req),
            error: (err, req, resOrSocket) => {
                console.error(`[${new Date().toLocaleTimeString()}] Error (${prefix} -> :${app.port}) ${req.url}: ${err.code || err.message}`);
                if (resOrSocket && typeof resOrSocket.writeHead === 'function') {
                    if (!resOrSocket.headersSent) {
                        resOrSocket.writeHead(502, { 'Content-Type': 'text/html; charset=utf-8' });
                    }
                    resOrSocket.end(badGatewayPage(app, err));
                } else if (resOrSocket && typeof resOrSocket.destroy === 'function') {
                    resOrSocket.destroy();
                }
            },
        },
    });
}

const proxies = {};
// Same apps WITHOUT a path filter: used to forward Visual Studio dev tooling unprefixed (below).
const rawProxies = {};
for (const [prefix, app] of Object.entries(apps)) {
    proxies[prefix] = makeProxy(prefix, app, (pathname) => prefixOf(pathname) === prefix);
    rawProxies[prefix] = makeProxy(prefix, app, undefined);
}

// Visual Studio / dotnet-watch tooling (browser refresh, Blazor hot reload, Browser Link) is served by
// middleware VS injects through a hosting startup, AHEAD of the app's UsePathBase / the WASM DevServer's
// pathbase. It answers ONLY the unprefixed path (/_framework/aspnetcore-browser-refresh.js), and the
// injected scripts request exactly that from the page's origin. So these are forwarded to the app the
// page belongs to (Referer) WITHOUT adding the prefix — redirecting them to /runner/_framework/... 404s,
// and a failed blazor-hotreload.js import throws inside the Runner's WASM entry point.
const DEV_TOOLING = /^\/(_framework\/(aspnetcore-browser-refresh\.js|blazor-hotreload(\.js)?)|_vs\/)/;

// --- certificate ---------------------------------------------------------------------------------
// Uses the machine's trusted ASP.NET Core dev certificate (the same one the apps serve on localhost),
// exported once to %LOCALAPPDATA%\StadiumApp. certificates\aspnetcore.pfx is NOT used: it is the
// Docker certificate and expires on its own schedule.
function loadCertificate() {
    const pfxPath = process.env.PROXY_PFX
        || path.join(process.env.LOCALAPPDATA || os.homedir(), 'StadiumApp', 'ngrok-proxy-cert.pfx');
    const passphrase = process.env.PROXY_PFX_PASSWORD || 'StadiumProxyDev!';

    // Node cannot read a PFX's expiry without parsing PKCS#12, so re-export whenever the copy is older
    // than 30 days. A dev certificate lives a year, and `dotnet dev-certs` always exports the current one.
    const MAX_AGE_MS = 30 * 24 * 3600 * 1000;
    const isFresh = fs.existsSync(pfxPath) && (Date.now() - fs.statSync(pfxPath).mtimeMs) < MAX_AGE_MS;

    if (!isFresh && !process.env.PROXY_PFX) {
        console.log(`Exporting the ASP.NET Core dev certificate to ${pfxPath} ...`);
        fs.mkdirSync(path.dirname(pfxPath), { recursive: true });
        execFileSync('dotnet', ['dev-certs', 'https', '--export-path', pfxPath, '--password', passphrase], { stdio: 'inherit' });
    }
    return { pfx: fs.readFileSync(pfxPath), passphrase };
}

// --- ngrok public URL ----------------------------------------------------------------------------
function getPublicUrl() {
    return new Promise((resolve) => {
        const req = http.get(NGROK_API, { timeout: 2000 }, (res) => {
            let body = '';
            res.on('data', c => (body += c));
            res.on('end', () => {
                try {
                    const tunnels = JSON.parse(body).tunnels || [];
                    const ours = tunnels.filter(t => String(t.config && t.config.addr).includes(String(PORT)));
                    const pick = (list) => list.find(t => String(t.public_url).startsWith('https://')) || list[0];
                    const tunnel = pick(ours) || pick(tunnels);
                    resolve(tunnel ? String(tunnel.public_url).replace(/\/$/, '') : null);
                } catch {
                    resolve(null);
                }
            });
        });
        req.on('timeout', () => { req.destroy(); resolve(null); });
        req.on('error', () => resolve(null));
    });
}

// --- unprefixed request recovery -----------------------------------------------------------------
// Probes an app for a path. Returns the status code, or null when the app is not reachable.
function probe(app, pathAndQuery, method) {
    return new Promise((resolve) => {
        const req = https.request({
            host: 'localhost', port: app.port, path: pathAndQuery, method,
            rejectUnauthorized: false, timeout: 1500,
            headers: { 'User-Agent': 'stadium-ngrok-proxy-probe' },
        }, (res) => { res.resume(); resolve(res.statusCode); });
        req.on('timeout', () => { req.destroy(); resolve(null); });
        req.on('error', () => resolve(null));
        req.end();
    });
}

// Apps that answer EVERYTHING with non-404 (Blazor fallback pages, API 401s) cannot be asked
// "do you have this path?". Detected once per app with a path that cannot exist.
const answersEverything = {};
const probeCache = new Map(); // path -> prefix | null
const NONSENSE = '/__stadium-proxy-probe-9f3a2b';

async function guessPrefixByProbing(pathAndQuery, method) {
    if (probeCache.has(pathAndQuery)) return probeCache.get(pathAndQuery);
    const matches = [];
    for (const [prefix, app] of Object.entries(apps)) {
        if (answersEverything[prefix] === undefined) {
            const s = await probe(app, prefix + NONSENSE, 'GET');
            if (s === null) continue; // not running; ask again next time
            answersEverything[prefix] = s !== 404;
        }
        if (answersEverything[prefix]) continue;
        const status = await probe(app, prefix + pathAndQuery, method);
        if (status !== null && status !== 404) matches.push(prefix);
    }
    const result = matches.length === 1 ? matches[0] : null;
    if (probeCache.size > 500) probeCache.clear();
    probeCache.set(pathAndQuery, result);
    return result;
}

function prefixFromReferer(req) {
    try {
        const ref = req.headers.referer;
        return ref ? prefixOf(new URL(ref).pathname) : null;
    } catch {
        return null;
    }
}

// --- express app ---------------------------------------------------------------------------------
const app = express();
app.disable('x-powered-by');

app.get('/', (req, res, next) => {
    // A root-absolute "/" from inside an app (e.g. NavigateTo("/") under /admin/) means that app's home.
    if (prefixFromReferer(req)) return next();
    const rows = Object.entries(apps).map(([p, a]) =>
        `<tr><td><a href="${a.landing}">${p}</a></td><td>${a.name}</td><td>https://localhost:${a.port}</td></tr>`).join('');
    res.type('html').send(`<!doctype html><meta charset="utf-8"><title>Stadium ngrok proxy</title>
<body style="font-family:system-ui;max-width:44rem;margin:3rem auto;padding:0 1rem">
<h1>Stadium ngrok proxy</h1><table cellpadding="6">${rows}</table>
<p>ngrok traffic inspector: <code>http://127.0.0.1:4040</code> (ngrok's own loopback UI)</p></body>`);
});

// Launch profiles open https://localhost:7000/open/<name>. Redirect to the public address when the
// tunnel is up, otherwise to the same prefixed path on this proxy — so F5 never lands on an ngrok
// "endpoint offline" page just because the tunnel is down.
app.get('/open/:name', async (req, res) => {
    const target = apps['/' + req.params.name];
    if (!target) return res.status(404).type('text').send(`Unknown app "${req.params.name}".`);
    const publicUrl = await getPublicUrl();
    const base = publicUrl || `https://${req.headers.host}`;
    res.set('Cache-Control', 'no-store').redirect(302, base + target.landing);
});

// Dev tooling that arrives prefixed anyway (/runner/_framework/blazor-hotreload.js): strip and forward.
app.use((req, res, next) => {
    const prefix = prefixOf(req.path);
    if (!prefix) return next();
    const rest = req.url.slice(prefix.length);
    if (!DEV_TOOLING.test(rest)) return next();
    req.url = rest;
    return rawProxies[prefix](req, res, next);
});

// "/admin" -> "/admin/" so relative URLs under <base href="/admin/"> resolve inside the app.
app.use((req, res, next) => {
    const pathname = req.path;
    if (apps[pathname] && (req.method === 'GET' || req.method === 'HEAD')) {
        const qs = req.originalUrl.slice(pathname.length);
        return res.redirect(308, pathname + '/' + qs);
    }
    next();
});

for (const prefix of prefixes) app.use(proxies[prefix]);

// Anything left has no prefix: a hand-typed address, a VS-injected script, a stray absolute link.
app.use(async (req, res) => {
    let prefix = prefixFromReferer(req);
    if (prefix && DEV_TOOLING.test(req.path)) return rawProxies[prefix](req, res, () => res.status(404).end());
    if (!prefix && (req.method === 'GET' || req.method === 'HEAD')) {
        prefix = await guessPrefixByProbing(req.originalUrl, req.method);
    }
    if (prefix) return res.redirect(307, prefix + req.originalUrl);

    const accept = String(req.headers.accept || '');
    if (accept.includes('text/html')) {
        const links = prefixes.map(p => `<li><a href="${p}${escapeHtml(req.originalUrl)}">${p}${escapeHtml(req.originalUrl)}</a> — ${apps[p].name}</li>`).join('');
        return res.status(404).type('html').send(`<!doctype html><meta charset="utf-8"><title>Which app?</title>
<body style="font-family:system-ui;max-width:44rem;margin:3rem auto;padding:0 1rem">
<h1>Which app did you mean?</h1><p><code>${escapeHtml(req.originalUrl)}</code> has no app prefix.</p><ul>${links}</ul></body>`);
    }
    res.status(404).type('text').send('No app prefix (/backend, /customer, /admin, /bar, /sim, /runner).');
});

// --- server --------------------------------------------------------------------------------------
const server = https.createServer(loadCertificate(), app);

// Blazor Server (Admin, Customer, Bar, Simulator) runs every click over a WebSocket at /_blazor.
// Upgrades never pass through Express middleware, so route them here by prefix (falling back to the
// Referer). Without this the page renders and then nothing responds to any click.
server.on('upgrade', (req, socket, head) => {
    let pathname = '';
    try { pathname = new URL(req.url, 'https://localhost').pathname; } catch { /* ignore */ }
    let prefix = prefixOf(pathname);
    if (!prefix) {
        prefix = prefixFromReferer(req);
        if (prefix) req.url = prefix + req.url;
    }
    if (!prefix) {
        console.warn(`WebSocket ${req.url}: no app prefix, closing`);
        return socket.destroy();
    }
    console.log(`[${new Date().toLocaleTimeString()}] WebSocket ${req.url} -> :${apps[prefix].port}`);
    proxies[prefix].upgrade(req, socket, head);
});

server.on('error', (err) => {
    console.error(err.code === 'EADDRINUSE' ? `Port ${PORT} is already in use — is another proxy running?` : err);
    process.exit(1);
});

// Loopback only: the proxy forwards X-Forwarded-* that the apps trust under the ngrok profile, so it
// must not be reachable from the LAN. ngrok connects to it locally.
server.listen(PORT, '127.0.0.1', () => {
    console.log(`Stadium ngrok proxy on https://localhost:${PORT}`);
    for (const [p, a] of Object.entries(apps)) console.log(`  ${p.padEnd(10)} -> https://localhost:${a.port}  (${a.name})`);
});
