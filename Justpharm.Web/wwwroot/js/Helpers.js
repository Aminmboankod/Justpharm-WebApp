//El script de abajo es para hacer scroll en el item seleccionado (es para el chat)
function ListView(args) {
    // move the scroller to correspoding element by using scrollintoview method. 
    document.querySelector('[data-uid="' + args.id + '"]').scrollIntoView();
    alert(args.id);
    console.log(args);
} 

function ClearLocalStorage(keyName) {
    window.localStorage.removeItem(keyName);
}

function AbreVisorWeb() {
    document.getElementById('AbreVWBtn').click();
}

function getNotifyBtnPos() {
    return document.getElementById("btnNotify").getBoundingClientRect();
}

function getEnviosBtnPos() {
    return document.getElementById("btnNotifyEnvios").getBoundingClientRect();
}

function getDOMRect(id) {
    return document.getElementById(id).getBoundingClientRect();
}

function getInnerText(id) {
    let el = document.getElementById(id);
    return el?.innerText;
}

function setFocus(id) {
    let el = document.getElementById(id);
    el?.focus();
}

function hasFocus(id) {
    let el = document.getElementById(id);
    return el?.focused || false;
}

function setFocusTo(selector) {
    document.querySelector(selector)?.focus();
}

function hasFocusTo(selector) {
    return document.querySelector(selector)?.focused || false;
}

function unfocus(selector) {
    document.querySelector(selector)?.blur();
}

function getValue(id) {
    let el = document.getElementById(id);
    return el?.value || "";
}

function saveAsFile(filename, bytesBase64) {
    if (navigator.msSaveBlob) {
        //Download document in Edge browser
        var data = window.atob(bytesBase64);
        var bytes = new Uint8Array(data.length);
        for (var i = 0; i < data.length; i++) {
            bytes[i] = data.charCodeAt(i);
        }
        var blob = new Blob([bytes.buffer], { type: "application/octet-stream" });
        navigator.msSaveBlob(blob, filename);
    }
    else {
        var link = document.createElement('a');
        link.download = filename;
        link.href = "data:application/octet-stream;base64," + bytesBase64;
        document.body.appendChild(link); // Needed for Firefox
        link.click();
        document.body.removeChild(link);
    }
}

/* View in fullscreen */
async function openFullscreen() {
    if (document.fullscreenElement) return;
    let htmlElement = document.documentElement;
    if (htmlElement.requestFullscreen) {
        await htmlElement.requestFullscreen();
    } else if (htmlElement.webkitRequestFullscreen) { /* Safari */
        htmlElement.webkitRequestFullscreen();
    } else if (htmlElement.msRequestFullscreen) { /* IE11 */
        htmlElement.msRequestFullscreen();
    } else {
        console.log('no requestFullscreen method found');
    }
}

/* Close fullscreen */
async function closeFullscreen() {
    if (!document.fullscreenElement) return;
    
    if (document.exitFullscreen) {
        await document.exitFullscreen();
    } else if (document.webkitExitFullscreen) { /* Safari */
        document.webkitExitFullscreen();
    } else if (document.msExitFullscreen) { /* IE11 */
        document.msExitFullscreen();
    } else {
        console.log('no exitFullscreen method found');
    }
}