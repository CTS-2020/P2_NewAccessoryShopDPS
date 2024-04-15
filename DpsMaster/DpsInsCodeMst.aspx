<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DpsInsCodeMst.aspx.cs" Inherits="DpsInsCodeMst" %>

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
                    <span class="titlemain"><asp:Label ID="lblTitleMain" runat="server" Text="DPS Instruction Code Master"></asp:Label></span>
                </td>
                </tr>
                <tr>
                <td colspan="4" class="titlesub">
                    <span class="titlesubfont"><asp:Label ID="lblTitleSub1" runat="server" Text="Search DPS Instruction Code Master List"></asp:Label></span>
                </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblModel" runat="server" Text="Model"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddModel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddModel_OnSelectedIndexChanged">
                        </asp:DropDownList></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblKatashiki" runat="server" Text="Katashiki"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddKatashiki" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddKatashiki_OnSelectedIndexChanged">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblSfx" runat="server" Text="SFX"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddSfx" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddSfx_OnSelectedIndexChanged">
                        </asp:DropDownList></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblColor" runat="server" Text="Color"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddColor" runat="server" AutoPostBack="true">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblInsCode" runat="server" Text="DPS Instruction Code"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtInsCode" runat="server" CssClass="TextboxLongStyle" MaxLength="6"></asp:TextBox></td>
                    <td class="tdfield" style="width: 20%; height: 28px; background-color: transparent;">
                        </td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblComment" runat="server" Text="Comment"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtComment" runat="server" CssClass="TextboxLongStyle" MaxLength="14"></asp:TextBox></td>
                    <td class="tdfield" style="width: 20%; height: 28px; background-color: transparent;">
                        </td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        </td>
                </tr>
                <tr>
                    <td colspan="4" class="alignRight">
                        <asp:Button ID="btnNewInsCode" runat="server" CssClass="button" Text="New" OnClick="btnNewInsCode_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" OnClick="btnClear_Click"/>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button" OnClick="btnSearch_Click"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:RangeValidator ID="vdInsCodeRange" runat="server" Type="Integer" 
                            MinimumValue="1" MaximumValue="999999" ControlToValidate="txtInsCode" 
                            ErrorMessage="DPS Instruction Code must be a whole number between 1 and 999999" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:GridView ID="gvInsCodeMstList" runat="server" AutoGenerateColumns="false" AllowPaging="True" CssClass="DataTable" OnRowDataBound="gvInsCodeMstList_RowDataBound" OnRowCommand ="gvInsCodeMstList_RowCommand" OnPageIndexChanging="gvInsCodeMstList_PageIndexChanging" AllowSorting="True" EmptyDataText="No Data">
                            <Columns>
                                <asp:BoundField HeaderText="DPS Instruction Code" DataField="ins_code"/>
                                <asp:BoundField HeaderText="SFX" DataField="sfx"/>
                                <asp:BoundField HeaderText="Color" DataField="color"/>
                                <asp:BoundField HeaderText="Katashiki" DataField="katashiki"/>
                                <asp:BoundField HeaderText="Model" DataField="model"/>
                                <asp:BoundField HeaderText="Comment" DataField="comment"/>
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
