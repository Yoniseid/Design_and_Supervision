using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using System.Configuration;
using System.Security.Cryptography;
//using System.Web.HttpContext.Current;


namespace Design_and_Supervion_Issue_Tracking.Repository_Classes
{
    public class DataAccessprovider
    { 
        #region memberVariables

        #endregion

        #region memberMethods
        public SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["design"].ConnectionString);

        /// <summary>
        /// Method that returns DataSet(can hold multiple tables) after executing a stored procedure using select statment
        /// </summary>
        /// <param name="strConnectionString"></param>
        /// <param name="strSchema"></param>
        /// <param name="strStoredProcedureName"></param>
        /// <param name="arrListParamName"></param>
        /// <param name="arrListParamValue"></param>
        /// <param name="strErrMsg"></param>
        /// <returns></returns>

       


      /*

        protected DataSet FillData(string sSQL, string sTable)
        {


            OleDbCommand cmd = new OleDbCommand(sSQL, conn);
            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                conn.Close();
                conn.Open();
                adapter.Fill(ds, sTable);
            }
            finally
            {
                conn.Close();
            }
            return ds;
        }
        public DataSet Populate(string sSQL, string TableName)
        {
            DS = FillDataSet(DS, sSQL, TableName);
            return DS;
        }
        
        public void FilldrToGrid(DataGridView DrView, string sSql, int col)
        {
            dr = FetchRecords2(sSql);
            DrView.Rows.Clear();
            fillRecordsInDataGrid(DrView, dr, col);
            dr.Close();
        }
        public void FilldrToGrid2(DataGridView DrView, string sSql, int col, int row)
        {
            dr = FetchRecords2(sSql);
            fillRecordsInDataGrid2(DrView, dr, col, row);

            dr.Close();
        }
        public DataSet Populate1(string sSQL)
        {
            DS = FillData1(sSQL);
            return DS;
        }



    */

        public  DataSet ExecuteDataSet(string strSchema, string strStoredProcedureName, ArrayList arrListParamName, ArrayList arrListParamValue, ref string strErrMsg)
        {
           // SqlConnection connection = new SqlConnection(strConnectionString);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataSet ds = new DataSet();

            try
            {
                sqlDataAdapter.SelectCommand = new SqlCommand();
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand.CommandText = strSchema + "." + strStoredProcedureName;
                sqlDataAdapter.SelectCommand.Connection = con;
                sqlDataAdapter.SelectCommand.CommandTimeout = 0;

                for (int i = 0; i < arrListParamName.Count; i++)
                {
                    sqlDataAdapter.SelectCommand.Parameters.Add(new SqlParameter(arrListParamName[i].ToString(), arrListParamValue[i].ToString()));
                }

                con.Open();
                sqlDataAdapter.Fill(ds);
            }
            catch (Exception e)
            {
                strErrMsg = e.Message;
            }
            finally
            {
                if (con.State.ToString() == System.Data.ConnectionState.Open.ToString())
                    con.Close();

                sqlDataAdapter.Dispose();
            }

            return ds;
        }

