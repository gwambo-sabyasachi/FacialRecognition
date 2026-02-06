using System;
using System.Collections.Generic;
 using System.Data;
using Microsoft.Data.SqlClient;

namespace FacialRecognition.Common
{
    public static class FacialRecognitionCommonSql
    {
        public static DataTable ExecuteStoredProcedure(string storedProcName,List<SqlParameter> parameters = null)
        {
            string connString = FacialRecognitionDALConstants.GetConnectionString();
            using (SqlConnection conn = new SqlConnection(connString))
            using (SqlCommand cmd = new SqlCommand(storedProcName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = FacialRecognitionDALConstants.CommandTimeOut;
                if (parameters != null && parameters.Count > 0)
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }
    }
}
