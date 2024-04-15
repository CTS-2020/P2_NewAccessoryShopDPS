<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RackMstReg.aspx.cs" Inherits="RackMst" %>

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
                    <span class="titlemain"><asp:Label ID="lblTitleMain" runat="server" Text="Rack Master"></asp:Label></span>
                </td>
                </tr>
                <tr>
                <td colspan="4" class="titlesub">
                    <span class="titlesubfont"><asp:Label ID="lblTitleSub1" runat="server" Text="Create/Update Rack Master"></asp:Label></span>
                </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblProcName" runat="server" Text="Process Name"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddProcName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddProcName_OnSelectedIndexChanged">
                        </asp:DropDownList></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblGroupName" runat="server" Text="Group Name"></asp:Label></td>
                    <td class="tdtextbox" style="width: 29%; height: 28px">
                        <asp:DropDownList ID="ddGroupName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddGroupName_OnSelectedIndexChanged">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 15%; height: 28px">
                        <asp:Label ID="lblPlcModel" runat="server" Text="GW Model"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddPlcModel" runat="server" AutoPostBack="true" Enabled="false">
                        </asp:DropDownList></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblBlockName" runat="server" Text="Block Name"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddBlockName" runat="server">
                        </asp:DropDownList></td>
                    <%--<td class="tdfield" style="width: 20%; height: 28px; background-color: transparent;">
                        </td>
                    <td class="tdtextbox" style="width: 29%; height: 28px">
                        </td>--%>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px; color: black; background-color: transparent;">
                        <asp:Label ID="Label3" runat="server" Text="-Rack Image-"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:RangeValidator ID="rvRowCnt" runat="server" 
                            ErrorMessage="Row cannot be less than 1." ControlToValidate="txtRowCnt" 
                            MinimumValue="1" MaximumValue="999999" Type="Integer"></asp:RangeValidator>
                    </td>
                    <td class="tdfield" style="width: 20%; height: 28px; background-color: transparent;">
                    </td>
                    <td class="tdtextbox" style="width: 29%; height: 28px">
                        <asp:RangeValidator ID="rvColCnt" runat="server" 
                            ErrorMessage="Column cannot be less than 1." ControlToValidate="txtColCnt" 
                            MinimumValue="1" MaximumValue="999999" Type="Integer"></asp:RangeValidator>
                    </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblRackName" runat="server" Text="Rack Name"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtRackName" runat="server" MaxLength="20"></asp:TextBox></td>
                    <td class="tdfield" style="width: 20%; height: 28px; background-color: transparent;">
                    </td>
                    <td class="tdtextbox" style="width: 29%; height: 28px">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblRowCnt" runat="server" Text="Row"></asp:Label></td>
                    <td class="tdtextbox" style="width: 29%; height: 28px">
                        <asp:TextBox ID="txtRowCnt" runat="server" ReadOnly="True" Enabled="false">1</asp:TextBox>&nbsp;
                        <asp:Button ID="btnRowInc" runat="server" Text="+" width="30px" OnClick="incRowCnt" CausesValidation="False"/>&nbsp;
                        <asp:Button ID="btnRowDec" runat="server" Text="-" width="30px" OnClick="decRowCnt"/></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblColCnt" runat="server" Text="Column"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtColCnt" runat="server" ReadOnly="True" Enabled="false">1</asp:TextBox>&nbsp;
                        <asp:Button ID="btnColInc" runat="server" Text="+" width="30px" OnClick="incColCnt" CausesValidation="False"/>&nbsp;
                        <asp:Button ID="btnColDec" runat="server" Text="-" width="30px" OnClick="decColCnt"/></td>
                </tr>
                <tr>
                    <td colspan="4" class="alignRight">
                        <asp:Button ID="btnRackSimulate" runat="server" Text="Rack Simulate" CssClass="button" OnClick="previewTable"/>
                        <asp:Button ID="btnNewRack" runat="server" CssClass="button" Text="Create" OnClick="btnNewRack_Click" OnClientClick="Confirm('Confirm to save?')"/>
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="button" OnClick="btnUpdate_Click" Visible="false" OnClientClick="Confirm('Confirm to update?')"/>
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" OnClick="btnClear_Click"/>                        <asp:Button ID="btnCancel" runat="server" CssClass="button" Text="Cancel" OnClick="btnCancel_Click"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblMsg" runat="server" Text="" Visible="false" CssClass="LabelWarnStyle"></asp:Label>
                        <asp:Label ID="lblTmpRackName" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblTmpRowCnt" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblTmpColCnt" runat="server" Visible="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Table ID="RackMstPreview" runat="server" width="100%" border="1"  Visible="False"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:GridView ID="gvRack" runat="server" AutoGenerateColumns="false" AllowPaging="True" CssClass="DataTable" OnPageIndexChanging="gvRack_PageIndexChanging" AllowSorting="True" EmptyDataText="No Data">
                            <Columns>
                                <asp:BoundField HeaderText="Rack Name" DataField="rack_name"/>
                                <asp:BoundField HeaderText="Column" DataField="col_cnt"/>
                                <asp:BoundField HeaderText="Row" DataField="row_cnt"/>
                                <asp:BoundField HeaderText="Process Name" DataField="proc_name"/>
                                <asp:BoundField HeaderText="Group Name" DataField="group_name"/>
                                <asp:BoundField HeaderText="Block Name" DataField="block_name"/>
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
