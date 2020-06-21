using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormBankomat_N_19.Models
{
    public class BankAccount
    {
        public int AccountID { get; private set; }
        public int CustomerID { get; private set; }
        public string AccountNo { get; set; }
        public double Balance { get; private set; }

        public virtual Customer Customer { get; set; }

        public static int WithdrawMoney(int amount, int accountId, double currentBalance)
        {
            if (amount > currentBalance) { return -1; }
            else if (amount < 0) { return -1; }

            double newBalance = currentBalance - amount;
            string query = "update BankAccounts SET Balance = @newBalance WHERE AccountID = @id";
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = query;
            sqlCmd.Parameters.AddWithValue("@newBalance", newBalance);
            sqlCmd.Parameters.AddWithValue("@id", accountId);

            try
            {
                DataAccessLayer dal = new DataAccessLayer();

                if (dal.connectionOpen())
                {
                    dal.queryExecution(sqlCmd);
                    dal.connectionClose();
                }
                Transaction transaction = new Transaction(accountId, OperationType.Withdrawal, amount);
                transaction.LogTransaction();

                return 0;
            }
            catch(Exception ex)
            {
                return -1;
            }
        } 

        public static int DepositMoney(int amount, int accountId, double currentBalance)
        {
            if (amount < 0) { return -1; }

            double newBalance = currentBalance + amount;
            string query = "update BankAccounts SET Balance = @newBalance WHERE AccountID = @id";
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = query;
            sqlCmd.Parameters.AddWithValue("@newBalance", newBalance);
            sqlCmd.Parameters.AddWithValue("@id", accountId);

            DataAccessLayer dal = new DataAccessLayer();
            try
            {
                if (dal.connectionOpen())
                {
                    dal.queryExecution(sqlCmd);
                    dal.connectionClose();
                }
                Transaction transaction = new Transaction(accountId, OperationType.Deposit, amount);
                transaction.LogTransaction();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 0;
        }

        public double CheckBalance()
        {
            return Balance;
        }

        public int GetAccountID(long pesel)
        {
            string query = "select b.AccountID, b.Balance from BankAccounts b left join Customers c on b.CustomerID = c.CustomerID where c.PersonalID = @pesel";
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
            Customer Customer = new Customer();
            if (dal.connectionOpen())
            {
                SqlDataReader reader = dal.returnReader(sqlCommand);
                if (reader.HasRows)
                {
                   
                    reader.Read();
                    Customer.Name= reader[0].ToString();
                    Customer.Surname = reader[1].ToString();
                    Customer.PersonalID = Convert.ToInt64(reader[2].ToString());
                    AccountNo = reader[3].ToString();
                    Balance = Convert.ToDouble(reader[4].ToString());
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
