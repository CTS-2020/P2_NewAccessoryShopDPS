using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Net;
using dpant;
using Itenso.Rtf;
using Itenso.Rtf.Converter.Html;
using Itenso.Rtf.Converter.Image;
using Itenso.Rtf.Converter.Text;
using Itenso.Rtf.Interpreter;
using Itenso.Rtf.Parser;
using Itenso.Rtf.Support;

public partial class PlcConvMst : System.Web.UI.Page
{

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                getDdProcName();
                getDdInsCode();
                getDdModel();
            }
            catch (Exception ex)
            {
                GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            }
        }
        lblMsg.Visible = false;
    }
    #endregion

    #region Method
    #region Get Drop Down List
    #region getDdProcName
    private void getDdProcName()
    {
        try
        {
            ddProcName.DataSource = GlobalFunc.getProcName();
            ddProcName.DataTextField = "Description";
            ddProcName.DataValueField = "Code";
            ddProcName.DataBind();
            ddProcName.Items.Insert(0, " ");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region getDdGroupName
    private void getDdGroupName()
    {
        try
        {
            String strProcName = Convert.ToString(ddProcName.SelectedItem);
            ddGroupName.DataSource = GlobalFunc.getGroupName(strProcName);
            ddGroupName.DataTextField = "Description";
            ddGroupName.DataValueField = "Description";
            ddGroupName.DataBind();
            ddGroupName.Items.Insert(0, " ");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region getDdBlockName
    private void getDdBlockName()
    {
        try
        {
            String strProcName = Convert.ToString(ddProcName.SelectedItem);
            String strGroupName = Convert.ToString(ddGroupName.SelectedValue);

            ddBlockName.DataSource = GlobalFunc.getBlockName(strProcName, strGroupName);
            ddBlockName.DataTextField = "Description";
            ddBlockName.DataValueField = "Description";
            ddBlockName.DataBind();
            ddBlockName.Items.Insert(0, " ");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region getDdRackName
    private void getDdRackName()
    {
        try
        {
            String strProcName = Convert.ToString(ddProcName.SelectedItem);
            String strGroupName = Convert.ToString(ddGroupName.SelectedValue);
            String strBlockName = Convert.ToString(ddBlockName.SelectedValue);

            ddRackName.DataSource = GlobalFunc.getRackName(strProcName, strGroupName, strBlockName);
            ddRackName.DataTextField = "Description";
            ddRackName.DataValueField = "Description";
            ddRackName.DataBind();
            ddRackName.Items.Insert(0, " ");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region getDdModel
    private void getDdModel()
    {
        try
        {
            ddModel.DataSource = GlobalFunc.getModel();
            ddModel.DataTextField = "Description";
            ddModel.DataValueField = "Description";
            ddModel.DataBind();
            ddModel.Items.Insert(0, " ");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region getDdKatashiki
    private void getDdKatashiki()
    {
        try
        {
            String strModel = ddModel.SelectedValue;
            ddKatashiki.DataSource = GlobalFunc.getKatashiki(strModel);
            ddKatashiki.DataTextField = "Description";
            ddKatashiki.DataValueField = "Description";
            ddKatashiki.DataBind();
            ddKatashiki.Items.Insert(0, " ");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region getDdSfx
    private void getDdSfx()
    {
        try
        {
            String strModel = ddModel.SelectedValue;
            String strKatashiki = ddKatashiki.SelectedValue;
            ddSfx.DataSource = GlobalFunc.getSfx(strModel, strKatashiki);
            ddSfx.DataTextField = "Description";
            ddSfx.DataValueField = "Description";
            ddSfx.DataBind();
            ddSfx.Items.Insert(0, " ");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region getDdColor
    private void getDdColor()
    {
        try
        {
            String strModel = ddModel.SelectedValue;
            String strKatashiki = ddKatashiki.SelectedValue;
            String strSfx = ddSfx.SelectedValue;
            ddColor.DataSource = GlobalFunc.getColor(strModel, strKatashiki, strSfx);
            ddColor.DataTextField = "Description";
            ddColor.DataValueField = "Description";
            ddColor.DataBind();
            ddColor.Items.Insert(0, " ");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region getDdInsCode
    private void getDdInsCode()
    {
        try
        {
            ddInsCode.DataSource = GlobalFunc.getInsCode();
            ddInsCode.DataTextField = "Description";
            ddInsCode.DataValueField = "Description";
            ddInsCode.DataBind();
            ddInsCode.Items.Insert(0, " ");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion
    #endregion

    #region SearchInsCodeMstList
    private DataTable SearchInsCodeMstList(String DpsInsCode, String Model, String Katashiki, String Sfx, String Color, String Comment)
    {
        DataSet dsSearch = new DataSet();
        DataTable dtSearch = new DataTable();

        try
        {
            dsSearch = csDatabase.SrcInsCode(DpsInsCode, Model, Katashiki, Sfx, Color, Comment);
            dtSearch = dsSearch.Tables[0];
            return dtSearch;
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return dtSearch;
        }
    }
    #endregion

    #region ClearAll
    private void ClearAll()
    {
        try
        {
            ddProcName.SelectedIndex = 0;
            ddGroupName.SelectedIndex = 0;
            ddBlockName.SelectedIndex = 0;
            ddRackName.SelectedIndex = 0;
            ddModel.SelectedIndex = 0;
            ddKatashiki.SelectedIndex = 0;
            ddSfx.SelectedIndex = 0;
            ddColor.SelectedIndex = 0;
            ddInsCode.SelectedIndex = 0;
            AisPreview.Controls.Clear();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Get Ins Code
    protected void getInsCode()
    {
        try
        {
            #region Check Assign Variable
            if (ddModel.SelectedIndex == 0)
            {
                ddInsCode.SelectedIndex = 0;
                return;
            }
            if (ddKatashiki.SelectedIndex == 0)
            {
                ddInsCode.SelectedIndex = 0;
                return;
            }
            if (ddSfx.SelectedIndex == 0)
            {
                ddInsCode.SelectedIndex = 0;
                return;
            }
            if (ddColor.SelectedIndex == 0)
            {
                ddInsCode.SelectedIndex = 0;
                return;
            }
            #endregion

            String strModel = Convert.ToString(ddModel.SelectedValue);
            String strKatashiki = Convert.ToString(ddKatashiki.SelectedValue);
            String strSfx = Convert.ToString(ddSfx.SelectedValue);
            String strColor = Convert.ToString(ddColor.SelectedValue);

            DataSet dsInsMst = new DataSet();
            DataTable dtInsMst = new DataTable();

            dtInsMst = SearchInsCodeMstList("", strModel, strKatashiki, strSfx, strColor, "");

            if (dtInsMst.Rows.Count > 0)
            {
                if (Convert.ToString(dtInsMst.Rows[0]["ins_code"]).Trim() != "")
                {
                    ddInsCode.SelectedValue = Convert.ToString(dtInsMst.Rows[0]["ins_code"]).Trim();
                }
            }
            else
            {
                ddInsCode.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Get Rack Master
    protected void getRackMst(out String strRackMstRow, out String strRackMstCol)
    {
        String strProcName = Convert.ToString(ddProcName.SelectedItem);
        String strPlcNo = Convert.ToString(ddProcName.SelectedValue);
        String strGroupName = Convert.ToString(ddGroupName.SelectedValue);
        String strBlockName = Convert.ToString(ddBlockName.SelectedValue);
        String strRackName = Convert.ToString(ddRackName.SelectedValue);

        strRackMstRow = "";
        strRackMstCol = "";

        DataSet dsRackMst = new DataSet();
        DataTable dtRackMst = new DataTable();

        dsRackMst = csDatabase.SrcRackMst(strRackName, strPlcNo, strProcName, strGroupName, strBlockName);
        dtRackMst = dsRackMst.Tables[0];

        if (dtRackMst.Rows.Count > 0)
        {
            if (Convert.ToString(dtRackMst.Rows[0]["row_cnt"]).Trim() != "")
            {
                strRackMstRow = Convert.ToString(dtRackMst.Rows[0]["row_cnt"]).Trim();
            }
            if (Convert.ToString(dtRackMst.Rows[0]["col_cnt"]).Trim() != "")
            {
                strRackMstCol = Convert.ToString(dtRackMst.Rows[0]["col_cnt"]).Trim();
            }
        }
        else
        {
            strRackMstRow = "";
            strRackMstCol = "";
        }
    }
    #endregion

    #region Generate Table
    private void genTable(String strRackMstRow, String strRackMstCol)
    {
        try
        {
            AisPreview.Controls.Clear();
            Table tbAis = new Table();
            tbAis.Width = Unit.Percentage(100);
            tbAis.BorderWidth = Unit.Pixel(1);

            int rowCnt = 0;
            int colCnt = 0;
            int rowCtr = 0;
            int colCtr = 0;

            rowCnt = int.Parse(strRackMstRow);
            colCnt = int.Parse(strRackMstCol);

            for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
            {
                TableRow tRow = new TableRow();
                tRow.Width = Unit.Percentage(100);
                tRow.Height = Unit.Pixel(150);
                tRow.BorderWidth = Unit.Pixel(1);
                tbAis.Rows.Add(tRow);

                for (colCtr = 1; colCtr <= colCnt; colCtr++)
                {
                    String strAisDet = "";
                    String strModDet = "";
                    String strRackName = Convert.ToString(ddRackName.SelectedValue);
                    String strRackDetId = strRackName + "^" + rowCtr + "^" + colCtr;

                    TableCell tCell = new TableCell();
                    tCell.BorderWidth = Unit.Pixel(1);
                    tCell.Wrap = false;
                    tCell.Style.Add("table-layout", "fixed");
                    tCell.Style.Add("padding", "0px 0px 0px 0px");
                    tCell.Style.Add("min-width", "250px");
                    tCell.Style.Add("min-height", "150px");
                    tCell.Width = Unit.Pixel(250);
                    tCell.HorizontalAlign = HorizontalAlign.Left;
                    tCell.VerticalAlign = VerticalAlign.Top;

                    tRow.Cells.Add(tCell);

                    #region Get and Assign Ais Detail
                    LiteralControl hlAisData = new LiteralControl();
                    strAisDet = getAisDetString(strRackDetId);

                    #region Sample Data Retrieve
                    // 0 strHjId
                    // 1 strHjItemId
                    // 2 strHjRow
                    // 3 strHjCol
                    // 4 strPartsTitle
                    // 5 strPartsNo
                    // 6 strColorSfx
                    // 7 strSymbolCode
                    // 8 strSymbolRtf;
                    #endregion
                    String[] tmpAisVal = strAisDet.Split('~');
                    int tmpAisValCnt = tmpAisVal.Length;
                    if (tmpAisValCnt == 1)
                    {
                        hlAisData.Text = strAisDet;
                    }
                    else
                    {
                        String strSymbol = Itenso.Solutions.Community.Rtf2Html.RtfConverterDemo.ConvertRtf2Txt(tmpAisVal[8]);
                        strSymbol = strSymbol.Replace("\r", "");
                        strSymbol = strSymbol.Replace("\n", "");

                        String strBgColor = csDatabase.GetPartsBgColor(tmpAisVal[5], tmpAisVal[6]);
                        String strFontColor = csDatabase.GetPartsFontColor(tmpAisVal[5], tmpAisVal[6]);

                        hlAisData.Text = tmpAisVal[4] + "</br>" +
                                         //"Row No : " + tmpAisVal[1] + "    Col No : " + tmpAisVal[2] + "</br>" +
                                         tmpAisVal[5] + " - " + tmpAisVal[5] + "</br>" +
                                          "<p style='text-align:center;'><font face='arial' size='6' color='" + strFontColor + "'><span style='background-color:" + strBgColor + ";'>&nbsp;" + strSymbol + "&nbsp;</span></font></p></br>";
                    }

                    tCell.Controls.Add(hlAisData);
                    #endregion

                    #region Get and Assign Module Address Detail
                    LiteralControl hlModuleAdd = new LiteralControl();
                    strModDet = getModDetString(strRackDetId);

                    #region Sample Data Retrieve
                    // 0 strModuleAdd
                    // 1 strModuleName
                    // 2 strLightColorDuringInstruction
                    #endregion
                    String[] tmpModVal = strModDet.Split('~');
                    int tmpModValCnt = tmpModVal.Length;
                    if (tmpModValCnt == 1)
                    {
                        hlModuleAdd.Text = strModDet;
                    }
                    else
                    {
                        hlModuleAdd.Text = "Module Address : " + tmpModVal[0];
                    }

                    #region Rack Light Simulation
                    if (tmpAisValCnt != 1)
                    {
                        if (tmpModValCnt != 1)
                        {
                            if (RackLightSimulation(tmpAisVal[5], tmpAisVal[6]) == true)
                            {
                                String strLightColor = GlobalFunc.getColorCode(tmpModVal[2]);

                                if (strLightColor != "")
                                {
                                    tCell.BackColor = System.Drawing.Color.FromName(strLightColor);
                                }
                                else
                                {
                                    tCell.BackColor = System.Drawing.Color.Gray;
                                }
                            }
                        }
                    }
                    #endregion

                    tCell.Controls.Add(hlModuleAdd);
                    #endregion
                }
            }
            AisPreview.Controls.Add(tbAis);
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Get AIS String
    protected String getAisDetString(String strRackDetId)
    {
        String strRackName = Convert.ToString(ddRackName.SelectedValue);
        String strAisRackDetAis = "</br></br></br></br></br>";
        DataSet dsRackDetAis = new DataSet();
        DataTable dtRackDetAis = new DataTable();

        dsRackDetAis = csDatabase.GetRackMstDetAis(strRackDetId);
        dtRackDetAis = dsRackDetAis.Tables[0];

        #region Initialise Variable
        String strHjId = "";
        String strHjItemId = "";
        String strHjRow = "";
        String strHjCol = "";
        String strPartsTitle = "";
        String strPartsNo = "";
        String strColorSfx = "";
        String strSymbolCode = "";
        String strSymbolRtf = "";
        #endregion

        if (dtRackDetAis.Rows.Count > 0)
        {
            #region Assign Variable
            if (Convert.ToString(dtRackDetAis.Rows[0]["hj_id"]).Trim() != "")
            {
                strHjId = Convert.ToString(dtRackDetAis.Rows[0]["hj_id"]).Trim();
            }
            if (Convert.ToString(dtRackDetAis.Rows[0]["hj_item_id"]).Trim() != "")
            {
                strHjItemId = Convert.ToString(dtRackDetAis.Rows[0]["hj_item_id"]).Trim();
            }
            if (Convert.ToString(dtRackDetAis.Rows[0]["hj_row"]).Trim() != "")
            {
                strHjRow = Convert.ToString(dtRackDetAis.Rows[0]["hj_row"]).Trim();
            }
            if (Convert.ToString(dtRackDetAis.Rows[0]["hj_col"]).Trim() != "")
            {
                strHjCol = Convert.ToString(dtRackDetAis.Rows[0]["hj_col"]);
            }
            if (Convert.ToString(dtRackDetAis.Rows[0]["parts_title"]).Trim() != "")
            {
                strPartsTitle = Convert.ToString(dtRackDetAis.Rows[0]["parts_title"]);
            }
            if (Convert.ToString(dtRackDetAis.Rows[0]["parts_no"]).Trim() != "")
            {
                strPartsNo = Convert.ToString(dtRackDetAis.Rows[0]["parts_no"]).Trim();
            }
            if (Convert.ToString(dtRackDetAis.Rows[0]["color_sfx"]).Trim() != "")
            {
                strColorSfx = Convert.ToString(dtRackDetAis.Rows[0]["color_sfx"]);
            }
            if (Convert.ToString(dtRackDetAis.Rows[0]["symbol_code"]).Trim() != "")
            {
                strSymbolCode = Convert.ToString(dtRackDetAis.Rows[0]["symbol_code"]);
            }
            if (Convert.ToString(dtRackDetAis.Rows[0]["symbol_rtf"]).Trim() != "")
            {
                strSymbolRtf = Convert.ToString(dtRackDetAis.Rows[0]["symbol_rtf"]);
            }
            #endregion

            if (strPartsNo != "")
            {
                strAisRackDetAis = strHjId + "~" + strHjItemId + "~" + strHjRow + "~" + strHjCol + "~" + strPartsTitle + "~" + strPartsNo + "~" + strColorSfx + "~" + strSymbolCode + "~" + strSymbolRtf;

                return strAisRackDetAis;
            }
            else
            {
                return strAisRackDetAis;
            }
        }
        else
        {
            return strAisRackDetAis;
        }
    }
    #endregion

    #region Get Module Address String
    protected String getModDetString(String strRackDetId)
    {
        String strRackName = Convert.ToString(ddRackName.SelectedValue);
        String strAisRackDetModule = "</br></br>";
        DataSet dsRackDetModule = new DataSet();
        DataTable dtRackDetModule = new DataTable();

        dsRackDetModule = csDatabase.GetRackMstDetModule(strRackDetId);
        dtRackDetModule = dsRackDetModule.Tables[0];

        #region Initialise Variable
        String strModuleAdd = "";
        String strModuleName = "";
        String strColorDI = "";
        #endregion

        if (dtRackDetModule.Rows.Count > 0)
        {
            #region Assign Variable
            if (Convert.ToString(dtRackDetModule.Rows[0]["module_add"]).Trim() != "")
            {
                strModuleAdd = Convert.ToString(dtRackDetModule.Rows[0]["module_add"]).Trim();
            }
            if (Convert.ToString(dtRackDetModule.Rows[0]["module_name"]).Trim() != "")
            {
                strModuleName = Convert.ToString(dtRackDetModule.Rows[0]["module_name"]);
            }
            if (Convert.ToString(dtRackDetModule.Rows[0]["color_di"]).Trim() != "")
            {
                strColorDI = Convert.ToString(dtRackDetModule.Rows[0]["color_di"]).Trim();
            }
            #endregion

            if (strModuleAdd != "")
            {
                strAisRackDetModule = strModuleAdd + "~" + strModuleName + "~" + strColorDI;
                return strAisRackDetModule;
            }
            else
            {
                return strAisRackDetModule;
            }
        }
        else
        {
            return strAisRackDetModule;
        }
    }
    #endregion

    #region Rack Light Simulation
    protected Boolean RackLightSimulation(String strRefPartsNum, String strRefColorSfx)
    {
        if (ddInsCode.SelectedIndex != 0)
        {
            String strModel = Convert.ToString(ddModel.SelectedValue);
            String strSfx = Convert.ToString(ddSfx.SelectedValue);
            String strColor = Convert.ToString(ddColor.SelectedValue);
            //String strInsCode = Convert.ToString(ddInsCode.SelectedValue);

            return csDatabase.GetRackLightAvailability(strModel, strSfx, strColor, strRefPartsNum, strRefColorSfx);

            //DataSet dsHjParts = new DataSet();
            //DataTable dtHjParts = new DataTable();

            //dsHjParts = csDatabase.SrcHjParts(strModel, strSfx, strColor);
            //dtHjParts = dsHjParts.Tables[0];

            //#region Initialise Variable
            //String strPartsNo = "";
            //String strColorSfx = "";
            //#endregion

            //if (dtHjParts.Rows.Count > 0)
            //{
            //    for (int i = 0; i <= dtHjParts.Rows.Count - 1; i++)
            //    {
            //        if (Convert.ToString(dtHjParts.Rows[i]["part_no"]).Trim() != "")
            //        {
            //            strPartsNo = Convert.ToString(dtHjParts.Rows[i]["part_no"]).Trim();
            //            if (strPartsNo == strRefPartsNum)
            //            {
            //                if (Convert.ToString(dtHjParts.Rows[i]["color_sfx"]).Trim() != "")
            //                {
            //                    strColorSfx = Convert.ToString(dtHjParts.Rows[i]["color_sfx"]).Trim();
            //                    if (strColorSfx == strRefColorSfx)
            //                    {
            //                        return true;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //return false;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region Submit Instruction Code List
    protected Boolean SubmitInsCodeList(String strPlcNo, String strProcName, String strGroupName, String strBlockName, String strInsCode)
    {

        #region Initialize Variables
        String strStartDeviceName;
        String strPlcStatus;
        int iTotDeviceInterval;
        int iTotDeviceCount;
        int iTotArrayCount;
        int tmpInsCodeCnt = 0;

        int iLsn = Convert.ToInt32(csDatabase.GetLsn(strPlcNo));
        int iInsCodeCnt = 0;
        int iLmCnt = 0;
        int iLmRackCnt = 0;
        int iGmCnt = 0;
        int iBmCnt = 0;
        int iColorCnt = 0;

        List<short> lstGroupMst = new List<short>();
        List<short> lstBlockMst = new List<short>();
        List<short> lstLampAdd = new List<short>();
        List<short> lstLampType = new List<short>();
        List<short> lstLampMode = new List<short>();
        List<short> lstInsCode = new List<short>();
        List<short> lstLmMap = new List<short>();

        String tmpInsCode = "";
        String tmpInsCodeMap = "";
        String tmpModel = "";
        String tmpKatashiki = "";
        String tmpSfx = "";
        String tmpColor = "";
        String tmpComment = "";
        String tmpGwNo = "";
        String byteInsCode = "";
        String byteModel = "";
        String byteKatashiki = "";
        String byteSfx = "";
        String byteColor = "";
        String byteComment = "";
        String tmpGroupId = "";
        String tmpGroupName = "";
        String tmpGroupLine = "";
        #endregion


        //Get converion type from PLC model
        String ConvType = "";
        ConvType = csDatabase.GetPLCConvType(strPlcNo, strProcName);

        #region Form Group Master Conv
        DataSet dsGmSearch = new DataSet();
        DataTable dtGmSearch = new DataTable();

        dsGmSearch = csDatabase.SrcGroupMst("", "", strPlcNo, strProcName);
        dtGmSearch = dsGmSearch.Tables[0];

        if (dtGmSearch.Rows.Count > 0)
        {
            csDatabase.ClrGroupMstConv(strPlcNo, strProcName);

            for (iGmCnt = 0; iGmCnt < dtGmSearch.Rows.Count; iGmCnt++)
            {
                if (Convert.ToString(dtGmSearch.Rows[iGmCnt]["group_id"]).Trim() != "")
                {
                    tmpGroupId = Convert.ToString(dtGmSearch.Rows[iGmCnt]["group_id"]).Trim();
                }
                if (Convert.ToString(dtGmSearch.Rows[iGmCnt]["group_name"]).Trim() != "")
                {
                    tmpGroupName = Convert.ToString(dtGmSearch.Rows[iGmCnt]["group_name"]).Trim();
                }
                if (Convert.ToString(dtGmSearch.Rows[iGmCnt]["group_line"]).Trim() != "")
                {
                    tmpGroupLine = Convert.ToString(dtGmSearch.Rows[iGmCnt]["group_line"]).Trim();
                }

                Boolean blSvGroupMstConv = csDatabase.SvGroupMstConv(strPlcNo, strProcName, tmpGroupId, tmpGroupName, tmpGroupLine);

                if (blSvGroupMstConv == false)
                {
                    GlobalFunc.ShowErrorMessage("Unable to form Group Master Conversion. Please Check.");
                    return false;
                }
            }
        }
        else
        {
            return false;
        }
        #endregion

        #region Form Block Master Conv
        DataSet dsBmSearch = new DataSet();
        DataTable dtBmSearch = new DataTable();

        dsBmSearch = csDatabase.SrcBlockMst(strPlcNo, "", "", "", "", "", strProcName, "", "", "");
        dsBmSearch.Tables[0].DefaultView.Sort = "gw_no ASC";
        dtBmSearch = dsBmSearch.Tables[0].DefaultView.ToTable();

        if (dtBmSearch.Rows.Count > 0)
        {
            csDatabase.ClrBlockMstConv(strPlcNo, strProcName);

            for (iBmCnt = 0; iBmCnt < dtBmSearch.Rows.Count; iBmCnt++)
            {
                tmpGwNo = "";
                String tmpBlockSeq = "";
                String tmpBlockName = "";

                if (Convert.ToString(dtBmSearch.Rows[iBmCnt]["gw_no"]).Trim() != "")
                {
                    tmpGwNo = Convert.ToString(dtBmSearch.Rows[iBmCnt]["gw_no"]);
                }
                if (Convert.ToString(dtBmSearch.Rows[iBmCnt]["group_name"]).Trim() != "")
                {
                    String sqlQuery = "SELECT group_id as ReturnField FROM dt_GroupMst WHERE group_name = '" + Convert.ToString(dtBmSearch.Rows[iBmCnt]["group_name"]) + "'";
                    tmpGroupId = Convert.ToString(ConnQuery.getReturnFieldExecuteReader(sqlQuery));
                }
                if (Convert.ToString(dtBmSearch.Rows[iBmCnt]["block_seq"]).Trim() != "")
                {
                    tmpBlockSeq = Convert.ToString(dtBmSearch.Rows[iBmCnt]["block_seq"]);
                }
                if (Convert.ToString(dtBmSearch.Rows[iBmCnt]["block_name"]).Trim() != "")
                {
                    tmpBlockName = Convert.ToString(dtBmSearch.Rows[iBmCnt]["block_name"]).Trim();
                }

                Boolean blSvBlockMstConv = csDatabase.SvBlockMstConv(strPlcNo, strProcName, tmpGwNo, tmpGroupId, tmpBlockSeq, tmpBlockName);

                if (blSvBlockMstConv == false)
                {
                    GlobalFunc.ShowErrorMessage("Unable to form Block Master Conversion. Please Check.");
                    return false;
                }
            }
        }
        else
        {
            return false;
        }
        #endregion

        #region Form Lamp Module Conv
        dsBmSearch = new DataSet();
        dtBmSearch = new DataTable();



        dsBmSearch = csDatabase.SrcBlockMst(strPlcNo, "", strBlockName, "", "", "", strProcName, strGroupName, "", "");
        dsBmSearch.Tables[0].DefaultView.Sort = "gw_no ASC";
        dtBmSearch = dsBmSearch.Tables[0].DefaultView.ToTable();

        if (dtBmSearch.Rows.Count > 0)
        {
            tmpGwNo = "";
            if (Convert.ToString(dtBmSearch.Rows[0]["gw_no"]).Trim() != "")
            {
                tmpGwNo = Convert.ToString(dtBmSearch.Rows[0]["gw_no"]);
            }

            csDatabase.ClrLmModeConv(strPlcNo, strProcName, tmpGwNo);
            csDatabase.ClrLmAddMatchingConv(strPlcNo, strProcName, tmpGwNo);

            for (iBmCnt = 0; iBmCnt < dtBmSearch.Rows.Count; iBmCnt++)
            {
                String tmpBlockName = "";
                if (Convert.ToString(dtBmSearch.Rows[iBmCnt]["block_name"]).Trim() != "")
                {
                    tmpBlockName = Convert.ToString(dtBmSearch.Rows[iBmCnt]["block_name"]).Trim();
                }

                #region Form Lamp Module Data (Mode Error/Waiting Instruction) Per Block
                String tmpWait = "";
                String tmpErr = "";
                String tmpStartModeCnt = "";
                String tmpEndModeCnt = "";

                if (Convert.ToString(dtBmSearch.Rows[iBmCnt]["light_wi"]).Trim() != "")
                {
                    String tmpLightWi = Convert.ToString(dtBmSearch.Rows[iBmCnt]["light_wi"]).Trim();
                    String tmpColorWi = Convert.ToString(dtBmSearch.Rows[iBmCnt]["color_wi"]).Trim();

                    int[] tmpWaitRGB = csDatabase.GetRGB(tmpLightWi, tmpColorWi);
                    tmpWait = string.Join("*", Array.ConvertAll(tmpWaitRGB, x => x.ToString()));
                }
                else
                {
                    tmpWait = "0*0*0";
                }
                tmpStartModeCnt = csDatabase.GetLmModeId(strPlcNo, strProcName, tmpGwNo);
                csDatabase.SvLmModeConv(strPlcNo, strProcName, tmpGwNo, tmpStartModeCnt, tmpWait);

                if (Convert.ToString(dtBmSearch.Rows[iBmCnt]["light_err"]).Trim() != "")
                {
                    String tmpLightErr = Convert.ToString(dtBmSearch.Rows[iBmCnt]["light_err"]).Trim();
                    String tmpColorErr = Convert.ToString(dtBmSearch.Rows[iBmCnt]["color_err"]).Trim();

                    int[] tmpErrRGB = csDatabase.GetRGB(tmpLightErr, tmpColorErr);
                    tmpErr = string.Join("*", Array.ConvertAll(tmpErrRGB, x => x.ToString()));
                }
                else
                {
                    tmpErr = "0*0*0";
                }
                tmpStartModeCnt = csDatabase.GetLmModeId(strPlcNo, strProcName, tmpGwNo);
                csDatabase.SvLmModeConv(strPlcNo, strProcName, tmpGwNo, tmpStartModeCnt, tmpErr);
                #endregion

                #region Form Lamp Module Data (Start/End Address and Mode Type) Per Block
                String tmpStartLmAdd = "";
                String tmpStartLmDi = "";
                String tmpStartLmAi = "";
                String tmpEndLmAdd = "";
                String tmpEndLmDi = "";
                String tmpEndLmAi = "";

                if (Convert.ToString(dtBmSearch.Rows[iBmCnt]["start_lm"]).Trim() != "")
                {
                    tmpStartLmAdd = Convert.ToString(dtBmSearch.Rows[iBmCnt]["start_lm"]);

                    String tmpLightDi;
                    String tmpColorDi;
                    String tmpLightAi;
                    String tmpColorAi;

                    csDatabase.GetLightColor(tmpStartLmAdd, strPlcNo, strProcName, out tmpLightDi, out tmpColorDi, out tmpLightAi, out tmpColorAi);

                    int[] tmpDiRGB = csDatabase.GetRGB(tmpLightDi, tmpColorDi);
                    tmpStartLmDi = string.Join("*", Array.ConvertAll(tmpDiRGB, x => x.ToString()));

                    int[] tmpAiRGB = csDatabase.GetRGB(tmpLightAi, tmpColorAi);
                    tmpStartLmAi = string.Join("*", Array.ConvertAll(tmpAiRGB, x => x.ToString()));
                }
                else
                {
                    tmpStartLmDi = "0*0*0";
                    tmpStartLmAi = "0*0*0";
                }
                tmpStartModeCnt = csDatabase.GetLmModeId(strPlcNo, strProcName, tmpGwNo);
                csDatabase.SvLmModeConv(strPlcNo, strProcName, tmpGwNo, tmpStartModeCnt, tmpStartLmDi);
                csDatabase.UpdLmAddMatchingConv(strPlcNo, strProcName, tmpGwNo, tmpStartModeCnt, tmpStartLmAdd);

                tmpEndModeCnt = csDatabase.GetLmModeId(strPlcNo, strProcName, tmpGwNo);
                csDatabase.SvLmModeConv(strPlcNo, strProcName, tmpGwNo, tmpEndModeCnt, tmpStartLmAi);

                if (Convert.ToString(dtBmSearch.Rows[iBmCnt]["end_lm"]).Trim() != "")
                {
                    tmpEndLmAdd = Convert.ToString(dtBmSearch.Rows[iBmCnt]["end_lm"]);

                    String tmpLightDi;
                    String tmpColorDi;
                    String tmpLightAi;
                    String tmpColorAi;

                    csDatabase.GetLightColor(Convert.ToString(tmpEndLmAdd), strPlcNo, strProcName, out tmpLightDi, out tmpColorDi, out tmpLightAi, out tmpColorAi);

                    int[] tmpDiRGB = csDatabase.GetRGB(tmpLightDi, tmpColorDi);
                    tmpEndLmDi = string.Join("*", Array.ConvertAll(tmpDiRGB, x => x.ToString()));

                    int[] tmpAiRGB = csDatabase.GetRGB(tmpLightAi, tmpColorAi);
                    tmpEndLmAi = string.Join("*", Array.ConvertAll(tmpAiRGB, x => x.ToString()));
                }
                else
                {
                    tmpEndLmDi = "0*0*0";
                    tmpEndLmAi = "0*0*0";
                }
                tmpStartModeCnt = csDatabase.GetLmModeId(strPlcNo, strProcName, tmpGwNo);
                csDatabase.SvLmModeConv(strPlcNo, strProcName, tmpGwNo, tmpStartModeCnt, tmpEndLmDi);
                csDatabase.UpdLmAddMatchingConv(strPlcNo, strProcName, tmpGwNo, tmpStartModeCnt, tmpEndLmAdd);

                tmpEndModeCnt = csDatabase.GetLmModeId(strPlcNo, strProcName, tmpGwNo);
                csDatabase.SvLmModeConv(strPlcNo, strProcName, tmpGwNo, tmpEndModeCnt, tmpEndLmAi);
                #endregion

                #region Form Lamp Module Data (Rack Address and Mode Type) Per Block
                DataSet dsLmSearch = new DataSet();
                DataTable dtLmSearch = new DataTable();

                dsLmSearch = csDatabase.SrcLmAddPerBlock(strPlcNo, strProcName, tmpBlockName);
                dtLmSearch = dsLmSearch.Tables[0];

                int RunPhyAdd = 2;


                if (dtLmSearch.Rows.Count > 0)
                {

                    csDatabase.DeleteLmRackMode(strPlcNo, strProcName, tmpGwNo);

                    for (iLmCnt = 0; iLmCnt < dtLmSearch.Rows.Count; iLmCnt++)
                    {
                        if (ConvType == "2")
                        {
                            while (RunPhyAdd + 1 < Convert.ToInt32(dtLmSearch.Rows[iLmCnt]["PhyAdd"]))
                            {
                                csDatabase.UpdLmAddMatchingConv(strPlcNo, strProcName, tmpGwNo, "0", "0");
                                RunPhyAdd = RunPhyAdd + 1;
                            }
                        }
                        RunPhyAdd = Convert.ToInt32(dtLmSearch.Rows[iLmCnt]["PhyAdd"]);

                        String tmpRackLmAdd = "";
                        String strModuleType = "";
                        String strLightDi = "";
                        String strColorDi = "";
                        String strLightAi = "";
                        String strColorAi = "";
                        String tmpRackLmDi = "";
                        String tmpRackLmAi = "";

                        if (Convert.ToString(dtLmSearch.Rows[iLmCnt]["module_add"]).Trim() != "")
                        {
                            tmpRackLmAdd = Convert.ToString(dtLmSearch.Rows[iLmCnt]["module_add"]);

                            strModuleType = Convert.ToString(dtLmSearch.Rows[iLmCnt]["module_type"]);
                            strLightDi = Convert.ToString(dtLmSearch.Rows[iLmCnt]["light_di"]);
                            strColorDi = Convert.ToString(dtLmSearch.Rows[iLmCnt]["color_di"]);
                            strLightAi = Convert.ToString(dtLmSearch.Rows[iLmCnt]["light_ai"]);
                            strColorAi = Convert.ToString(dtLmSearch.Rows[iLmCnt]["color_ai"]);

                            #region Form Lamp Module Rack Module Address

                            tmpStartModeCnt = csDatabase.GetLmRackMode(strPlcNo, strProcName, tmpGwNo, strModuleType);

                            if (tmpStartModeCnt == "")
                            {
                                int[] tmpDiRGB = csDatabase.GetRGB(strLightDi, strColorDi);
                                int[] tmpAiRGB = csDatabase.GetRGB(strLightAi, strColorAi);

                                tmpRackLmDi = string.Join("*", Array.ConvertAll(tmpDiRGB, x => x.ToString()));
                                tmpRackLmAi = string.Join("*", Array.ConvertAll(tmpAiRGB, x => x.ToString()));

                                tmpStartModeCnt = csDatabase.GetLmModeId(strPlcNo, strProcName, tmpGwNo);
                                csDatabase.SaveLmRackMode(strPlcNo, strProcName, tmpGwNo, strModuleType, Convert.ToInt32(tmpStartModeCnt));
                                csDatabase.SvLmModeConv(strPlcNo, strProcName, tmpGwNo, tmpStartModeCnt, tmpRackLmDi);

                                tmpEndModeCnt = csDatabase.GetLmModeId(strPlcNo, strProcName, tmpGwNo);
                                csDatabase.SvLmModeConv(strPlcNo, strProcName, tmpGwNo, tmpEndModeCnt, tmpRackLmAi);
                            }
                            csDatabase.UpdLmAddMatchingConv(strPlcNo, strProcName, tmpGwNo, tmpStartModeCnt, tmpRackLmAdd);
                            #endregion
                        }
                    }
                }
                #endregion
            }
        }
        else
        {
            return false;
        }
        #endregion

        #region Form Instruction Code Conversion Table
        DataSet dsInsCodeSearch = new DataSet();
        DataTable dtInsCodeSearch = new DataTable();

        dsInsCodeSearch = csDatabase.SrcInsCode(strProcName, strGroupName, strBlockName, strInsCode);
        dtInsCodeSearch = dsInsCodeSearch.Tables[0];

        if (dtInsCodeSearch.Rows.Count > 0)
        {
            for (iInsCodeCnt = 0; iInsCodeCnt < dtInsCodeSearch.Rows.Count; iInsCodeCnt++)
            {
                //String Data
                if (Convert.ToString(dtInsCodeSearch.Rows[iInsCodeCnt]["ins_code"]).Trim() != "")
                {
                    tmpInsCode = Convert.ToString(dtInsCodeSearch.Rows[iInsCodeCnt]["ins_code"]).Trim();
                }
                if (Convert.ToString(dtInsCodeSearch.Rows[iInsCodeCnt]["model"]).Trim() != "")
                {
                    tmpModel = Convert.ToString(dtInsCodeSearch.Rows[iInsCodeCnt]["model"]);
                }
                if (Convert.ToString(dtInsCodeSearch.Rows[iInsCodeCnt]["katashiki"]).Trim() != "")
                {
                    tmpKatashiki = Convert.ToString(dtInsCodeSearch.Rows[iInsCodeCnt]["katashiki"]);
                }
                if (Convert.ToString(dtInsCodeSearch.Rows[iInsCodeCnt]["sfx"]).Trim() != "")
                {
                    tmpSfx = Convert.ToString(dtInsCodeSearch.Rows[iInsCodeCnt]["sfx"]);
                }
                if (Convert.ToString(dtInsCodeSearch.Rows[iInsCodeCnt]["color"]).Trim() != "")
                {
                    tmpColor = Convert.ToString(dtInsCodeSearch.Rows[iInsCodeCnt]["color"]);
                }
                if (Convert.ToString(dtInsCodeSearch.Rows[iInsCodeCnt]["comment"]).Trim() != "")
                {
                    tmpComment = Convert.ToString(dtInsCodeSearch.Rows[iInsCodeCnt]["comment"]).Trim();
                }

                //Byte Data
                if (Convert.ToString(dtInsCodeSearch.Rows[iInsCodeCnt]["byte_ins_code"]).Trim() != "")
                {
                    byteInsCode = Convert.ToString(dtInsCodeSearch.Rows[iInsCodeCnt]["byte_ins_code"]).Trim();
                }
                if (Convert.ToString(dtInsCodeSearch.Rows[iInsCodeCnt]["byte_model"]).Trim() != "")
                {
                    byteModel = Convert.ToString(dtInsCodeSearch.Rows[iInsCodeCnt]["byte_model"]).Trim();
                }
                if (Convert.ToString(dtInsCodeSearch.Rows[iInsCodeCnt]["byte_katashiki"]).Trim() != "")
                {
                    byteKatashiki = Convert.ToString(dtInsCodeSearch.Rows[iInsCodeCnt]["byte_katashiki"]).Trim();
                }
                if (Convert.ToString(dtInsCodeSearch.Rows[iInsCodeCnt]["byte_sfx"]).Trim() != "")
                {
                    byteSfx = Convert.ToString(dtInsCodeSearch.Rows[iInsCodeCnt]["byte_sfx"]).Trim();
                }
                if (Convert.ToString(dtInsCodeSearch.Rows[iInsCodeCnt]["byte_color"]).Trim() != "")
                {
                    byteColor = Convert.ToString(dtInsCodeSearch.Rows[iInsCodeCnt]["byte_color"]).Trim();
                }
                if (Convert.ToString(dtInsCodeSearch.Rows[iInsCodeCnt]["byte_comment"]).Trim() != "")
                {
                    byteComment = Convert.ToString(dtInsCodeSearch.Rows[iInsCodeCnt]["byte_comment"]).Trim();
                }

                tmpInsCodeCnt = Convert.ToInt32(csDatabase.GetInsCodeConvId(tmpInsCode, strPlcNo, strProcName));
                Boolean blUpdInsCodeConv = csDatabase.UpdInsCodeConv(tmpInsCode, tmpModel, tmpKatashiki, tmpSfx, tmpColor, tmpComment, byteInsCode, byteModel, byteKatashiki, byteSfx, byteColor, byteComment, tmpInsCodeCnt, strPlcNo, strProcName);

                if (blUpdInsCodeConv == false)
                {
                    GlobalFunc.ShowErrorMessage("Unable to form Instruction Code Conversion. Please Check.");
                    return false;
                }
            }
        }
        else
        {
            return false;
        }
        #endregion

        #region Form Instruction Code Mapping With Lamp Module
        DataSet dsInsCodeConv = new DataSet();
        DataTable dtInsCodeConv = new DataTable();
        DataSet dsLmInsSearch = new DataSet();
        DataTable dtLmInsSearch = new DataTable();
        DataSet dsLmRack = new DataSet();
        DataTable dtLmRack = new DataTable();

        dsInsCodeConv = csDatabase.SrcInsCodeConv(strInsCode, strPlcNo, strProcName);
        dtInsCodeConv = dsInsCodeConv.Tables[0];

        if (dtInsCodeConv.Rows.Count > 0)
        {
            for (iInsCodeCnt = 0; iInsCodeCnt < dtInsCodeConv.Rows.Count; iInsCodeCnt++)
            {
                tmpInsCodeCnt = Convert.ToInt32(dtInsCodeConv.Rows[iInsCodeCnt]["ins_code_cnt"]);
                tmpInsCode = Convert.ToString(dtInsCodeConv.Rows[iInsCodeCnt]["ins_code"]).Trim();



                dsLmRack = csDatabase.GetLmAddPerBlock(strPlcNo, strProcName, strGroupName, strBlockName, tmpInsCode);
                dtLmRack = dsLmRack.Tables[0];


                if (ConvType == "1")
                {
                    dsLmInsSearch = csDatabase.SrcLmAddPerBlock(strPlcNo, strProcName, strBlockName);
                    dtLmInsSearch = dsLmInsSearch.Tables[0];
                }
                else if (ConvType == "2")
                {
                    dsLmInsSearch = csDatabase.InitialiaseTempLapMappingData(strPlcNo, strProcName, strBlockName, tmpInsCode);
                    dtLmInsSearch = dsLmInsSearch.Tables[0];
                }

                if ((dtLmInsSearch.Rows.Count > 0) && (dtLmRack.Rows.Count > 0))
                {
                    tmpInsCodeMap = "0" + "0";

                    for (iLmCnt = 0; iLmCnt < dtLmInsSearch.Rows.Count; iLmCnt++)
                    {
                        Boolean blExist = false;
                        for (iLmRackCnt = 0; iLmRackCnt < dtLmRack.Rows.Count; iLmRackCnt++)
                        {
                            if (dtLmRack.Rows[iLmRackCnt]["ins_code"].ToString() == tmpInsCode)
                            {
                                //if (Convert.ToString(dtLmInsSearch.Rows[iLmCnt]["module_add"]).Trim() != "" && Convert.ToString(dtLmRack.Rows[iLmRackCnt]["module_add"]).Trim() != "")
                                //{
                                //    if (dtLmInsSearch.Rows[iLmCnt]["module_add"].ToString() == dtLmRack.Rows[iLmRackCnt]["module_add"].ToString())
                                //    {
                                //        tmpGwNo = dtLmRack.Rows[iLmRackCnt]["gw_no"].ToString();
                                //        tmpInsCodeMap = "1" + tmpInsCodeMap;
                                //        blExist = true;
                                //    }
                                //}
                                if (Convert.ToString(dtLmInsSearch.Rows[iLmCnt]["module_add"]).Trim() != "" && Convert.ToString(dtLmRack.Rows[iLmRackCnt]["module_add"]).Trim() != "")
                                {
                                    if (dtLmInsSearch.Rows[iLmCnt]["module_add"].ToString() == dtLmRack.Rows[iLmRackCnt]["module_add"].ToString())
                                    {
                                        tmpGwNo = dtLmRack.Rows[iLmRackCnt]["gw_no"].ToString();
                                        tmpInsCodeMap = "1" + tmpInsCodeMap;
                                        blExist = true;
                                    }
                                }
                            }
                        }
                        if (!blExist)
                        {
                            tmpInsCodeMap = "0" + tmpInsCodeMap;
                        }
                    }

                    for (int i = tmpInsCodeMap.Length; i < 64; i++)
                    {
                        tmpInsCodeMap = "0" + tmpInsCodeMap;
                    }

                    Boolean blUpdInsCodeMapConv = csDatabase.UpdInsCodeMapConv(tmpInsCodeCnt, strPlcNo, strProcName, tmpGwNo, tmpInsCodeMap);

                    if (blUpdInsCodeMapConv == false)
                    {
                        GlobalFunc.ShowErrorMessage("Unable to form Instruction Code Mapping with Light Module Conversion. Please Check.");
                        return false;
                    }
                }
            }
        }
        else
        {
            return false;
        }
        #endregion

        #region Submit Group Master Data

        strStartDeviceName = "ZR9000";
        iTotDeviceInterval = 10;
        iTotDeviceCount = 12;
        iTotArrayCount = (iTotDeviceInterval * iTotDeviceCount);

        DataSet dsGmConvSearch = new DataSet();
        DataTable dtGmConvSearch = new DataTable();

        dsGmConvSearch = csDatabase.SrcGroupMstConv(strPlcNo, strProcName);
        dtGmConvSearch = dsGmConvSearch.Tables[0];

        if (dtGmConvSearch.Rows.Count > 0)
        {
            for (iGmCnt = 0; iGmCnt < dtGmConvSearch.Rows.Count; iGmCnt++)
            {
                if (Convert.ToString(dtGmConvSearch.Rows[iGmCnt]["group_id"]).Trim() != "")
                {
                    short sGroupId = Convert.ToInt16(dtGmConvSearch.Rows[iGmCnt]["group_id"]);
                    lstGroupMst.Add(sGroupId);
                }
                else
                {
                    lstGroupMst.Add(Convert.ToInt16(0));
                }
                if (Convert.ToString(dtGmConvSearch.Rows[iGmCnt]["group_name"]).Trim() != "")
                {
                    tmpGroupName = Convert.ToString(dtGmConvSearch.Rows[iGmCnt]["group_name"]).Trim();
                    short[] sGroupName = GlobalFunc.convStrToAscii(tmpGroupName, 8);

                    foreach (short element in sGroupName)
                    {
                        lstGroupMst.Add(element);
                    }
                }
                else
                {
                    for (int i = 0; i < 8; i++)
                    {
                        lstGroupMst.Add(Convert.ToInt16(0));
                    }
                }
                if (Convert.ToString(dtGmConvSearch.Rows[iGmCnt]["group_line"]).Trim() != "")
                {
                    tmpGroupLine = Convert.ToString(dtGmConvSearch.Rows[iGmCnt]["group_line"]).Trim();
                    short[] sGroupLine = GlobalFunc.convStrToAscii(tmpGroupLine, 1);

                    foreach (short element in sGroupLine)
                    {
                        lstGroupMst.Add(element);
                    }
                }
                else
                {
                    lstGroupMst.Add(Convert.ToInt16(0));
                }
            }



            while (lstGroupMst.Count < iTotArrayCount)
            {
                lstGroupMst.Add(Convert.ToInt16(0));
            }

            strPlcStatus = PlcConnQuery.WriteDeviceByBlock(strStartDeviceName, iTotArrayCount, lstGroupMst, iLsn);
            if (strPlcStatus != "")
            {
                return false;
            }
        }
        else
        {
            return false;
        }
        #endregion

        #region Submit Block Master Data
        DataSet dsBmConvSearch = new DataSet();
        DataTable dtBmConvSearch = new DataTable();

        dsBmConvSearch = csDatabase.SrcBlockMstConv(strPlcNo, strProcName);
        dsBmConvSearch.Tables[0].DefaultView.Sort = "gw_no ASC";
        dtBmConvSearch = dsBmConvSearch.Tables[0];

        if (dtBmConvSearch.Rows.Count > 0)
        {
            for (iBmCnt = 0; iBmCnt < dtBmConvSearch.Rows.Count; iBmCnt++)
            {
                String tmpBlockName = "";

                if (Convert.ToString(dtBmConvSearch.Rows[iBmCnt]["group_id"]).Trim() != "")
                {
                    short sGroupId = Convert.ToInt16(dtBmConvSearch.Rows[iBmCnt]["group_id"]);
                    lstBlockMst.Add(sGroupId);
                }
                else
                {
                    lstBlockMst.Add(0);
                }
                if (Convert.ToString(dtBmConvSearch.Rows[iBmCnt]["block_seq"]).Trim() != "")
                {
                    short sBlockSeq = Convert.ToInt16(dtBmConvSearch.Rows[iBmCnt]["block_seq"]);
                    lstBlockMst.Add(sBlockSeq);
                }
                else
                {
                    lstBlockMst.Add(0);
                }
                if (Convert.ToString(dtBmConvSearch.Rows[iBmCnt]["block_name"]).Trim() != "")
                {
                    tmpBlockName = Convert.ToString(dtBmConvSearch.Rows[iBmCnt]["block_name"]).Trim();
                    short[] sBlockName = GlobalFunc.convStrToAscii(tmpBlockName, 8);

                    foreach (short element in sBlockName)
                    {
                        lstBlockMst.Add(element);
                    }
                }
                else
                {
                    for (int i = 0; i < 8; i++)
                    {
                        lstBlockMst.Add(0);
                    }
                }
            }

            strStartDeviceName = "ZR9120";
            iTotDeviceInterval = 10;
            iTotDeviceCount = 12;
            iTotArrayCount = (iTotDeviceInterval * iTotDeviceCount);

            while (lstBlockMst.Count < iTotArrayCount)
            {
                lstBlockMst.Add(Convert.ToInt16(0));
            }

            strPlcStatus = PlcConnQuery.WriteDeviceByBlock(strStartDeviceName, iTotArrayCount, lstBlockMst, iLsn);
            if (strPlcStatus != "")
            {
                return false;
            }
        }
        else
        {
            return false;
        }
        #endregion

        #region Submit Lamp Address And Lamp Type Data
        DataSet dsLmAddConv = new DataSet();
        DataTable dtLmAddConv = new DataTable();

        for (int iGwNo = 1; iGwNo <= 12; iGwNo++)
        {
            dsLmAddConv = csDatabase.SrcLmAddMatchingConv(strPlcNo, strProcName, iGwNo);
            dtLmAddConv = dsLmAddConv.Tables[0];

            if (dtLmAddConv.Rows.Count > 0)
            {
                for (iLmCnt = 0; iLmCnt < dtLmAddConv.Rows.Count; iLmCnt++)
                {
                    if (Convert.ToString(dtLmAddConv.Rows[iLmCnt]["lm_add"]).Trim() != "")
                    {
                        short sLmAdd = Convert.ToInt16(dtLmAddConv.Rows[iLmCnt]["lm_add"]);
                        lstLampAdd.Add(sLmAdd);
                    }
                    else
                    {
                        lstLampAdd.Add(Convert.ToInt16(0));
                    }
                    if (Convert.ToString(dtLmAddConv.Rows[iLmCnt]["lm_mode_cnt"]).Trim() != "")
                    {
                        short sLmModeCnt = Convert.ToInt16(dtLmAddConv.Rows[iLmCnt]["lm_mode_cnt"]);
                        lstLampType.Add(sLmModeCnt);
                    }
                    else
                    {
                        lstLampType.Add(Convert.ToInt16(0));
                    }
                }
            }
            while (lstLampAdd.Count < (64 * iGwNo))
            {
                lstLampAdd.Add(Convert.ToInt16(0));
            }
            while (lstLampType.Count < (64 * iGwNo))
            {
                lstLampType.Add(Convert.ToInt16(0));
            }
        }

        strStartDeviceName = "ZR1000";
        iTotDeviceInterval = 64;
        iTotDeviceCount = 12;
        iTotArrayCount = (iTotDeviceInterval * iTotDeviceCount);

        while (lstLampAdd.Count < iTotArrayCount)
        {
            lstLampAdd.Add(Convert.ToInt16(0));
        }

        strPlcStatus = PlcConnQuery.WriteDeviceByBlock(strStartDeviceName, iTotArrayCount, lstLampAdd, iLsn);
        if (strPlcStatus != "")
        {
            return false;
        }

        strStartDeviceName = "ZR200";
        iTotDeviceInterval = 64;
        iTotDeviceCount = 12;
        iTotArrayCount = (iTotDeviceInterval * iTotDeviceCount);

        while (lstLampType.Count < iTotArrayCount)
        {
            lstLampType.Add(Convert.ToInt16(0));
        }

        strPlcStatus = PlcConnQuery.WriteDeviceByBlock(strStartDeviceName, iTotArrayCount, lstLampType, iLsn);
        if (strPlcStatus != "")
        {
            return false;
        }
        #endregion

        #region Submit Lamp Mode Data
        DataSet dsLmModeConv = new DataSet();
        DataTable dtLmModeConv = new DataTable();

        for (int iGwNo = 1; iGwNo <= 12; iGwNo++)
        {
            dsLmModeConv = csDatabase.SrcLmAddModeConv(strPlcNo, strProcName, iGwNo);
            dtLmModeConv = dsLmModeConv.Tables[0];

            if (dtLmModeConv.Rows.Count > 0)
            {
                for (iLmCnt = 0; iLmCnt < dtLmModeConv.Rows.Count; iLmCnt++)
                {
                    if (Convert.ToString(dtLmModeConv.Rows[iLmCnt]["mode_data"]).Trim() != "")
                    {
                        String tmpLight = Convert.ToString(dtLmModeConv.Rows[iLmCnt]["mode_data"]).Trim();

                        String[] tmpLightData = tmpLight.Split('*');

                        for (iColorCnt = 0; iColorCnt < tmpLightData.Length; iColorCnt++)
                        {
                            lstLampMode.Add(Convert.ToInt16(tmpLightData[iColorCnt]));
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            lstLampMode.Add(Convert.ToInt16(0));
                        }
                    }
                }
            }
            while (lstLampMode.Count < (96 * iGwNo))
            {
                lstLampMode.Add(Convert.ToInt16(0));
            }
        }
        strStartDeviceName = "ZR1768";
        iTotDeviceInterval = 96;
        iTotDeviceCount = 12;
        iTotArrayCount = (iTotDeviceInterval * iTotDeviceCount);

        while (lstLampMode.Count < iTotArrayCount)
        {
            lstLampMode.Add(Convert.ToInt16(0));
        }

        strPlcStatus = PlcConnQuery.WriteDeviceByBlock(strStartDeviceName, iTotArrayCount, lstLampMode, iLsn);
        if (strPlcStatus != "")
        {
            return false;
        }
        #endregion

        #region Submit Instruction Code List
        DataSet dsInsCodeSubmit = new DataSet();
        DataTable dtInsCodeSubmit = new DataTable();

        dsInsCodeSubmit = csDatabase.SrcInsCodeConv("", strPlcNo, strProcName);
        dtInsCodeSubmit = dsInsCodeSubmit.Tables[0];

        if (dtInsCodeSubmit.Rows.Count > 0)
        {
            for (iInsCodeCnt = 0; iInsCodeCnt < dtInsCodeSubmit.Rows.Count; iInsCodeCnt++)
            {
                if (Convert.ToString(dtInsCodeSubmit.Rows[iInsCodeCnt]["byte_ins_code"]).Trim() != "")
                {
                    byteInsCode = Convert.ToString(dtInsCodeSubmit.Rows[iInsCodeCnt]["byte_ins_code"]);

                    String[] tmpByteInsCode = byteInsCode.Split('*');
                    foreach (string element in tmpByteInsCode)
                    {
                        lstInsCode.Add(Convert.ToInt16(element));
                    }
                }
                else
                {
                    for (int i = 0; i < 2; i++)
                    {
                        lstInsCode.Add(Convert.ToInt16(0));
                    }
                }
                if (Convert.ToString(dtInsCodeSubmit.Rows[iInsCodeCnt]["byte_model"]).Trim() != "")
                {
                    byteModel = Convert.ToString(dtInsCodeSubmit.Rows[iInsCodeCnt]["byte_model"]).Trim();

                    String[] tmpByteModel = byteModel.Split('*');
                    foreach (string element in tmpByteModel)
                    {
                        lstInsCode.Add(Convert.ToInt16(element));
                    }
                }
                else
                {
                    for (int i = 0; i < 2; i++)
                    {
                        lstInsCode.Add(Convert.ToInt16(0));
                    }
                }
                if (Convert.ToString(dtInsCodeSubmit.Rows[iInsCodeCnt]["byte_katashiki"]).Trim() != "")
                {
                    byteKatashiki = Convert.ToString(dtInsCodeSubmit.Rows[iInsCodeCnt]["byte_katashiki"]).Trim();

                    String[] tmpByteKatashiki = byteKatashiki.Split('*');
                    foreach (string element in tmpByteKatashiki)
                    {
                        lstInsCode.Add(Convert.ToInt16(element));
                    }
                }
                else
                {
                    for (int i = 0; i < 6; i++)
                    {
                        lstInsCode.Add(Convert.ToInt16(0));
                    }
                }
                if (Convert.ToString(dtInsCodeSubmit.Rows[iInsCodeCnt]["byte_sfx"]).Trim() != "")
                {
                    byteSfx = Convert.ToString(dtInsCodeSubmit.Rows[iInsCodeCnt]["byte_sfx"]).Trim();

                    String[] tmpByteSfx = byteSfx.Split('*');
                    foreach (string element in tmpByteSfx)
                    {
                        lstInsCode.Add(Convert.ToInt16(element));
                    }
                }
                else
                {
                    for (int i = 0; i < 1; i++)
                    {
                        lstInsCode.Add(Convert.ToInt16(0));
                    }
                }
                if (Convert.ToString(dtInsCodeSubmit.Rows[iInsCodeCnt]["byte_color"]).Trim() != "")
                {
                    byteColor = Convert.ToString(dtInsCodeSubmit.Rows[iInsCodeCnt]["byte_color"]).Trim();

                    String[] tmpByteColor = byteColor.Split('*');
                    foreach (string element in tmpByteColor)
                    {
                        lstInsCode.Add(Convert.ToInt16(element));
                    }
                }
                else
                {
                    for (int i = 0; i < 2; i++)
                    {
                        lstInsCode.Add(Convert.ToInt16(0));
                    }
                }
                if (Convert.ToString(dtInsCodeSubmit.Rows[iInsCodeCnt]["byte_comment"]).Trim() != "")
                {
                    byteComment = Convert.ToString(dtInsCodeSubmit.Rows[iInsCodeCnt]["byte_comment"]).Trim();

                    String[] tmpByteComment = byteComment.Split('*');
                    foreach (string element in tmpByteComment)
                    {
                        lstInsCode.Add(Convert.ToInt16(element));
                    }
                }
                else
                {
                    for (int i = 0; i < 7; i++)
                    {
                        lstInsCode.Add(Convert.ToInt16(0));
                    }
                }
            }

            strStartDeviceName = "ZR3020";
            iTotDeviceInterval = 20;
            iTotDeviceCount = 299;
            iTotArrayCount = (iTotDeviceInterval * iTotDeviceCount);

            while (lstInsCode.Count < iTotArrayCount)
            {
                lstInsCode.Add(Convert.ToInt16(0));
            }

            strPlcStatus = PlcConnQuery.WriteDeviceByBlock(strStartDeviceName, iTotArrayCount, lstInsCode, iLsn);
            if (strPlcStatus != "")
            {
                return false;
            }
        }
        else
        {
            return false;
        }
        #endregion

        #region Submit Instruction Code Mapping With Lamp Module
        DataSet dsInsCodeMapConv = new DataSet();
        DataTable dtInsCodeMapConv = new DataTable();

        dsInsCodeSubmit = csDatabase.SrcInsCodeConv("", strPlcNo, strProcName);
        dtInsCodeSubmit = dsInsCodeSubmit.Tables[0];

        if (dtInsCodeSubmit.Rows.Count > 0)
        {
            for (iInsCodeCnt = 0; iInsCodeCnt < dtInsCodeSubmit.Rows.Count; iInsCodeCnt++)
            {
                if (Convert.ToString(dtInsCodeSubmit.Rows[iInsCodeCnt]["ins_code_cnt"]).Trim() != "")
                {
                    tmpInsCodeCnt = Convert.ToInt32(dtInsCodeSubmit.Rows[iInsCodeCnt]["ins_code_cnt"]);
                }

                dsInsCodeMapConv = csDatabase.SrcInsCodeMapConv(tmpInsCodeCnt, strPlcNo, strProcName);
                dtInsCodeMapConv = dsInsCodeMapConv.Tables[0];

                for (int iInsCodeMapCnt = 0; iInsCodeMapCnt < dtInsCodeMapConv.Rows.Count; iInsCodeMapCnt++)
                {
                    for (int iGw = 1; iGw <= 12; iGw++)
                    {
                        String strGw = Convert.ToString(iGw);
                        if (Convert.ToString(dtInsCodeMapConv.Rows[iInsCodeMapCnt][strGw]).Trim() != "")
                        {
                            tmpInsCodeMap = Convert.ToString(dtInsCodeMapConv.Rows[iInsCodeMapCnt][strGw]);

                            Char[] characters = tmpInsCodeMap.ToCharArray();

                            string strBin1 = new string(characters, 48, 16);
                            string strBin2 = new string(characters, 32, 16);
                            string strBin3 = new string(characters, 16, 16);
                            string strBin4 = new string(characters, 0, 16);

                            lstLmMap.Add(Convert.ToInt16(strBin1, 2));
                            lstLmMap.Add(Convert.ToInt16(strBin2, 2));
                            lstLmMap.Add(Convert.ToInt16(strBin3, 2));
                            lstLmMap.Add(Convert.ToInt16(strBin4, 2));
                        }
                        else
                        {
                            lstLmMap.Add(Convert.ToInt16(0));
                            lstLmMap.Add(Convert.ToInt16(0));
                            lstLmMap.Add(Convert.ToInt16(0));
                            lstLmMap.Add(Convert.ToInt16(0));
                        }
                    }
                }
            }
        }

        strStartDeviceName = "ZR58048";
        iTotDeviceInterval = 48;
        iTotDeviceCount = 299;
        iTotArrayCount = (iTotDeviceInterval * iTotDeviceCount);

        while (lstLmMap.Count < iTotArrayCount)
        {
            lstLmMap.Add(Convert.ToInt16(0));
        }

        strPlcStatus = PlcConnQuery.WriteDeviceByBlock(strStartDeviceName, iTotArrayCount, lstLmMap, iLsn);
        if (strPlcStatus != "")
        {
            return false;
        }
        #endregion

        return true;
    }
    #endregion
    #endregion

    #region Events
    #region On Selected Index Changed
    protected void ddProcName_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            getDdGroupName();
            getDdBlockName();
            getDdRackName();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }

    protected void ddGroupName_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            getDdBlockName();
            getDdRackName();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }

    protected void ddBlockName_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            getDdRackName();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }

    protected void ddInsCode_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddInsCode.SelectedIndex == 0)
            {
                ddModel.SelectedIndex = 0;
                ddKatashiki.SelectedIndex = 0;
                ddSfx.SelectedIndex = 0;
                ddColor.SelectedIndex = 0;
            }
            else
            {
                DataSet dsInsMst = new DataSet();
                DataTable dtInsMst = new DataTable();

                dtInsMst = SearchInsCodeMstList(Convert.ToString(ddInsCode.SelectedValue), "", "", "", "", "");

                if (dtInsMst.Rows.Count > 0)
                {
                    if (Convert.ToString(dtInsMst.Rows[0]["model"]).Trim() != "")
                    {
                        ddModel.SelectedValue = Convert.ToString(dtInsMst.Rows[0]["model"]);
                        getDdKatashiki();
                    }
                    if (Convert.ToString(dtInsMst.Rows[0]["katashiki"]).Trim() != "")
                    {
                        ddKatashiki.SelectedValue = Convert.ToString(dtInsMst.Rows[0]["katashiki"]);
                        getDdSfx();
                    }
                    if (Convert.ToString(dtInsMst.Rows[0]["sfx"]).Trim() != "")
                    {
                        ddSfx.SelectedValue = Convert.ToString(dtInsMst.Rows[0]["sfx"]);
                        getDdColor();
                    }
                    if (Convert.ToString(dtInsMst.Rows[0]["color"]).Trim() != "")
                    {
                        ddColor.SelectedValue = Convert.ToString(dtInsMst.Rows[0]["color"]);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }

    protected void ddRackName_SelectedIndexChanged(object sender, EventArgs e)
    {
        AisPreview.Controls.Clear();
    }

    protected void ddModel_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            getDdKatashiki();
            getInsCode();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }

    protected void ddKatashiki_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            getDdSfx();
            getInsCode();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }

    protected void ddSfx_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            getDdColor();
            getInsCode();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }

    protected void ddColor_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            getInsCode();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region BtnSearch
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            #region Check Searching Criteria Selected
            if (ddProcName.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Process Name.";
                lblMsg.Visible = true;
                return;
            }
            else if (ddGroupName.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Group Name.";
                lblMsg.Visible = true;
                return;
            }
            else if (ddBlockName.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Block Name.";
                lblMsg.Visible = true;
                return;
            }
            else if (ddRackName.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Rack Name.";
                lblMsg.Visible = true;
                return;
            }
            #endregion

            String strPlcNo = Convert.ToString(ddProcName.SelectedValue);
            String strProcName = Convert.ToString(ddProcName.SelectedItem);
            String strGroupName = Convert.ToString(ddGroupName.SelectedValue);
            String strBlockName = Convert.ToString(ddBlockName.SelectedValue);
            String strRackName = Convert.ToString(ddRackName.SelectedValue);
            String strInsCode = "";
            String strModel = "";
            String strKatashiki = "";
            String strSfx = "";
            String strColor = "";
            //String strInsCode = Convert.ToString(ddInsCode.SelectedValue);
            //String strModel = Convert.ToString(ddModel.SelectedValue);
            //String strKatashiki = Convert.ToString(ddKatashiki.SelectedValue);
            //String strSfx = Convert.ToString(ddSfx.SelectedValue);
            //String strColor = Convert.ToString(ddColor.SelectedValue);

            Response.Write("<script>window.open('PrvRackResult.aspx?" +
                        "plc_no=" + GlobalFunc.getReplaceToUrl(strPlcNo) +
                        "&proc_name=" + GlobalFunc.getReplaceToUrl(strProcName) +
                        "&group_name=" + GlobalFunc.getReplaceToUrl(strGroupName) +
                        "&block_name=" + GlobalFunc.getReplaceToUrl(strBlockName) +
                        "&rack_name=" + GlobalFunc.getReplaceToUrl(strRackName) +
                        "&ins_code=" + GlobalFunc.getReplaceToUrl(strInsCode) +
                        "&model=" + GlobalFunc.getReplaceToUrl(strModel) +
                        "&katashiki=" + GlobalFunc.getReplaceToUrl(strKatashiki) +
                        "&sfx=" + GlobalFunc.getReplaceToUrl(strSfx) +
                        "&color=" + GlobalFunc.getReplaceToUrl(strColor) +
                        "&flag=preview');</script>");

            //String strRackMstRow = "";
            //String strRackMstCol = "";

            //getRackMst(out strRackMstRow, out strRackMstCol);
            //genTable(strRackMstRow, strRackMstCol);
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region btnConfirm
    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            #region Check Searching Criteria Selected
            if (ddProcName.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Process Name.";
                lblMsg.Visible = true;
                return;
            }
            else if (ddGroupName.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Group Name.";
                lblMsg.Visible = true;
                return;
            }
            else if (ddBlockName.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Block Name.";
                lblMsg.Visible = true;
                return;
            }
            else if (ddRackName.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Rack Name.";
                lblMsg.Visible = true;
                return;
            }
            else if (ddModel.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Model.";
                lblMsg.Visible = true;
                return;
            }
            else if (ddKatashiki.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Katashiki.";
                lblMsg.Visible = true;
                return;
            }
            else if (ddSfx.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Sfx.";
                lblMsg.Visible = true;
                return;
            }
            else if (ddColor.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Color.";
                lblMsg.Visible = true;
                return;
            }
            else if (ddInsCode.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select DPS Instruction Code.";
                lblMsg.Visible = true;
                return;
            }
            #endregion

            String strPlcNo = Convert.ToString(ddProcName.SelectedValue);
            String strProcName = Convert.ToString(ddProcName.SelectedItem);
            String strGroupName = Convert.ToString(ddGroupName.SelectedValue);
            String strBlockName = Convert.ToString(ddBlockName.SelectedValue);
            String strRackName = Convert.ToString(ddRackName.SelectedValue);
            String strInsCode = Convert.ToString(ddInsCode.SelectedValue);
            String strModel = Convert.ToString(ddModel.SelectedValue);
            String strKatashiki = Convert.ToString(ddKatashiki.SelectedValue);
            String strSfx = Convert.ToString(ddSfx.SelectedValue);
            String strColor = Convert.ToString(ddColor.SelectedValue);

            Response.Write("<script>window.open('PrtConvResult.aspx?" +
                        "plc_no=" + GlobalFunc.getReplaceToUrl(strPlcNo) +
                        "&proc_name=" + GlobalFunc.getReplaceToUrl(strProcName) +
                        "&group_name=" + GlobalFunc.getReplaceToUrl(strGroupName) +
                        "&block_name=" + GlobalFunc.getReplaceToUrl(strBlockName) +
                        "&rack_name=" + GlobalFunc.getReplaceToUrl(strRackName) +
                        "&ins_code=" + GlobalFunc.getReplaceToUrl(strInsCode) +
                        "&model=" + GlobalFunc.getReplaceToUrl(strModel) +
                        "&katashiki=" + GlobalFunc.getReplaceToUrl(strKatashiki) +
                        "&sfx=" + GlobalFunc.getReplaceToUrl(strSfx) +
                        "&color=" + GlobalFunc.getReplaceToUrl(strColor) +
                        "&flag=preview');</script>");

            //String strRackMstRow = "";
            //String strRackMstCol = "";

            //getRackMst(out strRackMstRow, out strRackMstCol);
            //genTable(strRackMstRow, strRackMstCol);
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region btnClear
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            ClearAll();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region btnSubmit
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            #region Check Searching Criteria Selected
            if (ddProcName.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Process Name.";
                lblMsg.Visible = true;
                return;
            }
            else if (ddGroupName.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Group Name.";
                lblMsg.Visible = true;
                return;
            }
            else if (ddBlockName.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Block Name.";
                lblMsg.Visible = true;
                return;
            }
            else if (ddInsCode.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Instruction Code.";
                lblMsg.Visible = true;
                return;
            }
            #endregion

            String strPlcNo = Convert.ToString(ddProcName.SelectedValue);
            String strProcName = Convert.ToString(ddProcName.SelectedItem);
            String strGroupName = Convert.ToString(ddGroupName.SelectedValue);
            String strBlockName = Convert.ToString(ddBlockName.SelectedValue);
            String strRackName = Convert.ToString(ddRackName.SelectedValue);
            String strInsCode = Convert.ToString(ddInsCode.SelectedValue);

            if (csDatabase.ChkLmTypeMaxCnt(strPlcNo, strProcName, strBlockName))
            {
                lblMsg.Text = "Maximum 12 Lamp Module Types allowed for 1 block. Please check.";
                lblMsg.Visible = true;
                return;
            }

            if (csDatabase.ChkLmAddMaxCnt(strPlcNo, strProcName, strBlockName))
            {
                lblMsg.Text = "Maximum 64 of Lamp Module Address allowed only for 1 block. Please check.";
                lblMsg.Visible = true;
                return;
            }

            if (csDatabase.ChkLmChange(strPlcNo, strProcName, strBlockName))
            {
                lblMsg.Text = "LM changed for current block. Please use submit all for changes apply to the whole block.";
                lblMsg.Visible = true;
                return;
            }

            GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to submit Instruction Code [" + strInsCode + "] Block Name [" + strBlockName + "]");
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                Boolean strResult = SubmitInsCodeList(strPlcNo, strProcName, strGroupName, strBlockName, strInsCode);

                if (!strResult)
                {
                    GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> failed to submit Instruction Code [" + strInsCode + "] Block Name [" + strBlockName + "]");
                    lblMsg.Text = "Instruction Code [" + strInsCode + "] not submitted for Block Name [" + strBlockName + "]. Please check.";
                    lblMsg.Visible = true;
                    return;
                }
                else
                {
                    int iLsn = Convert.ToInt32(csDatabase.GetLsn(strPlcNo));
                    PlcConnQuery.WriteDeviceRefreshBlock("M24", 1, iLsn);
                    GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> submitted Instruction Code [" + strInsCode + "] Block Name [" + strBlockName + "]");
                    Response.Write("<script language='javascript'>alert('Instruction Code [" + strInsCode + "] submitted for Block Name [" + strBlockName + "]')</script>");
                }
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region btnSubmitAll
    protected void btnSubmitAll_Click(object sender, EventArgs e)
    {
        try
        {
            #region Check Searching Criteria Selected
            if (ddProcName.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Process Name.";
                lblMsg.Visible = true;
                return;
            }
            else if (ddGroupName.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Group Name.";
                lblMsg.Visible = true;
                return;
            }
            #endregion

            String strPlcNo = Convert.ToString(ddProcName.SelectedValue);
            String strProcName = Convert.ToString(ddProcName.SelectedItem);
            String strGroupName = Convert.ToString(ddGroupName.SelectedValue);
            String strBlockName = Convert.ToString(ddBlockName.SelectedValue);
            String strRackName = Convert.ToString(ddRackName.SelectedValue);

            if (csDatabase.ChkLmTypeMaxCnt(strPlcNo, strProcName, strBlockName))
            {
                lblMsg.Text = "Maximum 12 Lamp Module Types allowed for 1 block. Please check.";
                lblMsg.Visible = true;
                return;
            }

            if (csDatabase.ChkLmAddMaxCnt(strPlcNo, strProcName, strBlockName))
            {
                lblMsg.Text = "Maximum 64 of Lamp Module Address allowed only for 1 block. Please check.";
                lblMsg.Visible = true;
                return;
            }

            if (csDatabase.ChkLmAddPhysicalNotOneNTwo(strPlcNo, strProcName, strBlockName))
            {
                lblMsg.Text = "Physical Lamp Module Address cannot set Address 1 or 2.";
                lblMsg.Visible = true;
                return;
            }

            if (csDatabase.ChkDuplicateLmAddPhysical(strPlcNo, strProcName, strBlockName))
            {
                lblMsg.Text = "Duplicate Physical Lamp Module Address found.";
                lblMsg.Visible = true;
                return;
            }

            GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to submit ALL Instruction Code for Block Name [" + strBlockName + "]");
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                Boolean strResult = SubmitInsCodeList(strPlcNo, strProcName, strGroupName, strBlockName, "");

                if (!strResult)
                {
                    GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> failed to submit ALL Instruction Code for Block Name [" + strBlockName + "]");
                    lblMsg.Text = "WARNING : Submit All FAILED for Block Name [" + strBlockName + "]. PLC Data is now NG! Please check Network Connection and re-submit.";
                    lblMsg.Visible = true;
                    return;
                }
                else
                {
                    int iLsn = Convert.ToInt32(csDatabase.GetLsn(strPlcNo));
                    PlcConnQuery.WriteDeviceRefreshBlock("M24", 1, iLsn);
                    csDatabase.UpdLmChange(strPlcNo, strProcName, strBlockName, "NULL");
                    GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> submitted ALL Instruction Code for Block Name [" + strBlockName + "]");
                    Response.Write("<script language='javascript'>alert('Submit All COMPLETED for Block Name [" + strBlockName + "].')</script>");
                }
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region btnExport
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            #region Check Searching Criteria Selected
            if (ddProcName.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Process Name.";
                lblMsg.Visible = true;
                return;
            }
            else if (ddGroupName.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Group Name.";
                lblMsg.Visible = true;
                return;
            }
            else if (ddBlockName.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Block Name.";
                lblMsg.Visible = true;
                return;
            }
            else if (ddRackName.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Rack Name.";
                lblMsg.Visible = true;
                return;
            }
            else if (ddModel.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Model.";
                lblMsg.Visible = true;
                return;
            }
            else if (ddKatashiki.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Katashiki.";
                lblMsg.Visible = true;
                return;
            }
            else if (ddSfx.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Sfx.";
                lblMsg.Visible = true;
                return;
            }
            else if (ddColor.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Color.";
                lblMsg.Visible = true;
                return;
            }
            else if (ddInsCode.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select DPS Instruction Code.";
                lblMsg.Visible = true;
                return;
            }
            #endregion

            String strPlcNo = Convert.ToString(ddProcName.SelectedValue);
            String strProcName = Convert.ToString(ddProcName.SelectedItem);
            String strGroupName = Convert.ToString(ddGroupName.SelectedValue);
            String strBlockName = Convert.ToString(ddBlockName.SelectedValue);
            String strRackName = Convert.ToString(ddRackName.SelectedValue);
            String strInsCode = Convert.ToString(ddInsCode.SelectedValue);
            String strModel = Convert.ToString(ddModel.SelectedValue);
            String strKatashiki = Convert.ToString(ddKatashiki.SelectedValue);
            String strSfx = Convert.ToString(ddSfx.SelectedValue);
            String strColor = Convert.ToString(ddColor.SelectedValue);

            Response.Write("<script>window.open('PrtConvResultExp.aspx?" +
                        "plc_no=" + GlobalFunc.getReplaceToUrl(strPlcNo) +
                        "&proc_name=" + GlobalFunc.getReplaceToUrl(strProcName) +
                        "&group_name=" + GlobalFunc.getReplaceToUrl(strGroupName) +
                        "&block_name=" + GlobalFunc.getReplaceToUrl(strBlockName) +
                        "&rack_name=" + GlobalFunc.getReplaceToUrl(strRackName) +
                        "&ins_code=" + GlobalFunc.getReplaceToUrl(strInsCode) +
                        "&model=" + GlobalFunc.getReplaceToUrl(strModel) +
                        "&katashiki=" + GlobalFunc.getReplaceToUrl(strKatashiki) +
                        "&sfx=" + GlobalFunc.getReplaceToUrl(strSfx) +
                        "&color=" + GlobalFunc.getReplaceToUrl(strColor) +
                        "&flag=export');</script>");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #endregion
}
