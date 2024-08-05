<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GroupMst.aspx.cs" Inherits="GroupMst" %>

<?xml version="1.0" encoding="UTF-8" ?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="content-type" content="application/xhtml+xml;charset=UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <title>Perodua DPS Application</title>
    <link rel="stylesheet" href="../stylesheet.css" type="text/css" />
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
                        <span class="titlemain">
                            <asp:Label ID="lblTitleMain" runat="server" Text="Group Master"></asp:Label></span>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="titlesub">
                        <span class="titlesubfont">
                            <asp:Label ID="lblTitleSub1" runat="server" Text="Search Group Master List"></asp:Label></span>
                    </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblProcName" runat="server" Text="Process Name"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddProcName" runat="server">
                        </asp:DropDownList></td>
                    <td class="tdfield" style="width: 20%; height: 28px; background-color: transparent;"></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px"></td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblGroupId" runat="server" Text="Group ID"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtGroupId" runat="server" CssClass="TextboxLongStyle" MaxLength="5"></asp:TextBox></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblGroupName" runat="server" Text="Group Name"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtGroupName" runat="server" CssClass="TextboxLongStyle"
                            MaxLength="16"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblGroupLine" runat="server" Text="Group Line"></asp:Label></td>
                    <%--<td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtGroupLine" runat="server" CssClass="TextboxLongStyle" MaxLength="1"></asp:TextBox></td>--%>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddLineType" runat="server" AutoPostBack="true"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td colspan="4" class="alignRight">
                        <asp:Button ID="btnNewGroup" runat="server" CssClass="button" Text="New" OnClick="btnNewGroup_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" OnClick="btnClear_Click" />
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button" OnClick="btnSearch_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:CompareValidator ID="vdInteger" runat="server" Operator="DataTypeCheck" Type="Integer"
                            ControlToValidate="txtGroupId" ErrorMessage="Value must be a integer." />
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:GridView ID="gvGroupMst" runat="server" AutoGenerateColumns="false" AllowPaging="True" CssClass="DataTable" OnRowDataBound="gvGroupMst_RowDataBound" OnRowCommand="gvGroupMst_RowCommand" OnPageIndexChanging="gvGroupMst_PageIndexChanging" AllowSorting="True" EmptyDataText="No Data">
                            <Columns>
                                <asp:BoundField HeaderText="Group ID" DataField="group_id" />
                                <asp:BoundField HeaderText="Group Name" DataField="group_name" />
                                <asp:BoundField HeaderText="Group Line" DataField="group_line" />
                                <asp:BoundField HeaderText="PLC No" DataField="plc_no" />
                                <asp:BoundField HeaderText="Process Name" DataField="proc_name" />
                                <asp:BoundField HeaderText="Last Updated By" DataField="last_upd_by" />
                                <asp:BoundField HeaderText="Last Updated Date/Time" DataField="last_upd_dt" />
                                <asp:ButtonField CommandName="EditRecord" ButtonType="Link" Text="Edit" />
                                <asp:ButtonField CommandName="DeleteRecord" ButtonType="Link" Text="Delete" />
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: center;">&nbsp;</td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
