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

public partial class PhysicalAddMst : System.Web.UI.Page
{
    private int NewPageIndex
    {
        get { return (int)ViewState["NewPageIndex"]; }
        set { ViewState["NewPageIndex"] = value; }
    }

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        String tempPhysicalUid = Convert.ToString(Session["SessTempPhysicalUid"]);
        String tempPlcNo = Convert.ToString(Session["SessTempPlcNo"]);
        String tempProcName = Convert.ToString(Session["SessTempProcName"]);
        String tempModuleAdd = Convert.ToString(Session["SessTempModuleAdd"]);
        String tempPhysicalAdd = Convert.ToString(Session["SessTempPhysicalAdd"]);

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
                getPlcModel();

                if (tempPlcNo != "" && tempProcName != "" && tempModuleAdd != "" && tempPhysicalAdd != "")
                {
                    txtModuleAddTo.Visible = false;
                    lblTo.Visible = false;
                    ddProcName.Enabled = false;
                    ddPlcModel.Enabled = false; 
                    btnUpdate.Visible = true;
                    btnNewPhy.Visible = false;

                    DataSet dsPhysicalAddMst = new DataSet();
                    DataTable dtPhysicalAddMst = new DataTable();
                    dsPhysicalAddMst = csDatabase.SrcPhysicalAddMst(tempPlcNo, tempProcName, Convert.ToString(tempModuleAdd), "", Convert.ToString(tempPhysicalAdd), "");
                    dtPhysicalAddMst = dsPhysicalAddMst.Tables[0];
                    BindtoText(dtPhysicalAddMst);
                }
                else
                {
                    btnUpdate.Visible = false;
                    btnNewPhy.Visible = true;
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

    #region BindtoText
    public void BindtoText(DataTable dt)
    {
        try
        {
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToString(dt.Rows[0]["uid"]).Trim() != "")
                {
                    lblTmpPhysicalUid.Text = Convert.ToString(dt.Rows[0]["uid"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["plc_no"]).Trim() != "")
                {
                    ddProcName.SelectedValue = Convert.ToString(dt.Rows[0]["plc_no"]);
                    lblTmpPlcNo.Text = Convert.ToString(dt.Rows[0]["plc_no"]);
                }
                if (Convert.ToString(dt.Rows[0]["proc_name"]).Trim() != "")
                {
                    //ddProcName.SelectedItem.Text = Convert.ToString(dt.Rows[0]["proc_name"]).Trim();
                    lblTmpProcName.Text = Convert.ToString(dt.Rows[0]["proc_name"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["plc_model"]).Trim() != "") 
                {
                    ddPlcModel.SelectedItem.Text = Convert.ToString(dt.Rows[0]["plc_model"]);
                }
                if (Convert.ToString(dt.Rows[0]["module_add"]).Trim() != "")
                {
                    txtModuleAddFrm.Text = Convert.ToString(dt.Rows[0]["module_add"]);
                    lblTmpModuleAdd.Text = Convert.ToString(dt.Rows[0]["module_add"]);
                }
                if (Convert.ToString(dt.Rows[0]["physical_add"]).Trim() != "")
                {
                    txtPhysicalAdd.Text = Convert.ToString(dt.Rows[0]["physical_add"]);
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
            String strPlcNo = "";
            String strProcName = "";
            String strModuleAddFrm = "";
            String strModuleAddTo = "";
            String strPhyAddFrm = "";
            //String strPhyAddTo = "";

            if (Convert.ToString(Request.QueryString["pno"]) != "") strPlcNo = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["pno"]));
            if (Convert.ToString(Request.QueryString["pnm"]) != "") strProcName = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["pnm"]));
            if (Convert.ToString(Request.QueryString["maf"]) != "") strModuleAddFrm = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["maf"]));
            if (Convert.ToString(Request.QueryString["mat"]) != "") strModuleAddTo = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["mat"]));
            if (Convert.ToString(Request.QueryString["phyadd"]) != "") strPhyAddFrm = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["phyadd"]));
            //if (Convert.ToString(Request.QueryString["pat"]) != "") strPhyAddTo = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["pat"]));

            DataSet dsSearch = new DataSet();
            DataTable dtSearch = new DataTable();

            dsSearch = csDatabase.SrcPhysicalAddMst(strPlcNo, strProcName, strModuleAddFrm, strModuleAddTo, strPhyAddFrm, "");
            dtSearch = dsSearch.Tables[0];

            DataView dvPhysicalAddMst = new DataView(dtSearch);

            gvPhysicalAddMst.DataSource = dvPhysicalAddMst;
            gvPhysicalAddMst.PageIndex = NewPageIndex;
            gvPhysicalAddMst.DataBind();
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
            txtModuleAddFrm.Text = "";
            txtModuleAddTo.Text = "";
            txtPhysicalAdd.Text = "";
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Check Data Valid
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
            else if (Convert.ToString(txtPhysicalAdd.Text) == "")
            {
                lblMsg.Text = "Please enter Physical Address.";
                lblMsg.Visible = true;
                return false;
            }
            else if (!GlobalFunc.IsTextAValidInteger(txtPhysicalAdd.Text))
            {
                lblMsg.Text = "Physical Address must be a valid integer.";
                lblMsg.Visible = true;
                return false;
            }
            else if (!chkPhysicalAddValid(Convert.ToString(txtPhysicalAdd.Text)))
            {
                return false;
            }
            //else if (csDatabase.ChkDuplicatePhysicalAddMst(Convert.ToString(Session["SessTempPhysicalUid"]), ddProcName.SelectedValue, txtModuleAddFrm.Text))
            //{
            //    lblMsg.Text = "Duplicate of the combination Process Name and Module Address. Please check.";
            //    lblMsg.Visible = true;
            //    return false;
            //}
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

    #region chkPhysicalAddValid
    private Boolean chkPhysicalAddValid(String strPhysicalAdd)
    {
        DataSet dsPlcModelMst = new DataSet();
        DataTable dtPlcModelMst = new DataTable();
        dsPlcModelMst = csDatabase.GetPhysicalAddbyPlcModel(ddPlcModel.SelectedItem.Text);
        dtPlcModelMst = dsPlcModelMst.Tables[0];

        int minPhysicalAdd = Convert.ToInt32(dtPlcModelMst.Rows[0]["phyaddr_from"]);
        int maxPhysicalAdd = Convert.ToInt32(dtPlcModelMst.Rows[0]["phyaddr_to"]);
        //int digitPhysicalAdd = Convert.ToInt32(dtPlcModelMst.Rows[0]["digit_no"]);

        if (Convert.ToInt32(strPhysicalAdd) < minPhysicalAdd || Convert.ToInt32(strPhysicalAdd) > maxPhysicalAdd)
        {
            lblMsg.Text = "Physical Address must be between " + Convert.ToString(minPhysicalAdd) + " - " + Convert.ToString(maxPhysicalAdd) + " for GW Model (" + ddPlcModel.SelectedItem.Text + ").";
            lblMsg.Visible = true;
            return false;
        }
        else if (Convert.ToString(txtModuleAddTo.Text).Trim() != "")
        {
            if ((Convert.ToInt32(txtModuleAddTo.Text) - Convert.ToInt32(txtModuleAddFrm.Text)) > (maxPhysicalAdd - Convert.ToInt32(strPhysicalAdd)))
            {
                lblMsg.Text = "The range between Module Address From and Module Address To is more than the range of physical address.";
                lblMsg.Visible = true;
                return false;
            }
            return true;
        }
        //else if(Convert.ToString(strPhysicalAdd).Length != digitPhysicalAdd)
        //{
        //    lblMsg.Text = "The length of numeric for Physical Address must be " + Convert.ToString(digitPhysicalAdd) + " digit for GW Model (" + ddPlcModel.SelectedItem.Text + ").";
        //    lblMsg.Visible = true;
        //    return false;
        //}
        else
        {
            return true;
        }
    }
    #endregion

    #endregion

    #region Events

    #region btnNewPhy
    protected void btnNewPhy_Click(object sender, EventArgs e)
    {
        try
        {
            Boolean boolValid = ChkDataValid();
            if (boolValid)
            {
                String strPlcNo = Convert.ToString(ddProcName.SelectedValue);
                String strProcName = Convert.ToString(ddProcName.SelectedItem);
                String tempPhysicalUid = Convert.ToString(lblTmpPhysicalUid.Text);
                String tempModuleAdd = Convert.ToString(lblTmpModuleAdd.Text);
                String tempModuleProcName = Convert.ToString(lblTmpProcName.Text);
                String strModuleAddFrm = Convert.ToString(txtModuleAddFrm.Text).Trim();
                String strModuleAddTo = Convert.ToString(txtModuleAddTo.Text).Trim();
                String strPhysicalAdd = Convert.ToString(txtPhysicalAdd.Text);
                String strCurUser = Convert.ToString(Session["SessUserId"]);

                Boolean existFlag = csDatabase.ChkDuplicatePhysicalAddMst(tempPhysicalUid, strProcName, strModuleAddFrm, strModuleAddTo);
                if (!existFlag)
                {
                    string confirmValue = Request.Form["confirm_value"];
                    if (confirmValue == "Yes")
                    {
                        int iModuleAddFrm = Convert.ToInt32(strModuleAddFrm);
                        int iModuleAddTo = 0;
                        int iPhysicalAdd = Convert.ToInt32(strPhysicalAdd);

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
                            //Boolean tmpFlag = csDatabase.SvPhysicalAddMst(strProcName, Convert.ToString(i), strPhysicalAdd, strCurUser);
                            Boolean tmpFlag = csDatabase.SvPhysicalAddMst(strPlcNo, strProcName, Convert.ToString(i), Convert.ToString(iPhysicalAdd), strCurUser);
                            if (!tmpFlag)
                            {
                                GlobalFunc.ShowErrorMessage("Unable to save Physical Address:" + iPhysicalAdd + " for Module Address: i.");
                            }
                            else
                            {
                                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> created Physical Address['" + iPhysicalAdd + "'] for Module Address ['" + i + "'] PLC No['" + strPlcNo + "'] Process Name['" + strProcName + "']");
                                ClientScriptManager CSM = Page.ClientScript;
                                string strconfirm = "<script>if(!window.alert('Physical Address saved.')){window.location.href='PhysicalAddMst.aspx'}</script>";
                                CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                            }
                            iPhysicalAdd += 1;
                        }
                    }
                }
                else
                {
                    GlobalFunc.ShowErrorMessage("Duplicate of the combination Process Name and Module Address in PhysicalAddMst. Please check.");
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
                String strPlcNo = Convert.ToString(ddProcName.SelectedValue);
                String strProcName = Convert.ToString(ddProcName.SelectedItem);
                String tempPhysicalUid = Convert.ToString(lblTmpPhysicalUid.Text);
                String tempModuleAdd = Convert.ToString(lblTmpModuleAdd.Text);
                String tempModuleProcName = Convert.ToString(lblTmpProcName.Text);
                String strModuleAddFrm = Convert.ToString(txtModuleAddFrm.Text);
                //String strModuleAddTo = Convert.ToString(txtModuleAddTo.Text);
                String strPhysicalAdd = Convert.ToString(txtPhysicalAdd.Text);
                String strCurUser = Convert.ToString(Session["SessUserId"]);

                Boolean existFlag = csDatabase.ChkDuplicatePhysicalAddMst(tempPhysicalUid, strProcName, strModuleAddFrm, "");
                if (!existFlag)
                {
                    string confirmValue = Request.Form["confirm_value"];
                    if (confirmValue == "Yes")
                    {
                        Boolean tmpFlag = csDatabase.UpdPhysicalAddMst(tempPhysicalUid, strPlcNo, strProcName, strModuleAddFrm, strPhysicalAdd, strCurUser);
                        if (!tmpFlag)
                        {
                            GlobalFunc.ShowErrorMessage("Unable to update Physical Address for Process Name [" + tempModuleProcName + "] and Module Address [" + tempModuleAdd + "].");
                        }
                        else
                        {
                            GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> edited Physical Address['" + strPhysicalAdd + "'] for PLC No['" + strPlcNo + "'] Process Name['" + strProcName + "'] Module Address ['" + strModuleAddFrm + "']");
                            ClientScriptManager CSM = Page.ClientScript;
                            string strconfirm = "<script>if(!window.alert('Physical Address for Process Name [" + tempModuleProcName + "] and Module Address [" + tempModuleAdd + "] has updated.')){window.location.href='PhysicalAddMst.aspx'}</script>";
                            CSM.RegisterClientScriptBlock(this.GetType(), "Confirm", strconfirm, false);
                        }
                    }
                }
                else
                {
                    GlobalFunc.ShowErrorMessage("Same Process Name and Module Address existed in another record. Please check.");
                    //GlobalFunc.ShowErrorMessage("Duplicate of the combination Process Name and Module Address in PhysicalAddMst. Please check.");
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
            Response.Redirect("PhysicalAddMst.aspx");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Paging Index
    protected void gvPhysicalAddMst_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
