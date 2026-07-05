window.blazorCulture = {
    get: function() {
        const name = '.AspNetCore.Culture=';
        const decodedCookie = decodeURIComponent(document.cookie);
        const ca = decodedCookie.split(';');
        for(let i = 0; i < ca.length; i++) {
            let c = ca[i];
            while (c.charAt(0) === ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) === 0) {
                const value = c.substring(name.length, c.length);
                // Extract culture from cookie value (format: c=en|uic=en)
                const match = value.match(/c=([^|]+)/);
                return match ? match[1] : null;
            }
        }
        return null;
    },
    set: function(culture) {
        const cookieValue = `c=${culture}|uic=${culture}`;
        document.cookie = `.AspNetCore.Culture=${encodeURIComponent(cookieValue)};path=/;max-age=31536000`;
    }
};