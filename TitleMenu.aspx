<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TitleMenu.aspx.cs" Inherits="TitleMenu" %>
<?xml version="1.0" encoding="UTF-8"?><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="content-type" content="application/xhtml+xml;charset=UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=Edge"/>
    <title>Title Menu</title>
    <link rel="stylesheet" href="./stylesheet.css" type="text/css" />
    <link rel="stylesheet" href="styles/layout.css" type="text/css" media="all"/>
    <link rel="stylesheet" href="styles/mediaqueries.css" type="text/css" media="all"/>
</head>
<body class="menu">
    <form id="TitleForm" runat="server">
    <table style="width:100%" border="0">
        <tr>
            <td style="width:15%; padding-left: 7px;" rowspan="2">
                <img src="styles/images/Perodua-Logo.png" alt="" class="imageLogo"/>
            </td>
            <td style="width:70%; padding-right: 3px;" align="center">
                <span class="titlemain"><asp:Label ID="lblTitle" font-size="34px" runat="server" Text="Digital Picking System" ></asp:Label></span>
            </td>
            <td style="width:15%; padding-left: 7px;" rowspan="2">
                <%--<img src="styles/images/PGMlogo.png" alt="" width="150px"/>--%>
            </td>
            <td style="width:10%; padding-right: 3px; padding-top: 3px;" class="alignRight">
                <asp:Button ID="Button1" runat="server" CssClass="button" Text="Home" OnClick="btnHome_Click"/>
            </td>
        </tr>
        <tr>
            <td style="width:70%; padding-right: 3px;" align="center">
                <asp:Label ID="lblUser" runat="server" Text="" font-size="20px" CssClass="welcomeMessage" ></asp:Label>
            </td>
            <td style="width:10%; padding-right: 3px;" class="alignRight">
                <asp:Button ID="btnLogout" runat="server" CssClass="button" Text="Logout" OnClick="btnLogout_Click"/>
            </td>
        </tr>
    </table>
    <asp:Label ID="lblUserId" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="lblRoleCode" runat="server" Visible="false"></asp:Label>
    <asp:Label ID="lblButton" runat="server" Visible="False"></asp:Label>
    </form>
</body>
</html>
