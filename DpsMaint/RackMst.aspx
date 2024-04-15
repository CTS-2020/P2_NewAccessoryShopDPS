<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RackMst.aspx.cs" Inherits="RackMst" %>

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
                    <span class="titlesubfont"><asp:Label ID="lblTitleSub1" runat="server" Text="Search Rack Master List"></asp:Label></span>
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
                </tr>
                <tr>
                    <td style="width: 15%; height: 28px"></td>
                    <td style="width: 30%; height: 28px"></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblRackName" runat="server" Text="Rack Name"></asp:Label>
                        </td>
                    <td class="tdtextbox" style="width: 29%; height: 28px">
                        <asp:TextBox ID="txtRackName" runat="server" MaxLength="20"></asp:TextBox>
                        </td>
                </tr>
                <tr>
                <td class="LeftTextArea">
                    <asp:label ID="lblUploadExcel" runat="server" Text="Upload Excel:" CssClass="LabelWhite"></asp:label><span style="color: #ff0000">&nbsp;</span></td>
                <td></td>
                <td class="RightTextArea">
                    <asp:FileUpload ID="btnBrowse" runat="server" />
                    <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="button"  Height="23px" OnClick="btnUpload_Click"/>
                <td>
                    <asp:Label ID="lblUploadStatus" runat="server" ForeColor="Red" Text="Please Select File To Upload"></asp:Label>&nbsp;
                </td>
                
            </tr>
                <tr>
                    <td colspan="4" class="alignRight">
                        <asp:Label ID="lblMsg" runat="server" Text="" Visible="false" CssClass="LabelWarnStyle"></asp:Label>
                        <asp:Label ID="lblTmpLmFlg" runat="server" Text="" Visible="false"></asp:Label>
                        <asp:Button ID="btnExport" runat="server" CssClass="button" Text="Export" OnClick="btnExport_Click"/>
                        <asp:Button ID="btnNewRack" runat="server" CssClass="button" Text="New" OnClick="btnNewRack_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" OnClick="btnClear_Click"/>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button" OnClick="btnSearch_Click"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:GridView ID="gvRack" runat="server" AutoGenerateColumns="false" AllowPaging="True" CssClass="DataTable" OnRowDataBound="gvRack_RowDataBound" OnRowCommand ="gvRack_RowCommand" OnPageIndexChanging="gvRack_PageIndexChanging" AllowSorting="True" EmptyDataText="No Data">
                            <Columns>
                                <asp:BoundField HeaderText="Rack Name" DataField="rack_name"/>
                                <asp:BoundField HeaderText="Column" DataField="col_cnt"/>
                                <asp:BoundField HeaderText="Row" DataField="row_cnt"/>
                                <asp:BoundField HeaderText="PLC No" DataField="plc_no"/>
                                <asp:BoundField HeaderText="Process Name" DataField="proc_name"/>
                                <asp:BoundField HeaderText="Group Name" DataField="group_name"/>
                                <asp:BoundField HeaderText="Block Name" DataField="block_name"/>
                                <asp:BoundField HeaderText="Last Updated By" DataField="last_upd_by"/>
                                <asp:BoundField HeaderText="Last Updated Date/Time" DataField="last_upd_dt"/>
                                <asp:ButtonField CommandName="EditRecord" ButtonType="Link" Text="Edit Rack"/>
                                <asp:ButtonField CommandName="EditPartsRecord" ButtonType="Link" Text="Edit Rack Parts"/>
                                <asp:ButtonField CommandName="DeleteRecord" ButtonType="Link" Text="Delete"/>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: center" class="style1">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: center;">
                    </td>
                </tr>
                
               <tr>
                    <td colspan="4">
                        <asp:GridView ID="gvErrorLog" runat="server" AutoGenerateColumns="false" AllowPaging="False" CssClass="DataTable" OnRowDataBound = "Errorlogformatting"  AllowSorting="True" EmptyDataText="No Data">
                            <Columns>
                                <asp:BoundField HeaderText="Plc No" DataField="plc_no"/>
                                <asp:BoundField HeaderText="Rack Name" DataField="rack_name"/>
                                <asp:BoundField HeaderText="Group Name" DataField="group_name"/>
                                <asp:BoundField HeaderText="Row" DataField="row"/>
                                <asp:BoundField HeaderText="Proc Name" DataField="proc_name"/>
                                <asp:BoundField HeaderText="Column" DataField="column"/>
                                <asp:BoundField HeaderText="Block" DataField="block"/>
                                <asp:BoundField HeaderText="Part Titles" DataField="AisData"/>
                                <asp:BoundField HeaderText="Part/Color Sfx" DataField="PartColor"/>
                                <asp:BoundField HeaderText="Module" DataField="Module"/>
                                <asp:BoundField HeaderText="Column Position" DataField="actcol"/>
                                <asp:BoundField HeaderText="Row Position" DataField="actrow"/>
                                <asp:BoundField HeaderText="Result" DataField="Result"/>
                                <asp:BoundField HeaderText="Error log" DataField="Error_log" HtmlEncode="false" />
  
                              
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
