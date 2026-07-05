// Durable offline queue for the Runner, backed by IndexedDB (survives reload / tab close / app
// backgrounding — unlike in-memory state). One object store keyed by the action's client id.
const DB_NAME = 'runner-outbox';
const STORE = 'actions';
const VERSION = 1;

function openDb() {
    return new Promise((resolve, reject) => {
        const req = indexedDB.open(DB_NAME, VERSION);
        req.onupgradeneeded = () => {
            const db = req.result;
            if (!db.objectStoreNames.contains(STORE)) {
                db.createObjectStore(STORE, { keyPath: 'id' });
            }
        };
        req.onsuccess = () => resolve(req.result);
        req.onerror = () => reject(req.error);
    });
}

export async function add(record) {
    const db = await openDb();
    return new Promise((resolve, reject) => {
        const tx = db.transaction(STORE, 'readwrite');
        tx.objectStore(STORE).put(record);
        tx.oncomplete = () => resolve();
        tx.onerror = () => reject(tx.error);
    });
}

export async function list() {
    const db = await openDb();
    return new Promise((resolve, reject) => {
        const tx = db.transaction(STORE, 'readonly');
        const rq = tx.objectStore(STORE).getAll();
        rq.onsuccess = () => resolve(rq.result || []);
        rq.onerror = () => reject(rq.error);
    });
}

export async function remove(id) {
    const db = await openDb();
    return new Promise((resolve, reject) => {
        const tx = db.transaction(STORE, 'readwrite');
        tx.objectStore(STORE).delete(id);
        tx.oncomplete = () => resolve();
        tx.onerror = () => reject(tx.error);
    });
}

export function isOnline() {
    return navigator.onLine;
}

// Bridge browser connectivity + resume events to .NET so the outbox can flush at the right moments.
export function registerConnectivity(dotNetRef) {
    window.addEventListener('online', () => dotNetRef.invokeMethodAsync('OnConnectivityChanged', true));
    window.addEventListener('offline', () => dotNetRef.invokeMethodAsync('OnConnectivityChanged', false));
    document.addEventListener('visibilitychange', () => {
        if (!document.hidden) dotNetRef.invokeMethodAsync('OnResumed');
    });
    return navigator.onLine;
}
