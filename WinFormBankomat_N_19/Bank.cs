using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormBankomat_N_19
{
    public partial class Bank : Form
    {
        public Bank()
        {
            InitializeComponent();

            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = true;

            label1.Text = "Logowanie pracownika";
            label2.Text = "Podaj kod PIN";
            textBox1.Visible = true;
            button6.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 f1 = new Form1();
            f1.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainWindow f1 = new MainWindow();
            f1.ShowDialog();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "1122")
            {
                button1.Visible = true;
                button2.Visible = true;
                button3.Visible = true;
                button4.Visible = true;
                button5.Visible = false;

                label1.Text = "Zalogowano jako:";
                label2.Text = "Administrator";
                textBox1.Visible = false;
                button6.Visible = false;
            }
            else
            {
                label3.Text = "Nieprawidłowy kod PIN";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormAccounts f = new FormAccounts();
            f.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormCreditCards f = new FormCreditCards();
            f.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainWindow f = new MainWindow();
            f.ShowDialog();
        }
    }
}
