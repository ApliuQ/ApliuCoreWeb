function getCookie(name)//取cookies函数       
{
    var arr = document.cookie.match(new RegExp("(^| )"+name+"=([^;]*)(;|$)"));
     if(arr != null) return decodeURI(arr[2]); return null;
}
function delCookie(name)//删除cookie
{
    var exp = new Date();
    exp.setTime(exp.getTime() - 1);
    var cval=getCookie(name);
    if(cval!=null) document.cookie= name + "="+cval+";expires="+exp.toGMTString()+"; path=/";
}
function logout() {
	delCookie("cur_username");//删除cookie
	$("#logout").hide();
	$("#username").hide();
	$("#register").show();
	$("#login").show();
}
$(document).ready(function(){
	var cur_username = getCookie("cur_username");
	if(cur_username==null){
		$("#logout").hide();
		$("#username").hide();
	}
	else{
		$("#register").hide();
		$("#login").hide();
		$("#username").html("欢迎 "+cur_username+" 到来！");
	}
	var isliuyan = Request['liuyan'];
	if(isliuyan=="false") alert("留言失败，请重试！");
});

function isLogin(){
	var cur_username = getCookie("cur_username");
	var namevalue=document.getElementById("message").value;
	if(namevalue==null||namevalue=="")
	{
		alert("请输入留言！");
		return false;
	}
	else{
		if(cur_username==null||cur_username=="")
		{
			alert("请登录后再留言！");
			return false;
		}
		else{
			if(ClientBrowser()[0]=="IE") alert("由于IE兼容性问题，所以留言成功，但是但却未显示，需要关闭浏览器（只关闭当前页面或者刷新都没用哦），再打开此页面才会看到效果。");
			return true;
		}
	}
}

function Getdatagame(){
		$.ajax({
            url:"php/displaygame.php?id=gamerecord",
            type: "GET",
            dataType: "text",
            success: function(tablebody){
				//alert(tablebody);
				$("#gamerecord").after(tablebody.substring(17,tablebody.length-16));		
            }
        })
		$.ajax({
            url:"php/displaygame.php?id=GameMax",
            type: "GET",
            dataType: "text",
            success: function(tablebody){
				//alert(tablebody.substring(16,tablebody.length-16));
				$("#GameMax").after(tablebody.substring(17,tablebody.length-16));		
            }
        })
		$.ajax({
            url:"php/displaygame.php?id=Gameabout",
            type: "GET",
            dataType: "text",
            success: function(tablebody){
				var allabout=tablebody.substring(17,tablebody.length-16);
				var arrabout = new Array;
 				arrabout = allabout.split("|");
				$("#Game2048about").after(arrabout[2]);	
				$("#Flappy2048about").after(arrabout[1]);
				$("#pinsabout").after(arrabout[0]);
            }
        })
}

/*function Getdatamessage(){
	$.ajax({
            url:"php/displayliuyan.php",
            type: "GET",
            dataType: "text",
            success: function(tablebody){
                //alert(tablebody);//$a的值
				//alert(inhtml.substring(17,inhtml.length-16));
			$("#tablebody").after(tablebody.substring(17,tablebody.length-16));		
        }
    })
}*/
function GetRequest() { 
	var url = location.search; //获取url中"?"符后的字串 
	var theRequest = new Object(); 
	if (url.indexOf("?") != -1) { 
		var str = url.substr(1); 
		strs = str.split("&"); 
		for(var i = 0; i < strs.length; i ++) { 
		theRequest[strs[i].split("=")[0]]=unescape(strs[i].split("=")[1]); 
		} 
	} 
	return theRequest; 
}var Request = new Object(); 
Request = GetRequest(); 











