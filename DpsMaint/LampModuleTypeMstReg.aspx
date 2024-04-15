<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LampModuleTypeMstReg.aspx.cs" Inherits="LampModuleTypeMst" %>

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
                    <span class="titlemain"><asp:Label ID="lblTitleMain" runat="server" Text="Lamp Module Type Master"></asp:Label></span>
                </td>
                </tr>
                <tr>
                <td colspan="4" class="titlesub">
                    <span class="titlesubfont"><asp:Label ID="lblTitleSub1" runat="server" Text="Create/Update Lamp Module Type Master"></asp:Label></span>
                </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 12%; height: 28px">
                        <asp:Label ID="lblEquipType" runat="server" Text="Equipment Type"></asp:Label></td>
                    <td class="tdtextbox" style="width: 10%; height: 28px">
                        <asp:DropDownList ID="ddEquipType" runat="server">
                        </asp:DropDownList></td>
                    <td class="tdfield" style="width: 13%; height: 28px">
                        <asp:Label ID="lblModuleType" runat="server" Text="Module Type"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtModuleType" runat="server" CssClass="TextboxLongStyle" MaxLength="10"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 12%; height: 28px; background-color: transparent;">
                        <asp:Label ID="Label11" runat="server" Text="-During Instruction-" style="color: black"></asp:Label></td>
                    <td class="tdtextbox" style="width: 10%; height: 28px">
                    </td>
                    <td class="tdfield" style="width: 13%; height: 28px; background-color: transparent;">
                    </td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                    </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 12%; height: 28px">
                        <asp:Label ID="lblLampLightingDI" runat="server" Text="Lamp Lighting"></asp:Label></td>
                    <td class="tdtextbox" style="width: 10%; height: 28px">
                        <asp:DropDownList ID="ddLampLightingDI" runat="server" AutoPostBack=true
                            onselectedindexchanged="ddLampLightingDI_SelectedIndexChanged">
                        </asp:DropDownList></td>
                    <td class="tdfield" style="width: 13%; height: 28px">
                        <asp:Label ID="lblLampColorDI" runat="server" Text="Lamp Colour"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddLampColorDI" runat="server">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 12%; height: 28px; background-color: transparent;">
                        <asp:Label ID="Label12" runat="server" Text="-After Instruction-" style="color: black"></asp:Label></td>
                    <td class="tdtextbox" style="width: 10%; height: 28px">
                    </td>
                    <td class="tdfield" style="width: 13%; height: 28px; background-color: transparent;">
                    </td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                    </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 12%; height: 28px">
                        <asp:Label ID="lblLampLightingAI" runat="server" Text="Lamp Lighting"></asp:Label></td>
                    <td class="tdtextbox" style="width: 10%; height: 28px">
                        <asp:DropDownList ID="ddLampLightingAI" runat="server" AutoPostBack=true
                            onselectedindexchanged="ddLampLightingAI_SelectedIndexChanged">
                        </asp:DropDownList></td>
                    <td class="tdfield" style="width: 13%; height: 28px">
                        <asp:Label ID="lblLampColorAI" runat="server" Text="Lamp Colour"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddLampColorAI" runat="server">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td colspan="4" class="alignRight">
                        <asp:Button ID="btnNewLmType" runat="server" CssClass="button" Text="Create" OnClick="btnNewLmType_Click" OnClientClick="Confirm('Confirm to save?')"/>
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="button" OnClick="btnUpdate_Click" Visible="false" OnClientClick="Confirm('Confirm to update?')"/>
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" OnClick="btnClear_Click"/>                        <asp:Button ID="btnCancel" runat="server" CssClass="button" Text="Cancel" OnClick="btnCancel_Click"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblMsg" runat="server" Text="" Visible="false" CssClass="LabelWarnStyle"></asp:Label>
                        &nbsp;<asp:Label ID="lblTmpModuleType" runat="server" Visible="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:GridView ID="gvLampModuleTypeMst" runat="server" AutoGenerateColumns="false" AllowPaging="True" CssClass="DataTable" OnPageIndexChanging="gvLampModuleTypeMst_PageIndexChanging" AllowSorting="True" EmptyDataText="No Data">
                            <Columns>
                                <asp:BoundField HeaderText="Module Type" DataField="module_type"/>
                                <asp:BoundField HeaderText="Equipment Type" DataField="equip_type"/>
                                <asp:BoundField HeaderText="During Inst Light" DataField="light_di"/>
                                <asp:BoundField HeaderText="During Inst Colour" DataField="color_di"/>
                                <asp:BoundField HeaderText="After Inst Light" DataField="light_ai"/>
                                <asp:BoundField HeaderText="After Inst Colour" DataField="color_ai"/>
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
