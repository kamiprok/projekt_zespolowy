using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormBankomat_N_19
{
    class BankAccount
    {
        public int AccountID { get; private set; }
        public int CustomerID { get; private set; }
        public string AccountNo { get; set; }
        public double Balance { get; private set; }
        public Customer Customer { get; set; }

        public void WithdrawMoney(int amount)
        {

        }

        public void DepositMoney(int amount)
        {

        }

        public double CheckBalance()
        {
            return Balance;
        }

        public int GetAccountID(long pesel)
        {
            string query = "select AccountID from BankAccounts b left join Customers c on c.CustomerID = b.CustomerID where c.PersonalID = @pesel";
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
                    this.AccountID = Convert.ToInt32(reader[0].ToString());
                    reader.Close();
                    dal.connectionClose();
                    return this.AccountID;
                }
                else
                {
                    dal.connectionClose();
                    return -1; // brak konta w bazie banku
                }
            }
            else return -2; // brak połączenia z bazą danych
        }

        public int GetAccountInfo(long pesel)
        {
            string query = "select c.Name, c.Surname, c.PersonalID, AccountNo, Balance from BankAccounts b left join Customers c on c.CustomerID = b.CustomerID where c.PersonalID = @pesel";
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = query;
            sqlCommand.Parameters.AddWithValue("@pesel", pesel);
            DataAccessLayer dal = new DataAccessLayer();

            if (dal.connectionOpen())
            {
                SqlDataReader reader = dal.returnReader(sqlCommand);
                if (reader.HasRows)
                {
                    reader.Read();
                    this.Customer.Name = reader[0].ToString();
                    this.Customer.Surname = reader[1].ToString();
                    this.Customer.PersonalID = Convert.ToInt64(reader[2].ToString());
                    this.AccountNo = reader[3].ToString();
                    this.Balance = Convert.ToDouble(reader[4].ToString());
                    reader.Close();
                    dal.connectionClose();
                    return 1; // konto zostało znalezione
                }
                else
                {
                    dal.connectionClose();
                    return -1; // konto nie zostało znalezione
                }
            }
            else
            {
                return -2; // brak połączenia z bazą danych
            }
        }

        public void SetBankAccount(string[] accountTable)
        {
            this.CustomerID = Convert.ToInt32(accountTable[0]);
            this.AccountNo = accountTable[1];
            this.Balance = Convert.ToDouble(accountTable[2]);
        }

        public string AddAccount()
        {
            string query = "insert into BankAccounts (CustomerID, AccountNo, Balance) values (@customerID, @accountNo, @balance)";
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = query;
            sqlCommand.Parameters.AddWithValue("@customerID", this.CustomerID);
            sqlCommand.Parameters.AddWithValue("@accountNo", this.AccountNo);
            sqlCommand.Parameters.AddWithValue("@balance", this.Balance);

            DataAccessLayer dal = new DataAccessLayer();

            try
            {
                dal.connectionOpen();
                dal.queryExecution(sqlCommand);
                dal.connectionClose();

                return "Konto bankowe zostało dodane.";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
