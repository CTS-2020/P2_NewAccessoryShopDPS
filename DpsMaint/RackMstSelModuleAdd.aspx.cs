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

public partial class SelectionModuleAdd : System.Web.UI.Page
{
    private int NewPageIndex
    {
        get { return (int)ViewState["NewPageIndex"]; }
        set { ViewState["NewPageIndex"] = value; }
    }

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
                String SessModuleAddFrm = Convert.ToString(Session["SessModuleAddFrm"]);
                if (SessModuleAddFrm != "")
                {
                    txtModuleAddFrm.Text = SessModuleAddFrm;
                }

                String SessModuleAddTo = Convert.ToString(Session["SessModuleAddTo"]);
                if (SessModuleAddTo != "")
                {
                    txtModuleAddTo.Text = SessModuleAddTo;
                }

                if (strRackName != "")
                {
                    lblTmpRackName.Text = strRackName;
                    lblTmpPartId.Text = strPartId;
                    lblTmpPlcNo.Text = strPlcNo;
                    lblTmpProcName.Text = strProcName;
                }
                NewPageIndex = 0;
                SearchLampModuleAddMst();
            }
            catch (Exception ex)
            {
                GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            }
        }
    }
    #endregion

    #region Method

    #region SearchLampModuleAddMst
    private void SearchLampModuleAddMst()
    {
        try
        {
            DataSet dsSearch = new DataSet();
            DataTable dtSearch = new DataTable();

            String strRackName = Convert.ToString(lblTmpRackName.Text);
            String strPlcNo = Convert.ToString(lblTmpPlcNo.Text);
            String strProcName = Convert.ToString(lblTmpProcName.Text);
            String strModuleAddFrm = Convert.ToString(txtModuleAddFrm.Text);
            String strModuleAddTo = Convert.ToString(txtModuleAddTo.Text);

            String strShowUsed = "";
            if (cbShowUsed.Checked == true)
            {
                strShowUsed = "true";
            }

            dsSearch = csDatabase.SrcLmAddRackType(strPlcNo, strProcName, strShowUsed, strModuleAddFrm, strModuleAddTo);
            dtSearch = dsSearch.Tables[0];
            BindGridView(dtSearch);
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region BindGridView
    private bool BindGridView(DataTable dtLampModuleAddMst)
    {
        try
        {
            DataView dvLampModuleAddMst = new DataView(dtLampModuleAddMst);

            gvLampModuleAddMst.DataSource = dvLampModuleAddMst;
            gvLampModuleAddMst.PageIndex = NewPageIndex;
            gvLampModuleAddMst.DataBind();
            return true;
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }
    #endregion

    #region Clear Lamp Module Data
    private void ClearLmModuleData()
    {
        try
        {
            String strRackMstDetId = Convert.ToString(lblTmpPartId.Text);
            String strRackName = Convert.ToString(lblTmpRackName.Text);
            String strProcName = csDatabase.GetRackMstProcName(strRackName);
            String strPlcNo = csDatabase.GetRackMstPlcNo(strRackName);
            String strBlockName = csDatabase.GetRackMstBlockName(strRackName);
            String strCurUser = Convert.ToString(Session["SessUserId"]);  //***ace_20160416_001

            String strRackLoc = "";
            String[] tmpRackMstDetIdVal = strRackMstDetId.Split('^');
            int tmpRackMstDetIdCnt = tmpRackMstDetIdVal.Length;
            if (tmpRackMstDetIdCnt > 1)
            {
                strRackLoc = tmpRackMstDetIdVal[0] + "  " + tmpRackMstDetIdVal[1] + "-" + tmpRackMstDetIdVal[2];
            }

            GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to empty Rack Master Location '" + strRackLoc + "' Module Address");
            //Boolean blDelFlg = csDatabase.DelRackMstDetModule(strRackMstDetId);  //***ace_20160416_001
            Boolean blDelFlg = csDatabase.DelRackMstDetModule(strRackMstDetId, strCurUser);
            if (blDelFlg)
            {
                csDatabase.UpdLampModuleLoc(strProcName, "", "", strRackMstDetId, "");
                csDatabase.UpdLmChange(strPlcNo, strProcName, strBlockName, "");
                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> emptied Rack Master Location '" + strRackLoc + "' Module Address");
            }
            else
            {
                lblMsg.Text = "Unable to empty Module Address for Current Rack Item.";
                lblMsg.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #endregion

    #region Events

    #region BtnSearch
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            SearchLampModuleAddMst();
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

            if (!csDatabase.ChkRackMstDetExist(strRackMstDetId))
            {
                lblMsg.Text = "Module Address is not selected for Current Rack Item.";
                lblMsg.Visible = true;
                return;
            }
            else
            {
                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    ClearLmModuleData();
                }
            }

            Session["SessTempRackName"] = Convert.ToString(lblTmpRackName.Text);
            Response.Redirect("RackMstDet.aspx");
            //Response.Write("<script>function RefreshParent(){window.opener.location.reload();window.close();}RefreshParent();</script>");
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

    #region gvLampModuleAddMst_RowCommand
    protected void gvLampModuleAddMst_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (Convert.ToString(e.CommandArgument) == "First") return;
            if (Convert.ToString(e.CommandArgument) == "Last") return;
            Int32 index = Convert.ToInt32(e.CommandArgument);            

            if (e.CommandName == "SelectRecord")
            {
                GridViewRow selectedRow = ((GridView)e.CommandSource).Rows[index];

                String strRackMstDetId = Convert.ToString(lblTmpPartId.Text);
                String strRackName = Convert.ToString(lblTmpRackName.Text);
                String strModuleAdd = Convert.ToString(selectedRow.Cells[1].Text);
                String strModuleName = Convert.ToString(selectedRow.Cells[2].Text);
                String strProcName = csDatabase.GetRackMstProcName(strRackName);
                String strCurRackLoc = GlobalFunc.getReplaceFrmUrl(Convert.ToString(selectedRow.Cells[4].Text));
                String strCurUser = Convert.ToString(Session["SessUserId"]);  //***ace_20160416_001

                if (strCurRackLoc != " ")
                {
                    lblMsg.Text = "Current Module Address had already been selected in other rack location. Please check.";
                    lblMsg.Visible = true;
                    return;
                }

                String strRackLoc = "";
                String[] tmpRackMstDetIdVal = strRackMstDetId.Split('^');
                int tmpRackMstDetIdCnt = tmpRackMstDetIdVal.Length;
                if (tmpRackMstDetIdCnt > 1)
                {
                    strRackLoc = tmpRackMstDetIdVal[0] + "  " + tmpRackMstDetIdVal[1] + "-" + tmpRackMstDetIdVal[2];
                }

                if (!csDatabase.ChkRackMstDetExist(strRackMstDetId))
                {
                    lblMsg.Text = "Please select AIS Data before selecting Module Address for Current Rack Item.";
                    lblMsg.Visible = true;
                    return;
                }
                else
                {
                    GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to update Rack Master Location '" + strRackLoc + "' Module Address");
                    ClearLmModuleData();
                    //Boolean blUpdFlg = csDatabase.UpdRackDetModule(strRackMstDetId, strRackName, strModuleAdd, strModuleName);  //***ace_20160416_001
                    Boolean blUpdFlg = csDatabase.UpdRackDetModule(strRackMstDetId, strRackName, strModuleAdd, strModuleName, strCurUser);
                    if (blUpdFlg)
                    {
                        tmpRackMstDetIdVal = strRackMstDetId.Split('^');
                        tmpRackMstDetIdCnt = tmpRackMstDetIdVal.Length;
                        if (tmpRackMstDetIdCnt > 1)
                        {
                            strRackLoc = tmpRackMstDetIdVal[0] + "  " + tmpRackMstDetIdVal[1] + "-" + tmpRackMstDetIdVal[2];
                            csDatabase.UpdLampModuleLoc(strProcName, strModuleAdd, strModuleName, strRackMstDetId, strRackLoc);
                        }
                    }
                    else
                    {
                        lblMsg.Text = "Unable to select Current Module Address [" + strModuleAdd + "] for Current Rack Item.";
                        lblMsg.Visible = true;
                        return;
                    }
                }

                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> updated Rack Master Location '" + strRackLoc + "' Module Address");

                Session["SessModuleAddFrm"] = Convert.ToString(txtModuleAddFrm.Text);
                Session["SessModuleAddTo"] = Convert.ToString(txtModuleAddTo.Text);
                Session["SessTempRackName"] = Convert.ToString(lblTmpRackName.Text);

                //Response.Write("<script>function RefreshParent(){window.opener.location.reload();window.close();}RefreshParent();</script>");
                Response.Redirect("RackMstDet.aspx");
            }
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
            SearchLampModuleAddMst();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #endregion
}
