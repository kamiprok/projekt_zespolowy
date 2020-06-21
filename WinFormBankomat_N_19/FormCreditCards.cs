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
    public partial class FormCreditCards : Form
    {
        DataAccessLayer dal = new DataAccessLayer();
        SqlCommand sqlCommand = new SqlCommand();

        public FormCreditCards()
        {
            InitializeComponent();
            RefreshDataGridView();
            RefreshComboData();
        }

        private void RefreshDataGridView()
        {
            sqlCommand.CommandText = "select * from CreditCards";
            dataGridView1.DataSource = dal.selectData(sqlCommand);
        }

        public void RefreshComboData()
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

        public void RefreshAccountComboBox()
        {
            int customer = Convert.ToInt32(comboBox1.SelectedValue);
            DataRow dr;

            if (dal.connectionOpen())
            {
                SqlCommand cmd = new SqlCommand("Select AccountID, AccountNo from BankAccounts where CustomerID = @customer", dal.getConn());
                cmd.Parameters.AddWithValue("@customer", customer);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                dr = dt.NewRow();
                dr.ItemArray = new object[] { 0, "-- Wybierz Konto--" };
                dt.Rows.InsertAt(dr, 0);

                comboBox2.ValueMember = "AccountID";

                comboBox2.DisplayMember = "AccountNo";
                comboBox2.DataSource = dt;

                dal.connectionClose();
            }
        }

        private void buttonPowrot_Click(object sender, EventArgs e)
        {
            this.Hide();
            Bank b = new Bank();
            b.ShowDialog();
        }

        private void btnSearchID_Click(object sender, EventArgs e)
        {
            string cardno = textCardNo.Text;
            CreditCard card = new CreditCard();
            labID.Text = card.GetCardID(cardno).ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string cardno = textCardNo.Text;
            CreditCard card = new CreditCard();
            int wynik = card.GetCardInfo(cardno);
            if(wynik==1)
            {
                labCardInfo.Text = "Customer\nName = " + card.Customer.Name + "\nSurname = " + card.Customer.Surname +
                    "\n PersonalID = " + card.Customer.PersonalID.ToString() + "\nAccountNo = " + card.BankAccount.AccountNo +
                    "\n CardNo = " + card.CardNo.ToString() + "\nCard Holder = " + card.CardHolder +
                    "\n CardType = " + card.CardType + "\nRestricted = " + card.Restricted.ToString();
            }
        }

        private void buttonAddCard_Click(object sender, EventArgs e)
        {
            string[] cardTab = new string[8];
            cardTab[0] = textBox1.Text; //cardno
            cardTab[1] = comboBox1.SelectedValue.ToString(); //custID
            cardTab[2] = comboBox2.SelectedValue.ToString(); //accID
            cardTab[3] = textBox5.Text; //cvv
            cardTab[4] = textBox6.Text; //cardholder
            cardTab[5] = textBox8.Text; //type

            CreditCard card = new CreditCard();
            card.SetCreditCard(cardTab);

            labelInfo.Text = card.AddCard();

            RefreshDataGridView();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshAccountComboBox();
        }
    }
}
