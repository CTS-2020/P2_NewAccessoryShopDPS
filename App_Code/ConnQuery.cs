using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using Microsoft.CSharp;

public class ConnQuery
{
    #region Open Connection
    public static SqlConnection ConnectToSql()
    {
        SqlConnection sqlConn = new SqlConnection();
        string strConnSetting = ConfigurationManager.ConnectionStrings["ConnDpsSQL"].ToString();
        sqlConn.ConnectionString = strConnSetting;
        sqlConn.Open();
        return sqlConn;
    }

    public static SqlConnection ConnectToPisSql()
    {
        SqlConnection sqlConn = new SqlConnection();
        string strConnSetting = ConfigurationManager.ConnectionStrings["ConnPisSQL"].ToString();
        sqlConn.ConnectionString = strConnSetting;
        sqlConn.Open();
        return sqlConn;
    }
    #endregion

    #region Binding Dataset
    public static DataSet getBindingDatasetData(String sqlQuery)
    {
        SqlConnection sqlConn = null;
        SqlDataAdapter sqlAdapter = new SqlDataAdapter();
        DataSet DsResult = new DataSet();
        SqlCommand sqlCommand = new SqlCommand(sqlQuery);
        SqlTransaction transaction1;
        sqlConn = ConnectToSql();
        transaction1 = sqlConn.BeginTransaction();
        try
        {

            sqlCommand.Connection = sqlConn;
            sqlAdapter.SelectCommand = sqlCommand;
            transaction1.Commit();
            sqlAdapter.Fill(DsResult);

            return DsResult;
        }
        catch (Exception ex)
        {
            transaction1.Rollback();
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
        finally
        {
            sqlConn.Close();
            sqlConn.Dispose();
            sqlAdapter.Dispose();
        }
    }

    public static DataSet getPisBindingDatasetData(String sqlQuery)
    {
        SqlConnection sqlConn = null;
        SqlDataAdapter sqlAdapter = new SqlDataAdapter();
        DataSet DsResult = new DataSet();
        SqlCommand sqlCommand = new SqlCommand(sqlQuery);
        SqlTransaction transaction1 = null;
        sqlConn = ConnectToPisSql();
        transaction1 = sqlConn.BeginTransaction();
        try
        {
            sqlCommand.Connection = sqlConn;
            sqlAdapter.SelectCommand = sqlCommand;
            transaction1.Commit();
            sqlAdapter.Fill(DsResult);
            return DsResult;
        }
        catch (Exception ex)
        {
            transaction1.Rollback();
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            return null;
        }
        finally
        {
            sqlConn.Close();
            sqlConn.Dispose();
            sqlAdapter.Dispose();
        }
    }
    #endregion

    #region Check Exist Data in Database
    public static Boolean chkExistData(String sqlQuery)
    {
        SqlConnection sqlConn = null;
        SqlCommand sqlCommand = null;
        SqlDataReader sqlReader = null;

        Boolean ExistRow = false;

        sqlConn = ConnectToSql();
        sqlCommand = new SqlCommand(sqlQuery, sqlConn);
        sqlReader = sqlCommand.ExecuteReader();
        sqlReader.Read();
        if (sqlReader.HasRows)
        {
            if (Convert.ToInt32(sqlReader["CountRow"]) > 0)
            {
                ExistRow = true;
            }
        }

        sqlConn.Close();
        sqlConn.Dispose();

        return ExistRow;
    }
    #endregion

    #region get Identity Execute Reader
    public static String getIdentityExecuteReader(String sqlQuery)
    {
        SqlConnection sqlConn = null;
        SqlCommand sqlCommand = null;
        SqlDataReader sqlReader = null;
        String strNewId = "";

        sqlConn = ConnectToSql();
        sqlCommand = new SqlCommand(sqlQuery, sqlConn);
        sqlReader = sqlCommand.ExecuteReader();
        sqlReader.Read();

        if (sqlReader.HasRows)
        {
            strNewId = Convert.ToString(sqlReader.GetString(0));
        }
        else
        {
            strNewId = "";
        }
        sqlConn.Close();

        return strNewId;
    }
    #endregion

    #region get Return Field Execute Reader
    public static String getReturnFieldExecuteReader(string sqlQuery)
    {
        SqlConnection sqlConn = null;
        SqlCommand sqlCommand = null;
        SqlDataReader sqlReader = null;
        String strReturnField = "";

        sqlConn = ConnectToSql();
        sqlCommand = new SqlCommand(sqlQuery, sqlConn);
        sqlReader = sqlCommand.ExecuteReader();
        sqlReader.Read();

        if (sqlReader.HasRows)
        {
            strReturnField = Convert.ToString(sqlReader["ReturnField"]);
        }
        else
        {
            strReturnField = "";
        }
        sqlConn.Close();

        return strReturnField;
    }

    public static String getPisReturnFieldExecuteReader(string sqlQuery)
    {
        SqlConnection sqlConn = null;
        SqlCommand sqlCommand = null;
        SqlDataReader sqlReader = null;
        String strReturnField = "";

        sqlConn = ConnectToPisSql();
        sqlCommand = new SqlCommand(sqlQuery, sqlConn);
        sqlReader = sqlCommand.ExecuteReader();
        sqlReader.Read();

        if (sqlReader.HasRows)
        {
            strReturnField = Convert.ToString(sqlReader["ReturnField"]);
        }
        else
        {
            strReturnField = "";
        }
        sqlConn.Close();

        return strReturnField;
    }
    #endregion

    #region Execute Query
    public static Boolean ExecuteQuery(string sqlQuery)
    {
        SqlConnection sqlConn = null;
        SqlCommand sqlCommand = null;
        Boolean ExecuteResult = false;
        SqlTransaction transaction1;
        sqlConn = ConnectToSql();
        transaction1 = sqlConn.BeginTransaction();
        try
        {

            sqlCommand = new SqlCommand(sqlQuery, sqlConn);
            transaction1.Commit();
            if (sqlCommand.ExecuteNonQuery() >= 1)
            {
                ExecuteResult = true;
            }
            else
            {
                ExecuteResult = false;
            }

            sqlConn.Close();
            return ExecuteResult;
        }
        catch (Exception ex)
        {
            transaction1.Rollback();
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString(ex.Message) + " " + Convert.ToString(ex.TargetSite));
            ExecuteResult = false;
        }
        finally
        {
            try
            {
                sqlConn.Close();
                sqlConn.Dispose();
                sqlCommand.Dispose();
            }
            catch { }

        }

        return ExecuteResult;
    }

