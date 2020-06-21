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
    public partial class Bankomat : Form
    {
        private static string ZBYT_WIELE_LOGOWAN = "Zbyt wiele błędnych logowań, karta zablokowana";
        private static string LOGOWANIE_NIEUDANE = "Logowanie nieudane, sprawdź poprawność danych";
        private static string KARTA_ZABLOKOWANA = "Karta została zablokowana";
        private static string KARTA_PRZEDAWNIONA = "Twoja karta jest przedawniona, skontaktuj się z bankiem";
        private int CardID;

        public Bankomat()
        {
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string cardNo = textBoxNo.Text;
            string PIN = textBoxPIN.Text;
            labelBadLogin.Text = "";

            try
            {
                CreditCard creditCard = new CreditCard(cardNo);


                if (creditCard.isPinInvalid(PIN))
                {
                    creditCard.RecordFailedAccess();
                    {
                        labelBadLogin.Text = creditCard.isRestricted() ? ZBYT_WIELE_LOGOWAN : LOGOWANIE_NIEUDANE;
                    }
                    return;
                }
                else if(creditCard.isRestricted())
                {
                    labelBadLogin.Text = KARTA_ZABLOKOWANA;
                    return;
                }
                else if(creditCard.IsExpired())
                {
                    labelBadLogin.Text = KARTA_PRZEDAWNIONA;
                    return;
                }
                else
                {
                    CardID = creditCard.CardID;
                    this.Hide();
                    BankomatWplatomat b = new BankomatWplatomat(CardID);
                    b.ShowDialog();
                }
            }
            catch(CardNotFoundException)
            {
                labelBadLogin.Text = LOGOWANIE_NIEUDANE;
                return;
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
