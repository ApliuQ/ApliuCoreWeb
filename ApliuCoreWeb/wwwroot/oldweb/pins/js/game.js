var gamestart;
var time = -1;
var guan = 1;
var t;

// 计时
function showtime() {
    time++;
    t = setTimeout("showtime()", 1000);
}

function MC(t) {
    if (!(this instanceof MC)) return new MC(t);
    if (this.ctx = t.fill ? t : t = (t.getContext ? t : document.querySelector(t)).getContext("2d"), !this.fill) {
        var e = MC.prototype,
            n = {
                moveTo: "M",
                lineTo: "L",
                closePath: "Z",
                beginPath: "B",
                arcTo: "A",
                bezierCurveTo: "C",
                quadraticCurveTo: "Q"
            }, i = [],
            o = [];
        e.M = e.L = e.Z = e.B = e.A = e.C = e.Q = {};
        for (var a in t) ("function" == typeof t[a] ? o : i).push(a);
        i.forEach(function (t) {
            e[t] = function (e) {
                return arguments.length ? (this.ctx[t] = e, this) : this.ctx[t]
            }
        }), o.forEach(function (t) {
            e[t] = function () {
                return this.ctx[t].apply(this.ctx, arguments) || this
            }
        }), Object.keys(n).forEach(function (t) {
            e[n[t]] = e[t]
        })
    }
}

function gohw() {

}

var gameinit = function () {
    time = -1;
    guan = 1;
    document.getElementById("gohw"),
    function (t, e, n) {
        function i(t) {
            return t %= M, 0 > t ? t + M : t
        }

        function o(t) {
            var e = m + w * A(t);
            t = d + w * D(t), c.B().arc(e, t, p, 0, M, !1).fill(), c.M(m, d).L(e, t).stroke()
        }

        function a(t, e) {
            var n = m + e * A(t),
                i = d + e * D(t);
            c.B().arc(n, i, p, 0, M, !1).fill()
        }

        function r() {
            if ("over" != y && "pass" != y) {
                var t = P.d || "normal";
                for (z = (t && I[t] || I.e)(Date.now() - k), c.save().fillStyle("#aaa").fillRect(0, 0, h, g).restore(), c.B().arc(m, d, v, 0, M, !1).fill(), t = 0; t < q.length; t++) o(q[t] + z);
                for (var t = x, e = j; g - d + p > t && e > 0; t += 30, e--) {
                    a(C, t);
                    var s = C,
                        f = t,
                        u = e,
                        T = m + f * A(s),
                        s = d + f * D(s);
                    c.B().arc(T, s, p, 0, M, !1).fill(), c.save().fillStyle("#fff").fillText(u, T, s, 2 * p).restore()
                }
                if ("shooting" == y) {
                    if (t = C, u = x, s = w, T = s + 2 * p, e = (Date.now() - B) / S, e > 1 && (e = 1), u -= (u - s) * e, T = T > u) t: {
                        for (s = q.length; s--;)
                            if (T = i(q[s] - (t - z)), E > T || T > M - E) {
                                T = {
                                    which: s,
                                    a: T
                                };
                                break t
                            }
                        T = !1
                    }
                    T ? (e = w * D(T.a), e = w * A(T.a) + n.sqrt(4 * p * p - e * e), a(t, e), y = "over", c.save().font("bold 18px sans-serif").textAlign("right").fillStyle("#f00").fillText("失败了！", h - 10, g - 60).restore(), stop(), gamestart = l, gameover(F - 1)) : 1 > e ? a(t, u) : (q.push(i(t - z)), o(t), y = "idle", j || (y = "pass", ("jfcz-data", '{"aa":' + (F + 1) + "}")

    , c.save().font("bold 18px sans-serif").textAlign("right").fillStyle("#060").fillText("恭喜过关！", h - 10, g - 60).fillText("稍后一秒。", h - 10, g - 30).restore(), setTimeout(l, 1e3)))
                    guan = F;
                }

                c.save().font("0.8rem sans-serif").textAlign("left").fillStyle("#000").fillText(" 时间:" + (time) + " 分数:" + F * F * F, 10, g - 60).font("bold 48px sans-serif").textAlign("center").fillStyle("#fff").fillText(F, m, d).restore(), N(r)
            }
        }

        function s() {
            y = "shooting", B = Date.now(), j--
        }

        function f(t) {
            if ("idle" == y) {
                if ("img" === t.target.tagName.toLowerCase() && "gohwimg" === t.target.id) return void ("touchstart" === t.type || t.preventDefault());
                switch (T || ["mousedown", "touchstart", "MSPointerDown", "pointerdown"].forEach(function (n) {
                    n != t.type && e.removeEventListener(n, f, !1)
                }), t.type) {
                    case "mousedown":
                        if (0 < t.button) return;
                        s();
                        break;
                    case "touchstart":
                    case "MSPointerDown":
                    case "pointerdown":
                        s();
                        break;
                    default:
                        return
                }
                t.preventDefault()
            }
        }

        function l() {
            if (!y || "over" == y || "pass" == y) {
                "pass" == y && F++, y = "idle";
                var t;
                t = R.length, t = t >= F ? R[F - 1] : [R[t - 1][0], R[t - 1][1] + F - t], P = {
                    b: t[0],
                    c: t[1],
                    d: t[2] || (F % 2 ? "anti" : "normal")
                }, t = P.b;
                for (var e = 0, n = []; t > e; e++) n[e] = M * e / t;
                q = n, L = q.concat(), j = P.c, k = Date.now(), r()
            }
        }
        var c, u, h, g, m, d, v, p, w, x, y, T, b, S, M, C, A, D, k, B, E, L, q, P, j, z = 0,
            F = 1,
            R = [
                [3, 6],
                [5, 6],
                [6, 6],
                [7, 6],
                [9, 7],
                [9, 9],
                [8, 12],
                [9, 12],
                [7, 12],
                [8, 13]
            ],
            I = {
                normal: function (t) {
                    return M * (t % b) / b
                },
                anti: function (t) {
                    return M * (1 - t % b / b)
                }
            };
        v = 50, p = 10, w = 140, b = 5e3, x = 200, S = 100, y = "", M = 2 * n.PI, C = n.PI / 2, A = n.cos, D = n.sin, c = MC("#stage"), u = c.ctx, h = u.canvas.width, g = u.canvas.height, m = h / 2, d = .4 * g, c.fillStyle("#000").strokeStyle("#000"), c.font("12px sans-serif").textAlign("center").textBaseline("middle"), L = [0, C], q = L.concat(), E = 4 * n.asin(p / 2 / w), ["mousedown", "touchstart", "MSPointerDown", "pointerdown"].forEach(function (t) {
            e.addEventListener(t, f, !1)
        });
        var N = t.requestAnimationFrame || t.webkitRequestAnimationFrame || t.mozRequestAnimationFrame || t.msRequestAnimationFrame;
        ! function () {
            var t = ("jfcz-data");
            if (t) {
                try {
                    t = JSON.parse(t)
                } catch (e) { }
                t && t.aa && (F = 0 | t.aa)
            }
        }(), l()
    }(window, document, Math);
    showtime();
}

