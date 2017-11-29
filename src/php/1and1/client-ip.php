<?php
// -------------------------------------
// Works for IPV4 and IPV6 on 1and1 Host
// -------------------------------------

// client-ip.php : Demo script original by nixCraft <www.cyberciti.biz>
// get an IP address
//$ip = $_SERVER['REMOTE_ADDR'];
$ip = get_client_ip_server();
// display it back
//echo "<h2>Client IP Demo</h2>";
echo "Your IP address : " . $ip;
//echo "<br>Your hostname : ". gethostbyaddr($ip) ;

// Function to get the client ip address
function get_client_ip_server() {
    $ipaddress = '';
    if ($_SERVER['HTTP_CLIENT_IP'])
        $ipaddress = $_SERVER['HTTP_CLIENT_IP'];
    else if($_SERVER['HTTP_X_FORWARDED_FOR'])
        $ipaddress = $_SERVER['HTTP_X_FORWARDED_FOR'];
    else if($_SERVER['HTTP_X_FORWARDED'])
        $ipaddress = $_SERVER['HTTP_X_FORWARDED'];
    else if($_SERVER['HTTP_FORWARDED_FOR'])
        $ipaddress = $_SERVER['HTTP_FORWARDED_FOR'];
    else if($_SERVER['HTTP_FORWARDED'])
        $ipaddress = $_SERVER['HTTP_FORWARDED'];
    else if($_SERVER['REMOTE_ADDR'])
        $ipaddress = $_SERVER['REMOTE_ADDR'];
    else
        $ipaddress = 'UNKNOWN';
 
    return $ipaddress;
}
?>