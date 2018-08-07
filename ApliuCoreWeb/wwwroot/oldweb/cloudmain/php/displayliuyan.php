<html>
<body>
<?php
include '../../mysql/config.php';
$con = mysqli_connect($connectphp,$dbuserphp,$dbpasswordphp);
if (!$con)
  {
  //die('Could not connect: ' . mysql_error());
  }

mysqli_select_db($con,$dbnamephp);

$result = mysqli_query($con,"select Message.*,UserInfo.type,UserInfo.status from Message left join UserInfo on Message.username=UserInfo.username order by Message.date");

$tableone='<div class="job_right"><table border="1" height="150px" width="100%" rules="cols" bordercolor="#CCCCCC" cellpadding="0" cellspacing="0"><tr><td align="center" rowspan="2" width="100px">';
$tabletwo='</td><td align="left">';
$tablethree='</td></tr> <tr height="20px"><td align="right">';
$tablefour='</td></tr></table></div>';
#$tablebody=$tableone . "A" . $tabletwo . "B" . $tablethree . "C" . $tablefour;*/
$tablebody="";
$array = array();
$datestr="";
while($row = mysqli_fetch_array($result))
{
	if($row['type']=="admin")//加红色
	{
		//<font color="#FF0000"><b>文字</b></font>
		$tablebody =$tableone . "<font color='#FF0000'><b>" . $row['username'] ."</b></font>" . $tabletwo ."<font color='#FF0000'><b>" . $row['message']."</b></font>" . $tablethree."<font color='#FF0000'><b>管理员身份      " . $row['date']."</b></font>" . $tablefour .$tablebody;
	}else{
		$tablebody =$tableone  . $row['username'] . $tabletwo . $row['message'] . $tablethree . $row['date'] . $tablefour .$tablebody;
	}
	/*
	$item = array();
	$item['username'] = $row['username'];
	$item['message'] = $row['message'];
	$item['date'] = $row['date'];
	$array[] = $item;
	// 销毁单个变量
	unset ($item);
	
	$datestr = $datestr . $row['username'] . "|-|" . $row['message'] . "|-|" . $row['date'] . "|-|";
	*/
}
/*foreach($array as $k=>$val) //意思是for $book each $value( as )    
if( is_array($val) ) foreach( $val as $value) echo $value.'';    
else echo $k.'=>'.$val.''; */
echo $tablebody;
#echo urlencode(json_encode($array));
#echo $datestr;
mysqli_close($con);
?>
</body>
</html>