<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImpDataHJ.aspx.cs" Inherits="ImpDataHJ" %>

<?xml version="1.0" encoding="UTF-8"?><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="content-type" content="application/xhtml+xml;charset=UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=Edge"/>
    <title>Perodua DPS Application</title>
    <link rel="stylesheet" href="../stylesheet.css?v=1" type="text/css"/>
    <script src="../webfunction/jquery-2.1.4.min.js?v=1" type="text/javascript"></script>
    <script src="../webfunction/webfunc.js?v=1" type="text/javascript"></script>
    <link href="../webfunction/webfunc.css?v=1" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table class="table" width="100%" border="0">
                <tr>
                <td colspan="4">
                    <span class="titlemain"><asp:Label ID="lblTitleMain" runat="server" Text="Import Harigami Data"></asp:Label></span>
                </td>
                </tr>
                <tr>
                <td colspan="4" class="titlesub">
                    <span class="titlesubfont"><asp:Label ID="lblTitleSub1" runat="server" Text="Import Harigami Data"></asp:Label></span>
                </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblAisType" runat="server" Text="AIS Type"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddAisType" runat="server" AutoPostBack= "true" OnSelectedIndexChanged="ddAisType_OnSelectedIndexChanged"></asp:DropDownList></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblAisName" runat="server" Text="AIS Name"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddAisName" runat="server" AutoPostBack= "true"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblMsg" runat="server" Text="" Visible="false" CssClass="LabelWarnStyle"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="alignRight">
                        <asp:Button ID="btnImport" runat="server" CssClass="button" Text="Import" OnClick="btnImport_Click" OnClientClick = "Confirm('Confirm to Import?');ShowProgress('import');"/>
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" OnClick="btnClear_Click"/>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button" OnClick="btnSearch_Click"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:GridView ID="gvDataHJList" runat="server" AutoGenerateColumns="false" AllowPaging="True" CssClass="DataTable" OnPageIndexChanging="gvDataHJList_PageIndexChanging" AllowSorting="True" EmptyDataText="No Data">
                            <Columns>
                                <asp:BoundField HeaderText="Harigami ID" DataField="id"/>
                                <asp:BoundField HeaderText="Harigami Item ID" DataField="item_id"/>
                                <asp:BoundField HeaderText="Rev No" DataField="rev_no"/>
                                <asp:BoundField HeaderText="Col" DataField="col"/>
                                <asp:BoundField HeaderText="Row" DataField="row"/>
                                <asp:BoundField HeaderText="Name" DataField="name"/>
                                <asp:BoundField HeaderText="Parts No" DataField="part_no"/>
                                <asp:BoundField HeaderText="Parts Title" DataField="parts_title"/>
                                <asp:BoundField HeaderText="Color SFX" DataField="color_sfx"/>
                                <asp:BoundField HeaderText="Katashiki" DataField="katashiki"/>
                                <asp:BoundField HeaderText="Model" DataField="model"/>
                                <asp:BoundField HeaderText="SFX" DataField="sfx"/>
                                <asp:BoundField HeaderText="Color" DataField="color"/>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: center;">
                        <!--;ShowProgress('import');-->
                    </td>
                </tr>
            </table>
        </div>
        <div class="loadingimport" align="center">
            Importing. Please wait.<br />
            DO NOT close the browser !!!<br />
            <br />
            <img src="../styles/images/icon/loading.gif" alt="" />
        </div>
    </form>
</body>
</html>
