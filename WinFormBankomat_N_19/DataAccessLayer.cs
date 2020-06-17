using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace WinFormBankomat_N_19
{
    class DataAccessLayer
    {
        SqlConnection sqlConnection =
            new SqlConnection(ConfigurationManager.ConnectionStrings["abcd"].ConnectionString);

        public DataTable selectData(SqlCommand sqlCommand)
        {
            //obsługa zapytań typu select 
            // otwieranie i zamykanie połączenia z bazą danych wykonuje obiekt sda typy SqlDataAdapter
            sqlCommand.Connection = sqlConnection;
            SqlDataAdapter sda = new SqlDataAdapter(sqlCommand);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public SqlDataReader returnReader(SqlCommand sqlCommand)
        {
            //obsługa zapytań select, zwraca dane w postaci strumienia wierszy 
            // wymagane jest otwarte połaczenie z bazą danych
            sqlCommand.Connection = sqlConnection;
            SqlDataReader reader = sqlCommand.ExecuteReader();
            return reader;
        }

        public SqlConnection getConn()
        {
            return sqlConnection;
        }

        public void queryExecution(SqlCommand sqlCommand)
        {
            // wymagane jest otwarte połączenie z bazą danych
            // obsługa zapytań tyypu insert, update i delete
            sqlCommand.Connection = sqlConnection;
            sqlCommand.ExecuteNonQuery();
        }

        public bool connectionOpen()
        {
            try
            {
                sqlConnection.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool connectionClose()
        {
            try
            {
                sqlConnection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
