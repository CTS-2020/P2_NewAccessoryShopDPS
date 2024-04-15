<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManUpdDpsInsData.aspx.cs" Inherits="ManUpdDpsInsData" %>

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
                    <span class="titlemain"><asp:Label ID="lblTitleMain" runat="server" 
                        Text="Search Manual Update DPS Instruction Data"></asp:Label></span>
                </td>
                </tr>
                <tr>
                <td colspan="4" class="titlesub">
                    <span class="titlesubfont"><asp:Label ID="lblTitleSub1" runat="server" 
                        Text="Manual Update DPS Instruction Data"></asp:Label></span>
                </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblProcName" runat="server" Text="Process Name"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddProcName" runat="server"></asp:DropDownList></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblPlcNo" runat="server" Text="PLC No"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtPlcNo" runat="server" CssClass="TextboxLongStyle"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblPointer" runat="server" Text="Pointer"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtPointer" runat="server" CssClass="TextboxLongStyle"></asp:TextBox></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblModel" runat="server" Text="Model"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtModel" runat="server" CssClass="TextboxLongStyle"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblChasNo" runat="server" Text="Chassis No"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtChasNo" runat="server" CssClass="TextboxLongStyle"></asp:TextBox></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblSfx" runat="server" Text="SFX"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtSfx" runat="server" CssClass="TextboxLongStyle"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblColor" runat="server" Text="Color"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtColor" runat="server" CssClass="TextboxLongStyle"></asp:TextBox></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblInsCode" runat="server" Text="DPS Instruction Code"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtInsCode" runat="server" CssClass="TextboxLongStyle"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblBseq" runat="server" Text="B.Seq"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtBseq" runat="server" CssClass="TextboxLongStyle"></asp:TextBox></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblIdn" runat="server" Text="IDN"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtIdn" runat="server" CssClass="TextboxLongStyle"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblIdVer" runat="server" Text="ID Ver"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtIdVer" runat="server" CssClass="TextboxLongStyle"></asp:TextBox></td>
                    <td class="tdtextbox" style="width: 20%; height: 28px"></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px"></td>
                </tr>
                <tr>
                    <td colspan="4" class="alignRight" style="height: 32px">
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" OnClick="btnClear_Click"/>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button" OnClick="btnSearch_Click"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:GridView ID="gvDpsRsConv" runat="server" AutoGenerateColumns="false" AllowPaging="True" CssClass="DataTable" OnRowDataBound="gvDpsRsConv_RowDataBound" OnRowCommand ="gvDpsRsConv_RowCommand" OnPageIndexChanging="gvDpsRsConv_PageIndexChanging" AllowSorting="True" EmptyDataText="No Data">
                            <Columns>
                                <asp:BoundField HeaderText="DPS Result Conversion ID" DataField="dps_rs_conv_id">
                                    <HeaderStyle CssClass="hidden"/>
                                    <ItemStyle CssClass="hidden"/>
                                </asp:BoundField>
                                <asp:BoundField HeaderText="PLC No" DataField="plc_no"/>
                                <asp:BoundField HeaderText="Pointer" DataField="write_pointer"/>
                                <asp:BoundField HeaderText="DPS Instruction Code" DataField="dps_ins_code"/>
                                <asp:BoundField HeaderText="ID No" DataField="id_no"/>
                                <asp:BoundField HeaderText="ID Ver" DataField="id_ver"/>
                                <asp:BoundField HeaderText="Chassis No" DataField="chassis_no"/>
                                <asp:BoundField HeaderText="B.Seq" DataField="bseq"/>
                                <asp:BoundField HeaderText="Model" DataField="model"/>
                                <asp:BoundField HeaderText="Sfx" DataField="sfx"/>
                                <asp:BoundField HeaderText="Colour Code" DataField="color_code"/>
                                <asp:BoundField HeaderText="Last Updated" DataField="last_updated"/>
                                <asp:ButtonField CommandName="EditRecord" ButtonType="Link" Text="Edit"/>
                                <asp:ButtonField CommandName="DeleteRecord" ButtonType="Link" Text="Delete"/>
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
