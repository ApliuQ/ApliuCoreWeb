var yyy = document.getElementById('xxx');
var context = yyy.getContext('2d');
var lineWidth = 2;

autoSetCanvasSize(yyy);

listenToUser(yyy);


var eraserEnabled = false
pen.onclick = function () {
    eraserEnabled = false
    pen.classList.add('active')
    eraser.classList.remove('active')
}
eraser.onclick = function () {
    eraserEnabled = true
    eraser.classList.add('active')
    pen.classList.remove('active')
}
clear.onclick = function () {
    context.clearRect(0, 0, yyy.width, yyy.height);
}
download.onclick = function () {
    var url = yyy.toDataURL("image/png")
    var a = document.createElement('a')
    document.body.appendChild(a)
    a.href = url
    a.download = 'apliutools_drawing'
    a.target = '_blank'
    a.click()
}

black.onclick = function () {
    context.fillStyle = 'black'
    context.strokeStyle = 'black'
    black.classList.add('active')
    red.classList.remove('active')
    green.classList.remove('active')
    blue.classList.remove('active')
    usercolor.classList.remove('active')
}
red.onclick = function () {
    context.fillStyle = 'red'
    context.strokeStyle = 'red'
    black.classList.remove('active')
    red.classList.add('active')
    green.classList.remove('active')
    blue.classList.remove('active')
    usercolor.classList.remove('active')
}
green.onclick = function () {
    context.fillStyle = 'green'
    context.strokeStyle = 'green'
    black.classList.remove('active')
    red.classList.remove('active')
    green.classList.add('active')
    blue.classList.remove('active')
    usercolor.classList.remove('active')
}
blue.onclick = function () {
    context.fillStyle = 'blue'
    context.strokeStyle = 'blue'
    black.classList.remove('active')
    red.classList.remove('active')
    green.classList.remove('active')
    blue.classList.add('active')
    usercolor.classList.remove('active')
}
usercolor.onclick = function () {
    context.fillStyle = 'blue'
    context.strokeStyle = 'blue'
    black.classList.remove('active')
    red.classList.remove('active')
    green.classList.remove('active')
    blue.classList.remove('active')
    usercolor.classList.add('active')
}

thin.onclick = function () {
    lineWidth = 5
    thin.classList.add('active')
    thick.classList.remove('active')
}
thick.onclick = function () {
    lineWidth = 10
    thin.classList.remove('active')
    thick.classList.add('active')
}
usersizes.onchange = function () {
    if (usersizes.value <= 0) return;
    lineWidth = usersizes.value
    thin.classList.remove('active')
    thick.classList.remove('active')
    usersizes.classList.add('active')
}

/******/

function autoSetCanvasSize(canvas) {
    setCanvasSize()

    window.onresize = function () {
        setCanvasSize()
    }

    function setCanvasSize() {
        var pageWidth = document.documentElement.clientWidth
        var pageHeight = document.documentElement.clientHeight

        canvas.width = pageWidth
        canvas.height = pageHeight
    }
}

function drawCircle(x, y, radius) {
    context.beginPath()
    context.arc(x, y, radius, 0, Math.PI * 2);
    context.fill()
}

function drawLine(x1, y1, x2, y2) {
    context.beginPath();
    context.moveTo(x1, y1) // 起点
    context.lineWidth = lineWidth
    context.lineTo(x2, y2) // 终点
    context.stroke()
    context.closePath()
}

function listenToUser(canvas) {


    var using = false
    var lastPoint = {
        x: undefined,
        y: undefined
    }
    // 特性检测
    if (document.body.ontouchstart !== undefined) {
        // 触屏设备
        canvas.ontouchstart = function (aaa) {
            var x = aaa.touches[0].clientX
            var y = aaa.touches[0].clientY
            //console.log(x, y)
            using = true
            if (eraserEnabled) {
                context.clearRect(x - 5, y - 5, 10, 10)
            } else {
                lastPoint = {
                    "x": x,
                    "y": y
                }
            }
        }
        canvas.ontouchmove = function (aaa) {
            var x = aaa.touches[0].clientX
            var y = aaa.touches[0].clientY

            if (!using) { return }

            if (eraserEnabled) {
                context.clearRect(x - 5, y - 5, 10, 10)
            } else {
                var newPoint = {
                    "x": x,
                    "y": y
                }
                drawLine(lastPoint.x, lastPoint.y, newPoint.x, newPoint.y)
                lastPoint = newPoint
            }
        }
        canvas.ontouchend = function () {
            using = false
        }
    } else {
        // 非触屏设备
        canvas.onmousedown = function (aaa) {
            var x = aaa.clientX
            var y = aaa.clientY
            using = true
            if (eraserEnabled) {
                context.clearRect(x - 5, y - 5, 10, 10)
            } else {
                lastPoint = {
                    "x": x,
                    "y": y
                }
            }
        }
        canvas.onmousemove = function (aaa) {
            var x = aaa.clientX
            var y = aaa.clientY

            if (!using) { return }

            if (eraserEnabled) {
                context.clearRect(x - 5, y - 5, 10, 10)
            } else {
                var newPoint = {
                    "x": x,
                    "y": y
                }
                drawLine(lastPoint.x, lastPoint.y, newPoint.x, newPoint.y)
                lastPoint = newPoint
            }

        }
        canvas.onmouseup = function (aaa) {
            using = false
        }
    }

}


