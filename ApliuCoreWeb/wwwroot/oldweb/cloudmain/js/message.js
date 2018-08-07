function Scroll(n)
{
    temp=n;
    document.getElementById('News').scrollTop=document.getElementById('News').scrollTop+temp;
    if (temp==0) return;
    setTimeout("Scroll(temp)",20);
}
function zan(event){
	var id=ClientBrowser()[0] == "Firefox" ? event.target.id : event.srcElement.id;
	var messageid=id.substring(3,id.length);
	
	var addzan = "php/zan.php?id="+messageid;
	$.ajax({
            url:addzan,
            type: "GET",
            dataType: "text",
            success: function(save){
				//返回值包括html以及body标签
				var iszan=save.substring(17,save.length-16);
				//alert(iszan);
				if(iszan=="0"){
					/*
					var StrText=$("#"+id).text().toString();
					//str.substring(str.indexOf("("),str.indexOf(")"))
					
					//定义正则表达式对象，\表示转义字符,原点表示任意字符，+表示出现次数至少1次，igm表示忽略大小写，且全局匹配
					//pattern =new RegExp("\\((.| )+?\\)","igm");
					//取出匹配正则表达式的内容
					//$("#"+id).text().match(pattern).toString();//包括小括号
					
					var IntzanNum=Number(StrText.substring(StrText.indexOf("(")+1,StrText.indexOf(")")))+1;
					$("#"+id).text("赞("+IntzanNum+")");
					//alert(IntzanNum);*/
					if(ClientBrowser()[0]=="IE") alert("由于IE兼容性问题，所以点赞成功，但是刷新却没反应，需要关闭浏览器（只关闭当前页面或者刷新都没用哦），再打开此页面才会看到效果。");
					window.location.reload();
				}
				else alert("点赞失败");
            }
    });
	//alert(messageid);
}
function huifu(event){
	var id=ClientBrowser()[0] == "Firefox" ? event.target.id : event.srcElement.id;
	var messageid=id.substring(5,id.length);
	
	var textid="text"+messageid;
	var huifutext=document.getElementById(textid).value;
	if(huifutext==""||huifutext==null){ alert("请输入回复内容");return;}
	
	var addhuifu = "php/huifu.php";
	$.ajax({
            url:addhuifu,
            type: "POST",
			data:{id:messageid,text:huifutext},
            dataType: "html",
            success: function(save){
				//返回值包括html以及body标签
				var ishuifu=save.substring(17,save.length-16);
				if(ishuifu=="0"){
					//Getdatamessage();
				if(ClientBrowser()[0]=="IE") alert("由于IE兼容性问题，所以回复成功，但是刷新却没反应，需要关闭浏览器（只关闭当前页面或者刷新都没用哦），再打开此页面才会看到效果。");
					window.location.reload();
				}
				if(ishuifu=="2") alert("未登录无法回复");
				if(ishuifu=="1") alert("回复失败");
            }
    });
}
function Getdatamessage(){
	$.ajax({
            url:"php/moniliuyan.php",
            type: "GET",
            dataType: "text",
            success: function(tablebody){
                //alert(tablebody);//$a的值
				//alert(inhtml.substring(17,inhtml.length-16));
			var liuyandata=tablebody.substring(17,tablebody.length-16);
			var Arrayliuyan= new Array(); //定义一数组 
			Arrayliuyan=liuyandata.split("-DvZg-UqRm-"); //字符分割
			var i=0; 						
			
			var no1="<div style='border-style:solid; border-width:1px; border-color:#CCC;width:358px;'><div style='width:338px; background:rgba(255,255,255,1); border-radius: 2px;padding:10px;'><div style=' height:50px;'><div style='display:inline;float:left;'> <a href='javascript:;' title='头像(敬请期待)' target='_blank'><img style=' height:50px; border-radius:5px;' src='img/message/touxiang.jpg' /></a> </div><div> <div>&nbsp; <a style=' font-size:18px; text-decoration:none;color:#999;' target='_blank' href='javascript:;'>";
			var no2="(#Name)";
			var no3="</a></div><div>&nbsp; <span>";
			var no4="(#Time)";
			var no5="</span> <span><a style='color:#999;' href='javascript:;'>";
			var no6="赞(0)";
			var no7="</a></span> </div></div></div> <div align='left' style=' margin-top:5px;'>";
			var no8="(#messagemain)";
			var no9="</div></div><div style='width:338px; background:rgba(255,255,255,0.6); border-radius: 2px;padding:10px;'>";
			//floor
			var no16="<div style='display:inline;float:left;'><a href='javascript:;' target='_blank'><img style='height:35px; border-radius:4px;' src='img/message/touxiang.jpg' /></a> </div><div> &nbsp;<a target='_blank' href='javascript:;' style='text-decoration:none;'>";
			var no10="(#回复NAME)";
			var no11="</a>：<span>";
			var no12="(#回复Message)";
			var no13="</span><div> &nbsp;<span>";
			var no14="(#回复Time)";
			var no15="</span> </div></div>";
			//over
			var no17="<div style=' margin-top:10px;'><input style='display:inline;float:left; width:86%;' type='text' placeholder='回复'></div> <a style='margin-left:10px;border-style:solid; border-width:1px; border-color:#CCC; padding:3px; font-size:13px;text-decoration:none; color:#999;' href='javascript:;' >回复 </a> </div></div>";
			
			
			
			for (i=0;i<Arrayliuyan.length-1;i=i+8) 
			{
				//['username'] ['message'] ['likenum'] ['floor'] ['date']  $row['Id']   $row['type']  $row['status'];
				var allbody="";
				if(Arrayliuyan[i+7]=="0") continue;//用户被禁，status字段为0
				
				if(Arrayliuyan[i+6]=="vip") no2="<font face='Times New Roman, Times, serif' color='#FF0000'>"+Arrayliuyan[i]+"</font>";//<font color="#FF0000">(#Name)</font>//VIP用户，type字段为vip
				else{
					if(Arrayliuyan[i+6]=="Main") no2="<font face='Times New Roman, Times, serif' color='#F0F'>"+Arrayliuyan[i]+"</font>";//<font color="#FF0000">(#Name)</font>//VIP用户，type字段为vip
					else{
						if(Arrayliuyan[i+6]=="Admin") no2="<font face='Times New Roman, Times, serif' color='#00F'>"+Arrayliuyan[i]+"</font>";//<font color="#FF0000">(#Name)</font>//VIP用户，type字段为vip
						else no2="<font face='Times New Roman, Times, serif' >"+Arrayliuyan[i]+"</font>";
					}
				}
				no4=Arrayliuyan[i+4];
				
				//创建ID以便插入回复
				no5="</span> <span><a style='color:#999;' onclick='zan()' id='zan"+Arrayliuyan[i+5]+"' href='javascript:;'>";
				no17="<div style=' margin-top:10px;'><input style='display:inline;float:left; width:86%;' type='text' placeholder='回复' id='text"+Arrayliuyan[i+5]+"'></div> <a style='margin-left:10px;border-style:solid; border-width:1px; border-color:#CCC; padding:3px; font-size:13px;text-decoration:none; color:#999;' onclick='huifu()' id='huifu"+Arrayliuyan[i+5]+"' href='javascript:;' >回复 </a> </div></div>";
							
				no6="赞("+Arrayliuyan[i+2]+")";
				
				//var strliuyanmain="    "+Arrayliuyan[i+1];
				//strliuyanmain=strliuyanmain.replace(/(.{28})/g,'$1<br />');
				no8="&nbsp;&nbsp;&nbsp;&nbsp;"+Arrayliuyan[i+1];
				
				allbody=allbody+no1+no2+no3+no4+no5+no6+no7+no8+no9;

				$.ajax({
					url:"php/monihuifu.php?id="+Arrayliuyan[i+5],
					type: "GET",
					dataType: "text",
					async:false,//默认设置下，所有请求均为异步请求。如果需要发送同步请求，请将此选项设置为 false。注意，同步请求将锁住浏览器，用户其它操作必须等待请求完成才可以执行。
					success: function(monihuifu){
						//alert(tablebody);
						var huifudata=monihuifu.substring(17,monihuifu.length-16);
						var Arrayhuifu= new Array(); //定义一数组 
						Arrayhuifu=huifudata.split("-DvZg-UqRm-"); //字符分割
						//$row['name']   $row['huifu']   $row['date']
						var j=0;
						for (j=0;j<Arrayhuifu.length-1;j=j+3) 
						{
							if(Arrayhuifu[j]==""||Arrayhuifu[j]==null) break;
							
							if(Arrayliuyan[i+6]=="vip") no10="<font face='Times New Roman, Times, serif' color='#FF0000'>"+Arrayhuifu[j]+"</font>";//<font color="#FF0000">(#Name)</font>//VIP用户，type字段为vip
							else{
								if(Arrayliuyan[i+6]=="Main") no10="<font face='Times New Roman, Times, serif' color='#F0F'>"+Arrayhuifu[j]+"</font>";//<font color="#FF0000">(#Name)</font>//VIP用户，type字段为vip
								else{
									if(Arrayliuyan[i+6]=="Admin") no10="<font face='Times New Roman, Times, serif' color='#00F'>"+Arrayhuifu[j]+"</font>";//<font color="#FF0000">(#Name)</font>//VIP用户，type字段为vip
									else no10="<font face='Times New Roman, Times, serif' color='#999'>"+Arrayhuifu[j]+"</font>";
								}
							}
							//no10=Arrayhuifu[j];
							no12=Arrayhuifu[j+1];
							no14=Arrayhuifu[j+2];
							allbody=allbody+no16+no10+no11+no12+no13+no14+no15;
						}
						$("#tablebody").after(allbody+no17);
					}
				})
			}
			//$("#tablebody").after(no1+no2+no3+no4+no5+no6+no7+no8+no9+no16+no10+no11+no12+no13+no14+no15+no16+no10+no11+no12+no13+no14+no15+no17);
        }
    })
}

