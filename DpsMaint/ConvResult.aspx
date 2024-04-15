<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConvResult.aspx.cs" Inherits="ConvResult" %>

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
                    <span class="titlemain"><asp:Label ID="lblTitleMain" runat="server" Text="Conversion Result"></asp:Label></span>
                </td>
                </tr>
                <tr>
                <td colspan="4" class="titlesub">
                    <span class="titlesubfont"><asp:Label ID="lblTitleSub1" runat="server" Text="Conversion Result List"></asp:Label></span>
                </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblProcName" runat="server" Text="Process Name"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtProcName" runat="server" CssClass="TextboxLongStyle" ReadOnly="true"></asp:TextBox></td>
                    <td colspan="4" class="alignRight">
                        <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="button" OnClick="btnRefresh_Click"/>
                        <asp:Button ID="btnClose" runat="server" CssClass="button" Text="Close" OnClientClick="javascript:window.close();"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblMsg" runat="server" Text="" Visible="false" CssClass="LabelWarnStyle"></asp:Label>
                        <asp:Label ID="lblTmpPlcNo" runat="server" Visible="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <table width = "100%" border="0">
                        <tr>
                            <td style="width: 50px; height: 10px; background-color:#A4A4A4;">
                            </td>
                            <td> : Obtain data by PLC
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 50px; height: 10px; background-color:#F7FE2E;">
                            </td>
                            <td> : Not yet obtain Data by PLC
                            </td>
                        </tr>
                    </table>
                </tr>
                <tr style="height: 50px">
                    <td style="height: 50px"></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:GridView ID="gvConvResult" runat="server" AutoGenerateColumns="false" AllowCustomPaging="True" AllowPaging="True" PageSize="100" CssClass="DataTbl" OnRowDataBound="gvConvResult_RowDataBound" OnDataBound="gvConvResult_DataBound" OnPageIndexChanging="gvConvResult_PageIndexChanging" AllowSorting="True" EmptyDataText="No Data">
                            <Columns>
                                <asp:BoundField HeaderText="DPS Result Conversion ID" DataField="dps_rs_conv_id">
                                    <HeaderStyle CssClass="hidden"/>
                                    <ItemStyle CssClass="hidden"/>
                                </asp:BoundField>
                                <asp:BoundField ItemStyle-Wrap="false" HeaderText="Pointer" DataField="write_pointer"/>
                                <asp:TemplateField ItemStyle-Wrap="false" HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%#GetStatus(Eval("read_flag"))%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField ItemStyle-Wrap="false" HeaderText="B.Seq" DataField="bseq"/>
                                <asp:BoundField ItemStyle-Wrap="false" HeaderText="IDN" DataField="id_no"/>
                                <asp:BoundField ItemStyle-Wrap="false" HeaderText="IDNVer" DataField="id_ver"/>
                                <asp:BoundField ItemStyle-Wrap="false" HeaderText="Chassis No" DataField="chassis_no"/>
                                <asp:BoundField ItemStyle-Wrap="false" HeaderText="Instruction Code" DataField="dps_ins_code"/>
                                <asp:BoundField ItemStyle-Wrap="false" HeaderText="Model" DataField="model"/>
                                <asp:BoundField ItemStyle-Wrap="false" HeaderText="Sfx" DataField="sfx"/>
                                <asp:BoundField ItemStyle-Wrap="false" HeaderText="Colour" DataField="color_code"/>
                                <asp:BoundField ItemStyle-Wrap="false" HeaderText="Last Updated" DataField="last_updated"/>
                            </Columns>
                            <PagerTemplate>
                                <table width="100%">
                                    <tr>
                                        <td style="text-align:center">
                                            <asp:PlaceHolder ID="phPager" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </PagerTemplate>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