gameinit();

var loginalert = 0;
function gameover(level) {
    clearTimeout(t);
    var params = {
        "gamename": "Pins",
        "score": (guan * guan * guan - 1),
        "usetime": time,
        "stage": guan
    };
    var data = ApliuCommon.getoptions("Post", params, "");
    $.when(ApliuCommon.HttpSend("/api/common/setgamedata", data)).then(function (rst) {
        if (rst.code == "1") {
            if (loginalert == 0) {
                loginalert++;
                setTimeout(function () {
                    $.confirm("登录后可以保存成绩", "是否登录", function () {
                        window.location.href = "/User/Login";
                    }, function () {
                        //取消操作
                    })
                }, 500);
            }
        } else if (rst.code == "0") {
            $("#results").text("战绩已保存!");
        }
    }, function (rst) {

    });
    $(".gameoverscore").text("你一共闯关了" + level + "关!");
    //var inputgamephp = "php/inputgame.php?game=pins"+"&score="+(guan*guan*guan-1)+"&time="+time+"&max="+guan;
    //$.ajax({
    //		url:inputgamephp,
    //		type: "GET",
    //		dataType: "text",
    //		success: function(savegame){
    //							//返回值包括html以及body标签
    //		var isOK=savegame.substring(17,savegame.length-16);
    //							//不完整

    //			if(isOK=="0") $("#results").text("战绩已保存!");
    //			if(isOK=="1")
    //			{
    //				$("#results").text("未登录无法保存战绩！");
    //				$('#islogin').show();
    //			}
    //			if(isOK=="2") $("#results").text("连接服务器失败！");
    //			if(isOK=="3") $("#results").text("相同战绩已保存！");
    //     }
    //});

    $("#display2").text("分数: " + (guan * guan * guan - 1));
    $("#display3").text("用时: " + time + "s");
    $("#display4").text("关数: " + guan);

    setTimeout("$('#gameover').show()", 500);
}

function startGame() {
    $("#gameover").hide();
    showtime();
	gamestart();
    //var canvas = document.createElement("canvas");
    //canvas.height = "550rem";
    //canvas.style.marginTop = "-4rem";
    //canvas.id = "stage";
    //$("#gamemain").html('<canvas id="stage" height="550rem" style="margin-top: -4rem;"></canvas>');
    //gameinit();
    //document.location.href = document.location.href;
}

