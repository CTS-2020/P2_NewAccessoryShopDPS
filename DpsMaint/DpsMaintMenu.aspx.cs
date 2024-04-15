using System;
using System.Data;
using System.Data.SqlClient;
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

public partial class DpsMaintMenu : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        String role = Convert.ToString(Session["SessRoleCode"]);
        StringBuilder Sb = new StringBuilder();
        Int32 i = 1;

        try
        {
            #region SQL
            SqlConnection objConn = ConnQuery.ConnectToSql();
            String strSQL = "SELECT * FROM dps_MenuMain";

            if (role == "Admin")
            {
                strSQL = strSQL + " WHERE Menu_Code = 'MAE' AND Menu_PrgChk = '1' ORDER BY Menu_PrgSeq";
            }
            else
            {
                strSQL = strSQL + " WHERE Role_Code LIKE '%" + role + "%' AND Menu_Code = 'MAE' AND Menu_PrgChk = '1' ORDER BY Menu_PrgSeq";
            }

            SqlCommand objCmd = objConn.CreateCommand();
            objCmd.CommandText = strSQL;
            SqlDataReader Dr = objCmd.ExecuteReader();
            if (Dr.HasRows)
            {
                String MenuCode = "";
                String MenuDesc = "";
                String MenuSeq = "";

                String SubMenuId = "";
                String SubMenuDesc = "";
                while (Dr.Read())
                {
                    MenuCode = Dr["Menu_Code"].ToString();
                    MenuDesc = Dr["Menu_Desc"].ToString();
                    MenuSeq = Dr["Menu_PrgSeq"].ToString();

                    Sb.Append("<div id='masterdiv'>");
                    Sb.Append("<div>");
                    Sb.Append("<table class='menuheader' width='100%' cellpadding='0' cellspacing='0'>");
                    Sb.Append("<tr>");
                    Sb.Append("<td></td>");
                    Sb.Append("<td><span class='menuhlink'>" + MenuDesc + "</span></td>");
                    Sb.Append("</tr>");
                    Sb.Append("</table>");

                    Sb.Append("<div>");
                    Sb.Append("<table width='100%' cellpadding='0' cellspacing='0'>");
                    Sb.Append("<tr>");
                    Sb.Append("<td class=menucolor>");
                    Sb.Append("<table width='100%' cellpadding='1' cellspacing='1'>");

                    SqlConnection objConn2 = ConnQuery.ConnectToSql();
                    String strSQL2 = "SELECT * FROM dps_MenuSub";

                    if (role == "Admin")
                    {
                        strSQL2 = strSQL2 + " WHERE Menu_Code='" + MenuCode + "' AND SubMenu_PrgChk = '1' ORDER BY SubMenu_PrgSeq";
                    }
                    else
                    {
                        strSQL2 = strSQL2 + " WHERE Role_Code LIKE '%" + role + "%' AND Menu_Code='" + MenuCode + "' AND SubMenu_PrgChk = '1' ORDER BY SubMenu_PrgSeq";
                    }

                    SqlCommand objCmd2 = objConn2.CreateCommand();
                    objCmd2.CommandText = strSQL2;
                    SqlDataReader Dr2 = objCmd2.ExecuteReader();
                    if (Dr2.HasRows)
                    {
                        while (Dr2.Read())
                        {
                            SubMenuDesc = Convert.ToString(Dr2["SubMenu_Desc"]);
                            SubMenuId = Convert.ToString(Dr2["SubMenu_Prgid"]);

                            Sb.Append("<tr>");
                            Sb.Append("<td class='displayBlock'><a class='menulink2' href='" + SubMenuId + "'target=BottomRowFrame>");

                            Sb.Append(SubMenuDesc + "</a></td>");
                            Sb.Append("</tr>");
                        }
                    }
                    Dr2.Close();
                    objConn2.Close();

                    Sb.Append("</table>");
                    Sb.Append("</td>");
                    Sb.Append("</tr>");
                    Sb.Append("</table>");
                    Sb.Append("</div>");
                    i = i + 1;
                }
            }
            Dr.Close();
            objConn.Close();

            Sb.Append("</div>");

            #endregion

        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex.Message);
        }
        
        lblMenu.Text = Convert.ToString(Sb);
    }
}
			
			
		
	

  

