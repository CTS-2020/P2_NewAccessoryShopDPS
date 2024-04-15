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
        String tempPlcNo = Convert.ToString(Session["SessTempPlcNo"]);

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
                getPlcModel();                  //Added by YanTeng 29/09/2020

                if (tempPlcNo != "")
                {
                    btnUpdate.Visible = true;
                    btnNewPlcMst.Visible = false;

                    DataSet dsPlcMst = new DataSet();
                    DataTable dtPlcMst = new DataTable();
                    dsPlcMst = csDatabase.SrcPlcMstList(Convert.ToString(tempPlcNo), "", "", "", "", "");
                    dtPlcMst = dsPlcMst.Tables[0];
                    BindtoText(dtPlcMst);
                }
                else
                {
                    btnUpdate.Visible = false;
                    btnNewPlcMst.Visible = true;
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
                if (Convert.ToString(dt.Rows[0]["plc_no"]).Trim() != "")
                {
                    lblTmpPlcNo.Text = Convert.ToString(dt.Rows[0]["plc_no"]).Trim();
                    txtPlcNo.Text = Convert.ToString(dt.Rows[0]["plc_no"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["proc_name"]).Trim() != "")
                {
                    txtProcName.Text = Convert.ToString(dt.Rows[0]["proc_name"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["ip_add"]).Trim() != "")
                {
                    txtIpAdd.Text = Convert.ToString(dt.Rows[0]["ip_add"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["plc_model"]).Trim() != "")             //Added by YanTeng 10/09/2020
                {
                    //ddPlcModel.SelectedItem.Text = Convert.ToString(dt.Rows[0]["plc_model"]).Trim();
                    ddPlcModel.SelectedValue = Convert.ToString(dt.Rows[0]["plc_model"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["plc_nw_station"]).Trim() != "")
                {
                    txtPlcNwStation.Text = Convert.ToString(dt.Rows[0]["plc_nw_station"]);
                }
                if (Convert.ToString(dt.Rows[0]["plc_logical_station_no"]).Trim() != "")
                {
                    txtPlcLogicalStationNo.Text = Convert.ToString(dt.Rows[0]["plc_logical_station_no"]);
                }
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
            txtPlcNo.Text = "";
            txtProcName.Text = "";
            txtIpAdd.Text = "";
            txtPlcNwStation.Text = "";
            txtPlcLogicalStationNo.Text = "";
            ddPlcModel.SelectedIndex = 0;                 //Added by YanTeng 14/09/2020
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
            String strPlcNo = "";
            String strProcName = "";
            String strIpAdd = "";
            String strPlcNwStation = "";
            String strPlcLogicalStation = "";
            String strPlcModel = "";

            if (Convert.ToString(Request.QueryString["plcno"]) != "") strPlcNo = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["plcno"]));
            if (Convert.ToString(Request.QueryString["procname"]) != "") strProcName = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["procname"]));
            if (Convert.ToString(Request.QueryString["ipadd"]) != "") strIpAdd = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["ipadd"]));
            if (Convert.ToString(Request.QueryString["nwstation"]) != "") strPlcNwStation = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["nwstation"]));
            if (Convert.ToString(Request.QueryString["lsn"]) != "") strPlcLogicalStation = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["lsn"]));
            if (Convert.ToString(Request.QueryString["plcmodel"]) != "") strPlcModel = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["plcmodel"]));      //Added by YanTeng 10/09/2020

            DataSet dsSearch = new DataSet();
            DataTable dtSearch = new DataTable();

            dsSearch = csDatabase.SrcPlcMstList(strPlcNo, strProcName, strIpAdd, strPlcModel, strPlcNwStation, strPlcLogicalStation);
            dtSearch = dsSearch.Tables[0];

            DataView dvPlcMstList = new DataView(dtSearch);

            gvPlcMstList.DataSource = dvPlcMstList;
            gvPlcMstList.PageIndex = NewPageIndex;
            gvPlcMstList.DataBind();
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
            if (Convert.ToString(txtPlcNo.Text) == "")
            {
                lblMsg.Text = "Please enter PLC No.";
                lblMsg.Visible = true;
                return false;
            }
            else if (!GlobalFunc.IsTextAValidInteger(txtPlcNo.Text))
            {
                lblMsg.Text = "PLC No must be a valid integer.";
                lblMsg.Visible = true;
                return false;
            }
            else if (int.Parse(txtPlcNo.Text) < 1 || int.Parse(txtPlcNo.Text) > 12)
            {
                lblMsg.Text = "PLC No must be between 1 - 12 only.";
                lblMsg.Visible = true;
                return false;
            }
            else if (csDatabase.ChkDuplicatePlcNo(Convert.ToString(txtPlcNo.Text), Convert.ToString(Session["SessTempPlcNo"])))
            {
                lblMsg.Text = "Duplicate PLC Number. Please check.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Convert.ToString(txtProcName.Text) == "")
            {
                lblMsg.Text = "Please enter Process Name.";
                lblMsg.Visible = true;
                return false;
            }
            else if (!GlobalFunc.IsAlphaNum(Convert.ToString(txtProcName.Text)))
            {
                lblMsg.Text = "Process Name must be Alpha-Numeric only.";
                lblMsg.Visible = true;
                return false;
            }
            else if (csDatabase.ChkDuplicateProcName(Convert.ToString(txtProcName.Text), Convert.ToString(Session["SessTempPlcNo"])))
            {
                lblMsg.Text = "Duplicate Process Name. Please check.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Convert.ToString(txtIpAdd.Text) == "")
            {
                lblMsg.Text = "Please enter IP Address.";
                lblMsg.Visible = true;
                return false;
            }
            else if (!GlobalFunc.IsTextAValidIPAddress(Convert.ToString(txtIpAdd.Text)))
            {
                lblMsg.Text = Convert.ToString(txtIpAdd.Text) + " is not a valid IP Address.";
                lblMsg.Visible = true;
                return false;
            }
            else if (csDatabase.ChkDuplicateIpAdd(Convert.ToString(txtIpAdd.Text), Convert.ToString(Session["SessTempPlcNo"])))
            {
                lblMsg.Text = "Duplicate IP Address. Please check.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Convert.ToString(txtPlcNwStation.Text) == "")
            {
                lblMsg.Text = "Please enter Nw Station No.";
                lblMsg.Visible = true;
                return false;
            }
            else if (!GlobalFunc.IsTextAValidInteger(txtPlcNwStation.Text))
            {
                lblMsg.Text = "Nw Station No must be a valid integer.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Convert.ToString(txtPlcLogicalStationNo.Text) == "")
            {
                lblMsg.Text = "Please enter PLC Logical Station Number.";
                lblMsg.Visible = true;
                return false;
            }
            else if (!GlobalFunc.IsTextAValidInteger(txtPlcLogicalStationNo.Text))
            {
                lblMsg.Text = "PLC Logical Station Number must be a valid integer.";
                lblMsg.Visible = true;
                return false;
            }
            else if (int.Parse(txtPlcLogicalStationNo.Text) < 1 || int.Parse(txtPlcLogicalStationNo.Text) > 64)
            {
                lblMsg.Text = "PLC No must be between 1 - 64 only.";
                lblMsg.Visible = true;
                return false;
            }
            else if (csDatabase.ChkDuplicatePlcLsn(Convert.ToString(txtPlcLogicalStationNo.Text), Convert.ToString(Session["SessTempPlcNo"])))
            {
                lblMsg.Text = "Duplicate PLC Logical Station Number. Please check.";
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

    #region getPlcModel
    private void getPlcModel()              //Added by YanTeng 29/09/2020
    {
        try
        {
            ddPlcModel.DataSource = GlobalFunc.getAllPlcModel();
            ddPlcModel.DataTextField = "Code";
            ddPlcModel.DataValueField = "Code";
            ddPlcModel.DataBind();
            ddPlcModel.Items.Insert(0, " ");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #endregion

    #region Events

    #region btnNewPlcMst
    protected void btnNewPlcMst_Click(object sender, EventArgs e)
    {
        try
        {
            Boolean boolValid = ChkDataValid();

            if (boolValid)
            {
                String strPlcNo = Convert.ToString(txtPlcNo.Text);
                String strProcName = Convert.ToString(txtProcName.Text);
                String strIpAdd = Convert.ToString(txtIpAdd.Text);
                String strPlcNwStation = Convert.ToString(txtPlcNwStation.Text);
                String strPlcLogicalStationNo = Convert.ToString(txtPlcLogicalStationNo.Text);
                String strEnable = Convert.ToString(cbEnable.Checked);
                String strCurUser = Convert.ToString(Session["SessUserId"]);
                String strPlcModel = Convert.ToString(ddPlcModel.SelectedItem);            //Added by YanTeng 10/09/2020

                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    Boolean tmpFlag = csDatabase.SvPlcMst(strPlcNo, strProcName, strIpAdd, strPlcModel, strPlcNwStation, strPlcLogicalStationNo, strEnable, strCurUser);
                    if (!tmpFlag)
                    {
                        GlobalFunc.ShowMessage("Unable to save PLC No: " + strPlcNo + " .");
                    }
                    else
                    {
                        //csDatabase.SvBlockMstConv(strPlcNo, strProcName);
                        //csDatabase.SvPlcMstConv(strPlcNo, strProcName);
                        //csDatabase.SvPlcMstConvMap(strPlcNo, strProcName);
                        //csDatabase.SvLmMode(strPlcNo, strProcName);
                        //csDatabase.ResetPointer(strPlcNo);
                        GlobalFunc.Log("<" + strCurUser + "> created PLC No '" + strPlcNo + "'");
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = "<script>if(!window.alert('Plc No: " + strPlcNo + " saved.')){window.location.href='DpsPlcMst.aspx'}</script>";
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
                String strPlcNo = Convert.ToString(txtPlcNo.Text);
                String strProcName = Convert.ToString(txtProcName.Text);
                String strIpAdd = Convert.ToString(txtIpAdd.Text);
                String strPlcModel = Convert.ToString(ddPlcModel.SelectedItem);            //Added by YanTeng 11/09/2020
                String strPlcNwStation = Convert.ToString(txtPlcNwStation.Text);
                String strPlcLogicalStationNo = Convert.ToString(txtPlcLogicalStationNo.Text);
                String strEnable = Convert.ToString(cbEnable.Checked);
                String tempPlcNo = Convert.ToString(lblTmpPlcNo.Text);
                String strCurUser = Convert.ToString(Session["SessUserId"]);
                
                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    Boolean tmpFlag = csDatabase.UpdPlcMst(strPlcNo, strProcName, strIpAdd, strPlcModel, strPlcNwStation, strPlcLogicalStationNo, strEnable, tempPlcNo, strCurUser);
                    if (!tmpFlag)
                    {
                        GlobalFunc.ShowErrorMessage("Unable to update PLC No: " + strPlcNo + " .");
                    }
                    else
                    {
                        //csDatabase.UpdPlcAllRem(strPlcNo, strProcName, tempPlcNo, strCurUser);
                        GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> updated PLC No '" + tempPlcNo + "' to '" + strPlcNo + "'");
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = "<script>if(!window.alert('Plc No: " + strPlcNo + " updated.')){window.location.href='DpsPlcMst.aspx'}</script>";
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
            Response.Redirect("DpsPlcMst.aspx");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Paging Index
    protected void gvPlcMstList_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
