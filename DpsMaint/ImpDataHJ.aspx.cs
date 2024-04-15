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

public partial class ImpDataHJ : System.Web.UI.Page
{
    private int NewPageIndex
    {
        get { return (int)ViewState["NewPageIndex"]; }
        set { ViewState["NewPageIndex"] = value; }
    }

    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
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
                NewPageIndex = 0;
                getAisType();
                getAisName();
                SearchDataHJList();
            }
            catch (Exception ex)
            {
                GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            }
        }
    }
    #endregion

    #region Method

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
                ddAisName.DataSource = GlobalFunc.getAisName(strAisType);
                ddAisName.DataTextField = "Description";
                //ddAisName.DataValueField = "Name";
                ddAisName.DataValueField = "Description";
                ddAisName.DataBind();
                ddAisName.Items.Insert(0, " ");
                ddAisName.Items.Insert(1, "ALL");
            }
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region SearchDataHJList
    private void SearchDataHJList()
    {
        try
        {
            DataSet dsSearch = new DataSet();
            DataTable dtSearch = new DataTable();

            String strAisType = Convert.ToString(ddAisType.SelectedValue).Trim();
            String strAisItemId = Convert.ToString(ddAisName.SelectedValue).Trim();

            if (strAisItemId == "ALL") strAisItemId = "";

            dsSearch = csDatabase.SrcDataHJList(strAisType, strAisItemId, "");
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
    private bool BindGridView(DataTable dtDataHJList)
    {
        try
        {
            DataView dvDataHJList = new DataView(dtDataHJList);

            gvDataHJList.DataSource = dvDataHJList;
            gvDataHJList.PageIndex = NewPageIndex;
            gvDataHJList.DataBind();
            return true;
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return false;
        }
    }
    #endregion

    #region ClearAll
    private void ClearAll()
    {
        try
        {
            ddAisType.SelectedIndex = 0;
            ddAisName.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region ChkValid
    private Boolean ChkValid()
    {
        try
        {
            if (ddAisType.SelectedIndex == 0)
            {
                GlobalFunc.ShowMessage("Please select AIS Type.");
                return false;
            }
            else if (ddAisName.SelectedIndex == 0)
            {
                GlobalFunc.ShowMessage("Please select AIS Name.");
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

    #region btnImport
    protected void btnImport_Click(object sender, EventArgs e)
    {
        try
        {
            if (ChkValid())
            {
                String strAisType = Convert.ToString(ddAisType.SelectedValue);
                String strAisName = Convert.ToString(ddAisName.SelectedItem);
                String strAisItemId = Convert.ToString(ddAisName.SelectedValue);

                GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> attempted to Import Harigami/Jundate");
                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue == "Yes")
                {
                    Boolean blImpHjData = csDatabase.ImpHjData(strAisType, strAisItemId);

                    if (blImpHjData)
                    {
                        GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> " + strAisType + " Data [" + strAisName + "] imported successfully.");
                        GlobalFunc.ShowMessage(strAisType + " Data [" + strAisName + "] imported successfully.");
                    }
                    else
                    {
                        GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> " + strAisType + " Data [" + strAisName + "] import FAIL.");
                        GlobalFunc.ShowMessage(strAisType + " Data [" + strAisName + "] import FAIL. Please check");
                    }
                }
                else
                {
                    GlobalFunc.Log("<" + Convert.ToString(Session["SessUserId"]) + "> canceled to Import Harigami/Jundate");
                }
            }

            NewPageIndex = 0;
            SearchDataHJList();
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

    #region BtnSearch
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            NewPageIndex = 0;
            SearchDataHJList();
        }
        catch (Exception ex)
        {
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
        }
    }
    #endregion

    #region Paging Index
    protected void gvDataHJList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            NewPageIndex = e.NewPageIndex;
            SearchDataHJList();
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
            if (ddAisType.SelectedIndex != 0)
            {
                getAisName();
            }
            else
            {
                ddAisName.Items.Clear();
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
