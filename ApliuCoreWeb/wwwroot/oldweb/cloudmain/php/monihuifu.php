<html>
<body>
<?php
include '../../mysql/config.php';
$id=$_GET['id'];
$con = mysqli_connect($connectphp,$dbuserphp,$dbpasswordphp);
if (!$con)
  {
  //die('Could not connect: ' . mysql_error());
  }
//likenum floor
mysqli_select_db($con,$dbnamephp);
$sql="select * from Floor where messageid='" . $id ."' order by Id asc";
$result = mysqli_query($con,$sql);

$tablebody="";

while($row = mysqli_fetch_array($result))
{
	//if($row['username']!=""||$row['username']!=null) $tablebody=$tablebody."-DvZg-UqRm-";
	$tablebody = $tablebody . $row['name'] . "-DvZg-UqRm-" . $row['huifu'] . "-DvZg-UqRm-" . $row['date']."-DvZg-UqRm-";

}
echo $tablebody;
mysqli_close($con);
?>
</body>
</html>