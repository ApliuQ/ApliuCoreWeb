function ClientBrowser()
{
	var BrowserType= new Array(); //定义一数组 
    var Sys = {};
    var ua = navigator.userAgent.toLowerCase();
    var s;
    (s = ua.match(/msie ([\d.]+)/)) ? Sys.ie = s[1] :
    (s = ua.match(/firefox\/([\d.]+)/)) ? Sys.firefox = s[1] :
    (s = ua.match(/chrome\/([\d.]+)/)) ? Sys.chrome = s[1] :
    (s = ua.match(/opera.([\d.]+)/)) ? Sys.opera = s[1] :
    (s = ua.match(/version\/([\d.]+).*safari/)) ? Sys.safari = s[1] : 0;
    if (Sys.ie){BrowserType[0] = "IE";BrowserType[1] = Sys.ie;}//IE" + Sys.ie 
    else if (Sys.firefox){BrowserType[0] = "Firefox";BrowserType[1] = Sys.firefox;}//Firefox" + Sys.firefox 
    else if (Sys.chrome){BrowserType[0] = "Chrome";BrowserType[1] = Sys.chrome;}// Chrome" + Sys.chrome 
    else if (Sys.opera){BrowserType[0] = "Opera";BrowserType[1] = Sys.opera;}// Opera" + Sys.opera 
    else if (Sys.safari){BrowserType[0] = "Safari";BrowserType[1] = Sys.safari;}//Safari" + Sys.safari 
    else if (!!window.ActiveXObject || "ActiveXObject" in window) {BrowserType[0] = "IE";BrowserType[1] = "11";}//IE 11 
	else {BrowserType[0] = "";BrowserType[1] = "";}

	return BrowserType;
}
