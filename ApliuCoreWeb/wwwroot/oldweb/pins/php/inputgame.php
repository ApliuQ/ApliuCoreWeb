<html>
<body>
<?php
include '../../mysql/config.php';
$score=$_GET['score'];
$game=$_GET['game'];
$time=$_GET['time'];
$max=$_GET['max'];
date_default_timezone_set('Asia/Shanghai');
$date=date('Y-m-d H:i',time()+2*60);
$cur_username=$_COOKIE['cur_username'];
$savegame="2";
if($cur_username!=null||$cur_username!="")
{
	$con = mysqli_connect($connectphp,$dbuserphp,$dbpasswordphp);
	if (!$con)
	{
	  //die('Could not connect: ' . mysql_error());
	  //header("Location:../message.html?liuyan=false");
	}
	mysqli_select_db($con,$dbnamephp);
	
	$sqlselect="select * from Gamedata where username='". $cur_username ."' and game='pins' order by data desc limit 1";
	mysqli_query($con,"set names 'utf8'");//写库
	$result = mysqli_query($con,$sqlselect);
	while($row = mysqli_fetch_array($result))
	{
		if($row['score']==$score&&$row['data']==$date){
			$savegame="3";
		}else{
			$sql="INSERT INTO Gamedata (username,game,score,max,usetime,data) VALUES('".$cur_username."','".$game."','".$score."','".$max."','".$time."','".$date."')";
			if (mysqli_query($con,$sql)) $savegame="0";
			 
		}
	}
	mysqli_close($con);
}else $savegame="1";
echo $savegame;
?>
</body>
</html>