<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<?xml version="1.0" encoding="UTF-8"?><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <meta http-equiv="content-type" content="application/xhtml+xml;charset=UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=Edge"/>
    <title>Perodua DPS Application</title>
    <link href="./stylesheet.css" rel="stylesheet" type="text/css"/>
    <script language="javascript" type="text/javascript">
    function disableButtonOnClick(oButton, sCssClass)
    {
        oButton.disabled = true;
        oButton.setAttribute('className', sCssClass);
        oButton.setAttribute('class', sCssClass);
    }
    </script>
</head>
<body>
    <form id="LoginForm" runat="server">
    <div id='LoginMain' style="text-align:center; width:100%; position: static;">
            <div id="LoginContent"> 
                <table border="0" width="100%">
                    <%--<tr>
                        <td style="width: 4%;"></td>
                        <td style="width: 30%;" class="alignLeft">
                            <img src="styles/images/Perodua-Logo.png" alt="" class="imageLogo"/>
                            <span class="titlemain"><asp:Label ID="lblTitle" runat="server" Text="Digital Picking System"></asp:Label></span>
                            <img src="styles/images/PGMlogo.png" alt="" width="150px"/>
                        </td>
                    </tr>--%>
                    <tr>
                        <td style="width:4%; padding-left: 7px;">
                            <img src="styles/images/Perodua-Logo.png" alt="" class="imageLogo"/>
                        </td>
                        <td style="width:20%; padding-right: 3px; padding-top: 3px;" align="center">
                            <span class="titlemain"><asp:Label ID="Label1" runat="server" Text="Digital Picking System"></asp:Label></span>
                        </td>
                        <td style="width:4%; padding-left: 7px;">
                            <img src="styles/images/PGMlogo.png" alt="" width="150px"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="height:20px;" colspan="3"></td>
                    </tr>
                    <tr>
                        <td style="height: 20px;" colspan="3"></td>
                    </tr>
                    <tr>
                        <td style="height: 20px;" colspan="3"></td>
                    </tr>
                    <tr>
                        <td rowspan="1" colspan="3" valign="top" align="center">
                         <div id="all" style="width: 600px;">
                            <table id="tblLogin" runat="server" border="0" style="width: 600px;">
                                <tr style="height: 25px;">
                                    <td colspan="4" valign="top"></td>
                                </tr>
                                <tr style="height: 25px;">
                                    <td colspan="4"></td>
                                </tr>
                                <tr style="height: 25px;">
                                    <td colspan="4">
                                        <asp:Label ID="lblMsg" runat="server" Text="" Visible="False" CssClass="LabelWarnStyle"></asp:Label>
                                        <asp:Label ID="lblUserId" runat="server" Visible="False" CssClass="hidden"></asp:Label>
                                        <asp:Label ID="lblFirstLogin" runat="server" Visible="False" CssClass="hidden"></asp:Label>
                                    </td>
                                </tr>
                                <tr style="height: 25px;">
                                    <td style="width: 25%"></td>
                                    <td style="width: 25%"></td>
                                    <td style="width: 25%"></td>
                                    <td style="width: 25%"></td>
                                </tr>
                                <tr> 
                                    <td></td>
                                    <td class="alignLeft">
                                        <asp:Label ID="lblUserName" runat="server" Text="Username :" CssClass="LabelStyle"></asp:Label>
                                    </td>
                                    <td class="alignLeft">
                                        <asp:TextBox ID="txtUserName" runat="server" CssClass="textboxLogin"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class="alignRight"></td>
                                    <td class="alignLeft">
                                        <asp:Label ID="lblUserPassword" runat="server" Text="Password :" CssClass="LabelStyle"></asp:Label>
                                    </td>
                                    <td class="alignLeft">
                                        <asp:TextBox ID="txtUserPassword" runat="server" TextMode="Password" CssClass="textboxLogin"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr style="height:25px;">
                                    <td colspan="4" class="alignCenter">
                                        &nbsp;</td>
                                </tr>
                                <tr style="height: 30px;">
                                    <td></td>
                                    <td class="alignLeft">
                                        &nbsp;</td>
                                    <td class="alignLeft">
                                        <asp:Button ID="btnLogin" runat="server" CssClass="buttonLoginSmaller" Text="Login" OnClick="btnLogin_Click"/>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr style="height: 25px;">
                                    <td></td>
                                    <td></td>
                                    <td class="alignRight">
                                        &nbsp;</td>
                                    <td></td>
                                </tr>
                            </table>
                            </div>
                        </td>
                    </tr>
                </table>
               <!--<div id="LoginBottomTitle">Powered By</div>-->
               <div></div>   
            </div>
        </div>
    </form>
</body>
</html>
