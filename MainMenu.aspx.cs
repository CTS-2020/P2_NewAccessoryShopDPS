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
using System.IO;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Deployment.Internal;
using dpant;
using System.Text;

public partial class MainMenu : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        String role = "";
        StringBuilder Sb = new StringBuilder();
        try
        {
            if (Convert.ToString(Session["SessRoleCode"]) != "")
            {
                role = Convert.ToString(Session["SessRoleCode"]);
            }

            if (role == "Admin")
            {
                Sb.Append("<frame name='home_content' src='HomeAdmin.aspx' frameborder='0' scrolling='no' border='0' name='BottomRowFrame' id='BottomRowFrame' >");
            }
            else if (role == "IT")
            {
                Sb.Append("<frame name='home_content' src='HomeIT.aspx' frameborder='0' scrolling='no' border='0' name='BottomRowFrame' id='BottomRowFrame' >");
            }
            else if (role == "Production")
            {
                Sb.Append("<frame name='home_content' src='HomeProd.aspx' frameborder='0' scrolling='no' border='0' name='BottomRowFrame' id='BottomRowFrame' >");
            }

            lblFrame.Text = Convert.ToString(Sb);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Info",
            "alert('" + ex + "');", true);
        }
    }
}
			
			
		
	

  

