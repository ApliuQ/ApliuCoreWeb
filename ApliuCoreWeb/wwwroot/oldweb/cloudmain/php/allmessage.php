<html>
<body>
<?php
include '../../mysql/config.php';
$con = mysqli_connect($connectphp,$dbuserphp,$dbpasswordphp);
if (!$con)
{
}
mysqli_select_db($con,$dbnamephp);
mysqli_query($con,"set character set 'utf8'");//读库
$result = mysqli_query($con,"select Message.*,UserInfo.type from Message left join UserInfo on Message.username=UserInfo.username and UserInfo.status=1 where Message.username !='' and Message.username is not null order by Message.date");
while($row = mysqli_fetch_array($result))
{
	$tablebody = $tablebody . $row['Id'] ."-DvZg-UqRm-". $row['username'] . "-DvZg-UqRm-" . $row['message'] . "-DvZg-UqRm-" . $row['likenum'] . "-DvZg-UqRm-" . $row['floor'] . "-DvZg-UqRm-" . $row['date'] ."-DvZg-UqRm-". $row['type'] ."-DvZg-UqRm-";
}

$tablebody=$tablebody . "-DvZg1UqRm-";

$result = mysqli_query($con,"select Floor.*,UserInfo.type from Floor left join UserInfo on Floor.name=UserInfo.username and UserInfo.status=1 where Floor.name != '' and Floor.name is not null order by messageid");
while($row = mysqli_fetch_array($result))
{
	$tablebody = $tablebody . $row['messageid'] . "-DvZg-UqRm-" . $row['name'] . "-DvZg-UqRm-" . $row['huifu'] . "-DvZg-UqRm-" . $row['date']."-DvZg-UqRm-" . $row['type']."-DvZg-UqRm-";
}
echo $tablebody;
mysqli_close($con);
?>
</body>
</html>