       public List<SelectListItem> combofill(string strSchema,string strStoredProcedure, string text,string value, ref string strErrMs) 
        {
            DataTable dt = new DataTable();
            dt = ExecuteDataTable("dbo",strStoredProcedure, ref strErrMs);
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new SelectListItem
                {
                    Text = dr[text].ToString(),
                    Value = dr[value].ToString()
                });
            }

            return list;
        }
        /// <summary>
        /// Method that returns DataTabe(only one table) after executing a stored procedure using select statment
        /// </summary>
        /// <param name="strConnectionString"></param>
        /// <param name="strSchema"></param>
        /// <param name="strStoredProcedureName"></param>
        /// <param name="arrListParamName"></param>
        /// <param name="arrListParamValue"></param>
        /// <param name="strErrMsg"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable( string strSchema, string strStoredProcedureName, ArrayList arrListParamName, ArrayList arrListParamValue, ref string strErrMsg)
        {
            //SqlConnection connection = new SqlConnection(strConnectionString);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            string a, b;

            try
            {
                sqlDataAdapter.SelectCommand = new SqlCommand();
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand.CommandText = strSchema + "." + strStoredProcedureName;
                sqlDataAdapter.SelectCommand.Connection = con;
                sqlDataAdapter.SelectCommand.CommandTimeout = 0;


                for (int i = 0; i < arrListParamName.Count; i++)
                {
                    a = arrListParamName[i].ToString();
                    b = arrListParamValue[i].ToString();
                    sqlDataAdapter.SelectCommand.Parameters.Add(new SqlParameter(arrListParamName[i].ToString(), arrListParamValue[i].ToString()));
                }

                con.Open();
                sqlDataAdapter.Fill(dt);
            }
            catch (Exception e)
            {
                strErrMsg = e.Message;
                var dir = HttpContext.Current.Server.MapPath("~\\count");
                var file = Path.Combine(dir, "count.txt");

                Directory.CreateDirectory(dir);
                File.WriteAllText(file, strStoredProcedureName + "----" + strErrMsg);

            }
            finally
            {
                if (con.State.ToString() == System.Data.ConnectionState.Open.ToString())
                    con.Close();

                sqlDataAdapter.Dispose();
            }

            return dt;
        }

        /// <summary>
        /// Method that returns DataTable by accepting user defined types
        /// </summary>
        /// <param name="strConnectionString"></param>
        /// <param name="strSchema"></param>
        /// <param name="strStoredProcedureName"></param>
        /// <param name="arrListParamName"></param>
        /// <param name="arrListParamValue"></param>
        /// <param name="arrListParamNameForUserDefinedTypes"></param>
        /// <param name="lstParameterValuesForUserDefinedTypes"></param>
        /// <param name="arrListParamterTypeNameForUserDefinedTypes"></param>
        /// <param name="strErrMsg"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string strConnectionString, string strSchema, string strStoredProcedureName, ArrayList arrListParamName, ArrayList arrListParamValue, ArrayList arrListParamNameForUserDefinedTypes, List<DataTable> lstParameterValuesForUserDefinedTypes, ArrayList arrListParamterTypeNameForUserDefinedTypes, ref string strErrMsg)
        {
            
            SqlConnection connection = new SqlConnection(strConnectionString);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataTable dt = new DataTable();

            try
            {
                sqlDataAdapter.SelectCommand = new SqlCommand();
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand.CommandText = strSchema + "." + strStoredProcedureName;
                sqlDataAdapter.SelectCommand.Connection = connection;
                sqlDataAdapter.SelectCommand.CommandTimeout = 0;

                if (arrListParamName != null)
                {
                    for (int i = 0; i < arrListParamName.Count; i++)
                    {

                        sqlDataAdapter.SelectCommand.Parameters.Add(new SqlParameter(arrListParamName[i].ToString(), arrListParamValue[i].ToString()));
                    }
                }
                if (arrListParamNameForUserDefinedTypes != null)
                {
                    for (int k = 0; k < arrListParamNameForUserDefinedTypes.Count; k++)
                    {
                        SqlParameter p = new SqlParameter(arrListParamNameForUserDefinedTypes[k].ToString(), lstParameterValuesForUserDefinedTypes[k]);
                        p.SqlDbType = SqlDbType.Structured;
                        p.TypeName = arrListParamterTypeNameForUserDefinedTypes[k].ToString();
                        sqlDataAdapter.SelectCommand.Parameters.Add(p);

                    }
                }

                connection.Open();
                sqlDataAdapter.Fill(dt);
            }
            catch (Exception e)
            {
                strErrMsg = e.Message;
            }
            finally
            {
                if (connection.State.ToString() == System.Data.ConnectionState.Open.ToString())
                    connection.Close();

                sqlDataAdapter.Dispose();
            }

            return dt;
        }

        /// <summary>
        /// Method that returns DataSet by accepting user defined types
        /// </summary>
        /// <param name="strConnectionString"></param>
        /// <param name="strSchema"></param>
        /// <param name="strStoredProcedureName"></param>
        /// <param name="arrListParamName"></param>
        /// <param name="arrListParamValue"></param>
        /// <param name="arrListParamNameForUserDefinedTypes"></param>
        /// <param name="lstParameterValuesForUserDefinedTypes"></param>
        /// <param name="arrListParamterTypeNameForUserDefinedTypes"></param>
        /// <param name="strErrMsg"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string strConnectionString, string strSchema, string strStoredProcedureName, ArrayList arrListParamName, ArrayList arrListParamValue, ArrayList arrListParamNameForUserDefinedTypes, List<DataTable> lstParameterValuesForUserDefinedTypes, ArrayList arrListParamterTypeNameForUserDefinedTypes, ref string strErrMsg)
        {
            SqlConnection connection = new SqlConnection(strConnectionString);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataSet ds = new DataSet();

            try
            {
                sqlDataAdapter.SelectCommand = new SqlCommand();
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand.CommandText = strSchema + "." + strStoredProcedureName;
                sqlDataAdapter.SelectCommand.Connection = connection;
                sqlDataAdapter.SelectCommand.CommandTimeout = 0;

                if (arrListParamName != null)
                {
                    for (int i = 0; i < arrListParamName.Count; i++)
                    {

                        sqlDataAdapter.SelectCommand.Parameters.Add(new SqlParameter(arrListParamName[i].ToString(), arrListParamValue[i].ToString()));
                    }
                }
                if (arrListParamNameForUserDefinedTypes != null)
                {
                    for (int k = 0; k < arrListParamNameForUserDefinedTypes.Count; k++)
                    {
                        SqlParameter p = new SqlParameter(arrListParamNameForUserDefinedTypes[k].ToString(), lstParameterValuesForUserDefinedTypes[k]);
                        p.SqlDbType = SqlDbType.Structured;
                        p.TypeName = arrListParamterTypeNameForUserDefinedTypes[k].ToString();
                        sqlDataAdapter.SelectCommand.Parameters.Add(p);

                    }
                }

                connection.Open();
                sqlDataAdapter.Fill(ds);
            }
            catch (Exception e)
            {
                strErrMsg = e.Message;
            }
            finally
            {
                if (connection.State.ToString() == System.Data.ConnectionState.Open.ToString())
                    connection.Close();

                sqlDataAdapter.Dispose();
            }

            return ds;
        }

        /// <summary>
        /// Retrieve Non Parameter Storeprocedures
        /// </summary>
        /// <param name="strConnectionString"></param>
        /// <param name="strSchema"></param>
        /// <param name="strStoredProcedureName"></param>
        /// <param name="strErrMsg"></param>
        /// <returns></returns>
        public  DataTable ExecuteDataTable( string strSchema, string strStoredProcedureName, ref string strErrMsg)
        {
           // SqlConnection connecction = new SqlConnection(strConnectionString);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataTable dt = new DataTable();

            try
            {
                sqlDataAdapter.SelectCommand = new SqlCommand();
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand.CommandText = strSchema + "." + strStoredProcedureName;
                sqlDataAdapter.SelectCommand.Connection = con;
                sqlDataAdapter.SelectCommand.CommandTimeout = 0;


                con.Open();
                sqlDataAdapter.Fill(dt);
            }
            catch (Exception e)
            {
                strErrMsg = e.Message;
            }
            finally
            {
                if (con.State.ToString() == System.Data.ConnectionState.Open.ToString())
                    con.Close();

                sqlDataAdapter.Dispose();

            }

            return dt;
        }

        /// <summary>
        /// Method that executes stored procedure with Insert, Delete, and Update methods
        /// </summary>
        /// <param name="strConnectionString"></param>
        /// <param name="strSchema"></param>
        /// <param name="strStoredProcedureName"></param>
        /// <param name="arrListParamName"></param>
        /// <param name="arrListParamValue"></param>
        /// <param name="strErrMsg"></param>
        /// <returns></returns>
        public  bool ExecuteNonQuery( string strSchema, string strStoredProcedureName, ArrayList arrListParamName, ArrayList arrListParamValue, ref string strErrMsg)
        {
            //SqlConnection connection = new SqlConnection(strConnectionString);
            SqlCommand command = new SqlCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = strSchema + "." + strStoredProcedureName;
            command.Connection = con;

            for (int i = 0; i < arrListParamName.Count; i++)
            {
                command.Parameters.Add(new SqlParameter(arrListParamName[i].ToString(), arrListParamValue[i] == null ? "" : arrListParamValue[i].ToString()));
            }

            try
            {
                con.Open();
                command.ExecuteNonQuery();
                return true; 
            }
            catch (Exception e)
            {
                strErrMsg = e.Message;
            }
            finally
            {
                if (con.State.ToString() == System.Data.ConnectionState.Open.ToString())
                    con.Close();
            }
            return false;
        }

        /// <summary>
        /// Methods to return single value for example returning username by userid
        /// </summary>
        /// <param name="strConnectionString"></param>
        /// <param name="strSchema"></param>
        /// <param name="strStoredProcedureName"></param>
        /// <param name="arrListParamName"></param>
        /// <param name="arrListParamValue"></param>
        /// <param name="strErrMsg"></param>
        /// <returns></returns>
        public object ExecuteScalar(string strSchema, string strStoredProcedureName, ArrayList arrListParamName, ArrayList arrListParamValue, ref string strErrMsg)
        {
            //SqlConnection connection = new SqlConnection(strConnectionString);
            SqlCommand command = new SqlCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT " + strSchema + "." + strStoredProcedureName;
            command.Connection = con;

            string strParmValue = "(";

            for (int i = 0; i < arrListParamName.Count; i++)
            {
                if (i < (arrListParamName.Count - 1))
                    strParmValue = strParmValue + "'" + arrListParamValue[i] + "',";
                else
                    strParmValue = strParmValue + "'" + arrListParamValue[i] + "'";
                //command.Parameters.Add(new SqlParameter(arrListParamName[i].ToString(), arrListParamValue[i] == null ? "" : arrListParamValue[i].ToString()));
            }

            strParmValue += ")";
            command.CommandText = command.CommandText + strParmValue;
            try
            {
                con.Open();
                var returnValue = command.ExecuteScalar();
                return returnValue;
            }
            catch (Exception e)
            {
                strErrMsg = e.Message;
            }
            finally
            {
                if (con.State.ToString() == System.Data.ConnectionState.Open.ToString())
                    con.Close();
            }
            return null;
        }

        public object ExecuteScalar(string strSchema, string strStoredProcedureName, ref string strErrMsg)
        {
            //SqlConnection connection = new SqlConnection(strConnectionString);
            SqlCommand command = new SqlCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT " + strSchema + "." + strStoredProcedureName;
            command.Connection = con;

            string strParmValue = "()";

            command.CommandText = command.CommandText + strParmValue;
            try
            {
                con.Open();
                var returnValue = command.ExecuteScalar();
                return returnValue;
            }
            catch (Exception e)
            {
                strErrMsg = e.Message;
            }
            finally
            {
                if (con.State.ToString() == System.Data.ConnectionState.Open.ToString())
                    con.Close();
            }
            return null;
        }

        /// <summary>
        /// Method that accepts DataTable dt and converts it to a generic list item. For this method to work DataTable schema shall be same as List propertis.
        /// this Method calls GetItem() to interate through each column of each row 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            if (dt != null)
            {
                if (dt.Rows.Count > 0) 
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        try
                        {
                            T item = GetItem<T>(row);
                            data.Add(item);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            return data;
        }

        /// <summary>
        /// Method that accepts DataRow object and iterates through each columns in the passed in DataRow and maps each to generic type T. Note that 
        /// for this method to work DataRow columns schema shall be same as the properties of the genric type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        if (dr[column.ColumnName] != DBNull.Value)
                        {
                            pro.SetValue(obj, dr[column.ColumnName], null);
                        }
                    }
                    else
                        continue;
                }
            }
            return obj;
        }

        public static DataTable ConstructTVP(List<Guid> lst)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Guid", typeof(Guid));

            foreach (Guid g in lst)
            {
                DataRow row = dt.NewRow();
                row["Guid"] = g;
                dt.Rows.Add(row);
            }

            return dt;
        }
        public string encryption(string password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encrypt;
            UTF8Encoding encode = new UTF8Encoding();
            encrypt = md5.ComputeHash(encode.GetBytes(password));
            StringBuilder encryptdata = new StringBuilder();
            for(int i = 0; i < encrypt.Length; i++)
            {
                encryptdata.Append(encrypt[i].ToString());
            }
            return encryptdata.ToString();
        }
        public byte[] GetFile(string s)
        {
            string strErrMsg = "";

            try
            {
                System.IO.FileStream fs = System.IO.File.OpenRead(s);
                byte[] data = new byte[fs.Length];
                int br = fs.Read(data, 0, data.Length);
                if (br != fs.Length)
                    throw new System.IO.IOException(s);
                return data;
            }
            catch (Exception e)
            {
                strErrMsg = e.Message;
            }
            finally
            {
                if (con.State.ToString() == System.Data.ConnectionState.Open.ToString())
                    con.Close();
            }
            return null;
        }

        #endregion
    }
}