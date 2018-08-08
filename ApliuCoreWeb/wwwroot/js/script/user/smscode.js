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
        $.when(ApliuCommon.HttpSend("/api/tools/sendsmscode", data)).then(function (rst) {
            if (rst.code == "0") {
                dialog.loading.close();
                $this.sendCode('start');
                dialog.toast('已发送', 'success', 1500);
            }
            else {
                dialog.loading.close();
                apliualert(rst.msg);
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