<html>
<body>
<?php
include '../../mysql/config.php';
$username=$_POST['username'];
$password=$_POST['password'];
$id=$_GET['id'];
$con = mysqli_connect($connectphp,$dbuserphp,$dbpasswordphp);
if (!$con)
{
  //die('Could not connect: ' . mysql_error());
  header("Location:../register.html?register=false");
}

mysqli_select_db($con,$dbnamephp);

$sql="INSERT INTO UserInfo (username, password, type,status) VALUES('".$username."','".$password."','Normal','1')";
mysqli_query($con,"set names 'utf8'");//写库
if (!mysqli_query($con,$sql))
{
  header("Location:../register.html?register=false");
}
else{
	header("Location:../login.html?register=true&id=" . $id);
}

mysqli_close($con)
?>
</body>
</html>