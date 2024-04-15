<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManUpdDpsInsDataReg.aspx.cs" Inherits="ManUpdDpsInsData" %>

<?xml version="1.0" encoding="UTF-8"?><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="content-type" content="application/xhtml+xml;charset=UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=Edge"/>
    <title>Perodua DPS Application</title>
    <link rel="stylesheet" href="../stylesheet.css" type="text/css"/>
    <link href="../webfunction/webfunc.css" rel="stylesheet" type="text/css" />
    <%--<script src="../webfunction/jquery.1.8.3.min.js" type="text/javascript"></script>--%>
    <script src="../webfunction/jquery-2.1.4.min.js" type="text/javascript"></script>
    <script src="../webfunction/webfunc.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table class="table" width="100%" border="0">
                <tr>
                <td colspan="4">
                    <span class="titlemain"><asp:Label ID="lblTitleMain" runat="server" 
                        Text="Manual Update DPS Instruction Data"></asp:Label></span>
                </td>
                </tr>
                <tr>
                <td colspan="4" class="titlesub">
                    <span class="titlesubfont"><asp:Label ID="lblTitleSub1" runat="server" 
                        Text="Manual Update DPS Instruction Data"></asp:Label></span>
                </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblProcName" runat="server" Text="Process Name"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddProcName" runat="server" 
                            onselectedindexchanged="ddProcName_SelectedIndexChanged" Enabled="false"></asp:DropDownList></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblPlcNo" runat="server" Text="PLC No"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtPlcNo" runat="server" CssClass="TextboxLongStyle" 
                            ReadOnly="True" Enabled="false"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblPointer" runat="server" Text="Pointer"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtPointer" runat="server" CssClass="TextboxLongStyle" MaxLength="3" Enabled="false"></asp:TextBox></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblModel" runat="server" Text="Model"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtModel" runat="server" CssClass="TextboxLongStyle" MaxLength="4"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblChasNo" runat="server" Text="Chassis No"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtChasNo" runat="server" CssClass="TextboxLongStyle" MaxLength="19"></asp:TextBox></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblSfx" runat="server" Text="SFX"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtSfx" runat="server" CssClass="TextboxLongStyle" MaxLength="2"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblColor" runat="server" Text="Color"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtColor" runat="server" CssClass="TextboxLongStyle" MaxLength="4"></asp:TextBox></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblInsCode" runat="server" Text="DPS Instruction Code"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtInsCode" runat="server" CssClass="TextboxLongStyle" MaxLength="6"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblBseq" runat="server" Text="B.Seq"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtBseq" runat="server" CssClass="TextboxLongStyle" MaxLength="10"></asp:TextBox></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblIdn" runat="server" Text="IDN"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtIdn" runat="server" CssClass="TextboxLongStyle" MaxLength="12"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblIdVer" runat="server" Text="ID Ver"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtIdVer" runat="server" CssClass="TextboxLongStyle" MaxLength="2"></asp:TextBox></td>
                    <td class="tdtextbox" style="width: 20%; height: 28px"></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px"></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblDpsRsConvId" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblMsg" runat="server" Text="" Visible="false" CssClass="LabelWarnStyle"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="alignRight" style="height: 32px">
                        <asp:Button ID="btnUpdate" runat="server" CssClass="button" Text="Update" OnClick="btnUpdate_Click" OnClientClick="Confirm('Confirm to update?')"/>
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" OnClick="btnClear_Click"/>                        <asp:Button ID="btnCancel" runat="server" CssClass="button" Text="Cancel" OnClick="btnCancel_Click"/>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
