
$(function () {
    $("#btn-login").click(function () {
        if (logincheck()) {
            var fromdata = { method: "POST", params: $(this.form).serialize() };
            $.when(ApliuCommon.HttpSend("/api/common/login", fromdata)).then(function (rst) {
                if (rst.code == "0") {
                    window.location.href = "/";
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
                        window.location.href = "login";
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
                        window.location.href = "login";
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