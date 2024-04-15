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

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        SqlConnection conn = ConnQuery.ConnectToSql();

        try
        {
            String UserID = "";
            String UserName = "";
            String UserPassword = "";
            String RoleCode = "";

            String sqlQuery = "SELECT user_id, user_password, user_name, role_code FROM dps_User WHERE user_id = '" + txtUserName.Text + "' AND user_password = '" + txtUserPassword.Text + "'";
            SqlCommand SqlCommand = new SqlCommand(sqlQuery, conn);
            SqlDataReader Dr = SqlCommand.ExecuteReader();

            while (Dr.Read())
            {
                UserID = Convert.ToString(Dr["user_id"]);
                UserName = Convert.ToString(Dr["user_name"]);
                UserPassword = Convert.ToString(Dr["user_password"]);
                RoleCode = Convert.ToString(Dr["role_code"]);
            }
            Dr.Close();

            if (UserID != "")
            {
                if (UserPassword == "")
                {
                    if (txtUserPassword.Text != "")
                    {
                        lblMsg.Text = "Invalid Username or Password.";
                        lblMsg.Visible = true;
                        txtUserPassword.Focus();
                    }
                    else
                    {
                        lblMsg.Text = "Invalid Username or Password.";
                        lblMsg.Visible = true;
                        txtUserPassword.Focus();
                    }
                }
                else
                {
                    lblUserId.Text = Convert.ToString(UserName);

                    Session["SessUserID"] = UserID.Trim();
                    Session["SessUserName"] = UserName.Trim();
                    Session["SessRoleCode"] = RoleCode.Trim();

                    Response.Redirect("MainMenu.aspx");
                }
            }
            else
            {
                if (Convert.ToString(txtUserName.Text).Trim() == "")
                {
                    txtUserName.Focus();
                }
                else
                {
                    txtUserPassword.Focus();
                }
                lblMsg.Text = "Invalid Username or Password.";
                lblMsg.Visible = true;
            }      
        }
        catch (Exception ex)
        {
            lblMsg.Text = "An error occured while attempting to connect to DPS Server.";
            lblMsg.Visible = true;
            txtUserPassword.Focus();
            throw ex;
        }
        finally
        {
            conn.Close();
            conn.Dispose();
        }  
    }
}
    
 

