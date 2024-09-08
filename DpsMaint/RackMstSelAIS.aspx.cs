using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Collections.Generic;
using dpant;
using Itenso.Rtf;
using Itenso.Rtf.Converter.Html;
using Itenso.Rtf.Converter.Image;
using Itenso.Rtf.Converter.Text;
using Itenso.Rtf.Interpreter;
using Itenso.Rtf.Parser;
using Itenso.Rtf.Support;

public partial class SelectionAIS : System.Web.UI.Page
{
    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        String strRackName = Convert.ToString(Request.QueryString["rack_name"]);
        String strPartId = Convert.ToString(Request.QueryString["part_id"]);
        String strPlcNo = Convert.ToString(Request.QueryString["plc_no"]);
        String strProcName = Convert.ToString(Request.QueryString["proc_name"]);

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
                getAisType();
                String strSessAisType = Convert.ToString(Session["SessAisType"]);
                if (strSessAisType != "")
                {
                    ddAisType.SelectedValue = strSessAisType;
                }

                getAisName();
                String strSessAisName = Convert.ToString(Session["SessAisName"]);
                if (strSessAisName != "")
                {
                    ddAisName.SelectedValue = strSessAisName;
                }

                getAisRevNo();
                if (strRackName != "")
                {
                    lblTmpRackName.Text = strRackName;
                    lblTmpPartId.Text = strPartId;
                    lblTmpPlcNo.Text = strPlcNo;
                    lblTmpProcName.Text = strProcName;
                }
            }
            catch (Exception ex)
            {
                GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            }
        }
        SearchDataHJList();
    }
    #endregion

    #region Method

    #region Get Drop Down List
    #region getAisType
    private void getAisType()
    {
        try
        {
            ddAisType.DataSource = GlobalFunc.getAisType();
            ddAisType.DataTextField = "Description";
            ddAisType.DataValueField = "Description";
            ddAisType.DataBind();
            ddAisType.Items.Insert(0, " ");

            AisPreview.Controls.Clear();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region getAisName
    private void getAisName()
    {
        try
        {
            if (ddAisType.SelectedIndex != 0)
            {
                String strAisType = Convert.ToString(ddAisType.SelectedValue);
                ddAisName.DataSource = GlobalFunc.getAisNameDps(strAisType);
                ddAisName.DataTextField = "Description";
                //ddAisName.DataValueField = "Name";
                ddAisName.DataValueField = "Description";
                ddAisName.DataBind();
                ddAisName.Items.Insert(0, " ");

                AisPreview.Controls.Clear();
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region getAisRevNo
    private void getAisRevNo()
    {
        try
        {
            if (ddAisName.SelectedIndex != 0)
            {
                String strAisName = Convert.ToString(ddAisName.SelectedValue);
                ddRevNo.DataSource = GlobalFunc.getAisRevNo(strAisName);
                ddRevNo.DataTextField = "Description";
                ddRevNo.DataValueField = "Description";
                ddRevNo.DataBind();
                ddRevNo.Items.Insert(0, " ");

                AisPreview.Controls.Clear();
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion
    #endregion

    #region SearchDataHJList
    private void SearchDataHJList()
    {
        try
        {
            DataSet dsSearch = new DataSet();
            DataTable dtSearch = new DataTable();

            String strAisType = Convert.ToString(ddAisType.SelectedValue).Trim();
            String strAisName = Convert.ToString(ddAisName.SelectedValue).Trim();
            String strAisRevNo = Convert.ToString(ddRevNo.SelectedValue).Trim();
            String intRowCnt = csDatabase.GetHjRowCnt(strAisType, strAisName);
            String intColCnt = csDatabase.GetHjColCnt(strAisType, strAisName);

            if (strAisRevNo == " ")
            {
                strAisRevNo = "";
            }

            if (strAisType == "" || strAisName == "" || strAisRevNo == "")
            {
                return;
            }

            if (intRowCnt == "")
            {
                intRowCnt = "0";
            }

            if (intColCnt == "")
            {
                intColCnt = "0";
            }

            dsSearch = csDatabase.SrcDataHJList(strAisType, strAisName, strAisRevNo);
            dtSearch = dsSearch.Tables[0];
            genTable(dtSearch, intRowCnt, intColCnt);
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Generate Table
    private void genTable(DataTable dtDataHJList, String intRowCnt, String intColCnt)
    {
        try
        {
            AisPreview.Controls.Clear();
            Table tbAis = new Table();
            tbAis.Style.Add("table-layout", "fixed");
            tbAis.Width = Unit.Percentage(100);
            tbAis.BorderColor = System.Drawing.Color.Black;
            tbAis.BorderWidth = Unit.Pixel(1);

            int rowCnt = 0;
            int colCnt = 0;
            int rowCtr = 0;
            int colCtr = 0;
            int curRow = 0;

            rowCnt = int.Parse(intRowCnt);
            colCnt = int.Parse(intColCnt);

            for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
            {
                TableRow tRow = new TableRow();
                tRow.Style.Add("table-layout", "fixed");
                tRow.Width = Unit.Percentage(100);
                tRow.Height = Unit.Pixel(111);
                tRow.BorderWidth = Unit.Pixel(1);
                tRow.BorderColor = System.Drawing.Color.Black;
                tbAis.Rows.Add(tRow);

                for (colCtr = 1; colCtr <= colCnt; colCtr++)
                {
                    if (colCtr == 11)
                    {
                        tRow = new TableRow();
                        tRow.Width = Unit.Percentage(100);
                        tRow.Height = Unit.Pixel(111);
                        tRow.BorderWidth = Unit.Pixel(1);
                        tRow.BorderColor = System.Drawing.Color.Black;
                        tbAis.Rows.Add(tRow);
                    }

                    List<String> lstAisDet = new List<String>();
                    String[] tmpColHeader = new String[12];
                    String strRackName = Convert.ToString(lblTmpRackName.Text);
                    String strRackDetId = Convert.ToString(lblTmpPartId.Text);

                    TableCell tCell = new TableCell();
                    tCell.Style.Add("table-layout", "fixed");
                    tCell.BorderWidth = Unit.Pixel(1);
                    tCell.BorderColor = System.Drawing.Color.Black;
                    tCell.Wrap = false;
                    tCell.Style.Add("table-layout", "fixed");
                    tCell.Style.Add("padding", "1px 1px 1px 1px");
                    tCell.Style.Add("min-width", "260px");
                    tCell.Style.Add("min-height", "111px");
                    tCell.Width = Unit.Pixel(260);
                    tCell.HorizontalAlign = HorizontalAlign.Left;
                    tCell.VerticalAlign = VerticalAlign.Top;

                    tRow.Cells.Add(tCell);

                    if (Convert.ToString(ddAisType.SelectedValue) == "Jundate")
                    {
                        curRow = 0;
                    }
                    else
                    {
                        curRow = rowCtr;
                    }

                    lstAisDet = getAisString(dtDataHJList, curRow, colCtr);

                    #region Sample Return String
                    // 0 Harigami/Jundate ID *
                    // 1 Harigami/Jundate Item ID *
                    // 2 Harigami/Jundate Row * 
                    // 3 Harigami/Jundate Colums * 
                    // 4 Harigami/Jundate Parts Title *
                    // 5 Harigami/Jundate Part No * 
                    // 6 Harigami/Jundate Color Sfx * 
                    // 7 Parts Number Symbol Code * 
                    // 8 Parts Number Symbol Rtf
                    #endregion

                    if (lstAisDet.Count > 0)
                    {
                        String strColHeader = Convert.ToString(lstAisDet[0]);
                        tmpColHeader = strColHeader.Split('~');
                    }

                    Label lblRackHeader = new Label();
                    lblRackHeader.BorderWidth = Unit.Pixel(1);
                    lblRackHeader.Width = Unit.Percentage(84);
                    if (tmpColHeader[4] != "")
                    {
                        lblRackHeader.Text = tmpColHeader[4];
                    }
                    else
                    {
                        lblRackHeader.Text = "&nbsp;";
                    }

                    Label lblPos = new Label();
                    lblPos.BorderWidth = Unit.Pixel(1);
                    lblPos.Width = Unit.Percentage(14);
                    lblPos.Text = rowCtr + "-" + colCtr;

                    tCell.Controls.Add(lblRackHeader);
                    tCell.Controls.Add(lblPos);

                    foreach (String strAisDet in lstAisDet)
                    {
                        String strSymbolCode = "";
                        String strSymbol = "";
                        System.Web.UI.WebControls.LinkButton linkButton = new LinkButton();
                        linkButton.CommandName = "SaveAisData";
                        linkButton.CommandArgument = Convert.ToString(strRackDetId) + "~" + strAisDet;
                        linkButton.Font.Underline = false;

                        String[] tmpVal = strAisDet.Split('~');
                        strSymbolCode = tmpVal[8];
                        if (strSymbolCode != "")
                        {
                            //strSymbol = Itenso.Solutions.Community.Rtf2Html.RtfConverterDemo.ConvertRtf2Html(strSymbolCode);
                            strSymbol = Itenso.Solutions.Community.Rtf2Html.RtfConverterDemo.ConvertRtf2Txt(strSymbolCode);
                            strSymbol = strSymbol.Replace("\r", "");
                            strSymbol = strSymbol.Replace("\n", "");
                        }

                        String strBgColor = csDatabase.GetPartsBgColor(tmpVal[5], tmpVal[6]);
                        String strFontColor = csDatabase.GetPartsFontColor(tmpVal[5], tmpVal[6]);

                        linkButton.Text = "</br>" +
                                          "<p style='text-align:center;'><font face='arial' size='6' color='" + strFontColor + "'><span style='background-color:" + strBgColor + ";'>&nbsp;" + strSymbol + "&nbsp;</span></font></p></br>" +
                                          tmpVal[5] + "</br>" +
                                          tmpVal[6];
                        linkButton.Click += new System.EventHandler(this.link_Click);
                        tCell.Controls.Add(linkButton);
                    }
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
    protected List<String> getAisString(DataTable dtHjData, int asRowCtr, int asColCtr)
    {
        List<String> lstAisString = new List<String>();

        if (dtHjData.Rows.Count > 0)
        {
            #region Initialise Variable
            String strHjId = "";
            String strHjItemId = "";
            String strHjName = "";
            String strHjCol = "";
            String strHjRow = "";
            String strHjPartsTitle = "";
            String strHjSymbolCode = "";
            String strHjPartNo = "";
            String strHjColorSfx = "";
            String strHjModel = "";
            String strHjKatashiki = "";
            String strHjSfx = "";
            String strHjColor = "";
            String strPartDet = "";
            #endregion

            for (int i = 0; i <= dtHjData.Rows.Count - 1; i++)
            {
                if (Convert.ToString(dtHjData.Rows[i]["col"]).Trim() != "")
                {
                    strHjCol = Convert.ToString(dtHjData.Rows[i]["col"]);
                }
                if (Convert.ToString(dtHjData.Rows[i]["row"]).Trim() != "")
                {
                    strHjRow = Convert.ToString(dtHjData.Rows[i]["row"]);
                }
                else
                {
                    strHjRow = "0";
                }

                if (int.Parse(strHjRow) == asRowCtr)
                {
                    if (int.Parse(strHjCol) == asColCtr)
                    {
                        #region Check Table Data Exist Then Assign Variable
                        if (Convert.ToString(dtHjData.Rows[i]["id"]).Trim() != "")
                        {
                            strHjId = Convert.ToString(dtHjData.Rows[i]["id"]).Trim();
                        }
                        if (Convert.ToString(dtHjData.Rows[i]["item_id"]).Trim() != "")
                        {
                            strHjItemId = Convert.ToString(dtHjData.Rows[i]["item_id"]).Trim();
                        }
                        if (Convert.ToString(dtHjData.Rows[i]["parts_title"]).Trim() != "")
                        {
                            strHjPartsTitle = Convert.ToString(dtHjData.Rows[i]["parts_title"]).Trim();
                        }
                        if (Convert.ToString(dtHjData.Rows[i]["part_no"]).Trim() != "")
                        {
                            strHjPartNo = Convert.ToString(dtHjData.Rows[i]["part_no"]).Trim();
                        }
                        if (Convert.ToString(dtHjData.Rows[i]["color_sfx"]).Trim() != "")
                        {
                            strHjColorSfx = Convert.ToString(dtHjData.Rows[i]["color_sfx"]).Trim();
                        }
                        #endregion

                        strPartDet = getPartDetString(strHjPartNo, strHjColorSfx);
                        lstAisString.Add(strHjId + "~" + strHjItemId + "~" + strHjRow + "~" + strHjCol + "~" + strHjPartsTitle + "~" +
                                         strHjPartNo + "~" + strHjColorSfx + "~" + strPartDet);
                    }
                }
            }
            return lstAisString;
        }
        else
        {
            return lstAisString;
        }
    }
    #endregion

    #region Get Part Number String
    private String getPartDetString(String strHjPartNo, String strHjColorSfx)
    {
        String strPartDet = "";
        String strSymbolCode = csDatabase.GetPartsNumSymbolCode(strHjPartNo, strHjColorSfx).Trim();
        String strSymbolRtf = csDatabase.GetPartsNumSymbolRtf(strHjPartNo, strHjColorSfx).Trim();

        strPartDet = strSymbolCode + "~" + strSymbolRtf;
        return strPartDet;
    }
    #endregion

    #endregion

    #region Events

    #region BtnSearch
    //not using since it is not visible
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddAisType.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select AIS Type.";
                lblMsg.Visible = true;
                return;
            }
            else if (ddAisName.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select AIS Name.";
                lblMsg.Visible = true;
                return;
            }
            else
            {
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region btnEmpty
    protected void btnEmpty_Click(object sender, EventArgs e)
    {
        try
        {
            String strRackMstDetId = Convert.ToString(lblTmpPartId.Text);
            String strRackName = Convert.ToString(lblTmpRackName.Text);
            String strProcName = csDatabase.GetRackMstProcName(strRackName);
            String strPlcNo = Convert.ToString(lblTmpPlcNo.Text);

            String strRackLoc = "";
            String[] tmpRackMstDetIdVal = strRackMstDetId.Split('^');
            int tmpRackMstDetIdCnt = tmpRackMstDetIdVal.Length;
            if (tmpRackMstDetIdCnt > 1)
            {
                strRackLoc = tmpRackMstDetIdVal[0] + "  " + tmpRackMstDetIdVal[1] + "-" + tmpRackMstDetIdVal[2];
            }

            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to delete Rack Master Location '" + strRackLoc + "'");
                if (!csDatabase.ChkRackMstDetExist(strRackMstDetId))
                {
                    lblMsg.Text = "AIS Data is not selected for Current Rack Item.";
                    lblMsg.Visible = true;
                    return;
                }
                else
                {
                    Boolean blDelFlg = csDatabase.DelRackMstDetAis(strRackMstDetId);

                    if (blDelFlg)
                    {
                        if (csDatabase.ChkLmLocExist(strRackMstDetId))
                        {
                            csDatabase.UpdLampModuleLoc(strProcName, "", "", strRackMstDetId, "");
                        }
                        //ENHANCEMENT NEEDED -DONE
                        csDatabase.UpdPartsLoc("", "", strRackMstDetId, "", strRackName, strPlcNo);
                    }
                    else
                    {
                        lblMsg.Text = "Unable to empty Module Address for Current Rack Item.";
                        lblMsg.Visible = true;
                        return;
                    }
                }
                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> deleted Rack Master Location '" + strRackLoc + "'");
                Session["SessTempRackName"] = Convert.ToString(lblTmpRackName.Text);
                ClientScriptManager CSM = Page.ClientScript;
                string strconfirm = "<script>if(!window.alert('Current Rack Location emptied.')){window.location.href='RackMstDet.aspx'}</script>";
                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                //Response.Write("<script>function RefreshParent(){window.opener.location.reload();window.close();}RefreshParent();</script>");
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region BtnBack
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            Session["SessTempRackName"] = Convert.ToString(lblTmpRackName.Text);
            //Response.Write("<script>function RefreshParent(){window.opener.location.reload();window.close();}RefreshParent();</script>");

            Response.Redirect("RackMstDet.aspx");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region ddAisType_OnSelectedIndexChanged
    protected void ddAisType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            getAisName();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region ddAisName_OnSelectedIndexChanged
    protected void ddAisName_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            getAisRevNo();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region ddRevNo_OnSelectedIndexChanged
    protected void ddRevNo_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //SearchDataHJList();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region linkButton_Click
    protected void link_Click(object sender, System.EventArgs e)
    {
        if (sender is LinkButton)
        {
            LinkButton linkButton = (LinkButton)sender;
            if (linkButton.CommandName == "SaveAisData")
            {
                String strRackMstDet = linkButton.CommandArgument;
                String strCurUser = Convert.ToString(Session["SessUserId"]);  //***ace_20160416_001

                #region Assign Pass-In Variable to Save Rack Master Detail
                //strRackMstDet
                //0     L1GW1 Rack 1^1^1
                //1     ~HRG0003
                //2     ~HI00085
                //3     ~1
                //4     ~2
                //5     ~Spoiler Rr Upper
                //6     ~76091-BZ200-J0
                //7     ~0
                //8     ~BLUE
                //9     ~{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil\fcharset0 Microsoft Sans Serif;}}\viewkind4\uc1\pard\qc\b\fs161 BLUE\b0\f1\fs17\par}
                String[] arrRackMstDet = strRackMstDet.Split('~');
                String strRackMstDetId = arrRackMstDet[0];
                String strHjId = arrRackMstDet[1];
                String strHjItemId = arrRackMstDet[2];
                String strHjRow = arrRackMstDet[3];
                String strHjCol = arrRackMstDet[4];
                String strPartsTitle = arrRackMstDet[5];
                String strPartsNo = arrRackMstDet[6];
                String strColorSfx = arrRackMstDet[7];
                String strPartsNumSymbolCode = arrRackMstDet[8];
                String strPartsNumSymbolRtf = arrRackMstDet[9];
                String strRackName = Convert.ToString(lblTmpRackName.Text);
                String strPlcNo = Convert.ToString(lblTmpPlcNo.Text);
                String strProcName = Convert.ToString(lblTmpProcName.Text);
                //String strCurRackLoc = csDatabase.GetAisRackLoc(strHjId, strHjItemId, strHjRow, strHjCol, strPartsNo, strColorSfx);
                String strCurRackLoc = csDatabase.GetPartsRackLoc(strPartsNo, strColorSfx, strRackName, strPlcNo);
                Boolean strExeFlag = false;
                string msg;

                //
                //added by vincent
                //
                List<String> sqlQuery = new List<String>();
                //
                //Boolean blUpdFlg = false;
                // - end

                #endregion

                if (strCurRackLoc != "")
                {
                    lblMsg.Text = "This Part had already been selected in rack location : [" + strCurRackLoc + "]. Please check.";
                    lblMsg.Visible = true;
                    return;
                }

                //this part is to change the L1GW2 Rack 2^1^3 to L1GW2 Rack 2  1-3
                String strRackLoc = "";
                String[] tmpRackMstDetIdVal = strRackMstDetId.Split('^');
                int tmpRackMstDetIdCnt = tmpRackMstDetIdVal.Length;
                if (tmpRackMstDetIdCnt > 1)
                {
                    strRackLoc = tmpRackMstDetIdVal[0] + "  " + tmpRackMstDetIdVal[1] + "-" + tmpRackMstDetIdVal[2];
                }

                #region ChangeLog ***ace_20160405_001
                // check the dt_RackMstDet rack_det_id column with data L1GW2 Rack 2^1^3 if exist then return true
                if (!csDatabase.ChkRackMstDetExist(strRackMstDetId))
                {
                    //to make sure that the part no is exist in the ais_PartsNum table.
                    if (csDatabase.ChkAisPartsNumExist(strPartsNo, strColorSfx)) // ***ace_20160407_001
                    {
                        try
                        {
                            // update part

                            GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> edit/save Rack Master Location '" + strRackLoc + "'");

                            tmpRackMstDetIdVal = strRackMstDetId.Split('^');
                            tmpRackMstDetIdCnt = tmpRackMstDetIdVal.Length;
                            if (tmpRackMstDetIdCnt > 1)
                            {
                                strRackLoc = tmpRackMstDetIdVal[0] + "  " + tmpRackMstDetIdVal[1] + "-" + tmpRackMstDetIdVal[2];
                                //blUpdFlg =  csDatabase.UpdPartsLoc      (strPartsNo, strColorSfx, strRackMstDetId, strRackLoc);
                                //
                                // add by vincent
                                //
                                strPartsNo = strPartsNo.Replace("'", "''");
                                strColorSfx = strColorSfx.Replace("'", "''");
                                strRackMstDetId = strRackMstDetId.Replace("'", "''");
                                strRackLoc = strRackLoc.Replace("'", "''");


                                //ENHANCEMENT NEEDED -DONE
                                //when insert will need to determine which rack_det_id and rack_loc to update
                                //then use the rack_det_id , split them with the delimiter ^ to get the rack name 
                                //then use the rack name to filter in dt_RackMst to know which group_name it is 
                                //then we know which rack_det_id and rack_loc to insert.
                                //group_name : Line 1 will insert ot rack_det_id and rack_loc
                                //group_name : Line 2 will insert to rack_det_id2 and rack_loc2
                                string GroupLine = csDatabase.GetGroupLineByRackName(strRackName, strPlcNo);
                                string rackLoc = GroupLine == "1" ? "rack_loc" : "rack_loc_2";
                                string rackDetID = GroupLine == "1" ? "rack_det_id" : "rack_det_id_2";
                                //sqlQuery = string.Format("SELECT {0} AS ReturnField FROM ais_PartsNum WHERE part_no = '{1}' AND color_sfx = '{2}'", returnField, strPartsNo, strColorSfx);
                                sqlQuery.Add(string.Format("UPDATE ais_PartsNum SET {0} = '{1}', {2} = '{3}' WHERE part_no = '{4}' AND color_sfx = '{5}'", rackDetID, strRackMstDetId, rackLoc, strRackLoc, strPartsNo, strColorSfx));
                                //sqlQuery.Add("UPDATE ais_PartsNum SET rack_det_id = '" + strRackMstDetId + "', rack_loc = '" + strRackLoc + "' WHERE part_no = '" + strPartsNo + "' AND color_sfx = '" + strColorSfx + "'");
                                //
                                // ----- end 

                                // add by vincent end

                                strRackMstDetId = strRackMstDetId.Replace("'", "''");
                                strHjId = strHjId.Replace("'", "''");
                                strHjItemId = strHjItemId.Replace("'", "''");
                                strHjRow = strHjRow.Replace("'", "''");
                                strHjCol = strHjCol.Replace("'", "''");
                                strPartsTitle = strPartsTitle.Replace("'", "''");
                                strPartsNo = strPartsNo.Replace("'", "''");
                                strColorSfx = strColorSfx.Replace("'", "''");
                                strPartsNumSymbolCode = strPartsNumSymbolCode.Replace("'", "''");
                                strPartsNumSymbolRtf = strPartsNumSymbolRtf.Replace("'", "''");

                                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to insert Rack Master Location '" + strRackLoc + "'");

                                //ENHANCEMENT NEEDED? dt_RackMstDet
                                sqlQuery.Add("INSERT INTO dt_RackMstDet (rack_det_id, hj_id, hj_item_id, hj_row, hj_col, parts_title, parts_no, color_sfx, symbol_code, symbol_rtf, rack_name, plc_no, proc_name) VALUES ('" + strRackMstDetId + "', '" + strHjId + "','" + strHjItemId + "','" + strHjRow + "','" + strHjCol + "','" + strPartsTitle + "','" + strPartsNo + "','" + strColorSfx + "','" + strPartsNumSymbolCode + "','" + strPartsNumSymbolRtf + "','" + strRackName + "','" + strPlcNo + "','" + strProcName + "')");

                                try
                                {

                                    strExeFlag = ConnQuery.ExecuteTransSaveQuery(sqlQuery);

                                }
                                catch (Exception ex)
                                {
                                    GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to save Rack Master Location, transaction roll back '" + strRackLoc + "'");

                                    GlobalFunc.Log(ex);
                                    GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
                                }
                            }

                            // end add
                        }
                        catch (Exception ex)
                        {
                            GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> Not able to update Rack Master Location '" + strRackLoc + "'");
                            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));

                            //lblMsg.Text = "Not able to update Rack Master Location.";
                            //lblMsg.Visible = true;
                        }


                    }
                    else
                    {

                        lblMsg.Text = "Harigami Parts No not exist in Parts No master data.";
                        lblMsg.Visible = true;
                        return;
                    }
                }
                // if the dt_RackMstDet rack_det_id column with data L1GW2 Rack 2^1^3 exist
                else
                {
                    if (csDatabase.ChkAisPartsNumExist(strPartsNo, strColorSfx)) // ***ace_20160407_001
                    {
                        try
                        {
                            tmpRackMstDetIdVal = strRackMstDetId.Split('^');
                            tmpRackMstDetIdCnt = tmpRackMstDetIdVal.Length;
                            if (tmpRackMstDetIdCnt > 1)
                            {


                                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> update Rack Master Location '" + strRackLoc + "'");

                                //
                                // remove rack id as null old record
                                //
                                //
                                //ENHANCEMENT NEEDED - DONE
                                //set null for what rack_det_id and rack_loc
                                string GroupLine = csDatabase.GetGroupLineByRackName(strRackName, strPlcNo);
                                string rackLoc = GroupLine == "1" ? "rack_loc" : "rack_loc_2";
                                string rackDetID = GroupLine == "1" ? "rack_det_id" : "rack_det_id_2";
                                //sqlQuery.Add("UPDATE ais_PartsNum SET rack_det_id = NULL, rack_loc = NULL WHERE rack_det_id = '" + strRackMstDetId + "'");
                                sqlQuery.Add(string.Format("UPDATE ais_PartsNum SET {0} = NULL, {1} = NULL WHERE {0} = '{2}'", rackDetID, rackLoc, strRackMstDetId));


                                //
                                // add back the rack location
                                //


                                strRackLoc = tmpRackMstDetIdVal[0] + "  " + tmpRackMstDetIdVal[1] + "-" + tmpRackMstDetIdVal[2];
                                //blUpdFlg =  csDatabase.UpdPartsLoc      (strPartsNo, strColorSfx, strRackMstDetId, strRackLoc);
                                //
                                // add by vincent
                                //
                                strPartsNo = strPartsNo.Replace("'", "''");
                                strColorSfx = strColorSfx.Replace("'", "''");
                                strRackMstDetId = strRackMstDetId.Replace("'", "''");
                                strRackLoc = strRackLoc.Replace("'", "''");

                                //ENHANCEMENT NEEDED-DONE
                                //determine which to set.
                                //sqlQuery.Add("UPDATE ais_PartsNum SET rack_det_id = '" + strRackMstDetId + "', rack_loc = '" + strRackLoc + "' WHERE part_no = '" + strPartsNo + "' AND color_sfx = '" + strColorSfx + "'");
                                sqlQuery.Add(string.Format("UPDATE ais_PartsNum SET {0} = '{1}', {2} = '{3}' WHERE part_no = '{4}' AND color_sfx = '{5}'", rackDetID, strRackMstDetId, rackLoc, strRackLoc, strPartsNo, strColorSfx));


                                strRackMstDetId = strRackMstDetId.Replace("'", "''");
                                strHjId = strHjId.Replace("'", "''");
                                strHjItemId = strHjItemId.Replace("'", "''");
                                strHjRow = strHjRow.Replace("'", "''");
                                strHjCol = strHjCol.Replace("'", "''");
                                strPartsTitle = strPartsTitle.Replace("'", "''");
                                strPartsNo = strPartsNo.Replace("'", "''");
                                strColorSfx = strColorSfx.Replace("'", "''");
                                strPartsNumSymbolCode = strPartsNumSymbolCode.Replace("'", "''");
                                strPartsNumSymbolRtf = strPartsNumSymbolRtf.Replace("'", "''");

                                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to update Rack Master Location '" + strRackLoc + "'");

                                //ENHANCEMENT NEEDED? dt_RackMstDet
                                sqlQuery.Add("UPDATE dt_RackMstDet SET hj_id = '" + strHjId + "', hj_item_id = '" + strHjItemId + "', hj_row = '" + strHjRow + "', hj_col = '" + strHjCol + "', parts_title = '" + strPartsTitle + "', parts_no = '" + strPartsNo + "', color_sfx = '" + strColorSfx + "', symbol_code = '" + strPartsNumSymbolCode + "', symbol_rtf = '" + strPartsNumSymbolRtf + "', rack_name = '" + strRackName + "', plc_no = '" + strPlcNo + "', proc_name= '" + strProcName + "', last_upd_by = '" + strCurUser + "', last_upd_dt = CURRENT_TIMESTAMP WHERE rack_det_id = '" + strRackMstDetId + "'");

                                try
                                {
                                    strExeFlag = ConnQuery.ExecuteTransSaveQuery(sqlQuery);
                                }
                                catch (Exception ex)
                                {
                                    GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to update Rack Master Location, transaction roll back '" + strRackLoc + "'");

                                    //GlobalFunc.Log(ex);

                                }
                            }

                            // end add


                        }
                        catch (Exception ex)
                        {
                            GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> Not able to update Rack Master Location '" + strRackLoc + "'");
                            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));

                        }

                    }
                    else
                    {
                        lblMsg.Text = "Harigami Parts No not exist in Parts No master data.";
                        lblMsg.Visible = true;
                        return;
                    }
                }
                #endregion

                Session["SessAisName"] = Convert.ToString(ddAisName.SelectedValue);
                Session["SessAisType"] = Convert.ToString(ddAisType.SelectedValue);
                Session["SessTempRackName"] = Convert.ToString(lblTmpRackName.Text);
                //Response.Write("<script>function RefreshParent(){window.opener.location.reload();window.close();}RefreshParent();</script>");
                if (strExeFlag)
                {
                    Response.Redirect("RackMstDet.aspx");
                }

            }
        }
    }
    #endregion

    #endregion
}
