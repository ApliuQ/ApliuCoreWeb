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
} var Request = new Object(); 
Request = GetRequest(); 


var register,login; 
register = Request['register']; 
login = Request['login']; 
if(register=="false")
{
	alert("注册失败，请尝试重新注册！");
}
if(register=="true")
{
	alert("注册成功，请登录！");
}
if(login=="false")
{
	alert("登录失败，请尝试重新登录！");
}

function RegisterIsNull() {

/*    var text = document.getElementById("textarea").value;
    var ss = "汉字 "+ text.replace(/[^\u4e00-\u9fff]/ig,"").length +" 个。\n";
    ss += "英文 "+ text.replace(/[^a-z]/ig,"").length +" 个。\n";
    ss += "数字 "+ text.replace(/\D/ig,"").length +" 个。\n";
    ss += "空格 "+ text.replace(/[^ ]/ig,"").length +" 个。\n";
    ss += "回车 "+ text.replace(/[^\n]/ig,"").length +" 个。\n";
    ss += "特殊字符 "+ text.replace(/[\u4e00-\u9fffa-z\d\s\n\r]/ig,"").length +" 个。\n";
    alert(ss);*/
	
	var namevalue=document.getElementById("username").value;
	var wordValue=document.getElementById("password").value;
	if(namevalue==""||namevalue==null||wordValue==""||wordValue==null)
	{
		alert("用户名或密码不能为空！")
		return false;
	}
	else{
		var boolname=namevalue;
		var boolpass=wordValue;
		var num=boolname.replace(/[^\u4e00-\u9fff]/ig,"").length +boolname.replace(/[^ ]/ig,"").length+boolname.replace(/[^\n]/ig,"").length+boolname.replace(/[\u4e00-\u9fffa-z\d\s\n\r]/ig,"").length;
		num+=boolpass.replace(/[^\u4e00-\u9fff]/ig,"").length +boolpass.replace(/[^ ]/ig,"").length +boolpass.replace(/[^\n]/ig,"").length +boolpass.replace(/[\u4e00-\u9fffa-z\d\s\n\r]/ig,"").length;
		if(num>0){
			alert("用户名或密码不能含有中文或特殊字符！");
			return false;
		}else{
			onregister();
			return true;
		}
	}
}
function onregister() {
    document.forms['register'].action = "php/register.php?id="+Request['id'];
    document.forms['register'].submit();
}



function LoginIsNull() {
	var namevalue=document.getElementById("username").value;
	var wordValue=document.getElementById("password").value;
	if(namevalue==""||namevalue==null||wordValue==""||wordValue==null)
	{
		alert("用户名或密码不能为空！")
		return false;
	}else{
		onlogin();
		return true;
	}
}
function onlogin() {
  document.forms['login'].action = "php/boollogin.php?id="+Request['id'];
  document.forms['login'].submit();
}
function forget() {
	 alert("暂不提供找回密码，可点击左上角爱的故事返回主页，然后联系wo找回！");
}

function isExist(){
			$.ajax({
				url:"php/isexist.php?username="+document.getElementById("username").value,
				type: "GET",
				dataType: "text",
				success: function(isExist){
					var ui = document.getElementById("isExist");
					if(isExist.substring(17,isExist.length-16)=="1") ui.style.visibility="visible";
					else ui.style.visibility="hidden";
				}
			});
}