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
using dpant;
using System.Text;

public partial class TitleMenu : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder Sb = new StringBuilder();
        Sb.Append("Welcome " + Convert.ToString(Session["SessUserName"]) + "");
        lblUser.Text = Convert.ToString(Sb);

        if (!IsPostBack)
        {
            String userID = Convert.ToString(Session["SessUserId"]);
            lblUserId.Text = userID;

            String RoleCode = Convert.ToString(Session["SessRoleCode"]);
            lblRoleCode.Text = RoleCode;

            String role = "";
            role = Convert.ToString(Session["SessRoleCode"]);
            Sb = new StringBuilder();

            if (role == "Admin")
            {
                Sb.Append("<a href='HomeAdmin.aspx' target='home_content'><button class='button'>Home</button></a>");
            }
            else if (role == "IT")
            {
                Sb.Append("<a href='HomeIT.aspx' target='home_content'><button class='button'>Home</button></a>");
            }
            else if (role == "Production")
            {
                Sb.Append("<a href='HomeProd.aspx' target='home_content'><button class='button'>Home</button></a>");
            }

            lblButton.Text = Convert.ToString(Sb);
        }
    }

    protected void btnHome_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToString(Session["SessUserId"]) == "")
            {
                Response.Write("<script language='javascript'>alert('Your user session has timed out. Please login again.');window.top.location ='Login.aspx';</script>");
            }
            if (lblRoleCode.Text == "Admin")
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "redirect", "window.open('HomeAdmin.aspx','home_content')", true);
            }
            else if (lblRoleCode.Text == "IT")
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "redirect", "window.open('HomeIT.aspx','home_content')", true);
            }
            else if (lblRoleCode.Text == "Production")
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "redirect", "window.open('HomeProd.aspx','home_content')", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Info",
            "alert('" + ex + "');", true);
        }
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Response.Write("<script>window.top.location ='Login.aspx';</script>");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Info",
            "alert('" + ex + "');", true);
        }
    }
}
