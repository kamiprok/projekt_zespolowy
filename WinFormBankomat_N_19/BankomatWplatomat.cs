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
using WinFormBankomat_N_19.Models;

namespace WinFormBankomat_N_19
{
    public partial class BankomatWplatomat : Form
    {
        private int _cardID;
        private int _accountID;

        private static string WRONG_NOMINAL = "Bankomat obsługuje jedynie nominały 500,200,100,50,20 i 10";
        private static string WRONG_AMOUNT = "Podana kwota jest nieprawidłowa!";
        private static string OPERATION_ERROR = "Wystąpił błąd podczas realizacji transakcji, spróbuj ponownie";

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
                    int result = BankAccount.WithdrawMoney(withdrawAmount, _accountID, Convert.ToDouble(CheckBalance()));
                    
                    if(result == -1)
                    {
                        label6.ForeColor = Color.Red;
                        label6.Text = OPERATION_ERROR;
                    }
                    else
                    {
                        label6.ForeColor = Color.Black;
                        label6.Text = "Wypłacono " + withdrawAmount;
                    }
                    labelStanKonta.Text = CheckBalance();
                }
                else
                {
                    label6.Text = WRONG_NOMINAL;
                    label6.ForeColor = Color.Red;
                }
            }
            else
            {
                label6.Text = WRONG_AMOUNT;
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
                    int result = BankAccount.WithdrawMoney(depositAmount, _accountID, Convert.ToDouble(CheckBalance()));

                    if (result == -1)
                    {
                        label6.ForeColor = Color.Red;
                        label6.Text = OPERATION_ERROR;
                    }
                    else
                    {
                        label6.ForeColor = Color.Black;
                        label6.Text = "Wpłacono " + depositAmount;
                    }
                    labelStanKonta.Text = CheckBalance();
                }
                else
                {
                    label7.Text = WRONG_NOMINAL;
                    label7.ForeColor = Color.Red;
                }
            }
            else
            {
                label7.Text = WRONG_AMOUNT;
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
