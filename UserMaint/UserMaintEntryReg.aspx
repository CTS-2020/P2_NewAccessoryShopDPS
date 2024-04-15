<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserMaintEntryReg.aspx.cs" Inherits="UserMaintEntry" %>

<?xml version="1.0" encoding="UTF-8"?><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="content-type" content="application/xhtml+xml;charset=UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=Edge"/>
    <title>Perodua DPS Application</title>
    <link href="../stylesheet.css" rel="stylesheet" type="text/css"/>
    <link href="../webfunction/webfunc.css" rel="stylesheet" type="text/css" />
    <script src="../webfunction/jquery-2.1.4.min.js" type="text/javascript"></script>
    <script src="../webfunction/webfunc.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table class="table" width="100%" border="0">
                <tr>
                <td colspan="4">
                    <span class="titlemain"><asp:Label ID="lblTitleMain" runat="server" Text="User"></asp:Label></span>
                </td>
                </tr>
                <tr>
                <td colspan="4" class="titlesub">
                    <span class="titlesubfont"><asp:Label ID="lblTitleSub1" runat="server" 
                        Text="Create/Update User"></asp:Label></span>
                </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%;">
                        <asp:Label ID="lblUserID" runat="server" Text="User ID"></asp:Label>
                    </td>
                    <td class="tdtextbox" style="width: 30%;">
                        <asp:TextBox ID="txtUserID" runat="server" CssClass="TextboxLongStyle" MaxLength="10"></asp:TextBox>
                    </td>
                    <td class="tdfield" style="width: 20%;">
                        <asp:Label ID="lblRole" runat="server" Text="Role"></asp:Label>
                    </td>
                    <td class="tdtextbox" style="width: 30%;">
                        <asp:DropDownList ID="ddRole" runat="server" CssClass="DropDownLongStyle"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%;">
                        <asp:Label ID="lblUserName" runat="server" Text="User Name"></asp:Label>
                    </td>
                    <td class="tdtextbox" style="width: 30%;">
                        <asp:TextBox ID="txtUserName" runat="server" CssClass="TextboxLongStyle" MaxLength="50"></asp:TextBox>
                    </td>
                    <td class="tdfield" style="width: 20%;">
                        <asp:Label ID="lblUserPassword" runat="server" Text="User Passsword"></asp:Label>
                    </td>
                    <td class="tdtextbox" style="width: 30%;">
                        <asp:TextBox ID="txtUserPassword" runat="server" CssClass="TextboxLongStyle" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblMsg" runat="server" Text="" Visible="false" CssClass="LabelWarnStyle"></asp:Label>
                        <asp:Label ID="lblTmpUserId" runat="server" Visible="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="alignRight">
                        <asp:Button ID="btnNewUser" runat="server" CssClass="button" Text="Create User" OnClick="btnNewUser_Click" OnClientClick="Confirm('Confirm to save?')"/>
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="button" OnClick="btnUpdate_Click" Visible="false" OnClientClick="Confirm('Confirm to update?')"/>
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" OnClick="btnClear_Click"/>                        <asp:Button ID="btnCancel" runat="server" CssClass="button" Text="Cancel" OnClick="btnCancel_Click"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:GridView ID="gvUserList" runat="server" AutoGenerateColumns="false" AllowPaging="True" CssClass="DataTable" OnPageIndexChanging="gvUserList_PageIndexChanging" AllowSorting="True" EmptyDataText="No Data">
                            <Columns>
                                <asp:BoundField HeaderText="ID" DataField="user_id"/>
                                <asp:BoundField HeaderText="User Name" DataField="user_name"/>
                                <asp:BoundField HeaderText="Password" DataField="user_password"/>
                                <asp:BoundField HeaderText="Role" DataField="role_code"/>
                                <asp:BoundField HeaderText="Last Updated By" DataField="last_upd_by"/>
                                <asp:BoundField HeaderText="Last Updated Date/Time" DataField="last_upd_dt"/>
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
