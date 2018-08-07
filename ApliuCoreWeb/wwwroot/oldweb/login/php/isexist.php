<html>
<body>
<?php
include '../../mysql/config.php';
$isExist=$_GET['username'];
$boollogin=false;
$con = mysqli_connect($connectphp,$dbuserphp,$dbpasswordphp);
if (!$con)
{
  //die('Could not connect: ' . mysql_error());
}

mysqli_select_db($con,$dbnamephp);

$result = mysqli_query($con,"SELECT * FROM UserInfo");

while($row = mysqli_fetch_array($result))
{
	if($isExist==$row['username'])
	{
		$boollogin=true;
	}
}
echo $boollogin;
mysqli_close($con);
?>
</body>
</html>