    public static string SqlCommandToString(SqlCommand command)
    {
        // Start with the CommandText (the SQL query or command)
        string query = command.CommandText;

        // Iterate over the parameters and replace their placeholders in the query
        foreach (SqlParameter param in command.Parameters)
        {
            string paramValue = param.Value != null ? param.Value.ToString() : "NULL";

            // Depending on the type of parameter, add quotes for string types or leave as is for numbers
            if (param.SqlDbType == SqlDbType.VarChar || param.SqlDbType == SqlDbType.NVarChar || param.SqlDbType == SqlDbType.Char || param.SqlDbType == SqlDbType.Text)
            {
                paramValue = String.Format("'{0}'", paramValue); // Use String.Format for string types
            }

            // Replace the parameter name with its actual value in the query
            query = query.Replace(param.ParameterName, paramValue);
        }

        return query;
    }


    public static Boolean ExecuteTransSaveQuery(List<String> sqlQuery)
    {
        SqlConnection sqlConnDps = null;
        SqlCommand sqlCmd = null;
        SqlTransaction sqlTransDps = null;
        Boolean ExecuteResult = false;
        GlobalFunc.Log("can run ExecuteTransSaveQuery");
        try
        {
            GlobalFunc.Log("can run ExecuteTransSaveQuery, in try block");
            sqlConnDps = ConnectToSql();
            sqlTransDps = sqlConnDps.BeginTransaction();
            GlobalFunc.Log("after connection.");
            foreach (String element in sqlQuery)
            {
                GlobalFunc.Log("in looping");
                sqlCmd = null;
                sqlCmd = new SqlCommand(element, sqlConnDps, sqlTransDps);
                GlobalFunc.Log("sqlCmd: " + SqlCommandToString(sqlCmd));
                if (sqlCmd.ExecuteNonQuery() >= 1)
                {
                    GlobalFunc.Log("execute successfully");
                    ExecuteResult = true;
                    GlobalFunc.Log(element);
                }
                else
                {
                    try
                    {
                        GlobalFunc.Log("execute failed and rollback.");
                        ExecuteResult = false;
                        sqlTransDps.Rollback();
                        GlobalFunc.Log("after rollback.");
                    }
                    catch (Exception ex)
                    {
                        GlobalFunc.Log(ex);
                        GlobalFunc.ShowErrorMessage(Convert.ToString("Database timeout. Records not able to update"));
                        break;
                    }


                }
            }

            if (ExecuteResult == true)
            {
                GlobalFunc.Log("try to commit");
                sqlTransDps.Commit();
                GlobalFunc.Log("committed");
            }

            sqlConnDps.Close();

            return ExecuteResult;
        }
        catch (Exception ex)
        {
            GlobalFunc.Log(ex);
            GlobalFunc.ShowErrorMessage(Convert.ToString("Database timeout. Records not able to update"));
            ExecuteResult = false;
            if (sqlTransDps != null) sqlTransDps.Rollback();
        }
        finally
        {
            try
            {
                sqlConnDps.Close();
                sqlConnDps.Dispose();
                sqlCmd.Dispose();
                sqlTransDps.Dispose();
            }
            catch { }
        }

        return ExecuteResult;
    }

    public static Boolean ExecuteTransQuery(List<String> sqlQuery)
    {
        SqlConnection sqlConnDps = null;
        SqlCommand sqlCmd = null;
        SqlTransaction sqlTransDps = null;
        Boolean ExecuteResult = false;


        try
        {
            sqlConnDps = ConnectToSql();
            sqlTransDps = sqlConnDps.BeginTransaction();

            foreach (String element in sqlQuery)
            {
                sqlCmd = null;
                sqlCmd = new SqlCommand(element, sqlConnDps, sqlTransDps);

                if (sqlCmd.ExecuteNonQuery() >= 1)
                {
                    ExecuteResult = true;
                }
            }

            if (ExecuteResult == true)
            {
                sqlTransDps.Commit();
            }
            else
            {
                sqlTransDps.Rollback();
            }

            sqlConnDps.Close();

            return ExecuteResult;
        }
        catch (Exception ex)
        {

            GlobalFunc.Log(ex);
            ExecuteResult = false;
            //if (sqlTransDps != null) sqlTransDps.Rollback();
            //modify by ng 21/07/2016
            sqlTransDps.Rollback();
        }
        finally
        {
            try
            {
                sqlConnDps.Close();
                sqlConnDps.Dispose();
                sqlCmd.Dispose();
                sqlTransDps.Dispose();
            }
            catch { }
        }

        return ExecuteResult;
    }
    #endregion
}

