<?php  

$ts = time();
header("Content-Type:text/html;charset=utf-8");

//print_r("<p>  ".$sendJsonData."   </p>");

$fruits = array("seller=Hagrid", "service=get_goods_image", "timestamp=".$ts, "user=user", "pass=666666");
sort($fruits);
$code = "";
$KEY = "K632F764133F45be";
$index = 1;
foreach ($fruits as $key => $val) {
    echo "fruits[" . $key . "] = " . $val . "\n";
    $code = $code."&".$val;

}
 $code = substr($code, 1, strlen($code)); 
print_r("<br><br>".$code.$KEY);
$sign = md5($code.$KEY);

print_r("<br><br>".$sign);
$arrayData = array("data"=>array("seller" => "Hagrid", "service" => "get_goods_image",
	"timestamp"=>$ts,"user"=>"user","pass"=>"666666","goods_ids"=> array(187098 ,187099),
	"sign"=>$sign,"img_type"=>1));
$sendJsonData = json_encode($arrayData);

//print_r($sendJsonData);183.238.203.72/OMS
//print_r($data);http://localhost:50923/OMSService.svc
$ch = curl_init('http://localhost:50923/OMSService.svc/exec');
curl_setopt($ch, CURLOPT_CUSTOMREQUEST, "POST");
curl_setopt($ch, CURLOPT_POSTFIELDS, $sendJsonData);
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
curl_setopt($ch, CURLOPT_HTTPHEADER, array(
		'Content-Type: application/json',
		'Content-Length: '.strlen($sendJsonData))
);
//var_dump(curl_exec($ch));
$cexecute=curl_exec($ch); 
$httpCode = curl_getinfo($ch,CURLINFO_HTTP_CODE); 
print_r("<br><br>".$httpCode);
curl_close($ch); 

//$cexecute = mb_convert_encoding($cexecute,'utf-8');　
print_r($cexecute);

$result = json_decode($cexecute,true);
var_dump($result);


print_r($result['categorys']);
var_dump(json_decode($result['categorys'],true));
 //$json1 = '{"a":1,"b":2,"c":3,"d":4,"e":5}';
  $json1='{"ar":[{"c_id":1046,"c_title":"\u5168\u90E8 ","c_pid":1046,"c_level":"0"}]}';
print_r($json1);
var_dump(json_decode($json1,true));
?>