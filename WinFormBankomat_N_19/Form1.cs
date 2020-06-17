using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WinFormBankomat_N_19
{
    public partial class Form1 : Form
    {
        DataAccessLayer dal = new DataAccessLayer();
        SqlCommand sqlCommand = new SqlCommand();

        public Form1()
        {
            InitializeComponent();
            refreshDataGridView();
        }

        private void btnSearchID_Click(object sender, EventArgs e)
        {
            long pesel = Convert.ToInt64(txtBoxPesel.Text);
            Customer customer = new Customer();
            labID.Text = customer.getCustomerID(pesel).ToString();
        }

        private void btnSearchCustomerInfo_Click(object sender, EventArgs e)
        {
            long pesel = Convert.ToInt64(txtBoxPesel.Text);
            Customer customer = new Customer();
            int wynik = customer.getCustomerInfo(pesel);
            if (wynik == 1)
            {
                labCustomerInfo.Text = "Customer\nName = " + customer.Name + "\nSurname = " + customer.Surname +
                    "\nPhone = " + customer.PhoneNo.ToString() + "\nAddress = " + customer.Address +
                    "\nPersonalID = " + customer.PersonalID.ToString();
            }
            else if (wynik == -1) labCustomerInfo.Text = "brak klienta";
            else labCustomerInfo.Text = "brak połączenia z bazą danych";
        }

        private void btnInsertCustomer_Click(object sender, EventArgs e)
        {
            string[] customerTab = new string[5];
            customerTab[0] = txtBoxName.Text;
            customerTab[1] = txtBoxSurname.Text;
            customerTab[2] = txtBoxPhone.Text;
            customerTab[3] = txtBoxAddress.Text;
            customerTab[4] = txtBoxPesel2.Text;
            Customer customer = new Customer();
            customer.setCustomer(customerTab);
            
            labInsertInfo.Text= customer.addCustomer();

            refreshDataGridView();
        }
        private void refreshDataGridView()
        {
            sqlCommand.CommandText = "select * from Customers";
            dataGridView1.DataSource = dal.selectData(sqlCommand);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Bank b = new Bank();
            b.ShowDialog();
        }
    }
}
