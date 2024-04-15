<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManPointerChg.aspx.cs" Inherits="ManPointerChg" %>

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
                <td colspan="5">
                    <span class="titlemain"><asp:Label ID="lblTitleMain" runat="server" Text="Manual Pointer Change"></asp:Label></span>
                </td>
                </tr>
                <tr>
                <td colspan="5" class="titlesub">
                    <span class="titlesubfont"><asp:Label ID="lblTitleSub1" runat="server" Text="Manual Pointer Change"></asp:Label></span>
                </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblPlcName1" runat="server" Text="PLC Name"></asp:Label></td>
                    <td class="tdtextbox" style="width: 100px; height: 28px">
                        <asp:HyperLink ID="lblPlcName_1" runat="server" Target="_blank"></asp:HyperLink></td>
                    <td class="tdtextbox" style="width: 100px; height: 28px">
                        <asp:HyperLink ID="lblPlcName_2" runat="server" Target="_blank"></asp:HyperLink></td>
                    <td class="tdtextbox" style="width: 100px; height: 28px">
                        <asp:HyperLink ID="lblPlcName_3" runat="server" Target="_blank"></asp:HyperLink></td>
                    <td class="tdtextbox" style="width: 100px; height: 28px">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblPlcReadPointer1" runat="server" Text="PLC Write Pointer"></asp:Label></td>
                    <td class="tdtextbox" style="height: 28px">
                        <asp:TextBox ID="txtPlcWritePointer_1" runat="server" Width="55px" MaxLength="3"></asp:TextBox>
                        &nbsp;<asp:Button ID="btnPointerUpd_1" runat="server" CssClass="button" 
                            Text="Update" OnCommand="btnUpdate_Click" Width="80px" CommandArgument="1" OnClientClick="Confirm('Confirm to update PLC No 1?')"/></td>
                    <td class="tdtextbox" style="height: 28px">
                        <asp:TextBox ID="txtPlcWritePointer_2" runat="server" Width="55px" MaxLength="3"></asp:TextBox>
                        &nbsp;<asp:Button ID="btnPointerUpd_2" runat="server" CssClass="button" 
                            Text="Update" OnCommand="btnUpdate_Click" Width="80px" CommandArgument="2" OnClientClick="Confirm('Confirm to update PLC No 2?')"/></td>
                    <td class="tdtextbox" style="height: 28px">
                        <asp:TextBox ID="txtPlcWritePointer_3" runat="server" Width="55px" MaxLength="3"></asp:TextBox>
                        &nbsp;<asp:Button ID="btnPointerUpd_3" runat="server" CssClass="button" 
                            Text="Update" OnCommand="btnUpdate_Click" Width="80px" CommandArgument="3" OnClientClick="Confirm('Confirm to update PLC No 3?')"/></td>
                    <td class="tdtextbox" style="height: 28px">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblPlcWritePointer1" runat="server" 
                            Text="PLC Read Pointer (Reference)"></asp:Label></td>
                    <td class="tdtextbox" style="height: 28px">
                        <asp:TextBox ID="txtPlcReadPointer_1" runat="server" Width="55px" 
                            ReadOnly="True" Enabled="False"></asp:TextBox></td>
                    <td class="tdtextbox" style="height: 28px">
                        <asp:TextBox ID="txtPlcReadPointer_2" runat="server" Width="55px" 
                            ReadOnly="True" Enabled="False"></asp:TextBox></td>
                    <td class="tdtextbox" style="height: 28px">
                        <asp:TextBox ID="txtPlcReadPointer_3" runat="server" Width="55px" 
                            ReadOnly="True" Enabled="False"></asp:TextBox></td>
                    <td class="tdtextbox" style="height: 28px">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="alignRight" colspan="5">
                    </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblPlcName2" runat="server" Text="PLC Name"></asp:Label></td>
                    <td class="tdtextbox" style="width: 100px; height: 28px">
                        <asp:HyperLink ID="lblPlcName_4" runat="server" Target="_blank"></asp:HyperLink></td>
                    <td class="tdtextbox" style="width: 100px; height: 28px">
                        <asp:HyperLink ID="lblPlcName_5" runat="server" Target="_blank"></asp:HyperLink></td>
                    <td class="tdtextbox" style="width: 100px; height: 28px">
                        <asp:HyperLink ID="lblPlcName_6" runat="server" Target="_blank"></asp:HyperLink>
                    </td>
                    <td class="tdtextbox" style="width: 100px; height: 28px">
                    </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblPlcReadPointer2" runat="server" Text="PLC Write Pointer"></asp:Label></td>
                    <td class="tdtextbox" style="height: 28px">
                        <asp:TextBox ID="txtPlcWritePointer_4" runat="server" Width="55px" MaxLength="3"></asp:TextBox>
                        &nbsp;<asp:Button ID="btnPointerUpd_4" runat="server" CssClass="button" 
                            Text="Update" OnCommand="btnUpdate_Click" Width="80px" CommandArgument="4" OnClientClick="Confirm('Confirm to update PLC No 4?')"/></td>
                    <td class="tdtextbox" style="height: 28px">
                        <asp:TextBox ID="txtPlcWritePointer_5" runat="server" Width="55px" MaxLength="3"></asp:TextBox>
                        &nbsp;<asp:Button ID="btnPointerUpd_5" runat="server" CssClass="button" 
                            Text="Update" OnCommand="btnUpdate_Click" Width="80px" CommandArgument="5" OnClientClick="Confirm('Confirm to update PLC No 5?')"/></td>
                    <td class="tdtextbox" style="height: 28px">
                        <asp:TextBox ID="txtPlcWritePointer_6" runat="server" Width="55px" MaxLength="3"></asp:TextBox>
                        &nbsp;<asp:Button ID="btnPointerUpd_6" runat="server" CssClass="button" 
                            Text="Update" OnCommand="btnUpdate_Click" Width="80px" CommandArgument="6" OnClientClick="Confirm('Confirm to update PLC No 6?')"/>
                    </td>
                    <td class="tdtextbox" style="height: 28px">
                    </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblPlcWritePointer2" runat="server" 
                            Text="PLC Read Pointer (Reference)"></asp:Label></td>
                    <td class="tdtextbox" style="height: 28px">
                        <asp:TextBox ID="txtPlcReadPointer_4" runat="server" Width="55px" 
                            ReadOnly="True" Enabled="False"></asp:TextBox></td>
                    <td class="tdtextbox" style="height: 28px">
                        <asp:TextBox ID="txtPlcReadPointer_5" runat="server" ReadOnly="True" 
                            Width="55px" Enabled="False"></asp:TextBox></td>
                    <td class="tdtextbox" style="height: 28px">
                        <asp:TextBox ID="txtPlcReadPointer_6" runat="server" Width="55px" 
                            ReadOnly="True" Enabled="False"></asp:TextBox>
                    </td>
                    <td class="tdtextbox" style="height: 28px">
                    </td>
                </tr>
                <tr>
                    <td style="height: 28px">
                    </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblPlcName3" runat="server" Text="PLC Name"></asp:Label></td>
                    <td class="tdtextbox" style="width: 100px; height: 28px">
                        <asp:HyperLink ID="lblPlcName_7" runat="server" Target="_blank"></asp:HyperLink></td>
                    <td class="tdtextbox" style="width: 100px; height: 28px">
                        <asp:HyperLink ID="lblPlcName_8" runat="server" Target="_blank"></asp:HyperLink></td>
                    <td class="tdtextbox" style="width: 100px; height: 28px">
                        <asp:HyperLink ID="lblPlcName_9" runat="server" Target="_blank"></asp:HyperLink></td>
                    <td class="tdtextbox" style="width: 100px; height: 28px">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblPlcReadPointer3" runat="server" Text="PLC Write Pointer"></asp:Label></td>
                    <td class="tdtextbox" style="height: 28px">
                        <asp:TextBox ID="txtPlcWritePointer_7" runat="server" Width="55px" MaxLength="3"></asp:TextBox>
                        &nbsp;<asp:Button ID="btnPointerUpd_7" runat="server" CssClass="button" 
                            Text="Update" OnCommand="btnUpdate_Click" Width="80px" CommandArgument="7" OnClientClick="Confirm('Confirm to update PLC No 7?')"/></td>
                    <td class="tdtextbox" style="height: 28px">
                        <asp:TextBox ID="txtPlcWritePointer_8" runat="server" Width="55px" MaxLength="3"></asp:TextBox>
                        &nbsp;<asp:Button ID="btnPointerUpd_8" runat="server" CssClass="button" 
                            Text="Update" OnCommand="btnUpdate_Click" Width="80px" CommandArgument="8" OnClientClick="Confirm('Confirm to update PLC No 8?')"/></td>
                    <td class="tdtextbox" style="height: 28px">
                        <asp:TextBox ID="txtPlcWritePointer_9" runat="server" Width="55px" MaxLength="3"></asp:TextBox>
                        &nbsp;<asp:Button ID="btnPointerUpd_9" runat="server" CssClass="button" 
                            Text="Update" OnCommand="btnUpdate_Click" Width="80px" CommandArgument="9" OnClientClick="Confirm('Confirm to update PLC No 9?')"/></td>
                    <td class="tdtextbox" style="height: 28px">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblPlcWritePointer3" runat="server" 
                            Text="PLC Read Pointer (Reference)"></asp:Label></td>
                    <td class="tdtextbox" style="height: 28px">
                        <asp:TextBox ID="txtPlcReadPointer_7" runat="server" Width="55px" 
                            ReadOnly="True" Enabled="False"></asp:TextBox></td>
                    <td class="tdtextbox" style="height: 28px">
                        <asp:TextBox ID="txtPlcReadPointer_8" runat="server" Width="55px" 
                            ReadOnly="True" Enabled="False"></asp:TextBox></td>
                    <td class="tdtextbox" style="height: 28px">
                        <asp:TextBox ID="txtPlcReadPointer_9" runat="server" Width="55px" 
                            ReadOnly="True" Enabled="False"></asp:TextBox></td>
                    <td class="tdtextbox" style="height: 28px">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="alignRight" colspan="5">
                    </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblPlcName4" runat="server" Text="PLC Name"></asp:Label></td>
                    <td class="tdtextbox" style="width: 100px; height: 28px">
                        <asp:HyperLink ID="lblPlcName_10" runat="server" Target="_blank"></asp:HyperLink></td>
                    <td class="tdtextbox" style="width: 100px; height: 28px">
                        <asp:HyperLink ID="lblPlcName_11" runat="server" Target="_blank"></asp:HyperLink></td>
                    <td class="tdtextbox" style="width: 100px; height: 28px">
                        <asp:HyperLink ID="lblPlcName_12" runat="server" Target="_blank"></asp:HyperLink>
                    </td>
                    <td class="tdtextbox" style="width: 100px; height: 28px">
                    </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblPlcReadPointer4" runat="server" Text="PLC Write Pointer"></asp:Label></td>
                    <td class="tdtextbox" style="height: 28px">
                        <asp:TextBox ID="txtPlcWritePointer_10" runat="server" Width="55px" MaxLength="3"></asp:TextBox>
                        &nbsp;<asp:Button ID="btnPointerUpd_10" runat="server" CssClass="button" 
                            Text="Update" OnCommand="btnUpdate_Click" Width="80px" CommandArgument="10" OnClientClick="Confirm('Confirm to update PLC No 10?')"/></td>
                    <td class="tdtextbox" style="height: 28px">
                        <asp:TextBox ID="txtPlcWritePointer_11" runat="server" Width="55px" MaxLength="3"></asp:TextBox>
                        &nbsp;<asp:Button ID="btnPointerUpd_11" runat="server" CssClass="button" 
                            Text="Update" OnCommand="btnUpdate_Click" Width="80px" CommandArgument="11" OnClientClick="Confirm('Confirm to update PLC No 11?')"/></td>
                    <td class="tdtextbox" style="height: 28px">
                        <asp:TextBox ID="txtPlcWritePointer_12" runat="server" Width="55px" MaxLength="3"></asp:TextBox>
                        &nbsp;<asp:Button ID="btnPointerUpd_12" runat="server" CssClass="button" 
                            Text="Update" OnCommand="btnUpdate_Click" Width="80px" CommandArgument="12" OnClientClick="Confirm('Confirm to update PLC No 12?')"/>
                    </td>
                    <td class="tdtextbox" style="height: 28px">
                    </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblPlcWritePointer4" runat="server" 
                            Text="PLC Read Pointer (Reference)"></asp:Label></td>
                    <td class="tdtextbox" style="height: 28px">
                        <asp:TextBox ID="txtPlcReadPointer_10" runat="server" Width="55px" 
                            ReadOnly="True" Enabled="False"></asp:TextBox></td>
                    <td class="tdtextbox" style="height: 28px">
                        <asp:TextBox ID="txtPlcReadPointer_11" runat="server" ReadOnly="True" 
                            Width="55px" Enabled="False"></asp:TextBox></td>
                    <td class="tdtextbox" style="height: 28px">
                        <asp:TextBox ID="txtPlcReadPointer_12" runat="server" Width="55px" 
                            ReadOnly="True" Enabled="False"></asp:TextBox>
                    </td>
                    <td class="tdtextbox" style="height: 28px">
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
