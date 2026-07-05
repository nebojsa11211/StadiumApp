// Kitchen-display (Bar prep station) helpers.
window.barKds = {
    // Short two-tone chime for new orders. Uses Web Audio so no asset file is needed.
    // Browsers require a prior user gesture to start audio; the login click satisfies that.
    beep: function () {
        try {
            var Ctx = window.AudioContext || window.webkitAudioContext;
            if (!Ctx) return;
            var ctx = new Ctx();
            var tone = function (freq, start, dur) {
                var o = ctx.createOscillator();
                var g = ctx.createGain();
                o.connect(g); g.connect(ctx.destination);
                o.type = 'sine';
                o.frequency.value = freq;
                g.gain.setValueAtTime(0.0001, ctx.currentTime + start);
                g.gain.exponentialRampToValueAtTime(0.3, ctx.currentTime + start + 0.02);
                g.gain.exponentialRampToValueAtTime(0.0001, ctx.currentTime + start + dur);
                o.start(ctx.currentTime + start);
                o.stop(ctx.currentTime + start + dur + 0.02);
            };
            tone(880, 0, 0.35);
            tone(1175, 0.18, 0.45);
            setTimeout(function () { ctx.close(); }, 900);
        } catch (e) {
            console.warn('barKds.beep failed', e);
        }
    }
};
