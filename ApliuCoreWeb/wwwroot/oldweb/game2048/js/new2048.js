var locations;
var keys = [ "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c",
		"d", "e", "f" ];
// 不同的数字对于不同的颜色,
// colors是颜色的数组
var colors = [ "#FFF", "PINK", "GRAY", "#ABCDEF", "#0FF0FF", "#FF0", "#CDF0AB",
		"#FEDCBA", "#F0F", "#BBBBBB", "#00F", "#00FF00" ];

var score;// 总分数
var max;// 最大数
var time;// 计时
var t;
var isGame=false;
// 
// $(function() {
// init();
// });

// 初始化
function init() {
	isGame=true;
	t = setInterval(showtime, 1000);
	score = 0;
	max = 0;
	time = 0;
	locations = [ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ];
	locations[createLocation()] = createFixedNum();
	locations[createLocation()] = createFixedNum();
	paint();

	// 按键控制方向
	window.onkeydown = function(e) {
		if(isGame){
			var keyCode;
			if (!e)
				e = window.event;
			if (document.all) {
				keyCode = e.keyCode;
			} else {
				keyCode = e.which;
			}
			// ← 或 A
			if (keyCode == 37 || keyCode == 65) {
				$("#keys").text("←");
				toLeft();
				locations[createLocation()] = createFixedNum();
				paint();
				isEnd();
			}
			// ↑ 或 W
			if (keyCode == 38 || keyCode == 87) {
				$("#keys").text("↑");
				toUp();
				locations[createLocation()] = createFixedNum();
				paint();
				isEnd();
			}
			// → 或 D
			if (keyCode == 39 || keyCode == 68) {
				$("#keys").text("→");
				toRight();
				locations[createLocation()] = createFixedNum();
				paint();
				isEnd();
			}
			// ↓ 或 S
			if (keyCode == 40 || keyCode == 83) {
				$("#keys").text("↓");
				toDown();
				locations[createLocation()] = createFixedNum();
				paint();
				isEnd();
			}
		}
	};
}
function shang() {
	if(!isGame) return;
	$("#keys").text("↑");
	toUp();
	locations[createLocation()] = createFixedNum();
	paint();
	isEnd();
}
function xia() {
	if(!isGame) return;
	$("#keys").text("↓");
	toDown();
	locations[createLocation()] = createFixedNum();
	paint();
	isEnd();
}
function zuo() {
	if(!isGame) return;
	$("#keys").text("←");
	toLeft();
	locations[createLocation()] = createFixedNum();
	paint();
	isEnd();
}
function you() {
	if(!isGame) return;
	$("#keys").text("→");
	toRight();
	locations[createLocation()] = createFixedNum();
	paint();
	isEnd();
}
function isEnd() {
	var f = false;
	// 判断是否结束
	if (locations.indexOf(0) == -1) {// 如果数组中不包含0
	// 判断相邻的数是否为相等
	// alert("注意了哦~");
		$("#danger").text("注意了哦~");
		// if(isEndX()){
		if (isEndX() && isEndY()) {
			Over();
		}
	} else {
		$("#danger").text("");
	}
	return f;
}

function Over() {
	clearTimeout(t);
	isGame=false;
	var isOK;
	var isExistPhp = "php/inputgame.php?game=Game2048&score="+score+"&time="+time+"&max="+max;
	$.ajax({
            url:isExistPhp,
            type: "GET",
            dataType: "text",
            success: function(savegame){
				//返回值包括html以及body标签
				isOK=savegame.substring(17,savegame.length-16);
				//不完整
				//alert(isOK);login
				if(isOK=="0") $("#results").text("战绩已保存!");
				if(isOK=="1")
				{
					$("#results").text("未登录无法保存战绩！");
					$('#islogin').show();
				}
				if(isOK=="2") $("#results").text("连接服务器失败！");
            }
    });
	
	$("#display2").text("当前分数: " + score);
	$("#display3").text("用时: " + time +"s");
	$("#display4").text("最大数是: " + max);
	setTimeout("$('#gameover').show()",1000);
	
/*	if (window.confirm("结束了!\n当前分数: " + score + ";\n用时: " + time+ "S;\n最大数是: " + max + "。\n是否开始新的游戏?")) {
		init();
	} else {
		isGame=false;
		//window.close();
	}*/
}

function isEndX() {
	// 判断横向的数组
	// 如果相邻位置的数不相同,就结束
	var f = false;
	var w = new Array();
	for (var j = 0; j < 4; j++) {
		for (var i = 0; i < 4; i++) {
			w[i] = locations[4 * j + i];
		}
		// alert(w);
		f = (w[0] != w[1] && w[1] != w[2] && w[2] != w[3]);// 如果为真,表示相邻的两个数不相等
		if (!f) {
			break;
		}
	}

	return f;
}

