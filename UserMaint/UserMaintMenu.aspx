<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserMaintMenu.aspx.cs" Inherits="UserMaintMenu" %>

<?xml version="1.0" encoding="UTF-8"?><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="content-type" content="application/xhtml+xml;charset=UTF-8" /><title>Left Menu</title>
<link href="../stylesheet.css" rel="stylesheet" type="text/css" />

<script type="text/javascript" language="Javascript">
<!--
var bV=parseInt(navigator.appVersion);
NS4=(document.layers) ? true : false;
IE4=((document.all)&&(bV>=4)) ? true : false;
NS6=(document.getElementsByTagName) ? true : false;

can_display = (NS4 || IE4 || NS6) ? true : false;

bShow = false;

// alert(navigator.appVersion+"- "+bV+" - "+NS4+" - "+IE4+" - "+NS6+" - "+can_display);
// ie 5.5: 4 - f - t - t - t
// ns 4.7: 4 - t - f - f - t
// ns 6.2: 5 - f - f - t - t

function expandIt()
{
    return
}
function expandAll(){return}
//-->

function tutup(){
 cfm=confirm("Anda Ingin keluar daripada Sistem ini?")
 
 if (cfm)
   window.top.close();
 else
   return false  
 
}
</script>
<script type="text/javascript" language="JavaScript" src="../jslib/usr_menu1.js"></script>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1"/>
<meta http-equiv="X-UA-Compatible" content="IE=Edge"/>
</head>
<body class="menu">
    <asp:Label ID="lblMenu" runat="server" Text="Label" ></asp:Label>
</body>
</html>