(function (window) {

    $("#usercolor").bigColorpicker(function (el, color) {
        context.fillStyle = color;
        context.strokeStyle = color;
        $(el).css("background-color", color);
    });
    $("#none").bigColorpicker("none", "L", 6);

    var svgSprite = '<svg><symbol id="icon-clear" viewBox="0 0 1024 1024"><path d="M902.095238 141.409524 633.904762 141.409524 633.904762 90.209524C633.904762 53.638095 609.52381 24.380952 577.828571 24.380952l-129.219048 0C414.47619 24.380952 390.095238 53.638095 390.095238 90.209524l0 51.2L121.904762 141.409524c-14.628571 0-24.380952 9.752381-24.380952 24.380952s9.752381 24.380952 24.380952 24.380952l24.380952 0L146.285714 877.714286c0 68.266667 53.638095 121.904762 121.904762 121.904762l487.619048 0c68.266667 0 121.904762-53.638095 121.904762-121.904762L877.714286 190.171429l24.380952 0c14.628571 0 24.380952-9.752381 24.380952-24.380952S916.72381 141.409524 902.095238 141.409524zM438.857143 90.209524C438.857143 80.457143 443.733333 73.142857 446.171429 73.142857l129.219048 0C580.266667 73.142857 585.142857 80.457143 585.142857 90.209524l0 51.2-146.285714 0L438.857143 90.209524zM828.952381 877.714286c0 41.447619-31.695238 73.142857-73.142857 73.142857L268.190476 950.857143c-41.447619 0-73.142857-31.695238-73.142857-73.142857L195.047619 190.171429l633.904762 0L828.952381 877.714286z"  ></path><path d="M512 853.333333c14.628571 0 24.380952-9.752381 24.380952-24.380952L536.380952 431.542857c0-14.628571-9.752381-24.380952-24.380952-24.380952s-24.380952 9.752381-24.380952 24.380952L487.619048 828.952381C487.619048 841.142857 497.371429 853.333333 512 853.333333z"  ></path><path d="M668.038095 853.333333c14.628571 0 24.380952-9.752381 24.380952-24.380952L692.419048 497.371429c0-14.628571-9.752381-24.380952-24.380952-24.380952s-24.380952 9.752381-24.380952 24.380952L643.657143 828.952381C643.657143 841.142857 653.409524 853.333333 668.038095 853.333333z"  ></path><path d="M355.961905 853.333333c14.628571 0 24.380952-9.752381 24.380952-24.380952L380.342857 497.371429c0-14.628571-9.752381-24.380952-24.380952-24.380952s-24.380952 9.752381-24.380952 24.380952L331.580952 828.952381C331.580952 841.142857 343.771429 853.333333 355.961905 853.333333z"  ></path></symbol><symbol id="icon-pen" viewBox="0 0 1024 1024"><path d="M241.53404 893.694472l-111.148643-111.097445 36.45225-132.753768 18.738094-18.686897 207.398964 207.398963-18.686898 18.738095-132.753767 36.401052z m-153.23256 42.03272l27.748763-101.114034L189.466712 907.978429l-101.114035 27.748763zM737.274391 79.406515l207.398964 207.45016L426.150347 805.328486 218.751384 597.980719 737.274391 79.355318zM444.837245 890.417865l33.175642-33.175642L1006.980078 328.326229a58.671738 58.671738 0 0 0 0-82.939107L778.743944 17.150988a58.671738 58.671738 0 0 0-82.939106 0L166.888844 546.066982l-33.175643 33.22684-24.420959 24.369762a58.46695 58.46695 0 0 0-15.922261 29.643051L1.317812 977.043154c-3.583789 13.311215 0.614364 26.827217 9.829821 35.786689 8.959471 9.215456 22.526671 13.413609 35.786688 9.82982l344.606868-92.052169a59.849269 59.849269 0 0 0 11.109745-3.583788c6.450819-2.867031 12.492063-6.911592 17.816549-12.236078l24.369762-24.369763z"  ></path></symbol><symbol id="icon-save" viewBox="0 0 1024 1024"><path d="M896 661.333333v128c0 58.88-53.973333 106.666667-120.106667 106.666667H248.106667C181.973333 896 128 848.213333 128 789.333333v-128H85.333333v128c0 82.346667 72.96 149.333333 162.773334 149.333334h527.786666c89.813333 0 162.773333-66.986667 162.773334-149.333334v-128h-42.666667z" fill="" ></path><path d="M740.48 463.146667l-30.293333-30.293334-176.853334 177.066667V85.333333h-42.666666v524.586667l-176.853334-177.066667-30.293333 30.293334L512 691.413333l228.48-228.266666z" fill="" ></path></symbol><symbol id="icon-erase" viewBox="0 0 1024 1024"><path d="M542 832.1l457.034-457.2c33.234-33.316 33.234-87.244 0-120.558L769.65 24.966c-33.288-33.288-87.262-33.288-120.55 0L192.074 482.158l-0.018 0.01-167.09 166.932c-33.234 33.316-33.234 87.244 0 120.558l229.384 229.374a84.43 84.43 0 0 0 46.148 23.588 17.012 17.012 0 0 0 6.702 1.38h549.718c9.426 0 17.066-7.64 17.066-17.066s-7.64-17.066-17.066-17.066H384.086l34.168-34.134H768c9.426 0 17.066-7.64 17.066-17.066s-7.64-17.066-17.066-17.066H452.418l34.168-34.134H768c9.426 0 17.066-7.64 17.066-17.066s-7.64-17.066-17.066-17.066H520.754l21.23-21.208 0.016-0.028z m-191.232 142.8c-20.23 19.3-52.054 19.3-72.284 0L49.1 745.526c-19.926-19.976-19.926-52.308 0-72.284l155.016-154.858 301.65 301.658-154.998 154.858z m-122.518-480.65L673.234 49.1c20.23-19.3 52.054-19.3 72.284 0L974.9 278.476c19.924 19.978 19.924 52.314 0 72.292l-444.984 445.14-301.666-301.658z"  ></path><path d="M728.866 90.334a17.064 17.064 0 0 0-24.132 24.132l204.8 204.8a17.064 17.064 0 0 0 24.132-24.132l-204.8-204.8z"  ></path></symbol></svg>';
    var script = function () {
        var scripts = document.getElementsByTagName("script");
        return scripts[scripts.length - 1]
    }();
    var shouldInjectCss = script.getAttribute("data-injectcss");
    var ready = function (fn) {
        if (document.addEventListener) {
            if (~["complete", "loaded", "interactive"].indexOf(document.readyState)) {
                setTimeout(fn, 0)
            } else {
                var loadFn = function () {
                    document.removeEventListener("DOMContentLoaded", loadFn, false);
                    fn()
                };
                document.addEventListener("DOMContentLoaded", loadFn, false)
            }
        } else if (document.attachEvent) {
            IEContentLoaded(window, fn)
        }
        function IEContentLoaded(w, fn) {
            var d = w.document,
            done = false,
            init = function () {
                if (!done) {
                    done = true;
                    fn()
                }
            };
            var polling = function () {
                try {
                    d.documentElement.doScroll("left")
                } catch (e) {
                    setTimeout(polling, 50);
                    return
                }
                init()
            };
            polling();
            d.onreadystatechange = function () {
                if (d.readyState == "complete") {
                    d.onreadystatechange = null;
                    init()
                }
            }
        }
    };
    var before = function (el, target) {
        target.parentNode.insertBefore(el, target)
    };
    var prepend = function (el, target) {
        if (target.firstChild) {
            before(el, target.firstChild)
        } else {
            target.appendChild(el)
        }
    };
    function appendSvg() {
        var div, svg;
        div = document.createElement("div");
        div.innerHTML = svgSprite;
        svgSprite = null;
        svg = div.getElementsByTagName("svg")[0];
        if (svg) {
            svg.setAttribute("aria-hidden", "true");
            svg.style.position = "absolute";
            svg.style.width = 0;
            svg.style.height = 0;
            svg.style.overflow = "hidden";
            prepend(svg, document.body)
        }
    }
    if (shouldInjectCss && !window.__iconfont__svg__cssinject__) {
        window.__iconfont__svg__cssinject__ = true;
        try {
            document.write("<style>.svgfont {display: inline-block;width: 1em;height: 1em;fill: currentColor;vertical-align: -0.1em;font-size:16px;}</style>")
        } catch (e) {
            console && console.log(e)
        }
    }
    ready(appendSvg)
})(window)