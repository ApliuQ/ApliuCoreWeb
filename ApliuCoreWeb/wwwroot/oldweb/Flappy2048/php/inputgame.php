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
	
	mysqli_select_db( $con,$dbnamephp);
	//INSERT INTO Gamedata (username,game,score,max,usetime,data) VALUES('".$cur_username."','".$game."','".$score."','".$max."','".$time."','".$date."')
	$sql="INSERT INTO Gamedata (username,game,score,max,usetime,data) VALUES('".$cur_username."','".$game."','".$score."','".$max."','".$time."','".$date."')";
	mysqli_query($con,"set names 'utf8'");//写库
	if (!mysqli_query($con,$sql))
	{
	  //header("Location:../message.html?liuyan=false");
	}
	else{
		$savegame="0";
		//header("Location:../message.html");
	}
	mysqli_close($con);
}else $savegame="1";
echo $savegame;
?>
</body>
</html>