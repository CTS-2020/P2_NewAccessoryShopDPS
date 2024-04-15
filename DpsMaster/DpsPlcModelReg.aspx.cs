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

public partial class DpsPlcMst : System.Web.UI.Page
{
    private int NewPageIndex
    {
        get { return (int)ViewState["NewPageIndex"]; }
        set { ViewState["NewPageIndex"] = value; }
    }

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        //String tempPlcNo = Convert.ToString(Session["SessTempPlcNo"]);
        //String tempModelID = Convert.ToString(Session["SessTempModelID"]);
        String tempModelNo = Convert.ToString(Session["SessTempModelNo"]);

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
                if (tempModelNo != "")
                {
                    btnUpdate.Visible = true;
                    btnNewModel.Visible = false;

                    DataSet dsPlcModel = new DataSet();
                    DataTable dtPlcModel = new DataTable();
                    //dsPlcMst = csDatabase.SrcPlcMstList(Convert.ToString(tempPlcNo), "", "", "", "", "");
                    dsPlcModel = csDatabase.SrcPlcModelList(Convert.ToString(tempModelNo), "", "", "");
                    dtPlcModel = dsPlcModel.Tables[0];
                    BindtoText(dtPlcModel);
                }
                else
                {
                    btnUpdate.Visible = false;
                    btnNewModel.Visible = true;
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

    #region BindtoText
    public void BindtoText(DataTable dt)
    {
        try
        {
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToString(dt.Rows[0]["plcmodel_no"]).Trim() != "")
                {
                    lblTmpModelNo.Text = Convert.ToString(dt.Rows[0]["plcmodel_no"]).Trim();
                    txtPlcModelNo.Text = Convert.ToString(dt.Rows[0]["plcmodel_no"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["plcmodel_desc"]).Trim() != "")
                {
                    txtPlcModelDesc.Text = Convert.ToString(dt.Rows[0]["plcmodel_desc"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["phyaddr_from"]).Trim() != "")
                {
                    txtPhyAddrFrom.Text = Convert.ToString(dt.Rows[0]["phyaddr_from"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["phyaddr_to"]).Trim() != "")             //Added by YanTeng 10/09/2020
                {
                    txtPhyAddrTo.Text = Convert.ToString(dt.Rows[0]["phyaddr_to"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["conv_type"]).Trim() != "")
                {
                    ddConvType.SelectedValue = Convert.ToString(dt.Rows[0]["conv_type"]);
                }
                //if (Convert.ToString(dt.Rows[0]["digit_no"]).Trim() != "")
                //{
                //    ddDigitNo.SelectedValue = Convert.ToString(dt.Rows[0]["digit_no"]);
                //}
                if (Convert.ToString(dt.Rows[0]["enable"]).Trim() != "")
                {
                    cbEnable.Checked = Convert.ToBoolean(dt.Rows[0]["enable"]);
                }
            }
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
            txtPlcModelNo.Text = "";
            txtPlcModelDesc.Text = "";
            txtPhyAddrFrom.Text = "";
            txtPhyAddrTo.Text = "";
            ddConvType.SelectedIndex = 0;
            //ddDigitNo.SelectedIndex = 0; 
            cbEnable.Checked = false;
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
            String strPlcModelNo = "";
            String strPlcModelDesc = "";
            String strPhyAddrFrom = "";
            String strPhyAddrTo = "";
            //String strDigitNo = "";

            if (Convert.ToString(Request.QueryString["modelno"]) != "") strPlcModelNo = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["modelno"]));
            if (Convert.ToString(Request.QueryString["desc"]) != "") strPlcModelDesc = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["desc"]));
            if (Convert.ToString(Request.QueryString["addfrm"]) != "") strPhyAddrFrom = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["addfrm"]));
            if (Convert.ToString(Request.QueryString["addto"]) != "") strPhyAddrTo = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["addto"]));
            //if (Convert.ToString(Request.QueryString["digit"]) != "") strDigitNo = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["digit"]));

            DataSet dsSearch = new DataSet();
            DataTable dtSearch = new DataTable();

            dsSearch = csDatabase.SrcPlcModelList(strPlcModelNo, strPlcModelDesc, strPhyAddrFrom, strPhyAddrTo);
            dtSearch = dsSearch.Tables[0];

            DataView dvPlcModelList = new DataView(dtSearch);

            gvPlcModelList.DataSource = dvPlcModelList;
            gvPlcModelList.PageIndex = NewPageIndex;
            gvPlcModelList.DataBind();
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
            if (Convert.ToString(txtPlcModelNo.Text) == "")
            {
                lblMsg.Text = "Please enter GW Model No.";
                lblMsg.Visible = true;
                return false;
            }
            if (Convert.ToString(txtPlcModelDesc.Text) == "")
            {
                lblMsg.Text = "Please enter GW Model Description.";
                lblMsg.Visible = true;
                return false;
            }
            else if (csDatabase.ChkDuplicatePlcModel(Convert.ToString(txtPlcModelNo.Text), Convert.ToString(Session["SessTempModelNo"])))
            {
                lblMsg.Text = "Duplicate GW Model Number. Please check.";
                lblMsg.Visible = true;
                return false;
            }
            //else if (!GlobalFunc.IsAlphaNum(Convert.ToString(txtPlcModelDesc.Text)))
            //{
            //    lblMsg.Text = "GW Model Description must be Alpha-Numeric only.";
            //    lblMsg.Visible = true;
            //    return false;
            //}
            else if (Convert.ToString(txtPhyAddrFrom.Text) == "" || Convert.ToString(txtPhyAddrTo.Text) == "")
            {
                lblMsg.Text = "Please enter Physical Address.";
                lblMsg.Visible = true;
                return false;
            }
            else if (!GlobalFunc.IsTextAValidInteger(txtPhyAddrFrom.Text) || !GlobalFunc.IsTextAValidInteger(txtPhyAddrTo.Text))
            {
                lblMsg.Text = "Physical Address must be a valid integer.";
                lblMsg.Visible = true;
                return false;
            }
            //else if (ddDigitNo.SelectedIndex < 1)
            //{
            //    lblMsg.Text = "Please select the no. of digit for Physical Address.";
            //    lblMsg.Visible = true;
            //    return false;
            //}
            //else if (txtPhyAddrFrom.Text.Length != Convert.ToInt32(ddDigitNo.SelectedValue) || txtPhyAddrTo.Text.Length != Convert.ToInt32(ddDigitNo.SelectedValue))
            //{
            //    lblMsg.Text = "The length of Physical Address must be equivalent with the selected No. of Digit.";
            //    lblMsg.Visible = true;
            //    return false;
            //}
            else if(Convert.ToInt32(txtPhyAddrFrom.Text) >= Convert.ToInt32(txtPhyAddrTo.Text))
            {
                lblMsg.Text = "Physical Address To must be greater than Physical Address From.";
                lblMsg.Visible = true;
                return false;
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

    #endregion

    #region Events

    #region btnNewModel
    protected void btnNewModel_Click(object sender, EventArgs e)
    {
        try
        {
            Boolean boolValid = ChkDataValid();

            if (boolValid)
            {
                String strPlcModelNo = Convert.ToString(txtPlcModelNo.Text);
                String strPlcModelDesc = Convert.ToString(txtPlcModelDesc.Text);
                String strPhyAddrFrom = Convert.ToString(txtPhyAddrFrom.Text);
                String strPhyAddrTo = Convert.ToString(txtPhyAddrTo.Text);
                String strConvType = Convert.ToString(ddConvType.SelectedValue);
                //String strDigitNo = Convert.ToString(ddDigitNo.SelectedValue);
                String strEnable = Convert.ToString(cbEnable.Checked);
                String strCurUser = Convert.ToString(Session["SessUserId"]);
                
                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    Boolean tmpFlag = csDatabase.SvPlcModelMst(strPlcModelNo, strPlcModelDesc, strPhyAddrFrom, strPhyAddrTo, strConvType, strEnable, strCurUser);
                    if (!tmpFlag)
                    {
                        GlobalFunc.ShowMessage("Unable to save GW Model No: " + strPlcModelNo + " .");
                    }
                    else
                    {
                        //csDatabase.SvBlockMstConv(strPlcNo, strProcName);
                        //csDatabase.SvPlcMstConv(strPlcNo, strProcName);
                        //csDatabase.SvPlcMstConvMap(strPlcNo, strProcName);
                        //csDatabase.SvLmMode(strPlcNo, strProcName);
                        //csDatabase.ResetPointer(strPlcNo);
                        GlobalFunc.Log("<" + strCurUser + "> created GW Model No '" + strPlcModelNo + "'");
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = "<script>if(!window.alert('GW Model No: " + strPlcModelNo + " saved.')){window.location.href='DpsPlcModel.aspx'}</script>";
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
            Boolean boolValid = ChkDataValid();

            if (boolValid)
            {
                String strPlcModelNo = Convert.ToString(txtPlcModelNo.Text);
                String strPlcModelDesc = Convert.ToString(txtPlcModelDesc.Text);
                String strPhyAddrFrom = Convert.ToString(txtPhyAddrFrom.Text);
                String strPhyAddrTo = Convert.ToString(txtPhyAddrTo.Text);
                String strConvType = Convert.ToString(ddConvType.SelectedValue);
                //String strDigitNo = Convert.ToString(ddDigitNo.SelectedValue);
                String strEnable = Convert.ToString(cbEnable.Checked);
                String tempPlcModelNo = Convert.ToString(lblTmpModelNo.Text);
                String strCurUser = Convert.ToString(Session["SessUserId"]);
                
                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    Boolean tmpFlag = csDatabase.UpdPlcModelMst(strPlcModelNo, strPlcModelDesc, strPhyAddrFrom, strPhyAddrTo, strConvType, strEnable, tempPlcModelNo, strCurUser);
                    if (!tmpFlag)
                    {
                        GlobalFunc.ShowErrorMessage("Unable to update GW Model No: " + strPlcModelNo + " .");
                    }
                    else
                    {
                        //csDatabase.UpdPlcAllRem(strPlcNo, strProcName, tempPlcNo, strCurUser);
                        GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> updated GW Model No '" + tempPlcModelNo + "' to '" + strPlcModelNo + "'");
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = "<script>if(!window.alert('GW Model No: " + strPlcModelNo + " updated.')){window.location.href='DpsPlcModel.aspx'}</script>";
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
            Response.Redirect("DpsPlcModel.aspx");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Paging Index
    protected void gvPlcModelList_PageIndexChanging(object sender, GridViewPageEventArgs e)
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

    #endregion
}
