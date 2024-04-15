<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DpsPlcMstReg.aspx.cs" Inherits="DpsPlcMst" %>

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
                    <span class="titlemain"><asp:Label ID="lblTitleMain" runat="server" Text="DPS PLC Master"></asp:Label></span>
                </td>
                </tr>
                <tr>
                <td colspan="4" class="titlesub">
                    <span class="titlesubfont"><asp:Label ID="lblTitleSub1" runat="server" Text="Create/Update DPS PLC Master"></asp:Label></span>
                </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblPlcNo" runat="server" Text="PLC No"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtPlcNo" runat="server" CssClass="TextboxStyle"></asp:TextBox>
                    </td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblProcName" runat="server" Text="Process Name"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtProcName" runat="server" CssClass="TextboxStyle" MaxLength="20"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblIpAdd" runat="server" Text="IP Address (Reference)"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtIpAdd" runat="server" CssClass="TextboxStyle" MaxLength="20"></asp:TextBox></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblPlcModel" runat="server" Text="GW Model"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddPlcModel" runat="server" CssClass="TextboxLongStyle"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblLogicalStationNo" runat="server" Text="PLC Logical Station No"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtPlcLogicalStationNo" runat="server" CssClass="TextboxStyle"></asp:TextBox>
                    </td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblPlcNwStation" runat="server" Text="PLC N/W Station (Reference)"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtPlcNwStation" runat="server" CssClass="TextboxStyle" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblEnable" runat="server" Text="Enable"></asp:Label>
                        </td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:CheckBox ID="cbEnable" runat="server" />
                        </td>
                </tr>
                <tr>
                    <td colspan="4" class="alignRight">
                        <asp:Button ID="btnNewPlcMst" runat="server" CssClass="button" Text="Create" OnClick="btnNewPlcMst_Click" OnClientClick="Confirm('Confirm to save?')"/>
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="button" OnClick="btnUpdate_Click" Visible="false" OnClientClick="Confirm('Confirm to update? ATTENTION : THIS WILL UPDATE ALL GROUP MASTER, BLOCK MASTER, RACK MASTER DATA REGISTERED UNDER CURRENT PLC!')"/>
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" OnClick="btnClear_Click"/>
                        <asp:Button ID="btnCancel" runat="server" CssClass="button" Text="Cancel" OnClick="btnCancel_Click"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblMsg" runat="server" Text="" Visible="false" CssClass="LabelWarnStyle"></asp:Label>
                        &nbsp;
                        <asp:Label ID="lblTmpPlcNo" runat="server" Visible="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:GridView ID="gvPlcMstList" runat="server" AutoGenerateColumns="false" 
                            AllowPaging="True" CssClass="DataTable" 
                            OnPageIndexChanging="gvPlcMstList_PageIndexChanging" AllowSorting="True" 
                            EmptyDataText="No Data" PageSize="12">
                            <Columns>
                                <asp:BoundField HeaderText="PLC No" DataField="plc_no"/>
                                <asp:BoundField HeaderText="Process Name" DataField="proc_name"/>
                                <asp:BoundField HeaderText="IP Address (Reference)" DataField="ip_add"/>                                
                                <asp:BoundField HeaderText="GW Model" DataField="plc_model" />
                                <asp:BoundField HeaderText="PLC Logical Station No" HeaderStyle-Width="10%" DataField="plc_logical_station_no"/>
                                <asp:BoundField HeaderText="PLC N/W Station (Reference)" HeaderStyle-Width="5%" DataField="plc_nw_station"/>
                                <asp:BoundField HeaderText="Enable" DataField="enable"/>
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
