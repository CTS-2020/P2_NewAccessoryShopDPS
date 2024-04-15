<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImpPartNum.aspx.cs" Inherits="ImpPartNum" %>

<?xml version="1.0" encoding="UTF-8"?><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="content-type" content="application/xhtml+xml;charset=UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=Edge"/>
    <title>Perodua DPS Application</title>
    <link rel="stylesheet" href="../stylesheet.css?v=1" type="text/css"/>
    <link href="../webfunction/webfunc.css?v=1" rel="stylesheet" type="text/css" />
    <script src="../webfunction/jquery-2.1.4.min.js?v=1" type="text/javascript"></script>
    <script src="../webfunction/webfunc.js?v=1" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table class="table" width="100%" border="0">
                <tr>
                <td colspan="4">
                    <span class="titlemain"><asp:Label ID="lblTitleMain" runat="server" Text="Import Parts No"></asp:Label></span>
                </td>
                </tr>
                <tr>
                <td colspan="4" class="titlesub">
                    <span class="titlesubfont"><asp:Label ID="lblTitleSub1" runat="server" Text="Import Parts No List"></asp:Label></span>
                </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: right;">
                        <asp:Button ID="btnImport" runat="server" CssClass="button" Text="Import" OnClick="btnImport_Click" OnClientClick = "Confirm('Confirm to Import?');ShowProgress('import');"/>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblMsg" runat="server" Text="" Visible="false" CssClass="LabelWarnStyle"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:GridView ID="gvPartsNumList" runat="server" AutoGenerateColumns="false" AllowPaging="True" CssClass="DataTable" OnPageIndexChanging="gvPartsNumList_PageIndexChanging" AllowSorting="True" EmptyDataText="No Data">
                            <Columns>
                                <asp:BoundField HeaderText="Part No" DataField="part_no"/>
                                <asp:BoundField HeaderText="Part Name" DataField="part_name"/>
                                <asp:TemplateField HeaderText="Part Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPartType" runat="server" Text='<%#GetPartType(Eval("part_type"))%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Part Type" DataField="part_type" Visible="false"/>
                                <asp:BoundField HeaderText="Color SFX" DataField="color_sfx"/>
                                <asp:BoundField HeaderText="Symbol Code" DataField="symbol_code"/>
                                <asp:TemplateField HeaderText="Symbol">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSymbol" runat="server" Text='<%#GetSymbol(Eval("symbol"),Eval("part_no"),Eval("color_sfx"))%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Symbol" DataField="symbol" Visible="false"/>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                        </asp:GridView>
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