function allmessage(){
	$.ajax({
            url:"php/allmessage.php",
            type: "GET",
            dataType: "text",
            success: function(tablebody){
				//返回值包括html以及body标签
				
				var strall=tablebody.substring(17,tablebody.length-16);
				var Arrayall= new Array(); //定义一数组 
				Arrayall=strall.split("-DvZg1UqRm-"); //字符分割
				
				var Arrayliuyan= new Array(); //定义一数组 
				Arrayliuyan=Arrayall[0].split("-DvZg-UqRm-"); //字符分割
				
				var Arrayhuifu= new Array(); //定义一数组 
				Arrayhuifu=Arrayall[1].split("-DvZg-UqRm-"); //字符分割
				
				var i=0; 						
				
				var no1="<div style='border-style:solid; border-width:1px; border-color:#CCC;width:358px;'><div style='width:338px; background:rgba(255,255,255,1); border-radius: 2px;padding:10px;'><div style=' height:50px;'><div style='display:inline;float:left;'> <a href='javascript:;' title='头像(敬请期待)' target='_blank'><img style=' height:50px; border-radius:5px;' src='img/message/touxiang.jpg' /></a> </div><div> <div>&nbsp; <a style=' font-size:18px; text-decoration:none;color:#999;' target='_blank' href='javascript:;'>";
				var no2="(#Name)";
				var no3="</a></div><div>&nbsp; <span>";
				var no4="(#Time)";
				var no5="</span> <span><a style='color:#999;' href='javascript:;'>";
				var no6="赞(0)";
				var no7="</a></span> </div></div></div> <div align='left' style=' margin-top:5px;'>";
				var no8="(#messagemain)";
				var no9="</div></div><div style='width:338px; background:rgba(255,255,255,0.6); border-radius: 2px;padding:10px;'>";
				//floor
				var no16="<div style='display:inline;float:left;'><a href='javascript:;' target='_blank'><img style='height:35px; border-radius:4px;' src='img/message/touxiang.jpg' /></a> </div><div> &nbsp;<a target='_blank' href='javascript:;' style='text-decoration:none;'>";
				var no10="(#回复NAME)";
				var no11="</a>：<span>";
				var no12="(#回复Message)";
				var no13="</span><div> &nbsp;<span>";
				var no14="(#回复Time)";
				var no15="</span> </div></div>";
				//over
				var no17="<div style=' margin-top:10px;'><input style='display:inline;float:left; width:85%;' type='text' placeholder='回复'></div> <a style='margin-left:10px;border-style:solid; border-width:1px; border-color:#CCC; padding:3px; font-size:13px;text-decoration:none; color:#999;' href='javascript:;' >回复 </a> </div></div>";
				
				
				
				for (i=0;i<Arrayliuyan.length-1;i=i+7) 
				{
					//['username'] ['message'] ['likenum'] ['floor']    ['date']  $row['Id']   $row['type']  $row['status'];
					// ['Id']    ['username']  ['message']  ['likenum']  ['floor']  ['date']  ['type'] 
					var allbody="";
					
					if(Arrayliuyan[i+6]=="vip") no2="<font face='Times New Roman, Times, serif' color='#FF0000'>"+Arrayliuyan[i+1]+"</font>";
					else{
						if(Arrayliuyan[i+6]=="Main") no2="<font face='Times New Roman, Times, serif' color='#F0F'>"+Arrayliuyan[i+1]+"</font>";
						else{
							if(Arrayliuyan[i+6]=="Admin") no2="<font face='Times New Roman, Times, serif' color='#00F'>"+Arrayliuyan[i+1]+"</font>";
							else no2="<font face='Times New Roman, Times, serif' >"+Arrayliuyan[i+1]+"</font>";
						}
					}
					no4=Arrayliuyan[i+5];
					
					//创建ID以便插入回复
					no5="</span> <span><a style='color:#999;' onclick='zan(event)' id='zan"+Arrayliuyan[i]+"' href='javascript:;'>";
					no17="<div style=' margin-top:10px;'><input style='display:inline;float:left; width:84%;' type='text' placeholder='回复' id='text"+Arrayliuyan[i]+"' /></div> <a style='margin-left:10px;border-style:solid; border-width:1px; border-color:#CCC; padding:3px; font-size:13px;text-decoration:none; color:#999;' onclick='huifu(event)' id='huifu"+Arrayliuyan[i]+"' href='javascript:;' >回复 </a> </div></div>";
								
					no6="赞("+Arrayliuyan[i+3]+")";
					

					no8="&nbsp;&nbsp;&nbsp;&nbsp;"+Arrayliuyan[i+2];
					
					allbody=allbody+no1+no2+no3+no4+no5+no6+no7+no8+no9;
	

							var j=0;
							for (j=0;j<Arrayhuifu.length-1;j=j+5) 
							{
								// ['messageid']  ['name']  ['huifu']  ['date']  ['type']
								if(Arrayhuifu[j]==""||Arrayhuifu[j]==null) break;
								if(Arrayhuifu[j]!=Arrayliuyan[i]) continue;
								
								if(Arrayhuifu[j+4]=="vip") no10="<font face='Times New Roman, Times, serif' color='#FF0000'>"+Arrayhuifu[j+1]+"</font>";
								else{
									if(Arrayhuifu[j+4]=="Main") no10="<font face='Times New Roman, Times, serif' color='#F0F'>"+Arrayhuifu[j+1]+"</font>";
									else{
										if(Arrayhuifu[j+4]=="Admin") no10="<font face='Times New Roman, Times, serif' color='#00F'>"+Arrayhuifu[j+1]+"</font>";
										else no10="<font face='Times New Roman, Times, serif' color='#999'>"+Arrayhuifu[j+1]+"</font>";
									}
								}
								//no10=Arrayhuifu[j];
								no12=Arrayhuifu[j+2];
								no14=Arrayhuifu[j+3];
								allbody=allbody+no16+no10+no11+no12+no13+no14+no15;
							}
							$("#tablebody").after(allbody+no17);
				}
			}	
   	})

}

		function pageup()
		{
			document.getElementById('News').scrollTop=document.getElementById('News').scrollTop-400;
		}
		function pagedown()
		{
			document.getElementById('News').scrollTop=document.getElementById('News').scrollTop+400;
		}









