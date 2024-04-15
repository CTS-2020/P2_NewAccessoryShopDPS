<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainMenu.aspx.cs" Inherits="MainMenu" %>

<?xml version="1.0" encoding="UTF-8"?><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="content-type" content="application/xhtml+xml;charset=UTF-8" />
<meta http-equiv="X-UA-Compatible" content="IE=Edge"/>
<title>Perodua DPS Application</title>
<script type="text/javascript" language="JavaScript" src="jslib/coolmenus3.js"></script>
</head>
    <frameset cols="*,1600,*" frameborder="0">
        <frame src="BlankFrame.aspx" frameborder="0" scrolling="no" border="0" name="RightBlankFrame" id="RightBlankFrame" >
            <frameset rows="83,*,30" frameborder="0">
                <frame src="TitleMenu.aspx" frameborder="0" scrolling="no" border="0" name="TopFrame" id="TopFrame" >
                <frameset rows="100%,*">
                    <asp:Label id="lblFrame" runat="server" Text="Label"></asp:Label>
                </frameset>
            <frame src="Footer.aspx" frameborder="0" scrolling="no" border="0" name="FooterRowFrame" id="FooterRowFrame" >
            </frameset>
        <frame src="BlankFrame.aspx" frameborder="0" scrolling="no" border="0" name="LeftBlankFrame" id="LeftBlankFrame" >
    </frameset>
</html>