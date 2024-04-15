using System;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Itenso.Rtf.Converter.Html;
using Itenso.Rtf.Converter.Image;
using Itenso.Rtf.Converter.Text;
using Itenso.Rtf.Interpreter;
using Itenso.Rtf.Parser;
using Itenso.Rtf.Support;

public partial class RackMst : System.Web.UI.Page
{
    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        String tempRackName = Convert.ToString(Session["SessTempRackName"]);

        //PatchRackLocData();

        if (!ClientScript.IsStartupScriptRegistered("redirect"))
        {
            if (Convert.ToString(Session["SessUserId"]) == "")
            {
                Response.Write("<script language='javascript'>alert('Your user session has timed out. Please login again.');window.top.location ='../Login.aspx';</script>");
            }
        }

        if (!IsPostBack)
        {
            try
            {
                if (tempRackName != "")
                {
                    DataSet dsRackMst = new DataSet();
                    DataTable dtRackMst = new DataTable();
                    dsRackMst = csDatabase.SrcRackMst(tempRackName,"","","","");
                    dtRackMst = dsRackMst.Tables[0];
                    BindtoText(dtRackMst);
                    genTable();
                }
            }
            catch (Exception ex)
            {
                GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            }
        }
    }
    #endregion

    #region Method
    #region BindtoText
    public void BindtoText(DataTable dt)
    {
        try
        {
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToString(dt.Rows[0]["rack_name"]).Trim() != "")
                {
                    lblTmpRackName.Text = Convert.ToString(dt.Rows[0]["rack_name"]).Trim();
                    txtRackName.Text = Convert.ToString(dt.Rows[0]["rack_name"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["col_cnt"]).Trim() != "")
                {
                    txtColCnt.Text = Convert.ToString(dt.Rows[0]["col_cnt"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["row_cnt"]).Trim() != "")
                {
                    txtRowCnt.Text = Convert.ToString(dt.Rows[0]["row_cnt"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["proc_name"]).Trim() != "")
                {
                    txtProcName.Text = Convert.ToString(dt.Rows[0]["proc_name"]);
                }
                if (Convert.ToString(dt.Rows[0]["plc_no"]).Trim() != "")
                {
                    txtPlcNo.Text = Convert.ToString(dt.Rows[0]["plc_no"]);
                }
                if (Convert.ToString(dt.Rows[0]["group_name"]).Trim() != "")
                {
                    txtGroupName.Text = Convert.ToString(dt.Rows[0]["group_name"]);
                }
                if (Convert.ToString(dt.Rows[0]["block_name"]).Trim() != "")
                {
                    txtBlockName.Text = Convert.ToString(dt.Rows[0]["block_name"]);
                }
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Generate Table
    private void genTable()
    {
        try
        {
            AisPreview.Controls.Clear();
            Table tbAis = new Table();
            tbAis.Style.Add("table-layout", "fixed");
            tbAis.Width = Unit.Percentage(100);
            tbAis.BorderWidth = Unit.Pixel(1);
            tbAis.BorderColor = Color.Black;

            int rowCnt = 0;
            int colCnt = 0;
            int rowCtr = 0;
            int colCtr = 0;

            rowCnt = int.Parse(txtRowCnt.Text);
            colCnt = int.Parse(txtColCnt.Text);

            for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
            {
                TableRow tRow = new TableRow();
                tRow.Style.Add("table-layout", "fixed");
                tRow.Width = Unit.Percentage(100);
                tRow.Height = Unit.Pixel(150);
                tRow.BorderWidth = Unit.Pixel(1);
                tRow.BorderColor = Color.Black;
                tbAis.Rows.Add(tRow);

                for (colCtr = 1; colCtr <= colCnt; colCtr++)
                {
                    String strAisDet = "";
                    String strModDet = "";
                    String strRackName = Convert.ToString(txtRackName.Text);
                    String strPlcNo = Convert.ToString(txtPlcNo.Text);
                    String strProcName = Convert.ToString(txtProcName.Text);
                    String strRackDetId = strRackName + "^" + rowCtr + "^" + colCtr;

                    TableCell tCell = new TableCell();
                    tCell.BorderWidth = Unit.Pixel(1);
                    tCell.BorderColor = Color.Black;
                    tCell.Wrap = false;
                    tCell.Style.Add("table-layout", "fixed");
                    tCell.Style.Add("padding", "0px 0px 0px 0px");
                    tCell.Style.Add("min-width", "260px");
                    tCell.Style.Add("min-height", "150px");
                    tCell.Width = Unit.Pixel(260);
                    tCell.HorizontalAlign = HorizontalAlign.Left;
                    tCell.VerticalAlign = VerticalAlign.Top;

                    tRow.Cells.Add(tCell);

                    #region Get and Assign Ais Detail
                    System.Web.UI.WebControls.HyperLink hlAisData = new HyperLink();
                    strAisDet = getAisDetString(strRackDetId);
                    hlAisData.Font.Underline = false;

                    #region Sample Data Retrieve
                    // 0 strHjId
                    // 1 strHjRow
                    // 2 strHjCol
                    // 3 strPartsTitle
                    // 4 strPartsNo
                    // 5 strColorSfx
                    // 6 strSymbolCode
                    // 7 strSymbolRtf;
                    #endregion
                    String[] tmpAisVal = strAisDet.Split('~');
                    int tmpAisValCnt = tmpAisVal.Length;
                    if (tmpAisValCnt == 1)
                    {
                        hlAisData.Text = strAisDet;
                    }
                    else
                    {
                        String strSymbol = Itenso.Solutions.Community.Rtf2Html.RtfConverterDemo.ConvertRtf2Txt(tmpAisVal[7]);
                        strSymbol = strSymbol.Replace("\r", "");
                        strSymbol = strSymbol.Replace("\n", "");

                        String strBgColor = csDatabase.GetPartsBgColor(tmpAisVal[4], tmpAisVal[5]);
                        String strFontColor = csDatabase.GetPartsFontColor(tmpAisVal[4], tmpAisVal[5]);

                        hlAisData.Text = tmpAisVal[3] + "</br>" +
                                         //"Row No : " + tmpAisVal[1] + "    Col No : " + tmpAisVal[2] + "</br>" +
                                         tmpAisVal[4] + " - " + tmpAisVal[5] + "</br>" +
                                          "<p style='text-align:center;'><font face='arial' size='6' color='" + strFontColor + "'><span style='background-color:" + strBgColor + ";'>&nbsp;" + strSymbol + "&nbsp;</span></font></p></br>";
                    }
                    hlAisData.NavigateUrl = "RackMstSelAis.aspx?rack_name=" + GlobalFunc.getReplaceToUrl(strRackName) + "&part_id=" + GlobalFunc.getReplaceToUrl(strRackDetId) + "&plc_no=" + GlobalFunc.getReplaceToUrl(strPlcNo) + "&proc_name=" + GlobalFunc.getReplaceToUrl(strProcName);
                    //hlAisData.NavigateUrl = "javascript:window.open('RackMstSelAis.aspx?rack_name=" + GlobalFunc.getReplaceToUrl(strRackName) + "&part_id=" + GlobalFunc.getReplaceToUrl(strRackDetId) + "&plc_no=" + GlobalFunc.getReplaceToUrl(strPlcNo) + "&proc_name=" + GlobalFunc.getReplaceToUrl(strProcName) + "')";
                    tCell.Controls.Add(hlAisData);
                    #endregion

                    #region Get and Assign Module Address Detail
                    System.Web.UI.WebControls.HyperLink hlModuleAdd = new HyperLink();
                    strModDet = getModDetString(strRackDetId);


                    #region Sample Data Retrieve
                    // 0 strModuleAdd
                    // 1 strModuleName
                    #endregion
                    String[] tmpModVal = strModDet.Split('~');
                    int tmpModValCnt = tmpModVal.Length;
                    if (tmpModValCnt == 1)
                    {
                        hlModuleAdd.Text = strModDet;
                    }
                    else
                    {
                        hlModuleAdd.Text = "Module Address : " + tmpModVal[0] + "</br>";
                    }
                    hlModuleAdd.NavigateUrl = "RackMstSelModuleAdd.aspx?rack_name=" + GlobalFunc.getReplaceToUrl(strRackName) + "&part_id=" + GlobalFunc.getReplaceToUrl(strRackDetId) + "&plc_no=" + GlobalFunc.getReplaceToUrl(strPlcNo) + "&proc_name=" + GlobalFunc.getReplaceToUrl(strProcName);
                    //hlModuleAdd.NavigateUrl = "javascript:window.open('RackMstSelModuleAdd.aspx?rack_name=" + GlobalFunc.getReplaceToUrl(strRackName) + "&part_id=" + GlobalFunc.getReplaceToUrl(strRackDetId) + "&plc_no=" + GlobalFunc.getReplaceToUrl(strPlcNo) + "&proc_name=" + GlobalFunc.getReplaceToUrl(strProcName) + "')";
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
        String strRackName = Convert.ToString(txtRackName.Text);
        String strAisRackDetAis = "Select AIS Data</br></br></br></br></br></br>";
        DataSet dsRackDetAis = new DataSet();
        DataTable dtRackDetAis = new DataTable();

        dsRackDetAis = csDatabase.GetRackMstDetAis(strRackDetId);
        dtRackDetAis = dsRackDetAis.Tables[0];

        #region Initialise Variable
        String strHjId = "";
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
                strPartsNo = Convert.ToString(dtRackDetAis.Rows[0]["parts_no"]);
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
                strAisRackDetAis = strHjId + "~" + strHjRow + "~" + strHjCol + "~" + strPartsTitle + "~" + strPartsNo + "~" + strColorSfx + "~" + strSymbolCode + "~" + strSymbolRtf;
                
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
        String strRackName = Convert.ToString(txtRackName.Text);
        String strAisRackDetModule = "Select Module Address";
        DataSet dsRackDetModule = new DataSet();
        DataTable dtRackDetModule = new DataTable();

        dsRackDetModule = csDatabase.GetRackMstDetModule(strRackDetId);
        dtRackDetModule = dsRackDetModule.Tables[0];

        #region Initialise Variable
        String strModuleAdd = "";
        String strModuleName = "";
        String strLmAddress = "";                                                                   //Added by YanTeng 12/10/2020
        char[] splitchar = { '-' };
        #endregion

        if (dtRackDetModule.Rows.Count > 0)
        {
            #region Assign Variable
            if (Convert.ToString(dtRackDetModule.Rows[0]["module_add"]).Trim() != "")
            {
                strModuleAdd = Convert.ToString(dtRackDetModule.Rows[0]["module_add"]).Trim();
                strLmAddress = strModuleAdd.Split(splitchar)[0].Trim();                             //Added by YanTeng 12/10/2020
            }
            if (Convert.ToString(dtRackDetModule.Rows[0]["module_name"]).Trim() != "")
            {
                 strModuleName = Convert.ToString(dtRackDetModule.Rows[0]["module_name"]).Trim();
            }
            #endregion

            if (strModuleAdd != "")
            {
                strAisRackDetModule = strModuleAdd + "~" + strModuleName;

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

    #region Patch Rack Location
    protected void PatchRackLocData()
    {
        #region Previous Code  ***ace_20160416_001
        //DataSet dsPatch = new DataSet();
        //DataTable dtPatch = new DataTable();

        //String sqlQuery = "SELECT * FROM dt_RackMstDet WHERE parts_no is not null";
        //String strCurUser = Convert.ToString(Session["SessUserId"]);  //***ace_20160416_001

        //dsPatch = ConnQuery.getBindingDatasetData(sqlQuery);
        //dtPatch = dsPatch.Tables[0];

        //if (dtPatch.Rows.Count > 0)
        //{
        //    for (int i = 0; i < dtPatch.Rows.Count; i++)
        //    {
        //        if (Convert.ToString(dtPatch.Rows[i]["parts_no"]).Trim() != "")
        //        {
        //            String strRackDetId = Convert.ToString(dtPatch.Rows[i]["rack_det_id"]).Trim();
        //            String strPartsNo = Convert.ToString(dtPatch.Rows[i]["parts_no"]).Trim();
        //            String strPartsName = Convert.ToString(dtPatch.Rows[i]["parts_title"]).Trim();
        //            String strColorSfx = Convert.ToString(dtPatch.Rows[i]["color_sfx"]).Trim();

        //            String[] tmpRackMstDetIdVal = strRackDetId.Split('^');
        //            int tmpRackMstDetIdCnt = tmpRackMstDetIdVal.Length;
        //            if (tmpRackMstDetIdCnt > 1)
        //            {
        //                String strRackLoc = tmpRackMstDetIdVal[0] + "  " + tmpRackMstDetIdVal[1] + "-" + tmpRackMstDetIdVal[2];
        //                csDatabase.UpdPartsLoc(strPartsNo, strColorSfx, strRackDetId, strRackLoc);
        //            }

        //            String strPartsNumSymbolCode = csDatabase.GetPartsNumSymbolCode(strPartsNo, strColorSfx).Trim();
        //            String strPartsNumSymbolRtf = csDatabase.GetPartsNumSymbolRtf(strPartsNo, strColorSfx).Trim();

        //            //***ace_20160416_001
        //            //sqlQuery = "UPDATE dt_RackMstDet SET symbol_code = '" + strPartsNumSymbolCode + "', symbol_rtf = '" + strPartsNumSymbolRtf + "' WHERE rack_det_id = '" + strRackDetId + "'";
        //            sqlQuery = "UPDATE dt_RackMstDet SET symbol_code = '" + strPartsNumSymbolCode + "', symbol_rtf = '" + strPartsNumSymbolRtf + "', last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE rack_det_id = '" + strRackDetId + "'";
        //            ConnQuery.ExecuteQuery(sqlQuery);
        //        }
        //    }
        //}

        //dsPatch = new DataSet();
        //dtPatch = new DataTable();

        //sqlQuery = "SELECT * FROM dt_LampModuleAddMst WHERE rack_det_id is not null";

        //dsPatch = ConnQuery.getBindingDatasetData(sqlQuery);
        //dtPatch = dsPatch.Tables[0];

        //if (dtPatch.Rows.Count > 0)
        //{
        //    for (int i = 0; i < dtPatch.Rows.Count; i++)
        //    {
        //        if (Convert.ToString(dtPatch.Rows[i]["rack_det_id"]).Trim() != "")
        //        {
        //            String strRackDetId = Convert.ToString(dtPatch.Rows[i]["rack_det_id"]).Trim();
        //            String strModuleAdd = Convert.ToString(dtPatch.Rows[i]["module_add"]).Trim();
        //            String strModuleName = Convert.ToString(dtPatch.Rows[i]["module_name"]).Trim();

        //            String[] tmpRackMstDetIdVal = strRackDetId.Split('^');
        //            int tmpRackMstDetIdCnt = tmpRackMstDetIdVal.Length;
        //            if (tmpRackMstDetIdCnt > 1)
        //            {
        //                String strRackLoc = tmpRackMstDetIdVal[0] + "  " + tmpRackMstDetIdVal[1] + "-" + tmpRackMstDetIdVal[2];
        //                csDatabase.UpdLampModuleLoc("IP", strModuleAdd, strModuleName, strRackDetId, strRackLoc);
        //            }
        //        }
        //    }
        //}
        #endregion

        try
        {
            DataSet dsPatch = new DataSet();
            DataTable dtPatch = new DataTable();

            String sqlQuery = "SELECT * FROM dt_RackMstDet WHERE parts_no is not null";
            String strCurUser = Convert.ToString(Session["SessUserId"]);  //***ace_20160416_001

            dsPatch = ConnQuery.getBindingDatasetData(sqlQuery);
            dtPatch = dsPatch.Tables[0];

            if (dtPatch.Rows.Count > 0)
            {
                for (int i = 0; i < dtPatch.Rows.Count; i++)
                {
                    if (Convert.ToString(dtPatch.Rows[i]["parts_no"]).Trim() != "")
                    {
                        String strRackDetId = Convert.ToString(dtPatch.Rows[i]["rack_det_id"]).Trim();
                        String strPartsNo = Convert.ToString(dtPatch.Rows[i]["parts_no"]).Trim();
                        String strPartsName = Convert.ToString(dtPatch.Rows[i]["parts_title"]).Trim();
                        String strColorSfx = Convert.ToString(dtPatch.Rows[i]["color_sfx"]).Trim();

                        String[] tmpRackMstDetIdVal = strRackDetId.Split('^');
                        int tmpRackMstDetIdCnt = tmpRackMstDetIdVal.Length;
                        if (tmpRackMstDetIdCnt > 1)
                        {
                            String strRackLoc = tmpRackMstDetIdVal[0] + "  " + tmpRackMstDetIdVal[1] + "-" + tmpRackMstDetIdVal[2];
                            csDatabase.UpdPartsLoc(strPartsNo, strColorSfx, strRackDetId, strRackLoc);
                        }

                        String strPartsNumSymbolCode = csDatabase.GetPartsNumSymbolCode(strPartsNo, strColorSfx).Trim();
                        String strPartsNumSymbolRtf = csDatabase.GetPartsNumSymbolRtf(strPartsNo, strColorSfx).Trim();

                        //***ace_20160416_001
                        //sqlQuery = "UPDATE dt_RackMstDet SET symbol_code = '" + strPartsNumSymbolCode + "', symbol_rtf = '" + strPartsNumSymbolRtf + "' WHERE rack_det_id = '" + strRackDetId + "'";
                        sqlQuery = "UPDATE dt_RackMstDet SET symbol_code = '" + strPartsNumSymbolCode + "', symbol_rtf = '" + strPartsNumSymbolRtf + "', last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE rack_det_id = '" + strRackDetId + "'";
                        ConnQuery.ExecuteQuery(sqlQuery);
                    }
                }
            }

            dsPatch = new DataSet();
            dtPatch = new DataTable();

            sqlQuery = "SELECT * FROM dt_LampModuleAddMst WHERE rack_det_id is not null";

            dsPatch = ConnQuery.getBindingDatasetData(sqlQuery);
            dtPatch = dsPatch.Tables[0];

            if (dtPatch.Rows.Count > 0)
            {
                for (int i = 0; i < dtPatch.Rows.Count; i++)
                {
                    if (Convert.ToString(dtPatch.Rows[i]["rack_det_id"]).Trim() != "")
                    {
                        String strRackDetId = Convert.ToString(dtPatch.Rows[i]["rack_det_id"]).Trim();
                        String strModuleAdd = Convert.ToString(dtPatch.Rows[i]["module_add"]).Trim();
                        String strModuleName = Convert.ToString(dtPatch.Rows[i]["module_name"]).Trim();

                        String[] tmpRackMstDetIdVal = strRackDetId.Split('^');
                        int tmpRackMstDetIdCnt = tmpRackMstDetIdVal.Length;
                        if (tmpRackMstDetIdCnt > 1)
                        {
                            String strRackLoc = tmpRackMstDetIdVal[0] + "  " + tmpRackMstDetIdVal[1] + "-" + tmpRackMstDetIdVal[2];
                            csDatabase.UpdLampModuleLoc("IP", strModuleAdd, strModuleName, strRackDetId, strRackLoc);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion
    #endregion

    #region Events

    #region BtnBack
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Write("<script>window.close()</script>");        
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #endregion
}
