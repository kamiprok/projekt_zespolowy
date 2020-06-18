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
    public partial class FormAccounts : Form
    {
        DataAccessLayer dal = new DataAccessLayer();
        SqlCommand sqlCommand = new SqlCommand();

        public FormAccounts()
        {
            InitializeComponent();
            RefreshDataGridView();
            RefreshData();
        }

        private void FormAccounts_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Bank b = new Bank();
            b.ShowDialog();
        }

        private void btnSearchID_Click(object sender, EventArgs e)
        {
            long pesel;
            bool success = Int64.TryParse(textBox1.Text, out pesel);
            if (success)
            {
                //pesel = Convert.ToInt64(textBox1.Text);
                BankAccount account = new BankAccount();
                labID.Text = account.GetAccountID(pesel).ToString();
                //labAccountInfo.Text = labID.Text == "-1" ? "brak klienta" : "brak polaczenia z baza danych";
                if (labID.Text == "-1") labID.Text = "brak klienta";
                else if (labID.Text == "-2") labID.Text = "brak polaczenia z baza danych";
            }
            else
            {
                textBox1.Text = "podaj numer !";
            }

            //long pesel = Convert.ToInt64(textBox1.Text);
            //BankAccount account = new BankAccount();
            //labID.Text = account.GetAccountID(pesel).ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            long pesel;
            bool success = Int64.TryParse(textBox1.Text, out pesel);
            if (success)
            {
                //pesel = Convert.ToInt64(textBox1.Text);
               BankAccount account = new BankAccount();
                int bank = account.GetAccountInfo(pesel);
                Customer customer = new Customer();             
                int wynik = customer.getCustomerInfo(pesel);
                if (wynik == 1)
                {
                    labAccountInfo.Text = "Customer\nName = " + customer.Name + "\nSurname = " + customer.Surname +
                        "\n PersonalID = " + customer.PersonalID.ToString() + "\nAccountNo = " + account.AccountNo.ToString() +
                        "\nBalance = " + account.Balance.ToString();
                }
                else if (wynik == -1) labAccountInfo.Text = "brak klienta";
                else labAccountInfo.Text = "brak połączenia z bazą danych";
            }
            else
            {
                textBox1.Text = "podaj numer !";
            }
        } 

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void RefreshData()
        {
            DataRow dr;

            if (dal.connectionOpen())
            {
                SqlCommand cmd = new SqlCommand("Select CustomerID, CAST(PersonalID as nvarchar(50)) + ', ' + Name + ' ' + Surname as Client from Customers", dal.getConn());
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                dr = dt.NewRow();
                dr.ItemArray = new object[] { 0, "-- Wybierz Klienta--" };
                dt.Rows.InsertAt(dr, 0);

                comboBox1.ValueMember = "CustomerID";

                comboBox1.DisplayMember = "client";
                comboBox1.DataSource = dt;

                dal.connectionClose();
            }
            RefreshDataGridView();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnInsertCustomer_Click(object sender, EventArgs e)
        {
            string[] accountTab = new string[3];
            accountTab[0] = comboBox1.SelectedValue.ToString();
            accountTab[1] = textBox2.Text;
            accountTab[2] = textBox3.Text;

            BankAccount account = new BankAccount();
            account.SetBankAccount(accountTab);

            labInsertInfo.Text = account.AddAccount();

            RefreshDataGridView();
        }

        private void RefreshDataGridView()
        {
            sqlCommand.CommandText = "select AccountNo, Balance, c.PersonalID from BankAccounts b left join Customers c ON b.CustomerID = c.CustomerID";
            dataGridView1.DataSource = dal.selectData(sqlCommand);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string pesel;
            double amount = 0;
            double balance = 0;
            int id = 0;

            if (txtPeselDepo.Text.Length == 0 || txtAmountDepo.Text.Length == 0)
            {
                labErrorInfo.Text = "Musisz podać obie wartości!";
                labErrorInfo.ForeColor = Color.Red;
            }
            else if(!Double.TryParse(txtAmountDepo.Text, out amount))
            {
                labErrorInfo.Text = "Podana wartość musi być liczbą z maksymalnie 2 miejscami po przecinku!";
                labErrorInfo.ForeColor = Color.Red;
            }
            else
            {
                pesel = txtPeselDepo.Text;
                amount = Convert.ToDouble(txtAmountDepo.Text);

                string query = "select AccountID, Balance from BankAccounts b left join Customers c ON c.CustomerID = b.CustomerID where c.PersonalID = @pesel";
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandText = query;
                sqlCmd.Parameters.AddWithValue("@pesel", pesel);

                if(dal.connectionOpen())
                {
                    SqlDataReader reader = dal.returnReader(sqlCmd);
                    if(reader.HasRows)
                    {
                        reader.Read();
                        id = Convert.ToInt32(reader[0].ToString());
                        balance = Convert.ToDouble(reader[1].ToString());
                        reader.Close();
                    }
                    else
                    {
                        labErrorInfo.Text = "Nie istnieje konto dla klienta o podanym numerze PESEL";
                        labErrorInfo.ForeColor = Color.Red;
                        return;
                    }

                    query = "update BankAccounts SET Balance = @balance WHERE AccountID = @id";
                    sqlCmd = new SqlCommand();
                    sqlCmd.CommandText = query;
                    sqlCmd.Parameters.AddWithValue("@balance", balance + amount);
                    sqlCmd.Parameters.AddWithValue("@id", id);

                    dal.queryExecution(sqlCmd);
                    dal.connectionClose();

                    RefreshDataGridView();
                }        
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string pesel;
            double amount = 0;
            double balance = 0;
            int id = 0;

            if (txtPeselDepo.Text.Length == 0 || txtAmountDepo.Text.Length == 0)
            {
                labErrorInfo.Text = "Musisz podać obie wartości!";
                labErrorInfo.ForeColor = Color.Red;
            }
            else if (!Double.TryParse(txtAmountDepo.Text, out amount))
            {
                labErrorInfo.Text = "Podana wartość musi być liczbą z maksymalnie 2 miejscami po przecinku!";
                labErrorInfo.ForeColor = Color.Red;
            }
            else
            {
                pesel = txtPeselDepo.Text;
                amount = Convert.ToDouble(txtAmountDepo.Text);

                string query = "select AccountID, Balance from BankAccounts b left join Customers c ON c.CustomerID = b.CustomerID where c.PersonalID = @pesel";
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandText = query;
                sqlCmd.Parameters.AddWithValue("@pesel", pesel);

                if (dal.connectionOpen())
                {
                    SqlDataReader reader = dal.returnReader(sqlCmd);
                    if (reader.HasRows)
                    {
                        reader.Read();
                        id = Convert.ToInt32(reader[0].ToString());
                        balance = Convert.ToDouble(reader[1].ToString());
                        reader.Close();
                    }
                    else
                    {
                        labErrorInfo.Text = "Nie istnieje konto dla klienta o podanym numerze PESEL";
                        labErrorInfo.ForeColor = Color.Red;

                        return;
                    }

                    if (balance < amount)
                    {
                        labErrorInfo.Text = "Stan konta po wypłacie będzie mniejszy od 0! Maksymalna kwota wypłaty to " + balance;
                        labErrorInfo.ForeColor = Color.Red;
                    }
                    else
                    {
                        query = "update BankAccounts SET Balance = @balance WHERE AccountID = @id";
                        SqlCommand sqlCmd2 = new SqlCommand();
                        sqlCmd2.CommandText = query;
                        sqlCmd2.Parameters.AddWithValue("@balance", balance - amount);
                        sqlCmd2.Parameters.AddWithValue("@id", id);

                        dal.queryExecution(sqlCmd2);
                        dal.connectionClose();

                        RefreshDataGridView();
                    }
                }
            }
        }

        private void labID_Click(object sender, EventArgs e)
        {

        }

        private void labAccountInfo_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
