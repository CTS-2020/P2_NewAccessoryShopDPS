<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RackMstSelModuleAdd.aspx.cs" Inherits="SelectionModuleAdd" %>

<?xml version="1.0" encoding="UTF-8"?><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="content-type" content="application/xhtml+xml;charset=UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=Edge"/>
    <title>Module Address Selection</title>
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
                    <span class="titlemain"><asp:Label ID="lblTitleMain" runat="server" Text="Module Data Selection"></asp:Label></span>
                </td>
                </tr>
                <tr>
                <td colspan="4" class="titlesub" style="height: 30px">
                    <span class="titlesubfont"><asp:Label ID="lblTitleSub1" runat="server" Text="Module Data Selection List"></asp:Label></span>
                </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 15%; height: 28px">
                        <asp:Label ID="lblModuleAdd" runat="server" Text="Module Address"></asp:Label></td>
                    <td class="tdtextbox" style="width: 40%; height: 28px">
                        <asp:TextBox ID="txtModuleAddFrm" runat="server" CssClass="TextboxStyle"></asp:TextBox>&nbsp;
                        <asp:Label ID="lblTo" runat="server" Text=" To "></asp:Label>&nbsp;
                        <asp:TextBox ID="txtModuleAddTo" runat="server" CssClass="TextboxStyle"></asp:TextBox>
                        </td>
                    <td class="tdfield" style="width: 15%; height: 28px">
                        <asp:Label ID="lblShowUsed" runat="server" Text="Show Used LM"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:CheckBox ID="cbShowUsed" runat="server">
                        </asp:CheckBox>
                        </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblTmpRackName" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblTmpPartId" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblTmpPlcNo" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblTmpProcName" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblMsg" runat="server" Text="" Visible="false" CssClass="LabelWarnStyle"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="alignRight">
                        <asp:Button ID="btnBack" runat="server" CssClass="button" Text="Back" OnClick="btnBack_Click"/>
                        <asp:Button ID="btnEmpty" runat="server" Text="No Module" CssClass="button" OnClick="btnEmpty_Click" OnClientClick="Confirm('Confirm to empty current Rack Location Module Address?')"/>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button" OnClick="btnSearch_Click"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:GridView ID="gvLampModuleAddMst" runat="server" AutoGenerateColumns="false" AllowPaging="True" CssClass="DataTable" OnRowCommand ="gvLampModuleAddMst_RowCommand" OnPageIndexChanging="gvLampModuleAddMst_PageIndexChanging" AllowSorting="True" EmptyDataText="No Data">
                            <Columns>
                                <asp:TemplateField HeaderText="No">
                                    <HeaderStyle CssClass="hidden"/>
                                    <ItemStyle CssClass="hidden"/>
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Module Address" DataField="module_add"/>
                                <asp:BoundField HeaderText="Module Name" DataField="module_name"/>
                                <asp:BoundField HeaderText="Module Type" DataField="module_type"/>
                                <asp:BoundField HeaderText="Rack Location" DataField="rack_loc"/>
                                <asp:ButtonField CommandName="SelectRecord" ButtonType="Link" Text="Select"/>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
