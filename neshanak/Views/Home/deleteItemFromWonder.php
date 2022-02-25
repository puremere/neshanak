<?php

$servername = $_POST["servername"];
include_once ("../../../Mabbdaco.php");
$Mabbdaco = new Mabbdaco($servername);

$device = $Mabbdaco->post("device");
$code = $Mabbdaco->post("code");
$ID = $Mabbdaco->post("id");
$discount = $Mabbdaco->post("discount");
$token = $Mabbdaco->post("token");
$hour = $Mabbdaco->post("hour");

if ($code != md5($device . "ncase8934f49909")) {
    echo $code;
    die("InvalidRequest");
}

$query = "select userTypeID  from mbd_users where token = '$token'";

$q = mysqli_query($Mabbdaco->db_conn, $query) or die(mysqli_error($Mabbdaco->
    db_conn));
while ($row = mysqli_fetch_array($q)) {
    $userTypeID = $row["userTypeID"];
}

if ($userTypeID != "1") {
    die("unathorized access");
}


$query = "select productprice, discount from `mbd_products0` where `ID`= $ID";
$row = $Mabbdaco -> getRow($query);
$discount = (int)$row[discount]; 
$pp = ( (int)$row[productprice] - ((int)$row[productprice] * (int)$discount)/100);

$time = time() + ($hour*3600);
$Lastquery = "update `mbd_products0` set wonderTime =0 , wonderDiscount = 0, PriceNow =$pp where `ID`= $ID ";

$q = mysqli_query($Mabbdaco->db_conn, $Lastquery) or die(mysqli_error($Mabbdaco->
    db_conn));


$data["status"] = 200;
$data["message"] = "";
echo (json_encode($data));








?>