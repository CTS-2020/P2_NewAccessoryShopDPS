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
using System.IO;
using System.Diagnostics;
using dpant;

public partial class HomeProd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnDpsMaint_Click(object sender, EventArgs e)
    {
        Response.Redirect("./DpsMaint/DpsMaintFrame.aspx");
    }

    protected void btnDpsMaster_Click(object sender, EventArgs e)
    {
        Response.Redirect("./DpsMaster/DpsMasterFrame.aspx");
    }
}
