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
    public partial class Bankomat : Form
    {
        private int CardID;
        private DateTime _expiredDate;
        private bool _restricted;

        public Bankomat()
        {
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string cardNo = textBoxNo.Text;
            string PIN = textBoxPIN.Text;
            labelBadLogin.Text = "";

            string query = "select CardID, ExpiredDate, Restricted from CreditCards where CardNo = @cardNo and PIN = @pin";
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = query;
            sqlCmd.Parameters.AddWithValue("@cardNo", cardNo);
            sqlCmd.Parameters.AddWithValue("@pin", PIN);
            DataAccessLayer dal = new DataAccessLayer();

            if(dal.connectionOpen())
            {
                SqlDataReader reader = dal.returnReader(sqlCmd);
                if(reader.HasRows)
                {
                    reader.Read();
                    CardID = Convert.ToInt32(reader[0]);
                    _expiredDate = Convert.ToDateTime(reader[1]);
                    _restricted = Convert.ToBoolean(reader[2]);
                    reader.Close();

                    if(_restricted)
                    {
                        labelBadLogin.Text = "Zbyt wiele błędnych logowań, karta zablokowana";
                        return;
                    }

                    if (_expiredDate < DateTime.Now)
                    {
                        labelBadLogin.Text = "Twoja karta jest przedawniona, skontaktuj się z bankiem";
                        return;
                    }
                    else
                    {
                        query = "UPDATE CreditCards SET FailedLogins = 0 WHERE CardNo = @cardNo";
                        SqlCommand sqlCmd2 = new SqlCommand();
                        sqlCmd2.CommandText = query;
                        sqlCmd2.Parameters.AddWithValue("@cardNo", cardNo);
                        dal.queryExecution(sqlCmd2);

                        this.Hide();
                        BankomatWplatomat b = new BankomatWplatomat(CardID);
                        b.ShowDialog();
                    }
                }
                else
                {
                    labelBadLogin.Text = "Logowanie nieudane, sprawdź poprawność danych";

                    reader.Close();
                    int failedLogins = 0;

                    query = "SELECT FailedLogins from CreditCards WHERE CardNo = @cardNo";
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.CommandText = query;
                    sqlCommand.Parameters.AddWithValue("@cardNo", cardNo);
                    
                    reader = dal.returnReader(sqlCommand);
                    if(reader.HasRows)
                    {
                        reader.Read();
                        failedLogins = Convert.ToInt32(reader[0]);
                        reader.Close();
                    }
                    if(failedLogins >= 2)
                    {
                        query = "UPDATE CreditCards SET Restricted = 1 WHERE CardNo = @cardNo";
                        SqlCommand sqlCmd3 = new SqlCommand();
                        sqlCmd3.CommandText = query;
                        sqlCmd3.Parameters.AddWithValue("@cardNo", cardNo);
                        dal.queryExecution(sqlCmd3);

                        labelBadLogin.Text = "Karta została zablokowana";
                    }

                    query = "UPDATE CreditCards SET FailedLogins = FailedLogins + 1 WHERE CardNo = @cardNo";
                    SqlCommand sqlCmd2 = new SqlCommand();
                    sqlCmd2.CommandText = query;
                    sqlCmd2.Parameters.AddWithValue("@cardNo", cardNo);
                    dal.queryExecution(sqlCmd2);

                    dal.connectionClose();
                }
            }

        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainWindow b = new MainWindow();
            b.ShowDialog();
        }
    }
}
