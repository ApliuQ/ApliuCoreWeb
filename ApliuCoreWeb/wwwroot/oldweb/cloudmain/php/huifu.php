<html>
<body>
<?php
include '../../mysql/config.php';
$id=$_REQUEST['id'];
$text=$_REQUEST['text'];
date_default_timezone_set('Asia/Shanghai');
$date=date('Y-m-d H:i',time()+2*60);
$cur_username=$_COOKIE['cur_username'];

$issuccess="1";
if($cur_username!=""||$cur_username!=null)
{
	$con = mysqli_connect($connectphp,$dbuserphp,$dbpasswordphp);
	if (!$con)
	{
	  //die('Could not connect: ' . mysql_error());
	  //header("Location:../message.html?liuyan=false");
	}
	
	mysqli_select_db($con,$dbnamephp);
	//INSERT INTO Floor (messageid,name,huifu,date) VALUES('".$cur_username."','".$message."','1','00001','".$date."')
	$sql="INSERT INTO Floor (messageid,name,huifu,date) VALUES('".$id."','".$cur_username."','".$text."','".$date."')";
	mysqli_query($con,"set names 'utf8'");//写库
	if (mysqli_query($con,$sql))
	{
	  $issuccess=0;
	}
	mysqli_close($con);
}else $issuccess="2";
echo $issuccess;
?>
</body>
</html>