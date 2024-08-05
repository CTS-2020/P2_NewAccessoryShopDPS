<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GearUpLMReg.aspx.cs" Inherits="DpsMaint_GearUpLMReg" %>

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
                            <asp:Label ID="lblTitleMain" runat="server" Text="Gear Up Master"></asp:Label></span>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="titlesub">
                        <span class="titlesubfont">
                            <asp:Label ID="lblTitleSub1" runat="server" Text="Search Gear Up Master List"></asp:Label></span>
                    </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblProcName" runat="server" Text="Process Name"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddProcName" runat="server">
                        </asp:DropDownList></td>

                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblGearUpID" runat="server" Text="Gear Up ID"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txt_GearUpID" runat="server" CssClass="TextboxLongStyle"></asp:TextBox></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblPartId" runat="server" Text="Part ID"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtPartId" runat="server" CssClass="TextboxLongStyle" MaxLength="10"></asp:TextBox></td>
                </tr>
                <tr>

                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblLine" runat="server" Text="Line"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddLineType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddLineType_OnSelectedIndexChanged"></asp:DropDownList></td>
                    <%--<td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txt_Line" runat="server" CssClass="TextboxLongStyle" MaxLength="1"></asp:TextBox></td>--%>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblGwNo" runat="server" Text="G/W No"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txt_GwNo" runat="server" CssClass="TextboxLongStyle" MaxLength="2"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lbl_ModuleAddr" runat="server" Text="Lamp Module Address"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txt_ModuleAddr" runat="server" CssClass="TextboxLongStyle" MaxLength="4"></asp:TextBox></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lbl_PhysAddr" runat="server" Text="Lamp Physical Address"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txt_PhysAddr" runat="server" CssClass="TextboxLongStyle" MaxLength="2"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="4" class="alignRight">
                        <asp:Button ID="btnNewGroup" runat="server" CssClass="button" Text="Create" OnClick="btnNewGroup_Click" OnClientClick="Confirm('Confirm to save?')" />
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="button" OnClick="btnUpdate_Click" Visible="false" OnClientClick="Confirm('Confirm to update?')" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" OnClick="btnClear_Click" />
                        <asp:Button ID="btnCancel" runat="server" CssClass="button" Text="Cancel" OnClick="btnCancel_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblMsg" runat="server" Text="" Visible="false" CssClass="LabelWarnStyle"></asp:Label>
                        <asp:Label ID="lblTmpPartId" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblTmpLine" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblTmpGwNo" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblTmpGearID" runat="server" Visible="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:GridView ID="gvGearUp" runat="server" AutoGenerateColumns="false" AllowPaging="True" CssClass="DataTable" OnPageIndexChanging="gvGearUp_PageIndexChanging" AllowSorting="True" EmptyDataText="No Data">
                            <Columns>
                                <asp:BoundField HeaderText="Gear Up ID" DataField="Gear_id" />
                                <asp:BoundField HeaderText="PLC No" DataField="plc_no" />
                                <asp:BoundField HeaderText="Process Name" DataField="proc_name" />
                                <asp:BoundField HeaderText="Part ID" DataField="PartsID" />
                                <asp:BoundField HeaderText="Line" DataField="Line" />
                                <asp:BoundField HeaderText="G/W No" DataField="GwNo" />
                                <asp:BoundField HeaderText="Lamp Module Address" DataField="LmModuleAddress" />
                                <asp:BoundField HeaderText="Lamp Physical Address" DataField="LmPhysicalAddress" />
                                <asp:BoundField HeaderText="Last Updated By" DataField="last_upd_by" />
                                <asp:BoundField HeaderText="Last Updated Date/Time" DataField="last_upd_dt" />
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
