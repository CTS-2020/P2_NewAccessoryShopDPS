<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DpsPlcModel.aspx.cs" Inherits="DpsPlcMst" %>

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
                    <span class="titlemain"><asp:Label ID="lblTitleMain" runat="server" Text="DPS GW Model"></asp:Label></span>
                </td>
                </tr>
                <tr>
                <td colspan="4" class="titlesub">
                    <span class="titlesubfont"><asp:Label ID="lblTitleSub1" runat="server" Text="Search DPS GW Model List"></asp:Label></span>
                </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblPlcModelNo" runat="server" Text="GW Model No"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtPlcModelNo" runat="server" CssClass="TextboxLongStyle"></asp:TextBox></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblPlcModelDesc" runat="server" Text="GW Model Description"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtPlcModelDesc" runat="server" CssClass="TextboxLongStyle" MaxLength="50"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblPhyAddrFrom" runat="server" Text="Physical Address From"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtPhyAddrFrom" runat="server" CssClass="TextboxLongStyle" MaxLength="20"></asp:TextBox></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblPhyAddrTo" runat="server" Text="Physical Address To"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtPhyAddrTo" runat="server" CssClass="TextboxLongStyle"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblConvType" runat="server" Text="Conversion Type"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddConvType" runat="server" CssClass="TextboxLongStyle">
                            <asp:ListItem Text="" Value=""></asp:ListItem>
                            <asp:ListItem Text="4D-Conversion (Old Model)" Value="1"></asp:ListItem>
                            <asp:ListItem Text="64-Conversion (New Model)" Value="2"></asp:ListItem>
                        </asp:DropDownList></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblEnable" runat="server" Text="Enable"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:CheckBox ID="cbEnable" runat="server" /></td>
                </tr>
                <tr>
                    <td colspan="4" class="alignRight">
                        <asp:Button ID="btnNewModel" runat="server" CssClass="button" Text="New" OnClick="btnNewModel_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" OnClick="btnClear_Click"/>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button" OnClick="btnSearch_Click"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:GridView ID="gvPlcModelList" runat="server" AutoGenerateColumns="false" 
                            AllowPaging="True" CssClass="DataTable" 
                            OnRowDataBound="gvPlcModelList_RowDataBound" 
                            OnRowCommand ="gvPlcModelList_RowCommand" 
                            OnPageIndexChanging="gvPlcModelList_PageIndexChanging" AllowSorting="True" 
                            EmptyDataText="No Data" PageSize="12">
                            <Columns>
                                <asp:BoundField HeaderText="ID" DataField="uid"/>
                                <asp:BoundField HeaderText="GW Model No" DataField="plcmodel_no"/>
                                <asp:BoundField HeaderText="GW Model Description" HeaderStyle-Width="15%" DataField="plcmodel_desc"/>
                                <asp:BoundField HeaderText="Physical Address From" HeaderStyle-Width="12%" DataField="phyaddr_from"/>
                                <asp:BoundField HeaderText="Physical Address To" HeaderStyle-Width="12%" DataField="phyaddr_to" />
                                <asp:BoundField HeaderText="Conversion Type" HeaderStyle-Width="8%" DataField="conv_type" />
                                <%--<asp:BoundField HeaderText="No of Digit" DataField="digit_no"/>--%>
                                <asp:BoundField HeaderText="Enable" DataField="enable"/>
                                <asp:BoundField HeaderText="Last Updated By" DataField="last_upd_by"/>
                                <asp:BoundField HeaderText="Last Updated Date/Time" DataField="last_upd_dt"/>
                                <asp:ButtonField CommandName="EditRecord" ButtonType="Link" Text="Edit"/>
                                <asp:ButtonField CommandName="DeleteRecord" ButtonType="Link" Text="Delete"/>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: center;">
                        &nbsp;</td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
