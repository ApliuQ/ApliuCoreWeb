$(document).ready(function () {
    $.ajaxSetup({
        async: false
    });
});
var ApliuCommon = {};
ApliuCommon.HttpSend = function (apiurl, options) {
    jQuery.support.cors = true;
    var v;
    if (options) {
        if (options.params && options.method && options.method.toUpperCase() == "GET") {
            try {
                if (typeof JSON.parse(options.params) == "object") {
                    apiurl += $.param(options.params);
                } else {
                    apiurl += "?" + options.params;
                }
            } catch (e) {
                apiurl += "?" + options.params;
            }

        }
        if (options.method && options.method.toUpperCase() == "GET") {
            v = $.get(apiurl);
        }
        else if (options.method && options.method.toUpperCase() == "POST") {
            v = $.post(apiurl, options.params);
        }
        else {
            v = $.get(apiurl);
        }
    }
    else {
        v = $.get(apiurl);
    }
    var deferred = $.Deferred();
    //var v = $.ajax({ url: apiurl, async: false });
    v.done(function (data) {
        var rst;
        try {
            rst = JSON.parse(data);
        } catch (e) {
            try {
                if (data.readyState == null) rst = data;
                else rst = "readyState：" + data.readyState + "    \n    " + "responseText：" + data.responseText;
            } catch (ex) {
                rst = data;
            }
        }
        deferred.resolve(rst);
    }).fail(function (error) {
        var rst;
        try {
            rst = JSON.parse(error);
        } catch (e) {
            try {
                if (data.readyState == null) rst = data;
                else rst = "readyState：" + error.readyState + "    \n    " + "responseText：" + error.responseText;
            } catch (ex) {
                rst = error;
            }
        }
        deferred.reject(rst);
    });
    return deferred.promise();
}

var IsSuportLocalStorage = function () {
    if (window.Storage && window.localStorage && window.localStorage instanceof Storage) {
        return true;
    }
    else return false;
};

String.prototype.format = function () {
    var resultStr = this.toString();
    // 参数为对象
    if (typeof arguments[0] === "object") {
        for (var i in arguments[0]) {
            resultStr = resultStr.replace("{" + i + "}", arguments[0][i]);
        }
    }
        // 多个参数
    else {
        for (var i = 0; i < arguments.length; i++) {
            resultStr = resultStr.replace("{" + i + "}", arguments[i]);
        }
    }
    return resultStr;
};

ApliuCommon.isPoneAvailable = function (str) {
    var myreg = /^[1][3,4,5,7,8][0-9]{9}$/;
    if (!myreg.test(str)) {
        return false;
    } else {
        return true;
    }
}

ApliuCommon.getoptions = function (method, params, usertoken) {
    var options = {
        "method": method.toString().toUpperCase(),
        "params": params,
        "usertoken": usertoken
    }
    return options;
}

$(document).ready(function () {
    var data = { method: "Get" };
    $.when(ApliuCommon.HttpSend("/api/common/user", data)).then(function (rst) {
        if (rst.code == "0") {
            $("#userlogin").text(rst.msg);
            $("#userlogin").attr("href", "javascript:void(0);");

            $("#userregister").text("退出");
            $("#userregister").attr("href", "javascript:logout();");
        }
    })
})

var logout = function () {
    $.confirm("您确定要退出吗?", "确定退出?", function () {
        var data = { method: "Post" };
        $.when(ApliuCommon.HttpSend("/api/common/logout", data)).then(function (rst) {
            if (rst.code == "0") {
                window.location.href = "/";
            } else {
                $.alert("退出失败，原因：" + rst.msg);
            }
        }, function (rst) { $.alert("退出失败，原因：" + rst.msg); });
    });
}