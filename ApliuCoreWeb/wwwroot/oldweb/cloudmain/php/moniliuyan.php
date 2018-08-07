<html>
<body>
<?php
include '../../mysql/config.php';
$con = mysqli_connect($connectphp,$dbuserphp,$dbpasswordphp);
if (!$con)
  {
  //die('Could not connect: ' . mysql_error());
  }
//likenum floor
mysqli_select_db($con,$dbnamephp);

$result = mysqli_query($con,"select Message.*,UserInfo.type,UserInfo.status from Message left join UserInfo on Message.username=UserInfo.username order by Id asc");

$tablebody="";

while($row = mysqli_fetch_array($result))
{
	//if($row['username']!=""||$row['username']!=null) $tablebody=$tablebody."-DvZg-UqRm-";
	$tablebody = $tablebody . $row['username'] . "-DvZg-UqRm-" . $row['message'] . "-DvZg-UqRm-" . $row['likenum'] . "-DvZg-UqRm-" . $row['floor'] . "-DvZg-UqRm-" . $row['date'] ."-DvZg-UqRm-". $row['Id'] ."-DvZg-UqRm-". $row['type'] ."-DvZg-UqRm-". $row['status'] ."-DvZg-UqRm-";

}
echo $tablebody;
mysqli_close($con);
?>
</body>
</html>