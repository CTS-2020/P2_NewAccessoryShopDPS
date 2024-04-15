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
using dpant;

public partial class LampModuleAddMst : System.Web.UI.Page
{
    private int NewPageIndex
    {
        get { return (int)ViewState["NewPageIndex"]; }
        set { ViewState["NewPageIndex"] = value; }
    }

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        String tempModuleAdd = Convert.ToString(Session["SessTempModuleAdd"]);
        String tempModuleProcName = Convert.ToString(Session["SessTempModuleProcName"]);

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
                getModuleType();
                getPlcModel();                      //Added by YanTeng 17/09/2020

                if (tempModuleAdd != "" && tempModuleProcName != "")
                {
                    txtModuleAddTo.Visible = false;
                    lblTo.Visible = false;
                    ddProcName.Enabled = false;
                    ddPlcModel.Enabled = false;             //Added by YanTeng 18/09/2020
                    btnUpdate.Visible = true;
                    btnNewLmAdd.Visible = false;

                    DataSet dsLampModuleAddMst = new DataSet();
                    DataTable dtLampModuleAddMst = new DataTable();
                    dsLampModuleAddMst = csDatabase.SrcLampModuleAddMst("", Convert.ToString(tempModuleAdd), "", "", "", "", tempModuleProcName);       //Added parameter by YanTeng 15/09/2020 
                    dtLampModuleAddMst = dsLampModuleAddMst.Tables[0];
                    BindtoText(dtLampModuleAddMst);
                }
                else
                {
                    btnUpdate.Visible = false;
                    btnNewLmAdd.Visible = true;
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
    #endregion

    #region Method

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

    #region getModuleType
    private void getModuleType()
    {
        try
        {
            ddModuleType.DataSource = GlobalFunc.getModuleType("");
            ddModuleType.DataTextField = "Description";
            ddModuleType.DataValueField = "Value";
            ddModuleType.DataBind();
            ddModuleType.Items.Insert(0, " ");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region getPlcModel
    private void getPlcModel()
    {
        try
        {
            ddPlcModel.DataSource = GlobalFunc.getAllPlcModel();
            ddPlcModel.DataTextField = "Code";
            ddPlcModel.DataValueField = "Value";
            ddPlcModel.DataBind();
            ddPlcModel.Items.Insert(0, " ");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region getBlockforModuleName
    private void getBlockforModuleName()              //Added by YanTeng 06/12/2020
    {
        try
        {
            String strPlcNo = Convert.ToString(ddProcName.SelectedValue);
            String strProcName = Convert.ToString(ddProcName.SelectedItem);
            ddModuleName.DataSource = GlobalFunc.getBlockforModuleName(strPlcNo, strProcName);
            ddModuleName.DataTextField = "Description";
            ddModuleName.DataValueField = "Description";
            ddModuleName.DataBind();
            ddModuleName.Items.Insert(0, " ");
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
                if (Convert.ToString(dt.Rows[0]["module_add"]).Trim() != "")
                {
                    lblTmpModuleAdd.Text = Convert.ToString(dt.Rows[0]["module_add"]).Trim();
                    txtModuleAddFrm.Text = Convert.ToString(dt.Rows[0]["module_add"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["module_name"]).Trim() != "")
                {
                    ddModuleName.SelectedValue = Convert.ToString(dt.Rows[0]["module_name"]).Trim();
                    //txtModuleName.Text = Convert.ToString(dt.Rows[0]["module_name"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["plc_no"]).Trim() != "")
                {
                    ddProcName.SelectedValue = Convert.ToString(dt.Rows[0]["plc_no"]);
                    lblTmpModuleProcName.Text = Convert.ToString(ddProcName.SelectedItem).Trim();
                    getBlockforModuleName();                                                    //Added by YanTeng 06/12/2020
                }
                if (Convert.ToString(dt.Rows[0]["module_type"]).Trim() != "")
                {
                    ddModuleType.SelectedValue = Convert.ToString(dt.Rows[0]["module_type"]);
                }
                if (Convert.ToString(dt.Rows[0]["plc_model"]).Trim() != "")                     //Added by YanTeng 18/09/2020
                {
                    ddPlcModel.SelectedItem.Text = Convert.ToString(dt.Rows[0]["plc_model"]);
                    //ddPlcModel.SelectedValue = Convert.ToString(dt.Rows[0]["plc_model"]);
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
            String strModuleAddFrm = "";
            String strModuleAddTo = "";
            String strModuleName = "";
            String strModuleType = "";
            String strPlcNo = "";
            String strProcName = "";
            String strPlcModel = "";

            if (Convert.ToString(Request.QueryString["maf"]) != "") strModuleAddFrm = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["maf"]));
            if (Convert.ToString(Request.QueryString["mat"]) != "") strModuleAddTo = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["mat"]));
            if (Convert.ToString(Request.QueryString["mn"]) != "") strModuleName = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["mn"]));
            if (Convert.ToString(Request.QueryString["mt"]) != "") strModuleType = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["mt"]));
            if (Convert.ToString(Request.QueryString["pno"]) != "") strPlcNo = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["pno"]));
            if (Convert.ToString(Request.QueryString["pnm"]) != "") strProcName = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["pnm"]));
            if (Convert.ToString(Request.QueryString["plcm"]) != "") strPlcModel = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["plcm"]));      //Added by YanTeng 15/09/2020

            DataSet dsSearch = new DataSet();
            DataTable dtSearch = new DataTable();

            dsSearch = csDatabase.SrcLampModuleAddMst(strPlcModel, strModuleAddFrm, strModuleAddTo, strModuleName, strModuleType, strPlcNo, strProcName);        //Added parameter by YanTeng 15/09/2020
            dtSearch = dsSearch.Tables[0];

            DataView dvLampModuleAddMst = new DataView(dtSearch);

            gvLampModuleAddMst.DataSource = dvLampModuleAddMst;
            gvLampModuleAddMst.PageIndex = NewPageIndex;
            gvLampModuleAddMst.DataBind();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region ClearAll
    private void ClearAll()
    {
        try
        {
            if (ddProcName.Enabled.Equals(true))
            {
                getProcName();
                ddProcName.SelectedIndex = 0;
            }

            if (ddPlcModel.Enabled.Equals(true))
            {
                getPlcModel();
                ddPlcModel.SelectedIndex = 0;
            }

            getBlockforModuleName();            //Added by YanTeng 08/12/2020

            txtModuleAddFrm.Text = "";
            txtModuleAddTo.Text = "";
            ddModuleName.SelectedIndex = 0;
            ddModuleType.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Check Data Valid
    //private Boolean ChkDataValid()
    //{
    //    try
    //    {
    //        if (ddProcName.SelectedIndex == 0)
    //        {
    //            lblMsg.Text = "Please select Process Name.";
    //            lblMsg.Visible = true;
    //            return false;
    //        }
    //        else if (Convert.ToString(txtModuleAddFrm.Text) == "")
    //        {
    //            lblMsg.Text = "Please enter Module Address From.";
    //            lblMsg.Visible = true;
    //            return false;
    //        }
    //        //else if (Convert.ToString(txtModuleAddTo.Text) == "")
    //        //{
    //        //    lblMsg.Text = "Please enter Module Address To.";
    //        //    lblMsg.Visible = true;
    //        //    return false;
    //        //}
    //        else if (Convert.ToString(txtModuleName.Text) == "")
    //        {
    //            lblMsg.Text = "Please enter Module Name.";
    //            lblMsg.Visible = true;
    //            return false;
    //        }
    //        else if (txtModuleName.Text.Length > 20)
    //        {
    //            lblMsg.Text = "Module Name cannot be more than 20 characters.";
    //            lblMsg.Visible = true;
    //            return false;
    //        }
    //        else if (ddModuleType.SelectedIndex == 0)
    //        {
    //            lblMsg.Text = "Please select Module Type.";
    //            lblMsg.Visible = true;
    //            return false;
    //        }
    //        else if (!chkLampModuleAddValid(Convert.ToString(txtModuleAddFrm.Text)))
    //        {
    //            return false;
    //        }
    //        else if (!chkLampModuleAddValid(Convert.ToString(txtModuleAddTo.Text)))
    //        {
    //            return false;
    //        }
    //        else if (Convert.ToString(txtModuleAddTo.Text) != "" && Convert.ToInt32(txtModuleAddTo.Text) < Convert.ToInt32(txtModuleAddFrm.Text))
    //        {
    //            lblMsg.Text = "Module Address To must NOT LESS than Module Address From.";
    //            lblMsg.Visible = true;
    //            return false;
    //        }
    //        else
    //        {
    //            return true;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
    //        return false;
    //    }
    //}
    private Boolean ChkDataValid()
    {
        try
        {
            if (ddProcName.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Process Name.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Convert.ToString(txtModuleAddFrm.Text) == "")
            {
                lblMsg.Text = "Please enter Module Address From.";
                lblMsg.Visible = true;
                return false;
            }
            else if (!GlobalFunc.IsTextAValidInteger(Convert.ToString(txtModuleAddFrm.Text)))
            {
                lblMsg.Text = "Module Address From must be a valid integer.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Convert.ToInt32(txtModuleAddFrm.Text) < 0 || Convert.ToInt32(txtModuleAddFrm.Text) > 7999)
            {
                lblMsg.Text = "Module Address From must be between 1 - 7999.";
                lblMsg.Visible = true;
                return false;
            }
            else if (ddModuleName.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Module Name.";
                lblMsg.Visible = true;
                return false;
            }
            //else if (txtModuleName.Text.Length > 20)
            //{
            //    lblMsg.Text = "Module Name cannot be more than 20 characters.";
            //    lblMsg.Visible = true;
            //    return false;
            //}
            else if (ddModuleType.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Module Type.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Convert.ToString(txtModuleAddTo.Text) != "")
            {
                if (!GlobalFunc.IsTextAValidInteger(Convert.ToString(txtModuleAddTo.Text)))
                {
                    lblMsg.Text = "Module Address To must be a valid integer.";
                    lblMsg.Visible = true;
                    return false;
                }
                else if (Convert.ToInt32(txtModuleAddTo.Text) < 0 || Convert.ToInt32(txtModuleAddTo.Text) > 7999)
                {
                    lblMsg.Text = "Module Address To must be between 1 - 7999.";
                    lblMsg.Visible = true;
                    return false;
                }
                else if (Convert.ToInt32(txtModuleAddTo.Text) < Convert.ToInt32(txtModuleAddFrm.Text))
                {
                    lblMsg.Text = "Module Address To must be LESS than Module Address From.";
                    lblMsg.Visible = true;
                    return false;
                }
                return true;
            }
            else
            {
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

    //#region chkLampModuleAddValid
    //private Boolean chkLampModuleAddValid(String strLampModuleAdd)
    //{
    //    if (strLampModuleAdd.Trim() != "")
    //    {
    //        if (!GlobalFunc.IsTextAValidInteger(strLampModuleAdd))
    //        {
    //            lblMsg.Text = "Module Address must be a valid integer.";
    //            lblMsg.Visible = true;
    //            return false;
    //        }
    //        else if (ddPlcModel.SelectedValue == "PLC-M1990")
    //        {
    //            if (Convert.ToInt32(strLampModuleAdd) < 0 || Convert.ToInt32(strLampModuleAdd) > 7999)
    //            {
    //                lblMsg.Text = "For selected PLC GW Model (" + ddPlcModel.SelectedValue.ToString() + "), Module Address must be between 1 - 7999.";
    //                lblMsg.Visible = true;
    //                return false;
    //            }
    //            return true;
    //        }
    //        else if (ddPlcModel.SelectedValue == "PLC-M2020")
    //        {
    //            if (Convert.ToInt32(strLampModuleAdd) < 0 || Convert.ToInt32(strLampModuleAdd) > 64)
    //            {
    //                lblMsg.Text = "For selected PLC GW Model (" + ddPlcModel.SelectedValue.ToString() + "), Module Address must be between 1 - 64.";
    //                lblMsg.Visible = true;
    //                return false;
    //            }
    //            return true;
    //        }
    //        else
    //        {
    //            return true;
    //        }
    //    }
    //    else
    //    {
    //        return true;
    //    }
    //}
    //#endregion

    #endregion

    #region Events

    #region btnNewLmAdd
    protected void btnNewLmAdd_Click(object sender, EventArgs e)
    {
        try
        {
            Boolean boolValid = ChkDataValid();
            if (boolValid)
            {
                String strModuleAddFrm = Convert.ToString(txtModuleAddFrm.Text);
                String strModuleAddTo = Convert.ToString(txtModuleAddTo.Text);
                String strModuleName = Convert.ToString(ddModuleName.SelectedValue);
                String strPlcNo = Convert.ToString(ddProcName.SelectedValue);
                String strProcName = Convert.ToString(ddProcName.SelectedItem);
                String strModuleType = Convert.ToString(ddModuleType.SelectedValue);
                String tempModuleAdd = Convert.ToString(lblTmpModuleAdd.Text);
                String tempModuleProcName = Convert.ToString(lblTmpModuleProcName.Text);
                String strCurUser = Convert.ToString(Session["SessUserId"]);

                Boolean existFlag = csDatabase.ChkDuplicateLampModuleAddMst(strModuleAddFrm, strProcName, tempModuleProcName, tempModuleAdd, strModuleAddTo);
                if (!existFlag)
                {
                    string confirmValue = Request.Form["confirm_value"];
                    if (confirmValue == "Yes")
                    {
                        int iModuleAddFrm = Convert.ToInt32(strModuleAddFrm);
                        int iModuleAddTo = 0;

                        if (strModuleAddTo != "")
                        {
                            iModuleAddTo = Convert.ToInt32(strModuleAddTo);
                        }
                        else
                        {
                            iModuleAddTo = iModuleAddFrm;
                        }

                        for (int i = iModuleAddFrm; i <= iModuleAddTo; i++)
                        {
                            Boolean tmpFlag = csDatabase.SvLampModuleAddMst(strPlcNo, strProcName, Convert.ToString(i), strModuleName, strModuleType, strCurUser);
                            if (!tmpFlag)
                            {
                                GlobalFunc.ShowErrorMessage("Unable to save Lamp Module Address: " + i + " .");
                            }
                            else
                            {
                                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> created LM Address['" + i + "'] PLC No['" + strPlcNo + "'] Process Name['" + strProcName + "'] Module Name['" + strModuleName + "'] Module Type['" + strModuleType + "']");
                                ClientScriptManager CSM = Page.ClientScript;
                                string strconfirm = "<script>if(!window.alert('Module Address saved.')){window.location.href='LampModuleAddMst.aspx'}</script>";
                                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                            }
                        }
                    }
                }
                else
                {
                    GlobalFunc.ShowErrorMessage("Please check. Duplicate Module Address under same Process Name.");
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
            Boolean boolValid = ChkDataValid();
            if (boolValid)
            {
                String strModuleAdd = Convert.ToString(txtModuleAddFrm.Text);
                String strModuleName = Convert.ToString(ddModuleName.SelectedValue);
                String strPlcNo = Convert.ToString(ddProcName.SelectedValue);
                String strProcName = Convert.ToString(ddProcName.SelectedItem);
                String strModuleType = Convert.ToString(ddModuleType.SelectedValue);
                String tempModuleAdd = Convert.ToString(lblTmpModuleAdd.Text);
                String tempModuleProcName = Convert.ToString(lblTmpModuleProcName.Text);
                String strCurUser = Convert.ToString(Session["SessUserId"]);
                String strPlcModel = Convert.ToString(ddPlcModel.SelectedValue);                //Added by YanTeng 18/09/2020

                Boolean existFlag = csDatabase.ChkDuplicateLampModuleAddMst(strModuleAdd, strProcName, tempModuleProcName, tempModuleAdd, "");
                if (!existFlag)
                {
                    string confirmValue = Request.Form["confirm_value"];
                    if (confirmValue == "Yes")
                    {
                        Boolean tmpFlag = csDatabase.UpdLampModuleAddMst(strModuleAdd, strModuleName, strPlcNo, strProcName, strModuleType, tempModuleProcName, tempModuleAdd, strCurUser);
                        if (!tmpFlag)
                        {
                            GlobalFunc.ShowErrorMessage("Unable to update Lamp Module Address: " + strModuleAdd + " .");
                        }
                        else
                        {
                            //csDatabase.UpdLampModuleAddRem(strModuleAdd, strModuleName, tempModuleAdd);
                            GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> edited LM Address['" + strModuleAdd + "'] PLC No['" + strPlcNo + "'] Process Name['" + strProcName + "'] Module Name['" + strModuleName + "'] Module Type['" + strModuleType + "']");
                            ClientScriptManager CSM = Page.ClientScript;
                            string strconfirm = "<script>if(!window.alert('Module Address: " + tempModuleAdd + " updated.')){window.location.href='LampModuleAddMst.aspx'}</script>";
                            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                        }
                    }
                }
                else
                {
                    GlobalFunc.ShowErrorMessage("Please check. Duplicate Module Address under same Process Name.");
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
            Response.Redirect("LampModuleAddMst.aspx");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Paging Index
    protected void gvLampModuleAddMst_PageIndexChanging(object sender, GridViewPageEventArgs e)
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

    #region ddProcName_OnSelectedIndexChanged          
    protected void ddProcName_OnSelectedIndexChanged(object sender, EventArgs e)            //Added by YanTeng 17/09/2020
    {
        try
        {
            if (!string.IsNullOrEmpty(ddProcName.SelectedItem.Text.Trim()))
            {
                String strProcName = Convert.ToString(ddProcName.SelectedItem);
                ddPlcModel.DataSource = GlobalFunc.getPlcModelbyProcName(strProcName);
                ddPlcModel.DataTextField = "Description";
                ddPlcModel.DataValueField = "Value";
                ddPlcModel.DataBind();

                getBlockforModuleName();
            }
            else
            {
                getPlcModel();
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region ddPlcModel_OnSelectedIndexChanged          
    protected void ddPlcModel_OnSelectedIndexChanged(object sender, EventArgs e)            //Added by YanTeng 17/09/2020
    {
        try
        {
            if (!string.IsNullOrEmpty(ddPlcModel.SelectedItem.Text.Trim()))
            {
                String strPlcModel = Convert.ToString(ddPlcModel.SelectedItem);
                ddProcName.DataSource = GlobalFunc.getProcNamebyPlcModel(strPlcModel);
                ddProcName.DataTextField = "Description";
                ddProcName.DataValueField = "Code";
                ddProcName.DataBind();
                ddProcName.Items.Insert(0, " ");
            }
            else
            {
                getProcName();
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion
    #endregion
}
