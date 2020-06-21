using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace WinFormBankomat_N_19.Models
{
    public class Customer
    {
        public int CustomerID { get; private set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public long PhoneNo { get; private set; }
        public string Address { get; private set; }
        public long PersonalID { get; set; }

        public int getCustomerID(long pesel)
        {
            string query = "select CustomerID from Customers where PersonalID = @pesel";
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = query;
            sqlCommand.Parameters.AddWithValue("@pesel", pesel);
            DataAccessLayer dal = new DataAccessLayer();

            if (dal.connectionOpen())
            {
                SqlDataReader reader = dal.returnReader(sqlCommand);
                if (reader.HasRows)
                {
                    reader.Read(); // czyta 1 wiersz
                    this.CustomerID = Convert.ToInt32(reader[0].ToString());
                    reader.Close();
                    dal.connectionClose();
                    return this.CustomerID;
                }
                else
                {
                    dal.connectionClose();
                    return -1; // brak klienta w bazie banku
                }
            }
            else return -2; // brak połączenia z bazą danych

        }

        public int getCustomerInfo(long pesel)
        {
            string query = "select Name, Surname, PhoneNo, Address, PersonalID from Customers where PersonalID = @pesel";
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = query;
            sqlCommand.Parameters.AddWithValue("@pesel", pesel);
            DataAccessLayer dal = new DataAccessLayer();

            if(dal.connectionOpen())
            {
              SqlDataReader reader =   dal.returnReader(sqlCommand);
                if(reader.HasRows)
                {
                    reader.Read();
                    this.Name = reader[0].ToString();
                    this.Surname = reader[1].ToString();
                    this.PhoneNo = Convert.ToInt64(reader[2].ToString());
                    this.Address = reader[3].ToString();
                    this.PersonalID= Convert.ToInt64(reader[4].ToString());
                    reader.Close();
                    dal.connectionClose();
                    return 1; // klient został znaleziony
                }
                else
                {
                    dal.connectionClose();
                    return -1; // klient nie został znaleziony
                }
            }
            else
            {
                return -2; // brak połączenia z bazą danych
            }

        }

        public void setCustomer(string[] customerTable)
        {
            this.Name = customerTable[0];
            this.Surname = customerTable[1];
            this.PhoneNo = Convert.ToInt64(customerTable[2]);
            this.Address = customerTable[3];
            this.PersonalID = Convert.ToInt64(customerTable[4]);
        }

        public string addCustomer()
        {
            string query = "insert into Customers values(@name,@surname,@phone,@address,@pesel)";
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = query;
            sqlCommand.Parameters.AddWithValue("@name", this.Name);
            sqlCommand.Parameters.AddWithValue("@surname", this.Surname);
            sqlCommand.Parameters.AddWithValue("@phone", this.PhoneNo);
            sqlCommand.Parameters.AddWithValue("@address", this.Address);
            sqlCommand.Parameters.AddWithValue("@pesel", this.PersonalID);
            DataAccessLayer dal = new DataAccessLayer();

            try
            {
                dal.connectionOpen();
                dal.queryExecution(sqlCommand);
                dal.connectionClose();

                return "new customer inserted";
            }
            catch(Exception ex)
            {
              return  ex.ToString();
            }

            
        }

    }
}
