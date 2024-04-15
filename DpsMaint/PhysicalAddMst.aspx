<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PhysicalAddMst.aspx.cs" Inherits="PhysicalAddMst" %>

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
                    <span class="titlemain"><asp:Label ID="lblTitleMain" runat="server" Text="Physical Address Master"></asp:Label></span>
                </td>
                </tr>
                <tr>
                <td colspan="4" class="titlesub">
                    <span class="titlesubfont"><asp:Label ID="lblTitleSub1" runat="server" Text="Search Physical Address Master List"></asp:Label></span>
                </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 15%; height: 28px">
                        <asp:Label ID="lblProcName" runat="server" Text="Process Name"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddProcName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddProcName_OnSelectedIndexChanged">
                        </asp:DropDownList></td>
                    <td class="tdfield" style="width: 15%; height: 28px">
                        <asp:Label ID="lblModuleAdd" runat="server" Text="Module Address"></asp:Label></td>
                    <td class="tdtextbox" style="width: 40%; height: 28px">
                        <asp:TextBox ID="txtModuleAddFrm" runat="server" CssClass="TextboxStyle"></asp:TextBox>&nbsp;
                        <asp:Label ID="lblTo" runat="server" Text=" To "></asp:Label>&nbsp;
                        <asp:TextBox ID="txtModuleAddTo" runat="server" CssClass="TextboxStyle"></asp:TextBox>
                        </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 15%; height: 28px">
                        <asp:Label ID="lblPlcModel" runat="server" Text="GW Model"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddPlcModel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddPlcModel_OnSelectedIndexChanged">
                        </asp:DropDownList></td>
                    <td class="tdfield" style="width: 15%; height: 30px">
                        <asp:Label ID="lblPhysicalAdd" runat="server" Text="Physical Address"></asp:Label></td>
                    <td class="tdtextbox" style="width: 40%; height: 30px">
                        <asp:TextBox ID="txtPhysicalAddFrm" runat="server" CssClass="TextboxStyle"></asp:TextBox>&nbsp;
                        <asp:Label ID="lblPhyTo" runat="server" Text=" To "></asp:Label>&nbsp;
                        <asp:TextBox ID="txtPhysicalAddTo" runat="server" CssClass="TextboxStyle"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="alignRight">
                        <asp:Button ID="btnNewPhysical" runat="server" CssClass="button" Text="New" OnClick="btnNewPhysical_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" OnClick="btnClear_Click"/>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button" OnClick="btnSearch_Click"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        &nbsp;<asp:Label ID="lblMsg" runat="server" Text="" Visible="false" CssClass="LabelWarnStyle"></asp:Label>
                        &nbsp;<asp:Label ID="lblTmpPhysicalUid" runat="server" Visible="False"></asp:Label>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:GridView ID="gvPhysicalAddMst" runat="server" AutoGenerateColumns="false" AllowPaging="True" CssClass="DataTable" OnRowDataBound="gvPhysicalAddMst_RowDataBound" OnRowCommand ="gvPhysicalAddMst_RowCommand" OnPageIndexChanging="gvPhysicalAddMst_PageIndexChanging" AllowSorting="True" EmptyDataText="No Data">
                            <Columns>
                                <asp:BoundField HeaderText="ID" DataField="uid" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                                <asp:BoundField HeaderText="PLC No" DataField="plc_no"/>
                                <asp:BoundField HeaderText="Process Name" DataField="proc_name"/>
                                <asp:BoundField HeaderText="GW Model" DataField="plc_model"/>
                                <asp:BoundField HeaderText="Module Address" DataField="module_add"/>
                                <asp:BoundField HeaderText="Physical Address" DataField="physical_add"/>
                                <asp:BoundField HeaderText="Last Updated By" DataField="last_upd_by"/>
                                <asp:BoundField HeaderText="Last Updated Date/Time" DataField="last_upd_dt"/>
                                <asp:ButtonField CommandName="EditRecord" ButtonType="Link" Text="Edit"/>
                                <asp:ButtonField CommandName="DeleteRecord" ButtonType="Link" Text="Delete" />
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
