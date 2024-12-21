using Microsoft.Data.SqlClient;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Clarion.Ecom.API.DAL
{
    public class DataAccess
    {
        #region "Declaration And Initialization."

        SqlConnection con = null;
        SqlCommand comm = null;
        SqlDataAdapter adap = null;

        public DataAccess(string connectionString = "")
        {
            //
            // TODO: Add constructor logic here
            //
            try
            {
                if (connectionString.Trim().Length == 0)
                {
                    //////connectionString = new AppConfiguration().ConnectionString;
                }
                if (connectionString == null || connectionString.Trim().Length == 0)
                {
                    connectionString = @"Data Source=DESKTOP-K1MMPU0\SQLEXPRESS;Initial Catalog=ECOM;User ID=sa;password=sasa;trustServerCertificate=True";
                }
                con = new SqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (comm != null)
                    {
                        if (comm.Connection.State == ConnectionState.Open)
                            comm.Connection.Close();
                        comm.Dispose();
                    }
                    if (adap != null)
                        adap.Dispose();
                    if (con != null)
                        con.Dispose();
                }
                GC.Collect();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

        #region "ExecuteReader."
        /// <summary>
        /// This method is used to get dataset using stored procedure
        /// </summary>
        /// <param name="spName">Stored procedure</param>
        /// <returns>dataset</returns>
        public DataSet ExecuteReaderDS(string spName)
        {
            DataSet ds = new DataSet();
            try
            {
                comm = new SqlCommand(spName, con);
                comm.CommandType = CommandType.StoredProcedure;
                adap = new SqlDataAdapter(comm);
                adap.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
                ds.Dispose();
            }
        }
        /// <summary>
        /// This method is used to get dataset using stored procedure
        /// </summary>
        /// <param name="spName">Stored procedure</param>
        /// <param name="param">Parameters to be passed as array</param>
        /// <returns>dataset</returns>
        public DataSet ExecuteReaderDS(string spName, SqlParameter[] param)
        {
            DataSet ds = new DataSet();
            try
            {
                comm = new SqlCommand(spName, con);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddRange(param);
                adap = new SqlDataAdapter(comm);
                adap.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
                ds.Dispose();
            }

        }
        /// <summary>
        /// This method is used to get dataset using select stored procedure
        /// </summary>
        /// <param name="spName">stored procedure</param>
        /// <param name="param">Parameters to be passed as array</param>
        /// <param name="outputParams">Parameters got as output from query</param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteReaderDS(string spName, SqlParameter[] param, ref string[] outputParams)
        {
            DataSet ds = new DataSet();
            int i = 0;
            try
            {
                comm = new SqlCommand(spName, con);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddRange(param);
                adap = new SqlDataAdapter(comm);
                adap.Fill(ds);
                foreach (SqlParameter p in param)
                {
                    if (p.Direction.Equals(ParameterDirection.Output))
                    {
                        outputParams[i] = p.Value.ToString();
                        i++;
                    }
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
                ds.Dispose();
            }
        }
        /// <summary>
        /// Gets both dataset and return value from database. 
        /// </summary>
        /// <param name="spName">Stored procedure</param>
        /// <param name="param">Parameters to be passed as array</param>
        /// <param name="returnValue">Return value</param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteReaderDS(string spName, SqlParameter[] param, ref int returnValue)
        {
            DataSet ds = new DataSet();
            SqlParameter prmReturn = null;
            try
            {
                comm = new SqlCommand(spName, con);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddRange(param);

                prmReturn = new SqlParameter("RETURN_VALUE", SqlDbType.Int);
                prmReturn.Direction = ParameterDirection.ReturnValue;
                comm.Parameters.Add(prmReturn);

                adap = new SqlDataAdapter(comm);
                adap.Fill(ds);

                returnValue = Convert.ToInt32(comm.Parameters["RETURN_VALUE"].Value);

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
                ds.Dispose();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="spName">Stored procedure</param>
        /// <param name="param">Parameters to be passed as array</param>
        /// <param name="outputParams">Parameters got as output from query</param>
        /// <param name="returnValue">Return value from db</param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteReaderDS(string spName, SqlParameter[] param, ref string[] outputParams, ref int returnValue)
        {
            DataSet ds = new DataSet();
            SqlParameter prmReturn = null;
            int i = 0;
            try
            {
                comm = new SqlCommand(spName, con);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddRange(param);

                prmReturn = new SqlParameter("RETURN_VALUE", SqlDbType.Int);
                prmReturn.Direction = ParameterDirection.ReturnValue;
                comm.Parameters.Add(prmReturn);

                adap = new SqlDataAdapter(comm);
                adap.Fill(ds);
                foreach (SqlParameter p in param)
                {
                    if (p.Direction.Equals(ParameterDirection.Output))
                    {
                        outputParams[i] = p.Value.ToString();
                        i++;
                    }
                }

                returnValue = Convert.ToInt32(comm.Parameters["RETURN_VALUE"].Value);

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
                ds.Dispose();
            }
        }
        /// <summary>
        /// This method is used to get DataTable using stored procedure
        /// </summary>
        /// <param name="spName">Stored procedure</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteReader(string spName)
        {
            DataTable dt = new DataTable();
            try
            {
                comm = new SqlCommand(spName, con);
                comm.CommandType = CommandType.StoredProcedure;
                adap = new SqlDataAdapter(comm);
                adap.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
                dt.Dispose();
            }
        }
        /// <summary>
        /// This method is used to get DataTable using stored procedure
        /// </summary>
        /// <param name="spName">Stored procedure</param>
        /// <param name="param">Parameters to be passed as array</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteReader(string spName, SqlParameter[] param)
        {
            DataTable dt = new DataTable();
            try
            {
                comm = new SqlCommand(spName, con);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddRange(param);
                adap = new SqlDataAdapter(comm);
                adap.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
                dt.Dispose();
            }
        }
        /// <summary>
        /// This method is used to get datatable using select stored procedure
        /// </summary>
        /// <param name="spName">stored procedure</param>
        /// <param name="param">Parameters to be passed as array</param>
        /// <param name="outputParams">Parameters got as output from query</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteReader(string spName, SqlParameter[] param, ref string[] outputParams)
        {
            DataTable dt = new DataTable();
            int i = 0;
            try
            {
                comm = new SqlCommand(spName, con);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddRange(param);
                adap = new SqlDataAdapter(comm);
                adap.Fill(dt);
                foreach (SqlParameter p in param)
                {
                    if (p.Direction.Equals(ParameterDirection.Output))
                    {
                        outputParams[i] = p.Value.ToString();
                        i++;
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
                dt.Dispose();
            }
        }
        /// <summary>
        /// This method is used to get datatable using select stored procedure
        /// </summary>
        /// <param name="spName">stored procedure</param>
        /// <param name="param">Parameters to be passed as array</param>
        /// <param name="outputParams">Parameters got as output from query</param>
        /// <param name="returnValue">Return value from db</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteReader(string spName, SqlParameter[] param, ref string[] outputParams, ref int returnValue)
        {
            DataTable dt = new DataTable();
            SqlParameter prmReturn = null;
            int i = 0;
            try
            {
                comm = new SqlCommand(spName, con);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddRange(param);

                prmReturn = new SqlParameter("RETURN_VALUE", SqlDbType.Int);
                prmReturn.Direction = ParameterDirection.ReturnValue;
                comm.Parameters.Add(prmReturn);

                adap = new SqlDataAdapter(comm);
                adap.Fill(dt);
                foreach (SqlParameter p in param)
                {
                    if (p.Direction.Equals(ParameterDirection.Output))
                    {
                        outputParams[i] = p.Value.ToString();
                        i++;
                    }
                }

                returnValue = Convert.ToInt32(comm.Parameters["RETURN_VALUE"].Value);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
                dt.Dispose();
            }
        }
        /// <summary>
        /// This method is used to get datable and return value from database
        /// </summary>
        /// <param name="spName">Stored Procedure</param>
        /// <param name="param">Parameters to be passed as array</param>
        /// <param name="returnValue">Return value from db</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteReader(string spName, SqlParameter[] param, ref int returnValue)
        {
            DataTable dt = new DataTable();
            SqlParameter prmReturn = null;
            try
            {
                comm = new SqlCommand(spName, con);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddRange(param);

                prmReturn = new SqlParameter("RETURN_VALUE", SqlDbType.Int);
                prmReturn.Direction = ParameterDirection.ReturnValue;
                comm.Parameters.Add(prmReturn);

                adap = new SqlDataAdapter(comm);
                adap.Fill(dt);

                returnValue = Convert.ToInt32(comm.Parameters["RETURN_VALUE"].Value);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
                dt.Dispose();
            }
        }

        #endregion


        #region "ExecuteNonQuery"
        /// <summary>
        /// This method is used to execute insert / update db using stored procedure
        /// </summary>
        /// <param name="spName">Stored procedure</param>
        /// <param name="param">Parameters to be passed as array</param>
        /// <returns>insert or update pass/fail</returns>
        public bool ExecuteNonQuery(string spName, SqlParameter[] param)
        {
            bool res = false;
            try
            {
                comm = new SqlCommand(spName, con);
                comm.Connection.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddRange(param);
                int x = comm.ExecuteNonQuery();
                res = (x >= 1 ? true : false);
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }
        /// <summary>
        /// This method is used to execute insert / update db using stored procedure
        /// </summary>
        /// <param name="spName">Stored procedure</param>
        /// <param name="param">Parameters to be passed as array</param>
        /// <param name="outputParams">Output parameters to be passed as array</param>
        /// <returns>insert or update pass/fail</returns>
        public bool ExecuteNonQuery(string spName, SqlParameter[] param, ref string[] outputParams)
        {
            bool res = false;
            int i = 0;
            try
            {
                comm = new SqlCommand(spName, con);
                comm.Connection.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddRange(param);
                int x = comm.ExecuteNonQuery();
                res = (x >= 1 ? true : false);

                foreach (SqlParameter p in param)
                {
                    if (p.Direction.Equals(ParameterDirection.Output))
                    {
                        outputParams[i] = p.Value.ToString();
                        i++;
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public bool ExecuteNonQuery(string spName, SqlParameter[] param, ref int returnValue)
        {
            bool res = false;
            SqlParameter prmReturn = null;
            try
            {
                comm = new SqlCommand(spName, con);
                comm.Connection.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddRange(param);

                prmReturn = new SqlParameter("RETURN_VALUE", SqlDbType.Int);
                prmReturn.Direction = ParameterDirection.ReturnValue;
                comm.Parameters.Add(prmReturn);

                int x = comm.ExecuteNonQuery();
                res = (x >= 1 ? true : false);

                returnValue = Convert.ToInt32(comm.Parameters["RETURN_VALUE"].Value);
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }
        /// <summary>
        /// This method is used to execute insert / update / check existance db using stored procedure
        /// </summary>
        /// <param name="spName">Stored Procedure</param>
        /// <param name="param">Parameters to be passed as array</param>
        /// <returns>int - (Return value from db.)</returns>
        public int ExecuteNonQueryReturn(string spName, SqlParameter[] param)
        {
            SqlParameter prmReturn = null;
            try
            {
                comm = new SqlCommand(spName, con);
                comm.Connection.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddRange(param);

                prmReturn = new SqlParameter("RETURN_VALUE", SqlDbType.Int);
                prmReturn.Direction = ParameterDirection.ReturnValue;
                comm.Parameters.Add(prmReturn);

                int x = comm.ExecuteNonQuery();

                int returnValue = Convert.ToInt32(comm.Parameters["RETURN_VALUE"].Value);
                return returnValue;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }


        #endregion

        #region "ExecuteScalar"
        /// <summary>
        /// This method is used to get the single value from DB
        /// </summary>
        /// <param name="spName">Stored procedure</param>
        /// <returns>object</returns>
        public object ExecuteScalar(string spName)
        {
            object x = null;
            try
            {
                comm = new SqlCommand(spName, con);
                comm.Connection.Open();
                comm.CommandType = CommandType.StoredProcedure;
                x = comm.ExecuteScalar();
                return x;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }
        /// <summary>
        /// This method is used to get the single value from DB
        /// </summary>
        /// <param name="spName">Stored procedure</param>
        /// <param name="param">Parameters to be passed as array</param>
        /// <returns>object</returns>
        public object ExecuteScalar(string spName, SqlParameter[] param)
        {
            object x = null;
            try
            {
                comm = new SqlCommand(spName, con);
                comm.Connection.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddRange(param);
                x = comm.ExecuteScalar();
                return x;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        #endregion

        #region "DataReader"
        /// <summary>
        /// This method is used to get datareader using stored procedure
        /// </summary>
        /// <param name="spName">Stored procedure</param>
        /// <returns>sql data reader</returns>
        public SqlDataReader ExecuteDataReader(string spName)
        {
            SqlDataReader dr = null;
            try
            {
                comm = new SqlCommand(spName, con);
                comm.Connection.Open();
                comm.CommandType = CommandType.StoredProcedure;
                dr = comm.ExecuteReader();
                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
                dr.Dispose();
            }
        }
        /// <summary>
        /// This method is used to get datareader using stored procedure
        /// </summary>
        /// <param name="spName">Stored procedure</param>
        /// <param name="param">Parameters to be passed as array</param>
        /// <returns>sql data reader</returns>
        public SqlDataReader ExecuteDataReader(string spName, SqlParameter[] param)
        {
            SqlDataReader dr = null;
            try
            {
                comm = new SqlCommand(spName, con);
                comm.Connection.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddRange(param);
                dr = comm.ExecuteReader();
                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
                dr.Dispose();
            }
        }
        #endregion



        //public async Task<DataTable> ExecuteReaderAsync(string storedProcedureName, SqlParameter[] parameters)
        //{

        //        comm.CommandType = CommandType.StoredProcedure;

        //        if (parameters != null)
        //        {
        //            comm.Parameters.AddRange(parameters);
        //        }

        //        await con.OpenAsync(); // Asynchronously open the connection

        //        using (SqlDataReader reader = await comm.ExecuteReaderAsync()) // Asynchronously execute reader
        //        {
        //            DataTable dataTable = new DataTable();
        //            dataTable.Load(reader); // Load data into DataTable
        //            return dataTable;
        //        }
            
        //}

        //    public async Task<DataTable> ExecuteReaderAsync(string spName)
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        using (SqlCommand comm = new SqlCommand(spName, con))
        //        {
        //            comm.CommandType = CommandType.StoredProcedure;

        //            // Open connection asynchronously
        //            if (con.State != ConnectionState.Open)
        //                await con.OpenAsync();

        //            using (SqlDataReader reader = await comm.ExecuteReaderAsync())
        //            {
        //                dt.Load(reader);
        //            }
        //        }
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex; // Consider using a more detailed exception handling mechanism
        //    }
        //    finally
        //    {
        //        if (con.State == ConnectionState.Open)
        //            await con.CloseAsync();
        //        dt.Dispose();
        //    }
        //}



    }
}
