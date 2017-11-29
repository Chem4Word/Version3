<?php
// -----------------------
// Works for IPV4 on Azure
// -----------------------

// client-ip.php : Demo script by nixCraft <www.cyberciti.biz>
// get an IP address
//$ip = $_SERVER['REMOTE_ADDR'];
$ip = get_client_ip_server();
// display it back
//echo "<h2>Client IP Demo</h2>";
echo "Your IP address : " . $ip . "<br/>";
//echo "<br>Your hostname : ". gethostbyaddr($ip) ;

//echo "HTTP_CLIENT_IP " . $_SERVER['HTTP_CLIENT_IP'] . "<br/>" ;
//echo "HTTP_X_FORWARDED_FOR " . $_SERVER['HTTP_X_FORWARDED_FOR'] . "<br/>";
//echo "HTTP_X_FORWARDED " . $_SERVER['HTTP_X_FORWARDED'] . "<br/>";
//echo "HTTP_FORWARDED_FOR " . $_SERVER['HTTP_FORWARDED_FOR'] . "<br/>";
//echo "HTTP_FORWARDED " . $_SERVER['HTTP_FORWARDED'] . "<br/>";
//echo "REMOTE_ADDR " . $_SERVER['REMOTE_ADDR'] . "<br/>";

//echo remove_port($_SERVER['HTTP_X_FORWARDED_FOR']);

// Function to get the client ip address
function get_client_ip_server() {
    $ipaddress = '';
    if ($_SERVER['HTTP_CLIENT_IP'])
        $ipaddress = remove_port($_SERVER['HTTP_CLIENT_IP']);
    else if($_SERVER['HTTP_X_FORWARDED_FOR'])
        $ipaddress = remove_port($_SERVER['HTTP_X_FORWARDED_FOR']);
    else if($_SERVER['HTTP_X_FORWARDED'])
        $ipaddress = remove_port($_SERVER['HTTP_X_FORWARDED']);
    else if($_SERVER['HTTP_FORWARDED_FOR'])
        $ipaddress = remove_port($_SERVER['HTTP_FORWARDED_FOR']);
    else if($_SERVER['HTTP_FORWARDED'])
        $ipaddress = remove_port($_SERVER['HTTP_FORWARDED']);
    else if($_SERVER['REMOTE_ADDR'])
        $ipaddress = remove_port($_SERVER['REMOTE_ADDR']);
    else
        $ipaddress = '0.0.0.0';
 
    return $ipaddress;
}

function remove_port($ipaddress) {
	$parts = explode(':', $ipaddress);
	//echo $ipaddress;
	return $parts[0];
	//return $ipaddress;
}
?>