<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RackMstSelAIS.aspx.cs" Inherits="SelectionAIS" %>

<?xml version="1.0" encoding="UTF-8"?><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="content-type" content="application/xhtml+xml;charset=UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=Edge"/>
    <title>AIS Selection</title>
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
                <td>
                    <span class="titlemain"><asp:Label ID="lblTitleMain" runat="server" 
                        Text="Part Data Selection"></asp:Label></span>
                </td>
                </tr>
                <tr>
                <td class="titlesub">
                    <span class="titlesubfont"><asp:Label ID="lblTitleSub1" runat="server" 
                        Text="Part Data Selection"></asp:Label></span>
                </td>
                </tr>
                <tr>
                    <td>
                        <table border="0" class="table" width="100%">
                            <tr>
                                <td class="tdfield" style="width: 20%; height: 28px">
                                    <asp:Label ID="lblAisType" runat="server" Text="AIS Type"></asp:Label></td>
                                <td class="tdtextbox" style="width: 30%; height: 28px">
                                    <asp:DropDownList ID="ddAisType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddAisType_OnSelectedIndexChanged">
                                    </asp:DropDownList></td>
                                <td class="tdfield" style="width: 20%; height: 28px">
                                    <asp:Label ID="lblAisName" runat="server" Text="AIS Name"></asp:Label></td>
                                <td class="tdtextbox" style="width: 29%; height: 28px">
                                    <asp:DropDownList ID="ddAisName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddAisName_OnSelectedIndexChanged"></asp:DropDownList></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table border="0" class="table" width="100%">
                            <tr>
                                <td class="tdfield" style="width: 20%; height: 28px">
                                    <asp:Label ID="lblRevNo" runat="server" Text="Rev No"></asp:Label></td>
                                <td class="tdtextbox" style="width: 30%; height: 28px">
                                    <asp:DropDownList ID="ddRevNo" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddRevNo_OnSelectedIndexChanged">
                                    </asp:DropDownList></td>
                                <td class="tdtextbox" style="width: 20%; height: 28px"></td>
                                <td class="tdtextbox" style="width: 29%; height: 28px"></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblTmpRackName" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblTmpPartId" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblTmpPlcNo" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblTmpProcName" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblMsg" runat="server" Text="" Visible="false" CssClass="LabelWarnStyle"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Button ID="btnBack" runat="server" CssClass="button" Text="Back" OnClick="btnBack_Click"/>
                        <asp:Button ID="btnEmpty" runat="server" Text="No Parts" CssClass="button" OnClick="btnEmpty_Click" OnClientClick="Confirm('Confirm to empty current Rack Location?')"/>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button" OnClick="btnSearch_Click" Visible="false"/>
                    </td>
                </tr>
            </table>
        </div>
            <asp:Panel ID="AisPreview" runat="server"/>
    </form>
</body>
</html>
