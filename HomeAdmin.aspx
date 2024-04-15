<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HomeAdmin.aspx.cs" Inherits="HomeAdmin" %>

<?xml version="1.0" encoding="UTF-8"?><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="content-type" content="application/xhtml+xml;charset=UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=Edge"/>
    <title>Home</title>
    <meta charset="iso-8859-1"/>
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<link rel="stylesheet" href="styles/layout.css" type="text/css" media="all"/>
<link rel="stylesheet" href="styles/mediaqueries.css" type="text/css" media="all"/>
<link rel="stylesheet" href="./stylesheet.css" type="text/css"/>
<script src="scripts/jquery.1.9.0.min.js"></script>
<script src="scripts/jquery-mobilemenu.min.js"></script>
    <%--<script src="../webfunction/jquery.1.8.3.min.js" type="text/javascript"></script>--%>
    <script src="../webfunction/jquery-2.1.4.min.js" type="text/javascript"></script>

<!--[if lt IE 9]>
<link rel="stylesheet" href="styles/ie.css" type="text/css" media="all">
<script src="scripts/ie/css3-mediaqueries.min.js"></script>
<script src="scripts/ie/ie9.js"></script>
<script src="scripts/ie/html5shiv.min.js"></script>
<![endif]-->

<!-- ################### using metro bootsrap for css############################### -->
        <link rel="stylesheet" href="metro/css/metro-bootstrap.css"/>
        <script src="metro/js/jquery/jquery.min.js"></script>
        <script src="metro/js/jquery/jquery.widget.min.js"></script>
        <script src="metro/js/metro.min.js"></script>
        <link rel="stylesheet" href="metro/css/iconFont.css"/>
</head>
<body class="metro">
    <form id="form1" runat="server">
    <div>
    <div class="three_fifth">
        <table border="0">
            <tr>
                <td colspan="3" style="padding-left: 5px;">
                    <asp:Label ID="lblUser" runat="server" Text="" CssClass="welcomeMessage"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="padding-left: 5px;">
                    <asp:Button ID="btnUserMaint" runat="server" CssClass="admin" OnClick="btnUserMaint_Click"/>
                </td>
                <td>
                    <asp:Button ID="btnDpsMaster" runat="server" CssClass="order" OnClick="btnDpsMaster_Click"/>
                </td>
                <td>
                    <asp:Button ID="btnDpsMaint" runat="server" CssClass="soa" OnClick="btnDpsMaint_Click"/>
                </td>
            </tr>
        </table>
    </div>
    </div>
    </form>
</body>
</html>