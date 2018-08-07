<html>
<body>
<?php
include '../../mysql/config.php';
$username=$_POST['username'];
$password=$_POST['password'];
$id=$_GET['id'];
//Game2048
$boollogin=false;
$con = mysqli_connect($connectphp,$dbuserphp,$dbpasswordphp);
if (!$con)
{
  //die('Could not connect: ' . mysql_error());
  header("Location:../login.html?login=false");
}

mysqli_select_db($con,$dbnamephp);
mysqli_query($con,"set character set 'utf8'");//读库
$result = mysqli_query($con,"SELECT * FROM UserInfo");

while($row = mysqli_fetch_array($result))
  {
	  if($row['status']=="1")
	  {
		  if($username==$row['username'])
		  {
			  if($password==$row['password'])
			  {
				  //函数原型:int setcookie(string name,string value,int expire,string path,string domain,int secure)
				  setcookie("cur_username",$username,0,"/");
				  //if($id==""||$id==null) header("Location:../../index.html");
				  switch ($id)
					{
					  case "message":header("Location:../../cloudmain/message.html");
					  break;
					  case "about":header("Location:../../cloudmain/about.html");
					  break;
					  case "game":header("Location:../../cloudmain/game.html");
					  break;
					  case "gift":header("Location:../../cloudmain/gift.html");
					  break;
					  case "keeplove":header("Location:../../cloudmain/keeplove.html");
					  break;
					  case "licheng":header("Location:../../cloudmain/licheng.html");
					  break;
					  case "other":header("Location:../../cloudmain/other.html");
					  break;
					  case "xiuenai":header("Location:../../cloudmain/xiuenai.html");
					  break;
					  case "Game2048":header("Location:../../game2048/index.html");
					  break;
					  case "Flappy2048":header("Location:../../Flappy2048/index.html");
					  break;
					  case "pins":header("Location:../../pins/index.html");
					  break;
					default:header("Location:../../index.html");break;
					}
				$boollogin=true;
			  }
		  }
	  }
  }
  if(!$boollogin)
  {
	  header("Location:../login.html?login=false");
  }
mysqli_close($con);
?>
</body>
</html>