<html>
<body>
<?php
include '../../mysql/config.php';
$id=$_GET['id'];
$issuccess="1";
$sql="";
$con = mysqli_connect($connectphp,$dbuserphp,$dbpasswordphp);
if (!$con)
{
  //die('Could not connect: ' . mysql_error());
  //header("Location:../message.html?liuyan=false");
}

mysqli_select_db($con,$dbnamephp);
//INSERT INTO Message (username,message,likenum,floor,date) VALUES('".$cur_username."','".$message."','1','00001','".$date."')
$sql="UPDATE Message set likenum=likenum+1 where id='". $id ."'";

if (mysqli_query($con,$sql))
{
  $issuccess=0;
}
echo $issuccess;
mysqli_close($con)
?>
</body>
</html>