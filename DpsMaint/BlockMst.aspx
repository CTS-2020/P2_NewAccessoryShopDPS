<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BlockMst.aspx.cs" Inherits="BlockMst" %>

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
                    <span class="titlemain"><asp:Label ID="lblTitleMain" runat="server" Text="Block Master"></asp:Label></span>
                </td>
                </tr>
                <tr>
                <td colspan="4" class="titlesub">
                    <span class="titlesubfont"><asp:Label ID="lblTitleSub1" runat="server" Text="Search Block Master List"></asp:Label></span>
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
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddGroupName" runat="server">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblBlockSeq" runat="server" Text="Block Seq"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtBlockSeq" runat="server" CssClass="TextboxLongStyle" MaxLength="2"></asp:TextBox></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblBlockName" runat="server" Text="Block Name"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtBlockName" runat="server" CssClass="TextboxLongStyle" 
                            MaxLength="10"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblGwNo" runat="server" Text="G/W No"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:TextBox ID="txtGwNo" runat="server" CssClass="TextboxLongStyle" MaxLength="2"></asp:TextBox></td>
                    <td class="tdfield" style="width: 20%; height: 28px; background-color: transparent">
                    </td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                    </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px; color: black; background-color: transparent;">
                        <asp:Label ID="Label8" runat="server" Text="-Instruction Type-"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        &nbsp;</td>
                    <td class="tdfield" style="width: 20%; height: 28px; background-color: transparent">
                    </td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblStartLm" runat="server" Text="Start LM"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddStartLm" runat="server"></asp:DropDownList></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblEndLm" runat="server" Text="End LM"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddEndLm" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblStartModuleType" runat="server" Text="Module Type"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddStartModuleType" runat="server"></asp:DropDownList></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblEndModuleType" runat="server" Text="Module Type"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddEndModuleType" runat="server"></asp:DropDownList></td>
                </tr>
                <%--<tr>
                    <td class="tdfield" style="width: 12%; height: 28px; background-color: transparent;">
                        <asp:Label ID="Label13" runat="server" Text="-Wait Instruction-" style="color: black" vis></asp:Label></td>
                    <td class="tdtextbox" style="width: 10%; height: 28px">
                    </td>
                    <td class="tdfield" style="width: 13%; height: 28px; background-color: transparent;">
                    </td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                    </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 12%; height: 28px">
                        <asp:Label ID="lblLampLightingWI" runat="server" Text="Lamp Lighting"></asp:Label></td>
                    <td class="tdtextbox" style="width: 10%; height: 28px">
                        <asp:DropDownList ID="ddLampLightingWI" runat="server">
                        </asp:DropDownList></td>
                    <td class="tdfield" style="width: 13%; height: 28px">
                        <asp:Label ID="lblLampColorWI" runat="server" Text="Lamp Colour"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddLampColorWI" runat="server">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 12%; height: 28px; background-color: transparent;">
                        <asp:Label ID="Label14" runat="server" Text="-Error-" style="color: black"></asp:Label></td>
                    <td class="tdtextbox" style="width: 10%; height: 28px">
                    </td>
                    <td class="tdfield" style="width: 13%; height: 28px; background-color: transparent;">
                    </td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                    </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 12%; height: 28px">
                        <asp:Label ID="lblLampLightingERR" runat="server" Text="Lamp Lighting"></asp:Label></td>
                    <td class="tdtextbox" style="width: 10%; height: 28px">
                        <asp:DropDownList ID="ddLampLightingERR" runat="server">
                        </asp:DropDownList></td>
                    <td class="tdfield" style="width: 13%; height: 28px">
                        <asp:Label ID="lblLampColorERR" runat="server" Text="Lamp Colour"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddLampColorERR" runat="server">
                        </asp:DropDownList></td>
                </tr>--%>
                <tr>
                    <td colspan="4" class="alignRight">
                        <asp:Button ID="btnNewBlock" runat="server" CssClass="button" Text="New" OnClick="btnNewBlock_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" OnClick="btnClear_Click"/>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button" OnClick="btnSearch_Click"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:GridView ID="gvBlockMst" runat="server" AutoGenerateColumns="false" AllowPaging="True" CssClass="DataTable" OnRowDataBound="gvBlockMst_RowDataBound" OnRowCommand ="gvBlockMst_RowCommand" OnPageIndexChanging="gvBlockMst_PageIndexChanging" AllowSorting="True" EmptyDataText="No Data">
                            <Columns>
                                <asp:BoundField ItemStyle-Wrap="false" HeaderText="PLC No" DataField="plc_no"/>
                                <asp:BoundField ItemStyle-Wrap="false" HeaderText="Process Name" DataField="proc_name"/>
                                <asp:BoundField ItemStyle-Wrap="false" HeaderText="Group Name" DataField="group_name"/>
                                <asp:BoundField ItemStyle-Wrap="false" HeaderText="Group Line" DataField="group_line"/>
                                <asp:BoundField ItemStyle-Wrap="false" HeaderText="Block Seq" DataField="block_seq"/>
                                <asp:BoundField ItemStyle-Wrap="false" HeaderText="Block Name" DataField="block_name"/>
                                <asp:BoundField ItemStyle-Wrap="false" HeaderText="G/W No" DataField="gw_no"/>
                                <asp:BoundField ItemStyle-Wrap="false" HeaderText="Start LM" DataField="start_lm"/>
                                <asp:BoundField ItemStyle-Wrap="false" HeaderText="Start LM Module Type" DataField="start_module_type"/>
                                <asp:BoundField ItemStyle-Wrap="false" HeaderText="End LM" DataField="end_lm"/>
                                <asp:BoundField ItemStyle-Wrap="false" HeaderText="End LM Module Type" DataField="end_module_type"/>
                                <asp:TemplateField ItemStyle-Wrap="false" HeaderText="Wait Inst Light">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLightWI" runat="server" Text='<%#GetLight(Eval("light_wi"))%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField ItemStyle-Wrap="false" HeaderText="Wait Inst Colour" DataField="color_wi"/>
                                <asp:TemplateField ItemStyle-Wrap="false" HeaderText="Error Light">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLightErr" runat="server" Text='<%#GetLight(Eval("light_err"))%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField ItemStyle-Wrap="false" HeaderText="Error Colour" DataField="color_err"/>
                                <asp:BoundField ItemStyle-Wrap="false" HeaderText="Last Updated By" DataField="last_upd_by"/>
                                <asp:BoundField ItemStyle-Wrap="false" HeaderText="Last Updated Date/Time" DataField="last_upd_dt"/>
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
