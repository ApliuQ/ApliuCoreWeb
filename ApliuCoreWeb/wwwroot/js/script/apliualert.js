var apliualert = function (content) {
    $.alert(content);
    return;


    var shield = document.createElement("DIV");
    shield.id = "shield";
    shield.style.position = "absolute";
    shield.style.left = "0";
    shield.style.top = "0";
    shield.style.width = "100%";
    shield.style.height = "120%";
    shield.style.zIndex = "25";
    shield.style.backgroundColor = "rgba(51, 51, 51, 0.2)";
    var alertFram = document.createElement("DIV");
    alertFram.id = "alertFram";
    alertFram.style.position = "absolute";
    //alertFram.style.width = "280px";
    alertFram.style.height = "150px";
    alertFram.style.left = "50%";
    alertFram.style.top = "50%";
    alertFram.style.marginLeft = "-140px";
    alertFram.style.marginTop = "-110px";
    alertFram.style.textAlign = "center";
    alertFram.style.lineHeight = "150px";
    alertFram.style.zIndex = "300";
    alertFram.style.borderRadius = "5px";
    alertFram.style.backgroundColor = "white";
    alertFram.style.minWidth = "200px";
    strHtml = "<div>\n";
    strHtml += "<ul style=\"list-style:none;margin:0 auto;padding:0px;border-radius: 5px;\">\n";
    //strHtml += " <li style=\"background:#4794FA;text-align:left;padding-left:20px;font-size:14px;font-weight:bold;height:25px;line-height:25px;border:1px solid #F9CADE;color:white\">提示</li>\n";
    strHtml += " <li style=\"background: #fff url(/img/icon/tips.png) no-repeat scroll 5px center;text-align:center;font-size:large;line-height:90px;padding-left: 45px;padding-right: 10px;border-radius: 5px;\">" + content + "</li>\n";
    strHtml += " <li style=\"background:#fff;text-align:center;font-weight:bold;height:50px;line-height:50px;\"><input type=\"button\" value=\"关闭\" onclick=\"doOk()\" style=\"width:80px;height:30px;background:#4693FA;color:white;border:1px solid white;font-size:.55rem;line-height:0px;font-family: Microsoft Yahei,Helvetica,Arial;outline:none;border-radius: 5px;margin-top: 4px\"/></li>\n";
    strHtml += "</div></ul>\n";
    alertFram.innerHTML = strHtml;
    document.body.appendChild(alertFram);
    document.body.appendChild(shield);

    //动态调整宽度
    var width = $("#alertFram").outerWidth();
    $("#alertFram").css("margin-left", -width / 2);

    this.doOk = function () {
        document.body.removeChild(alertFram);
        document.body.removeChild(shield);
    }
    alertFram.focus();
    document.body.onselectstart = function () { return false; };
}