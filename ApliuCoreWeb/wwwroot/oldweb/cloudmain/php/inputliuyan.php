<html>
<body>
<?php
include '../../mysql/config.php';
$message=$_POST['message'];
date_default_timezone_set('Asia/Shanghai');
$date=date('Y-m-d H:i',time()+2*60);
$cur_username=$_COOKIE['cur_username'];

$con = mysqli_connect($connectphp,$dbuserphp,$dbpasswordphp);
if (!$con)
{
  //die('Could not connect: ' . mysql_error());
  header("Location:../message.html?liuyan=false");
}

mysqli_select_db($con,$dbnamephp);
//INSERT INTO Message (username,message,likenum,floor,date) VALUES('".$cur_username."','".$message."','1','00001','".$date."')
$sql="INSERT INTO Message (username,message,likenum,floor,date) VALUES('".$cur_username."','".$message."','0','00001','".$date."')";
mysqli_query($con,"set names 'utf8'");//写库
if (!mysqli_query($con,$sql))
{
  header("Location:../message.html?liuyan=false");
}
else{
	header("Location:../message.html");
}

mysqli_close($con)
?>
</body>
</html>