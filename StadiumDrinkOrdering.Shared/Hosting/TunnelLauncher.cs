using System.Diagnostics;
using System.Net.Sockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace StadiumDrinkOrdering.Shared.Hosting;

/// <summary>
/// Makes F5 on an <c>ngrok</c> launch profile the whole procedure: when an app starts in reverse-proxy
/// mode and the tunnel is not fully up, it runs <c>start-ngrok.bat auto</c> in its own window.
/// See Ngrok.md §6.
/// </summary>
public static class TunnelLauncher
{
    private const int ProxyPort = 7000;
    private const int NgrokAgentPort = 4040;
    private const string ScriptName = "start-ngrok.bat";
    private const string MutexName = @"Local\Stadium-ngrok-tunnel-launcher";

    // Held for the lifetime of the process: the API and the Blazor apps start in the same second, and
    // two concurrent batch runs would kill each other's ngrok agent (one agent per account). An
    // abandoned mutex (previous holder killed by Stop Debugging) counts as acquired.
    private static Mutex? _mutex;

    public static void EnsureRunning(IConfiguration configuration, IHostEnvironment environment)
    {
        try
        {
            // The launch profile NAME is deliberately not read: DOTNET_LAUNCH_PROFILE is not set on every
            // VS path, and a missing value would silently mean "not ngrok". Proxy:Enabled is what counts.
            if (!OperatingSystem.IsWindows()
                || !environment.IsDevelopment()
                || Environment.GetEnvironmentVariable("STADIUM_TUNNEL") == "0"
                || !ReverseProxySetup.IsEnabled(configuration))
            {
                return;
            }

            // A deployed container may set Proxy__Enabled for its load balancer but has no batch file.
            var script = FindScript(environment.ContentRootPath);
            if (script is null)
            {
                return;
            }

            if (!TryAcquireMutex())
            {
                Console.WriteLine("ngrok: another Stadium app is taking care of the tunnel");
                return;
            }

            // Check BOTH halves: ngrok outlives Stop Debugging, so "agent up" alone says nothing about
            // whether the proxy window is still open (every public path would answer 502).
            var agentUp = IsListening(NgrokAgentPort);
            var proxyUp = IsListening(ProxyPort);
            if (agentUp && proxyUp)
            {
                Console.WriteLine("ngrok: tunnel already running");
                return;
            }

            Console.WriteLine($"ngrok: starting tunnel (agent {(agentUp ? "up" : "down")}, proxy {(proxyUp ? "up" : "down")})");

            // Through "start" so the tunnel gets its own window and survives the debugging session —
            // the next F5 reuses it and the public address never changes under anyone.
            var startInfo = new ProcessStartInfo("cmd.exe")
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Path.GetDirectoryName(script)!,
            };
            foreach (var arg in new[] { "/c", "start", "ngrok tunnel", script, "auto" })
            {
                startInfo.ArgumentList.Add(arg);
            }
            Process.Start(startInfo)?.Dispose();
        }
        catch (Exception ex)
        {
            // A convenience must never stop an application from starting.
            Console.WriteLine($"ngrok: could not start the tunnel: {ex.Message}");
        }
    }

    private static bool TryAcquireMutex()
    {
        _mutex ??= new Mutex(initiallyOwned: false, MutexName);
        try
        {
            return _mutex.WaitOne(0);
        }
        catch (AbandonedMutexException)
        {
            return true;
        }
    }

    private static string? FindScript(string startDirectory)
    {
        for (var dir = new DirectoryInfo(startDirectory); dir is not null; dir = dir.Parent)
        {
            var candidate = Path.Combine(dir.FullName, ScriptName);
            if (File.Exists(candidate))
            {
                return candidate;
            }
        }
        return null;
    }

    private static bool IsListening(int port)
    {
        try
        {
            using var client = new TcpClient();
            return client.ConnectAsync("127.0.0.1", port).Wait(TimeSpan.FromMilliseconds(500)) && client.Connected;
        }
        catch
        {
            return false;
        }
    }
}
