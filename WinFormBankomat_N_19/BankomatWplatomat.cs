using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormBankomat_N_19
{
    public partial class BankomatWplatomat : Form
    {
        private int _cardID;
        private int _accountID;

        public BankomatWplatomat(int cardID)
        {
            _cardID = cardID;
            InitializeComponent();
            labelStanKonta.Text = CheckBalance();
        }

        private void BankomatWplatomat_Load(object sender, EventArgs e)
        {

        }

        public string CheckBalance()
        {
            string balance = "";

            string query = "select Balance, c.AccountID from BankAccounts b left join CreditCards c on b.AccountID = c.AccountID where c.CardID = @cardId";
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = query;
            sqlCmd.Parameters.AddWithValue("@cardId", _cardID);
            DataAccessLayer dal = new DataAccessLayer();

            if (dal.connectionOpen())
            {
                SqlDataReader reader = dal.returnReader(sqlCmd);
                if (reader.HasRows)
                {
                    reader.Read();
                    balance = reader[0].ToString();
                    _accountID = Convert.ToInt32(reader[1]);
                    reader.Close();
                }
            }

            return balance;
        }

        private void buttonWithdraw_Click(object sender, EventArgs e)
        {
            int withdrawAmount;

            if(Int32.TryParse(textBoxWithdraw.Text, out withdrawAmount))
            {
                if (withdrawAmount > Convert.ToDouble(CheckBalance()))
                {
                    label6.Text = "Nie możesz wypłacić więcej niż masz na koncie!";
                    label6.ForeColor = Color.Red;

                    return;
                }

                if (IsBankNote(withdrawAmount))
                {
                    double newBalance = Convert.ToDouble(CheckBalance()) - Convert.ToDouble(withdrawAmount);
                    string query = "update BankAccounts SET Balance = @newBalance WHERE AccountID = @id";
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandText = query;
                    sqlCmd.Parameters.AddWithValue("@newBalance", newBalance);
                    sqlCmd.Parameters.AddWithValue("@id", _accountID);

                    DataAccessLayer dal = new DataAccessLayer();
                    if(dal.connectionOpen())
                    {
                        dal.queryExecution(sqlCmd);
                        dal.connectionClose();
                    }

                    labelStanKonta.Text = CheckBalance();

                    label6.Text = "Wypłacono " + withdrawAmount;
                    label6.ForeColor = Color.Black;
                }
                else
                {
                    label6.Text = "Bankomat wypłaca jedynie nominały 500,200,100,50,20 i 10";
                    label6.ForeColor = Color.Red;
                }
            }
            else
            {
                label6.Text = "Podana kwota jest nieprawidłowa!";
                label6.ForeColor = Color.Red;
            }
        }

        private bool IsBankNote(int amount)
        {
            if(amount%10==0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void buttonDeposit_Click(object sender, EventArgs e)
        {
            int depositAmount;

            if (Int32.TryParse(textBoxDeposit.Text, out depositAmount))
            {
                if (IsBankNote(depositAmount))
                {
                    double newBalance = Convert.ToDouble(CheckBalance()) + Convert.ToDouble(depositAmount);
                    string query = "update BankAccounts SET Balance = @newBalance WHERE AccountID = @id";
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandText = query;
                    sqlCmd.Parameters.AddWithValue("@newBalance", newBalance);
                    sqlCmd.Parameters.AddWithValue("@id", _accountID);

                    DataAccessLayer dal = new DataAccessLayer();
                    if (dal.connectionOpen())
                    {
                        dal.queryExecution(sqlCmd);
                        dal.connectionClose();
                    }

                    labelStanKonta.Text = CheckBalance();

                    label7.Text = "Wpłacono " + depositAmount;
                    label7.ForeColor = Color.Black;
                }
                else
                {
                    label7.Text = "Do wpłatomatu można włożyć jedynie nominały 500,200,100,50,20 i 10";
                    label7.ForeColor = Color.Red;
                }
            }
            else
            {
                label7.Text = "Podana kwota jest nieprawidłowa!";
                label7.ForeColor = Color.Red;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Bankomat b = new Bankomat();
            b.ShowDialog();
        }
    }
}
