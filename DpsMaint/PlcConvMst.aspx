<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlcConvMst.aspx.cs" Inherits="PlcConvMst" %>

<?xml version="1.0" encoding="UTF-8"?><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="content-type" content="application/xhtml+xml;charset=UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=Edge"/>
    <title>Perodua DPS Application</title>
    <link href="../stylesheet.css?v=1" rel="stylesheet" type="text/css"/>
    <link href="../webfunction/webfunc.css?v=1" rel="stylesheet" type="text/css" />
    <script src="../webfunction/jquery-2.1.4.min.js?v=1" type="text/javascript"></script>
    <script src="../webfunction/webfunc.js?v=1" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table class="table" width="100%" border="0">
                <tr>
                <td colspan="4">
                    <span class="titlemain"><asp:Label ID="lblTitleMain" runat="server" Text="PLC Conversion Master"></asp:Label></span>
                </td>
                </tr>
                <tr>
                <td colspan="4" class="titlesub">
                    <span class="titlesubfont"><asp:Label ID="lblTitleSub1" runat="server" Text="PLC Conversion Master List"></asp:Label></span>
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
                        <asp:DropDownList ID="ddGroupName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddGroupName_OnSelectedIndexChanged">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblBlockName" runat="server" Text="Block Name"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddBlockName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddBlockName_OnSelectedIndexChanged">
                        </asp:DropDownList></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblRackName" runat="server" Text="Rack Name"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddRackName" runat="server" AutoPostBack="true" 
                            onselectedindexchanged="ddRackName_SelectedIndexChanged" >
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px; color: black; background-color: transparent;">
                        <asp:Label ID="Label3" runat="server" Text="-Confirmation-"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                    </td>
                    <td class="tdfield" style="width: 20%; height: 28px; background-color: transparent;">
                        &nbsp;</td>
                    <td class="alignRight" style="width: 30%; height: 28px" >
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button" OnClick="btnSearch_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblInsCode" runat="server" Text="DPS Instruction Code"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddInsCode" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddInsCode_OnSelectedIndexChanged">
                        </asp:DropDownList></td>
                    <td class="tdfield" style="width: 20%; height: 28px; background-color: transparent;">
                    </td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
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
                        <asp:Label ID="lblSfx" runat="server" Text="Sfx"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddSfx" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddSfx_OnSelectedIndexChanged">
                        </asp:DropDownList></td>
                    <td class="tdfield" style="width: 20%; height: 28px">
                        <asp:Label ID="lblColor" runat="server" Text="Color"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                        <asp:DropDownList ID="ddColor" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddColor_OnSelectedIndexChanged">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblMsg" runat="server" Text="" Visible="false" CssClass="LabelWarnStyle"></asp:Label>
                        <asp:Label ID="lblTmpLmFlg" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="alignRight" style="height: 32px">
                        <asp:Button ID="btnExport" runat="server" CssClass="button" Text="Export" OnClick="btnExport_Click"/>
                        <asp:Button ID="btnConfirm" runat="server" Text="Confirm" CssClass="button" OnClick="btnConfirm_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" OnClick="btnClear_Click"/>
                    </td>
                </tr>
                <tr>
                    <td class="tdfield" style="width: 20%; height: 28px; color: black; background-color: transparent;">
                        <asp:Label ID="Label11" runat="server" Text="-Rack Image-" Visible="false"></asp:Label></td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                    </td>
                    <td class="tdfield" style="width: 20%; height: 28px; background-color: transparent;">
                    </td>
                    <td class="tdtextbox" style="width: 30%; height: 28px">
                    </td>
                </tr>
                <tr>
                    <asp:Panel ID="AisPreview" runat="server"/>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: center;">
                        <asp:Button ID="btnSubmit" runat="server" CssClass="button" Text="Submit" OnClick="btnSubmit_Click" OnClientClick = "Confirm('Confirm to Submit?');ShowProgress('submit');"/>
                        <asp:Button ID="btnSubmitAll" runat="server" CssClass="button" Text="Submit All" OnClick="btnSubmitAll_Click" OnClientClick = "Confirm('Confirm to Submit All?');ShowProgress('submit');"/>
                    </td>
                </tr>
            </table>
        </div>
        <div class="loadingsubmit" align="center">
            Submitting. Please wait.<br />
            DO NOT close the browser !!!<br />
            <br />
            <img src="../styles/images/icon/loading.gif" alt="" />
        </div>
    </form>
</body>
</html>
