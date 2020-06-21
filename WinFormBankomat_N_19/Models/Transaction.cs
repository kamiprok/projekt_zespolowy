using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormBankomat_N_19.Models
{
    public enum OperationType
    {
        Deposit,
        Withdrawal
    }

    public class Transaction
    {
        private int TransactionID { get; set; }
        private int AccountID { get; set; }
        private OperationType Operation { get; set; }
        private double Amount { get; set; }

        public Transaction(int accountID, OperationType operation, double amount)
        {
            this.AccountID = accountID;
            this.Operation = operation;
            this.Amount = amount;
        }

        public int LogTransaction()
        {
            string query = "insert into Transactions (AccountID, Operation, Amount) values (@acc_id, @oper, @amount)";
            SqlCommand sqlCmd = new SqlCommand(query);
            sqlCmd.Parameters.AddWithValue("@acc_id", this.AccountID);
            sqlCmd.Parameters.AddWithValue("@oper", this.Operation);
            sqlCmd.Parameters.AddWithValue("@amount", this.Amount);

            DataAccessLayer dal = new DataAccessLayer();

            try
            {
                dal.connectionOpen();
                dal.queryExecution(sqlCmd);
                dal.connectionClose();

                return 0;
            }
            catch (Exception ex)
            {
                //Możliwość logowania wyjątków do pliku
                return -1;
            }
        }
    }
}
