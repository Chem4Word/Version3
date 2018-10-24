<?php
// -----------------------
// Works for IPV4 on Azure
// -----------------------

// client-ip.php : Demo script by nixCraft <www.cyberciti.biz>

// Get IP Address
$ip = get_client_ip_server();
echo "Your IP address : " . $ip . "<br/>";
echo "UTC Date : " . date("Y,m,d,H,i,s", time() - date("Z"));

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
	return $parts[0];
}
?>