function isEndY() {
	// 判断纵向的数组
	// 如果相邻位置的数不相同,就结束
	var f = false;
	var w = new Array();
	for (var j = 0; j < 4; j++) {
		for (var i = 0; i < 4; i++) {
			w[i] = locations[4 * i + j];
		}
		// alert(w);
		f = (w[0] != w[1] && w[1] != w[2] && w[2] != w[3]);// 如果为真,表示相邻的两个数不相等
		if (!f) {
			break;
		}
	}

	return f;
}

function toDown() {
	// 向下
	for (var i = 0; i < 4; i++) {
		// 判断每一行
		var row = [ locations[i + 12], locations[i + 8], locations[i + 4],
				locations[i] ];
		configurationD(i, row);
	}
}

function toRight() {
	// 向右
	for (var i = 0; i < 4; i++) {
		// 判断每一行
		var row = [ locations[i * 4 + 3], locations[i * 4 + 2],
				locations[i * 4 + 1], locations[i * 4] ];
		// alert(i+"\t"+row);
		configurationR(i, row);
	}
}

function toLeft() {
	// 向左
	for (var i = 0; i < 4; i++) {
		// 判断每一行
		var row = [ locations[i * 4], locations[i * 4 + 1],
				locations[i * 4 + 2], locations[i * 4 + 3] ];
		configurationL(i, row);
	}
}

function toUp() {
	// 向上
	for (var i = 0; i < 4; i++) {
		var row = [ locations[i + 0], locations[i + 4], locations[i + 8],
				locations[i + 12] ];
		configurationU(i, row);
	}
}

function configurationD(i, r) {
	makeArray(r);

	for (var j = 0; j < 4; j++) {
		locations[4 * (3 - j) + i] = r[j];
	}
}

function configurationR(i, r) {
	// 向右
	makeArray(r);

	for (var j = 0; j < 4; j++) {
		locations[3 + 4 * i - j] = r[j];
	}
}

function configurationU(i, r) {
	makeArray(r);

	for (var j = 0; j < 4; j++) {
		locations[4 * j + i] = r[j];
	}
}

function configurationL(i, r) {
	makeArray(r);

	for (var j = 0; j < 4; j++) {
		locations[4 * i + j] = r[j];
	}
}

function makeArray(r) {
	if (!isZero(r)) {
		// 把数组中是0往后移动
		for (var m = 0; m < 4; m++) {
			for (var n = 0; n < 3; n++) {
				if (r[n] == 0) {
					r[n] = r[n + 1];
					r[n + 1] = 0;
				}
			}
		}
	}

	for (var m = 0; m < 3; m++) {
		if (r[m] == r[m + 1]) {
			var k = m;
			r[k] += r[k + 1];
			score += r[k];
			while (++k < 3) {
				r[k] = r[k + 1];
			}
			r[3] = 0;
		}
	}

	return r;
}

// 绘制点的位置
function paint() {
	for (var i = 0; i < 16; i++) {
		$("#box" + keys[i]).text((locations[i] == 0) ? "" : locations[i]);
		var index = (locations[i] == 0) ? 0
				: (locations[i].toString(2).length - 1);
		$("#box" + keys[i]).css("background", colors[index]);
		// 选出最大数
		if (locations[i] > max) {
			max = locations[i];
		}
	}
	$("#score").text("总分为 : " + score);
	$("#max").text("当前最大数 : " + max);
}

// 随机生成两个数
function createFixedNum() {
	// 生成2/4;
	// 生成2的概率是0.8
	return Math.random() < 0.8 ? 2 : 4;
}
// 生成位置
function createLocation() {
	// 在空位置中随机生成
	var num = Math.floor(Math.random() * 16);
	// 如果该位置有值,就返回重新生成
	var isOver1=true;
	var isOver2=true;
	for(var i=0;i<15;i++)
	{
	    if(locations[i]==0) isOver1=false;
	}
	for(var i=15;i>=0;i--)
	{
	    if(locations[i]==0) isOver2=false;
	}
	if(isOver1&&isOver2)
	{
	    //Over();//没有可生成的不算结束，只有当没有生成也没有消除的才游戏结束
	    return;
    }
	while (locations[num] != 0) {
		num = Math.floor(Math.random() * 16);
	}
	return num;
}

function isZero(m) {
	return m[0] == 0 && m[1] == 0 && m[2] == 0 && m[3] == 0;
}

// 计时
function showtime() {
	$("#time").text("当前用时 :" + (++time) + " s。");
}

function startGame(){
	$("#gameover").hide();
	init();
}
function clickMore(){
    window.location.href="../cloudmain/game.html"; 
}
function login(){
	window.location.href="../login/login.html?id=Game2048"; 
}


function getCookie(name)//取cookies函数       
{
    var arr = document.cookie.match(new RegExp("(^| )"+name+"=([^;]*)(;|$)"));
     if(arr != null) return (arr[2]); return null;
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
	$('#islogin').hide();
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
});

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


















