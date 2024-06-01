export function requestPermission() {
    return new Promise((resolve, reject) => {
        Notification
            .requestPermission()
            .then(r => resolve(r));
    });
}

export function isSupported() {
    return "Notification" in window;
}

export function create(title, options) {
    return new Notification(title, options);
}