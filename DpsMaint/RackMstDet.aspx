<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RackMstDet.aspx.cs" Inherits="RackMst" %>

<?xml version="1.0" encoding="UTF-8"?><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="content-type" content="application/xhtml+xml;charset=UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=Edge"/>
    <title>Perodua DPS Application</title>
    <link rel="stylesheet" href="../stylesheet.css" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table class="table" width="100%" border="0">
                <tr>
                <td colspan="4">
                    <span class="titlemain"><asp:Label ID="lblTitleMain" runat="server" Text="Rack Master"></asp:Label></span>
                </td>
                </tr>
                <tr>
                <td colspan="4" class="titlesub">
                    <span class="titlesubfont"><asp:Label ID="lblTitleSub1" runat="server" Text="Rack Master Detail"></asp:Label></span>
                </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblPlcNo" runat="server" Text="PLC No"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtPlcNo" runat="server" ReadOnly="True"></asp:TextBox></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblProcName" runat="server" Text="Process Name"></asp:Label></td>
                    <td class="tdtextbox" style="width: 29%; height: 28px">
                        <asp:TextBox ID="txtProcName" runat="server" ReadOnly="True"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblGroupName" runat="server" Text="Group Name"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtGroupName" runat="server" ReadOnly="True"></asp:TextBox></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblBlockName" runat="server" Text="Block Name"></asp:Label></td>
                    <td class="tdtextbox" style="width: 29%; height: 28px">
                        <asp:TextBox ID="txtBlockName" runat="server" ReadOnly="True"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px; color: black; background-color: transparent;">
                        <asp:Label ID="Label3" runat="server" Text="-Rack Image-"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        &nbsp;</td>
                    <td class="tdfield" style="width: 20%; height: 28px; background-color: transparent;">
                    </td>
                    <td class="tdtextbox" style="width: 29%; height: 28px">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblRackName" runat="server" Text="Rack Name"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtRackName" runat="server" ReadOnly="True"></asp:TextBox></td>
                    <td class="tdfield" style="width: 20%; height: 28px; background-color: transparent;">
                    </td>
                    <td class="tdtextbox" style="width: 29%; height: 28px">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblRowCnt" runat="server" Text="Row"></asp:Label></td>
                    <td class="tdtextbox" style="width: 29%; height: 28px">
                        <asp:TextBox ID="txtRowCnt" runat="server" ReadOnly="True"></asp:TextBox></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblColCnt" runat="server" Text="Column"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtColCnt" runat="server" ReadOnly="True"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="4" class="alignRight">
                        <asp:Button ID="btnBack" runat="server" CssClass="button" Text="Close" OnClick="btnBack_Click"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblMsg" runat="server" Text="" Visible="false" CssClass="LabelWarnStyle"></asp:Label>
                        <asp:Label ID="lblVar2" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:Label ID="lblTmpRackName" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblTmpRackDet" runat="server" Visible="False"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
            <asp:Panel ID="AisPreview" runat="server"/>
    </form>
</body>
</html>
