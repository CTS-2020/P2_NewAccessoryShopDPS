using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DpsMaint_GearUpLMReg : System.Web.UI.Page
{
    private int NewPageIndex
    {
        get { return (int)ViewState["NewPageIndex"]; }
        set { ViewState["NewPageIndex"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        String tempPartId = Convert.ToString(Session["SessTempPartId"]);
        String tempPlcNo = Convert.ToString(Session["SessTempPlcNo"]);
        String tempProcName = Convert.ToString(Session["SessTempProcName"]);
        String tempLine = Convert.ToString(Session["SessTempLine"]);
        String tempLineGwNo = Convert.ToString(Session["SessTempLineGwNo"]);
        String tempGearUpID = Convert.ToString(Session["SessTempGearID"]);

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
                getProcName();
                getLineType();

                if (tempPartId != "")
                {
                    ddProcName.Enabled = false;
                    btnUpdate.Visible = true;
                    btnNewGroup.Visible = false;
                    if (btnUpdate.Visible) txt_GearUpID.Enabled = false;
                    if (btnNewGroup.Visible) txt_GearUpID.Enabled = true;


                    DataSet dsGearUp = new DataSet();
                    DataTable dtGearUp = new DataTable();
                    //SrcGearUpLM(strPlcNo, strPartID, strProcName, strLine, strGwNo, strModAddr, strPhysAddr, strGearID);
                    dsGearUp = csDatabase.SrcGearUpLM(Convert.ToString(tempPlcNo)
                        , Convert.ToString(GlobalFunc.ReplaceToEmptyString(tempPartId))
                        , Convert.ToString(GlobalFunc.ReplaceToEmptyString(tempProcName))
                        , Convert.ToString(GlobalFunc.ReplaceToEmptyString(tempLine))
                        , Convert.ToString(GlobalFunc.ReplaceToEmptyString(tempLineGwNo))
                        , ""
                        , ""
                        , Convert.ToString(GlobalFunc.ReplaceToEmptyString(tempGearUpID)));
                    dtGearUp = dsGearUp.Tables[0];
                    BindtoText(dtGearUp);
                }
                else
                {
                    btnUpdate.Visible = false;
                    btnNewGroup.Visible = true;
                    if (btnUpdate.Visible) txt_GearUpID.Enabled = false;
                    if (btnNewGroup.Visible) txt_GearUpID.Enabled = true;
                }

                NewPageIndex = 0;
                showRefGrid();
            }
            catch (Exception ex)
            {
                GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            }
        }
    }

    #region Method
    #region getLineType
    private void getLineType()
    {
        try
        {
            ddLineType.DataSource = GlobalFunc.getLineType();
            ddLineType.DataTextField = "Description";
            ddLineType.DataValueField = "Description";
            ddLineType.DataBind();
            ddLineType.Items.Insert(0, " ");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region getProcName
    private void getProcName()
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

    #region BindtoText
    public void BindtoText(DataTable dt)
    {
        try
        {
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToString(dt.Rows[0]["PartsID"]).Trim() != "")
                {
                    txtPartId.Text = Convert.ToString(dt.Rows[0]["PartsID"]).Trim();
                    lblTmpPartId.Text = Convert.ToString(dt.Rows[0]["PartsID"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["Gear_id"]).Trim() != "")
                {
                    txt_GearUpID.Text = Convert.ToString(dt.Rows[0]["Gear_id"]).Trim();
                    lblTmpGearID.Text = Convert.ToString(dt.Rows[0]["Gear_id"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["plc_no"]).Trim() != "")
                {
                    ddProcName.SelectedValue = Convert.ToString(dt.Rows[0]["plc_no"]);
                }
                if (Convert.ToString(dt.Rows[0]["Line"]).Trim() != "")
                {
                    ddLineType.SelectedValue = Convert.ToString(dt.Rows[0]["Line"]);
                    //txt_Line.Text = Convert.ToString(dt.Rows[0]["Line"]).Trim();
                    lblTmpLine.Text = Convert.ToString(dt.Rows[0]["Line"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["LmModuleAddress"]).Trim() != "")
                {
                    txt_ModuleAddr.Text = Convert.ToString(dt.Rows[0]["LmModuleAddress"]).Trim();
                    //lblGroupLine.Text = Convert.ToString(dt.Rows[0]["group_line"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["LmPhysicalAddress"]).Trim() != "")
                {
                    txt_PhysAddr.Text = Convert.ToString(dt.Rows[0]["LmPhysicalAddress"]).Trim();
                    //lblGroupLine.Text = Convert.ToString(dt.Rows[0]["group_line"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["GwNo"]).Trim() != "")
                {
                    txt_GwNo.Text = Convert.ToString(dt.Rows[0]["GwNo"]).Trim();
                    lblTmpGwNo.Text = Convert.ToString(dt.Rows[0]["GwNo"]).Trim();
                }
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region showRefGrid
    private void showRefGrid()
    {
        try
        {
            String strGearID = "";
            String strPartID = "";
            String strPlcNo = "";
            String strProcName = "";
            String strLine = "";
            String strGwNo = "";
            String strModAddr = "";
            String strPhysAddr = "";

            if (Convert.ToString(Request.QueryString["pid"]) != "") strPartID = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["pid"]));
            if (Convert.ToString(Request.QueryString["pno"]) != "") strPlcNo = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["pno"]));
            if (Convert.ToString(Request.QueryString["pnm"]) != "") strProcName = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["pnm"]));
            if (Convert.ToString(Request.QueryString["gid"]) != "") strGearID = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["gid"]));
            if (Convert.ToString(Request.QueryString["line"]) != "") strLine = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["line"]));
            if (Convert.ToString(Request.QueryString["gwn"]) != "") strGwNo = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["gwn"]));
            if (Convert.ToString(Request.QueryString["mad"]) != "") strModAddr = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["mad"]));
            if (Convert.ToString(Request.QueryString["pad"]) != "") strPhysAddr = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["pad"]));

            DataSet dsSearch = new DataSet();
            DataTable dtSearch = new DataTable();

            //SrcGearUpLM(strPlcNo, strPartID, strProcName, strLine, strGwNo, strModAddr, strPhysAddr, strGearID);

            dsSearch = csDatabase.SrcGearUpLM(strPlcNo, strPartID, strProcName, strLine, strGwNo, strModAddr, strPhysAddr, strGearID);

            if (dsSearch != null && dsSearch.Tables.Count > 0 && dsSearch.Tables[0].Rows.Count > 0)
            {
                dtSearch = dsSearch.Tables[0];
            }

            DataView dvGearUp = new DataView(dtSearch);

            gvGearUp.DataSource = dvGearUp;
            gvGearUp.PageIndex = NewPageIndex;
            gvGearUp.DataBind();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Check Data Valid
    private Boolean ChkDataValid(String Type)
    {
        try
        {
            if (ddProcName.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Process Name.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Convert.ToString(txtPartId.Text) == "")
            {
                lblMsg.Text = "Please enter Part ID.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Convert.ToString(txt_GearUpID.Text) == "")
            {
                lblMsg.Text = "Please enter Gear Up ID.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Convert.ToString(txt_GearUpID.Text) != "" && !GlobalFunc.IsTextAValidInteger(Convert.ToString(txt_GearUpID.Text)))
            {
                lblMsg.Text = "Gear Up ID must be Integer. Please check.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Type == BUTTON_ACTION.NEW && (Convert.ToString(txt_GearUpID.Text) != "") && (csDatabase.ChkDuplicateGearUpID(txt_GearUpID.Text)))
            {
                lblMsg.Text = "Duplicate Gear Up ID is not allowed. Please check.";
                lblMsg.Visible = true;
                return false;
            }
            //else if (Convert.ToString(txt_Line.Text) == "")
            //{
            //    lblMsg.Text = "Please enter Line.";
            //    lblMsg.Visible = true;
            //    return false;
            //}
            //else if ((Convert.ToString(txt_Line.Text) != "") && ((Convert.ToString(txt_Line.Text) != "A") && (Convert.ToString(txt_Line.Text) != "B")))
            //{
            //    lblMsg.Text = "Please enter Line (A or B).";
            //    lblMsg.Visible = true;
            //    return false;
            //}
            else if (Convert.ToString(ddLineType.SelectedValue) == "")
            {
                lblMsg.Text = "Please enter Line.";
                lblMsg.Visible = true;
                return false;
            }
            else if ((Convert.ToString(ddLineType.SelectedValue) != "") && ((Convert.ToString(ddLineType.SelectedValue) != "A") && (Convert.ToString(ddLineType.SelectedValue) != "B")))
            {
                lblMsg.Text = "Please enter Line.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Convert.ToString(txt_GwNo.Text) == "")
            {
                lblMsg.Text = "Please enter G/W No.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Convert.ToString(txt_ModuleAddr.Text) == "")
            {
                lblMsg.Text = "Please enter Lamp Module Address.";
                lblMsg.Visible = true;
                return false;
            }

            else if (Convert.ToString(txt_PhysAddr.Text) == "")
            {
                lblMsg.Text = "Please enter Lamp Physicall Address.";
                lblMsg.Visible = true;
                return false;
            }
            #region check numeric string
            else if ((Convert.ToString(txt_GwNo.Text) != "") && !IsNumeric(txt_GwNo.Text))
            {
                lblMsg.Text = "Please enter valid G/W No within range (1~12).";
                lblMsg.Visible = true;
                return false;
            }

            else if ((Convert.ToString(txt_ModuleAddr.Text) != "") && !IsNumeric(txt_ModuleAddr.Text))
            {
                lblMsg.Text = "Please enter valid Lamp Module Address (1~9999).";
                lblMsg.Visible = true;
                return false;
            }

            else if ((Convert.ToString(txt_PhysAddr.Text) != "") && !IsNumeric(txt_PhysAddr.Text))
            {
                lblMsg.Text = "Please enter valid Lamp Physical Address (1~64).";
                lblMsg.Visible = true;
                return false;
            }
            #endregion

            #region check range
            else if ((Convert.ToString(txt_ModuleAddr.Text) != "") && (Convert.ToInt32(txt_ModuleAddr.Text.Trim()) < 1))
            {
                lblMsg.Text = "Please enter Lamp Module Address (1~9999).";
                lblMsg.Visible = true;
                return false;
            }

            else if ((Convert.ToString(txt_GwNo.Text) != "") && ((Convert.ToInt32(txt_GwNo.Text.Trim()) < 1) || (Convert.ToInt32(txt_GwNo.Text.Trim()) > 12)))
            {
                lblMsg.Text = "Please enter valid Gate Way within range (1~12).";
                lblMsg.Visible = true;
                return false;
            }
            else if ((Convert.ToString(txt_PhysAddr.Text) != "") && ((Convert.ToInt32(txt_PhysAddr.Text.Trim()) < 1) || (Convert.ToInt32(txt_PhysAddr.Text.Trim()) > 64)))
            {
                lblMsg.Text = "Please enter Lamp Physical Address within range (1~64).";
                lblMsg.Visible = true;
                return false;
            }
            #endregion

            #region check duplication
            //else if ((lblTmpGwNo.Text != txt_GwNo.Text) && (csDatabase.ChkAllDuplicateGWNoGearUp(Convert.ToString(ddProcName.SelectedValue), Convert.ToString(txt_GwNo.Text), Convert.ToString(txt_Line.Text), Convert.ToString(txt_PhysAddr.Text))))
            //{
            //    lblMsg.Text = "Duplicate G/W No and Lamp Physical Address are not allowed in same Process. Please check.";
            //    lblMsg.Visible = true;
            //    return false;
            //}
            else if ((lblTmpGwNo.Text != txt_GwNo.Text) && (csDatabase.ChkAllDuplicateGWNoGearUp(Convert.ToString(ddProcName.SelectedValue), Convert.ToString(txt_GwNo.Text), Convert.ToString(ddLineType.SelectedValue), Convert.ToString(txt_PhysAddr.Text))))
            {
                lblMsg.Text = "Duplicate G/W No and Lamp Physical Address are not allowed in same Process. Please check.";
                lblMsg.Visible = true;
                return false;
            }
            #endregion

            else
            {
                lblMsg.Text = "";
                lblMsg.Visible = false;
                return true;
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }
    #endregion

    #region check numeric value
    public static bool IsNumeric(string input)
    {
        int result;
        return int.TryParse(input, out result);
    }
    #endregion

    #region ClearAll
    private void ClearAll()
    {
        try
        {
            txt_GwNo.Text = "";
            txt_ModuleAddr.Text = "";
            txt_PhysAddr.Text = "";
            //txt_Line.Text = "";
            ddLineType.SelectedIndex = 0;
            txt_GearUpID.Text = "";
            txtPartId.Text = "";
            ddProcName.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #endregion

    #region Events

    #region btnNewGroup
    protected void btnNewGroup_Click(object sender, EventArgs e)
    {
        try
        {
            Boolean boolValid = ChkDataValid(BUTTON_ACTION.NEW);
            if (boolValid)
            {
                String strPartID = Convert.ToString(txtPartId.Text);
                String strPlcNo = Convert.ToString(ddProcName.SelectedValue);
                String strProcName = Convert.ToString(ddProcName.SelectedItem);
                String strGearID = Convert.ToString(txt_GearUpID.Text);
                //String strLine = Convert.ToString(txt_Line.Text);
                String strLine = Convert.ToString(ddLineType.SelectedValue);
                String strGwNo = Convert.ToString(txt_GwNo.Text);
                String strModAddr = Convert.ToString(txt_ModuleAddr.Text);
                String strPhysAddr = Convert.ToString(txt_PhysAddr.Text);
                String strCurUser = Convert.ToString(Session["SessUserId"]);

                //if (csDatabase.ChkGroupMaxCnt(strProcName))
                //{
                //    Response.Write("<script language='javascript'>alert('Maximum Limit Group Count of 6 per Process reached. Please delete/edit unused group.')</script>");
                //    return;
                //}

                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    //SvGearUpLM(String strPlcNo, String strPartID, String strLine, String strGwNo, String strModAddr, String strPhysAddr, String strGearID, String strCurUser)
                    Boolean tmpFlag = csDatabase.SvGearUpLM(strPlcNo, strPartID, strLine, strGwNo, strModAddr, strPhysAddr, strGearID, strCurUser);
                    if (!tmpFlag)
                    {
                        GlobalFunc.ShowErrorMessage("Unable to save Gear Up ID: " + strGearID + " .");
                    }
                    else
                    {
                        GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> created Gear Up ID['"
                            + strGearID + "'] Part No['"
                            + strPartID + "'] PLC No['"
                            + strPlcNo + "'] Line['"
                            + strLine + "'] G/W No['"
                            + strGwNo + "'] Lamp Module Address No['"
                            + strModAddr + "'] Lamp Physical Address['"
                            + strPhysAddr + "']");
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = "<script>if(!window.alert('Gear Up ID: " + strGearID + " saved.')){window.location.href='GearUpLM.aspx'}</script>";
                        CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region BtnClear
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

    #region btnUpdate_Click
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            Boolean boolValid = ChkDataValid(BUTTON_ACTION.UPDATE);
            if (boolValid)
            {
                String strPartID = Convert.ToString(txtPartId.Text);
                String strPlcNo = Convert.ToString(ddProcName.SelectedValue);
                String strProcName = Convert.ToString(ddProcName.SelectedItem);
                String strGearID = Convert.ToString(txt_GearUpID.Text);
                //String strLine = Convert.ToString(txt_Line.Text);
                String strLine = Convert.ToString(ddLineType.SelectedValue);
                String strGwNo = Convert.ToString(txt_GwNo.Text);
                String strModAddr = Convert.ToString(txt_ModuleAddr.Text);
                String strPhysAddr = Convert.ToString(txt_PhysAddr.Text);
                String strCurUser = Convert.ToString(Session["SessUserId"]);
                String tempPartID = Convert.ToString(lblTmpPartId.Text);
                String tempGearID = Convert.ToString(lblTmpGearID.Text);
                String tempGwNo = Convert.ToString(lblTmpGwNo.Text);
                String tempLine = Convert.ToString(lblTmpLine.Text);
                //String tempGroupName = Convert.ToString(lblTmpGroupName.Text);

                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    //UpdGearUpLM(String strPlcNo, String strPartID, String strLine, String strGwNo, String strModAddr, String strPhysAddr, String strGearID, String strCurUser, String tempPartID, String tempGearID, String tempLine, String tempGwNo)
                    Boolean tmpFlag = csDatabase.UpdGearUpLM(strPlcNo, strPartID, strLine, strGwNo, strModAddr, strPhysAddr, strGearID, strCurUser, tempPartID, tempGearID, tempLine, tempGwNo);
                    if (!tmpFlag)
                    {
                        GlobalFunc.ShowErrorMessage("Unable to update Gear Up ID: " + lblTmpGearID + " .");
                    }
                    else
                    {
                        GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> updated Gear Up ID['"
                            + tempGearID + "'] to Gear Up ID['"
                            + strGearID + "'] Part No['"
                            + strPartID + "'] PLC No['"
                            + strPlcNo + "'] Line['"
                            + strLine + "'] G/W No['"
                            + strGwNo + "'] Lamp Module Address No['"
                            + strModAddr + "'] Lamp Physical Address['"
                            + strPhysAddr + "']");
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = "<script>if(!window.alert('Gear Up ID: " + tempGearID + " updated.')){window.location.href='GearUpLM.aspx'}</script>";
                        CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region BtnBack
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("GearUpLM.aspx");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Paging Index
    protected void gvGearUp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            NewPageIndex = e.NewPageIndex;
            showRefGrid();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region ddLineType_OnSelectedIndexChanged
    protected void ddLineType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #endregion
}

public static class BUTTON_ACTION
{
    public static readonly string UPDATE = "U";
    public static readonly string NEW = "N";
}