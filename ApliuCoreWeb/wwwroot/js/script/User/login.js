
$(function () {
    $("#btn-login").click(function () {
        if (logincheck()) {
            var fromdata = { method: "POST", params: $(this.form).serialize() };
            $.when(ApliuCommon.HttpSend("/api/common/login", fromdata)).then(function (rst) {
                if (rst.code == "0") {
                    window.location.href = "../Home/Index";
                } else {
                    $.alert(rst.msg);
                }
            }, function (rst) {
                $.alert(rst.msg);
            });
        }
    });

    $("#btn-register").click(function () {
        if (registercheck()) {
            var fromdata = { method: "POST", params: $(this.form).serialize() };
            $.when(ApliuCommon.HttpSend("/api/common/register", fromdata)).then(function (rst) {
                if (rst.code == "0") {
                    $.confirm(rst.msg + "，是否返回登录?", "提示", function () {
                        window.location.href = "Login";
                    }, function () {
                        //取消操作
                    });
                } else {
                    $.alert(rst.msg);
                }
            }, function (rst) {
                $.alert(rst.msg);
            });
        }
    });

    $("#btn-changepassword").click(function () {
        if (changepassword()) {
            var fromdata = { method: "POST", params: $(this.form).serialize() };
            $.when(ApliuCommon.HttpSend("/api/common/changepassword", fromdata)).then(function (rst) {
                if (rst.code == "0") {
                    $.confirm(rst.msg + "，是否返回登录?", "提示", function () {
                        window.location.href = "Login";
                    }, function () {
                        //取消操作
                    });
                } else {
                    $.alert(rst.msg);
                }
            }, function (rst) {
                $.alert(rst.msg);
            });
        }
    });
})

var logincheck = function () {
    var username = $("#username").val();
    var password = $("#password").val();
    if (username == "") {
        $.alert("用户名不能为空", "提示");
        return false;
    }
    if (!ApliuCommon.isPoneAvailable(username)) {
        $.alert("用户名格式有误", "提示");
        return false;
    }
    if (password == "") {
        $.alert("密码不能为空", "提示");
        return false;
    }
    return true;
}

var registercheck = function () {
    var user = "手机号码";
    var phone = $("#username").val();
    var smscode = $("#smscode").val();
    var password = $("#password").val();
    var passwordag = $("#passwordag").val();
    if (phone == "") {
        $.alert(user + "不能为空", "提示");
        return false;
    }
    if (!ApliuCommon.isPoneAvailable(phone)) {
        $.alert(user + "格式有误", "提示");
        return false;
    }
    if (smscode == "") {
        $.alert("请输入短信验证码", "提示");
        return false;
    }
    if (password == "") {
        $.alert("密码不能为空", "提示");
        return false;
    }
    if (password.length < 3) {
        $.alert("密码长度必须大于3", "提示");
        return false;
    }
    if (passwordag == "") {
        $.alert("请再次输入密码", "提示");
        return false;
    }
    if (password != passwordag) {
        $.alert("两次密码不一致", "提示");
        return false;
    }
    if (!$('#registerservice').is(':checked')) {
        $.alert("请同意服务协议", "提示");
        return false;
    }
    return true;
}

var changepassword = function () {
    var user = "手机号码";
    var phone = $("#username").val();
    var smscode = $("#smscode").val();
    var password = $("#password").val();
    var passwordag = $("#passwordag").val();
    if (phone == "") {
        $.alert(user + "不能为空", "提示");
        return false;
    }
    if (!ApliuCommon.isPoneAvailable(phone)) {
        $.alert(user + "格式有误", "提示");
        return false;
    }
    if (smscode == "") {
        $.alert("请输入短信验证码", "提示");
        return false;
    }
    if (password == "") {
        $.alert("密码不能为空", "提示");
        return false;
    }
    if (password.length < 3) {
        $.alert("密码长度必须大于3", "提示");
        return false;
    }
    if (passwordag == "") {
        $.alert("请再次输入密码", "提示");
        return false;
    }
    if (password != passwordag) {
        $.alert("两次密码不一致", "提示");
        return false;
    }
    return true;
}

!function (win, $) {

    var dialog = win.YDUI.dialog;

    var $getCode = $('#getsmscode');

    // 定义参数
    $getCode.sendCode({
        disClass: 'getsmscode-disabled', // 禁用按钮样式【必填】
        secs: 60, // 倒计时时长 [可选，默认：60秒]
        run: false,// 是否初始化自动运行 [可选，默认：false]
        runStr: '{%s}秒后重新获取',// 倒计时显示文本 [可选，默认：58秒后重新获取]
        resetStr: '重新获取验证码'// 倒计时结束后按钮显示文本 [可选，默认：重新获取验证码]
    });

    $getCode.on('click', function () {
        var phone = $('#username').val();
        if (phone == null || phone.length <= 0) {
            apliualert("请输入手机号码");
            return;
        }
        var $this = $(this);
        dialog.loading.open('发送中...');
        // ajax 成功发送验证码后调用【start】
        var codetypevalue = $getCode.attr("codetype");
        var data = ApliuCommon.getoptions("Post", { Mobile: phone, codeType: codetypevalue }, "false");
        $.when(ApliuCommon.HttpSend("/api/toolapi/sendsmscode", data)).then(function (rst) {
            if (rst.code == "0") {
                dialog.loading.close();
                $this.sendCode('start');
                dialog.toast('已发送', 'success', 1500);
            }
            else {
                dialog.loading.close();
                apliualert("短信发送失败, 请重试");
            }
        }, function (error) {
            dialog.loading.close();
            apliualert("短信发送失败, 请重试");
        })
        //setTimeout(function () { //模拟ajax发送
        //    dialog.loading.close();
        //    $this.sendCode('start');
        //    dialog.toast('已发送', 'success', 1500);
        //}, 800);
    });

}(window, jQuery);