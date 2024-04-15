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
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using dpant;

public partial class RackMst : System.Web.UI.Page
{
    private int NewPageIndex
    {
        get { return (int)ViewState["NewPageIndex"]; }
        set { ViewState["NewPageIndex"] = value; }
    }
    
    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        String tempRackName = Convert.ToString(Session["SessTempRackName"]);

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
                getGroupName();
                getBlockName();
                getPlcModel();          //Added by YanTeng 17/09/2020

                if (tempRackName != "")
                {
                    ddProcName.Enabled = false;
                    btnUpdate.Visible = true;
                    btnNewRack.Visible = false;

                    DataSet dsRack = new DataSet();
                    DataTable dtRack = new DataTable();
                    dsRack = csDatabase.SrcRackMst(Convert.ToString(tempRackName), "", "", "", "");
                    dtRack = dsRack.Tables[0];
                    BindtoText(dtRack);
                    genTable();
                }
                else
                {
                    btnUpdate.Visible = false;
                    btnNewRack.Visible = true;
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

    #region getGroupName
    private void getGroupName()
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

    #region getBlockName
    private void getBlockName()
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

    #region getPlcModel
    private void getPlcModel()              //Added by YanTeng 15/09/2020
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
                if (Convert.ToString(dt.Rows[0]["rack_name"]).Trim() != "")
                {
                    lblTmpRackName.Text = Convert.ToString(dt.Rows[0]["rack_name"]).Trim();
                    txtRackName.Text = Convert.ToString(dt.Rows[0]["rack_name"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["col_cnt"]).Trim() != "")
                {
                    lblTmpColCnt.Text = Convert.ToString(dt.Rows[0]["col_cnt"]).Trim();
                    txtColCnt.Text = Convert.ToString(dt.Rows[0]["col_cnt"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["row_cnt"]).Trim() != "")
                {
                    lblTmpRowCnt.Text = Convert.ToString(dt.Rows[0]["row_cnt"]).Trim();
                    txtRowCnt.Text = Convert.ToString(dt.Rows[0]["row_cnt"]).Trim();
                }
                if (Convert.ToString(dt.Rows[0]["proc_name"]).Trim() != "")
                {
                    String strPlcNo = Convert.ToString(csDatabase.getPlcNo(Convert.ToString(dt.Rows[0]["proc_name"])));
                    ddProcName.SelectedValue = strPlcNo;

                    //Added by YanTeng 02/12/2020
                    String strProcName = Convert.ToString(ddProcName.SelectedItem);
                    ddPlcModel.DataSource = GlobalFunc.getPlcModelbyProcName(strProcName);
                    ddPlcModel.DataTextField = "Description";
                    ddPlcModel.DataValueField = "Value";
                    ddPlcModel.DataBind();
                    getGroupName();
                }
                if (Convert.ToString(dt.Rows[0]["group_name"]).Trim() != "")
                {
                    ddGroupName.SelectedValue = Convert.ToString(dt.Rows[0]["group_name"]);
                    getBlockName();
                }
                if (Convert.ToString(dt.Rows[0]["block_name"]).Trim() != "")
                {
                    ddBlockName.SelectedValue = Convert.ToString(dt.Rows[0]["block_name"]);
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
            String RackName = "";
            String ProcName = "";
            String PlcNo = "";
            String GroupName = "";
            String BlockName = "";

            if (Convert.ToString(Request.QueryString["rnm"]) != "") RackName = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["rnm"]));
            if (Convert.ToString(Request.QueryString["pnm"]) != "") ProcName = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["pnm"]));
            if (Convert.ToString(Request.QueryString["pno"]) != "") PlcNo = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["pno"]));
            if (Convert.ToString(Request.QueryString["gnm"]) != "") GroupName = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["gnm"]));
            if (Convert.ToString(Request.QueryString["bnm"]) != "") BlockName = GlobalFunc.getReplaceFrmUrl(Convert.ToString(Request.QueryString["bnm"]));

            DataSet dsSearch = new DataSet();
            DataTable dtSearch = new DataTable();

            dsSearch = csDatabase.SrcRackMst(RackName, PlcNo, ProcName, GroupName, BlockName);
            dtSearch = dsSearch.Tables[0];

            DataView dvRack = new DataView(dtSearch);

            gvRack.DataSource = dvRack;
            gvRack.PageIndex = NewPageIndex;
            gvRack.DataBind();
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
                ddProcName.SelectedIndex = 0;
            }
            ddBlockName.SelectedIndex = 0;
            ddGroupName.SelectedIndex = 0;
            txtRackName.Text = "";
            txtColCnt.Text = "1";
            txtRowCnt.Text = "1";
            RackMstPreview.Visible = false;
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
            int rowCnt = 0;
            int colCnt = 0;
            int rowCtr = 0;
            int colCtr = 0;

            rowCnt = int.Parse(txtRowCnt.Text);
            colCnt = int.Parse(txtColCnt.Text);

            for (rowCtr = 1; rowCtr <= rowCnt; rowCtr++)
            {
                TableRow tRow = new TableRow();
                RackMstPreview.Rows.Add(tRow);

                for (colCtr = 1; colCtr <= colCnt; colCtr++)
                {
                    TableCell tCell = new TableCell();
                    tRow.Cells.Add(tCell);

                    string prodID = rowCtr + ", " + colCtr;
                    tCell.Controls.Add(new LiteralControl(prodID));
                }
            }
            RackMstPreview.Visible = true;
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
            else if (ddGroupName.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Group Name.";
                lblMsg.Visible = true;
                return false;
            }
            else if (ddBlockName.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select Block Name.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Convert.ToString(txtRackName.Text) == "")
            {
                lblMsg.Text = "Please enter Rack Name.";
                lblMsg.Visible = true;
                return false;
            }
            else if (csDatabase.ChkDuplicateRackName(Convert.ToString(txtRackName.Text), Convert.ToString(lblTmpRackName.Text)))
            {
                lblMsg.Text = "Duplicate Rack Name not allowed. Please check.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Convert.ToString(txtColCnt.Text) == "" || int.Parse(txtColCnt.Text) < 1)
            {
                lblMsg.Text = "Please enter Column Count.";
                lblMsg.Visible = true;
                return false;
            }
            else if (Convert.ToString(txtRowCnt.Text) == "" || int.Parse(txtRowCnt.Text) < 1)
            {
                lblMsg.Text = "Please enter Row Count.";
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

    #region btnNewRack
    protected void btnNewRack_Click(object sender, EventArgs e)
    {
        try
        {
            Boolean boolValid = ChkDataValid();
            if (boolValid)
            {
                String strRackName = Convert.ToString(txtRackName.Text);
                String strColCnt = Convert.ToString(txtColCnt.Text);
                String strRowCnt = Convert.ToString(txtRowCnt.Text);
                String strProcName = Convert.ToString(ddProcName.SelectedItem);
                String strPlcNo = Convert.ToString(ddProcName.SelectedValue);
                String strGroupName = Convert.ToString(ddGroupName.SelectedValue);
                String strBlockName = Convert.ToString(ddBlockName.SelectedValue);
                String strCurUser = Convert.ToString(Session["SessUserId"]);

                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    Boolean tmpFlag = csDatabase.SvRackMst(strRackName, strColCnt, strRowCnt, strPlcNo, strProcName, strGroupName, strBlockName, strCurUser);
                    if (!tmpFlag)
                    {
                        GlobalFunc.ShowErrorMessage("Unable to save Rack Name: " + strRackName + " .");
                    }
                    else
                    {
                        Session["SessTempRackName"] = strRackName;
                        GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> created Rack Name['" + strRackName + "'] Collumn['" + strColCnt + "'] Row['" + strRowCnt + "'] PLC No['" + strPlcNo + "'] Process Name['" + strProcName + "'] Group Name['" + strGroupName + "'] Block Name['" + strBlockName + "']");
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = "<script>if(!window.alert('Rack Name: " + strRackName + " saved.')){window.location.href='RackMst.aspx'}</script>";
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
                String strRackName = Convert.ToString(txtRackName.Text);
                String strColCnt = Convert.ToString(txtColCnt.Text);
                String strRowCnt = Convert.ToString(txtRowCnt.Text);
                String strProcName = Convert.ToString(ddProcName.SelectedItem);
                String strPlcNo = Convert.ToString(ddProcName.SelectedValue);
                String strGroupName = Convert.ToString(ddGroupName.SelectedValue);
                String strBlockName = Convert.ToString(ddBlockName.SelectedValue);
                String strTmpRackName = Convert.ToString(lblTmpRackName.Text);
                String strTmpRowCnt = Convert.ToString(lblTmpRowCnt.Text);
                String strTmpColCnt = Convert.ToString(lblTmpColCnt.Text);
                String strCurUser = Convert.ToString(Session["SessUserId"]);

                Boolean blRackLocOcc = csDatabase.ChkRackLocOcc(strTmpRackName, strTmpRowCnt, strTmpColCnt, strRowCnt, strColCnt);
                if (blRackLocOcc)
                {
                    GlobalFunc.ShowErrorMessage("Unable to update Rack Name: " + strRackName + " due to part occupied in decreased row/collumn. Please remove the parts under the decreased row/collumn before proceed!");
                    return;
                }

                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    Boolean tmpFlag = csDatabase.UpdRackMst(strRackName, strColCnt, strRowCnt, strPlcNo, strProcName, strGroupName, strBlockName, strTmpRackName, strCurUser);
                    if (!tmpFlag)
                    {
                        GlobalFunc.ShowErrorMessage("Unable to update Rack Name: " + strRackName + " .");
                    }
                    else
                    {
                        //csDatabase.UpdRackMstRem(strRackName, strRowCnt, strColCnt, strTmpRackName, strTmpRowCnt, strTmpColCnt);
                        GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> updated '" + strTmpRackName + "' to Rack Name['" + strRackName + "'] Collumn['" + strColCnt + "'] Row['" + strRowCnt + "'] PLC No['" + strPlcNo + "'] Process Name['" + strProcName + "'] Group Name['" + strGroupName + "'] Block Name['" + strBlockName + "']");
                        Session["SessTempRackName"] = strRackName;
                        ClientScriptManager CSM = Page.ClientScript;
                        string strconfirm = "<script>if(!window.alert('Rack Name: " + strRackName + " updated.')){window.location.href='RackMst.aspx'}</script>";
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
            Response.Redirect("RackMst.aspx");
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region On Drop Down List Selected Index Change
    protected void ddProcName_OnSelectedIndexChanged(object sender, EventArgs e)
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
        getGroupName();
        getBlockName();
    }

    protected void ddGroupName_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        getBlockName();
    }
    #endregion

    #region Preview Table
    protected void previewTable(object sender, EventArgs e)
    {
        genTable();
    }
    #endregion

    #region Row/Column Count Increase/Decrease
    protected void incColCnt(object sender, EventArgs e)
    {
        int colCnt = int.Parse(txtColCnt.Text);
        colCnt++;
        if (colCnt > 10)
        {
            colCnt = 10;
        }
        txtColCnt.Text = Convert.ToString(colCnt);
    }

    protected void decColCnt(object sender, EventArgs e)
    {
        int colCnt = int.Parse(txtColCnt.Text);
        colCnt--;
        txtColCnt.Text = Convert.ToString(colCnt);
    }

    protected void incRowCnt(object sender, EventArgs e)
    {
        int rowCnt = int.Parse(txtRowCnt.Text);
        rowCnt++;
        if (rowCnt> 5) 
        {
            rowCnt = 5;
        }
        txtRowCnt.Text = Convert.ToString(rowCnt);
    }

    protected void decRowCnt(object sender, EventArgs e)
    {
        int rowCnt = int.Parse(txtRowCnt.Text);
        rowCnt--;
        txtRowCnt.Text = Convert.ToString(rowCnt);
    }
    #endregion

    #region Paging Index
    protected void gvRack_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
