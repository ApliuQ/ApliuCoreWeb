<html>
<body>
<?php
include '../../mysql/config.php';
$sql="";
$con = mysqli_connect($connectphp,$dbuserphp,$dbpasswordphp);
if (!$con)
{
}

mysqli_select_db($con,$dbnamephp);
$sql="UPDATE Other set count=count+1 where id='1'";

if (mysqli_query($con,$sql))
{
}
mysqli_close($con)
?>
</body>
</html>