var staticFileVersion={
    "ver": "1.0.0",
    "jquery":"1.8.2",
    "jquery.validate":"1.12.0",
    "error.css":"1.0.0",
    "util":"1.0.0",
    "error": "1.0.0",
    "default":"1.0.0"

};
var ext = {
    endWith : function (a, b) {
        var aL = a.length;
        var bL = b.length;
        var start = a.indexOf(b)
        if (aL - start > bL) return false;
        var s = a.substring(start, aL)
        if (s == b)
            return true;
        return false;    
    }
};

function writeStaticFile() {
    var h = "",s = ""
    for (var i = 0; i < arguments.length; i++) {
        var f = arguments[i];
        var v = staticFileVersion[f];
        v = v ? "?VER=" + v : "";
        var t = v ? "": "?t=" + Math.round(new Date().getTime() / 1000);
        if (ext.endWith(f, ".css")) {
            if (f.charAt(0) != "/") f = "/res/css/" + f;    
            s += '<link href="' + h + f + v + t + '" rel="stylesheet" type="text/css">'
        }
        else {
            if (f.charAt(0) != "/") f = "/res/js/" + f;
            s += '<script type="text/javascript" src="'+h + f + '.js' + v + t + '"></script>'
        }
    }
    document.write(s);
}

function WSF(option) {
    if (option instanceof Array) {
        writeStaticFile(option);
        return false;
    }
    var origin = option.origin, component = option.component;
    if (origin || component) {
        if (origin && origin instanceof Array) {
            writeStaticFile(origin);
        }
        if (component && component instanceof Array) {
            writeStaticFile(component);
        }
    }    
}

function writeComponents() {
    var h = "", s = ""
    for (var i = 0; i < arguments.length; i++) {
        var f = arguments[i];
        if (ext.endWith(f, ".css")) {
            if (f.charAt(0) != "/") f = "/res/lib/" + f;
            s += '<link href="' + h + f  + '" rel="stylesheet" type="text/css">'
        }
        else {
            if (f.charAt(0) != "/") f = "/res/lib/" + f;
            s += '<script type="text/javascript" src="' + h + f + '.js' + '"></script>'
        }
    }
    document.write(s);
}
