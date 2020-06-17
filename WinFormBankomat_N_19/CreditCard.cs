﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormBankomat_N_19
{
    class CreditCard
    {
        public int CardID { get; private set; }
        public string CardNo { get; private set; }
        public DateTime ExpiredDate { get; private set; }
        public int AccountID { get; private set; }
        public int CustomerID { get; private set; }
        public int CVV { get; private set; }
        public string CardHolder { get; private set; }
        public int PIN { get; private set; }
        public string CardType { get; private set; }
        public int FailedLogins { get; set; }
        public bool Restricted { get; private set; }
        public Customer Customer { get; set; }
        public BankAccount BankAccount { get; set; }

        public void VerifyCardNo()
        {

        }

        public bool VerifyExpiredDate(int cardNo)
        {
            string query = "select ExpiredDate from CreditCards where CardNo = @cardNo";
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = query;
            sqlCmd.Parameters.AddWithValue("@cardNo", cardNo);

            DataAccessLayer dal = new DataAccessLayer();

            if(dal.connectionOpen())
            {
                SqlDataReader reader = dal.returnReader(sqlCmd);
                if(reader.HasRows)
                {
                    reader.Read();
                    this.ExpiredDate = Convert.ToDateTime(reader[0].ToString());
                    reader.Close();
                    dal.connectionClose();
                }
            }

            if(this.ExpiredDate > DateTime.Now)
            {
                //zwróć fałsz jeżeli karta nie wygasła
                return false;
            }
            else
            {
                return true;
            }
        }

        public void VerifyPIN()
        {

        }

        public int GetCardID(string cardNo)
        {
            string query = "select CardID from CreditCards where CardNo = @cardNo";
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = query;
            sqlCommand.Parameters.AddWithValue("@cardNo", cardNo);
            DataAccessLayer dal = new DataAccessLayer();

            if (dal.connectionOpen())
            {
                SqlDataReader reader = dal.returnReader(sqlCommand);
                if (reader.HasRows)
                {
                    reader.Read(); // czyta 1 wiersz
                    this.CardID = Convert.ToInt32(reader[0].ToString());
                    reader.Close();
                    dal.connectionClose();
                    return this.CardID;
                }
                else
                {
                    dal.connectionClose();
                    return -1; // brak karty w bazie banku
                }
            }
            else return -2; // brak połączenia z bazą danych
        }

        public int GetCardInfo(string cardNo)
        {
            string query = "select c.Name, c.Surname, c.PersonalID, a.AccountNo, CardNo, CardHolder, CardType, Restricted from CreditCards cc left join Customers c on c.CustomerID = cc.CustomerID left join BankAccounts a ON a.AccountID = cc.AccountID where cc.CardNo = @cardNo";
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = query;
            sqlCommand.Parameters.AddWithValue("@cardNo", cardNo);
            DataAccessLayer dal = new DataAccessLayer();

            if (dal.connectionOpen())
            {
                SqlDataReader reader = dal.returnReader(sqlCommand);
                if (reader.HasRows)
                {
                    reader.Read();
                    this.Customer.Name = reader[0].ToString();
                    this.Customer.Surname = reader[1].ToString();
                    this.Customer.PersonalID = Convert.ToInt64(reader[2].ToString());
                    this.BankAccount.AccountNo = reader[3].ToString();
                    this.CardNo = reader[4].ToString();
                    this.CardHolder = reader[5].ToString();
                    this.CardType = reader[6].ToString();
                    this.Restricted = Convert.ToBoolean(reader[6].ToString());
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

        public void SetCreditCard(string[] cardTable)
        {
            //cardTab[0] = textBox1.Text; //cardno
            //cardTab[1] = comboBox1.SelectedValue.ToString(); //custID
            //cardTab[2] = comboBox2.SelectedValue.ToString(); //accID
            //cardTab[3] = textBox5.Text; //cvv
            //cardTab[4] = textBox6.Text; //cardholder
            //cardTab[5] = textBox8.Text; //type
            this.CardNo = cardTable[0];
            this.CustomerID = Convert.ToInt32(cardTable[1]);
            this.AccountID = Convert.ToInt32(cardTable[2]);
            this.ExpiredDate = DateTime.Now.AddYears(2);
            this.CVV = Convert.ToInt32(cardTable[3]);
            this.CardHolder = cardTable[4];
            this.CardType = cardTable[5];
            this.Restricted = false;
            this.FailedLogins = 0;
            Random rnd = new Random();
            this.PIN = rnd.Next(1000, 9999);
        }

        public string AddCard()
        {
            string query = "insert into CreditCards (CardNo, ExpiredDate, AccountID, CustomerID, CVV, CardHolder, PIN, CardType, Restricted, FailedLogins) values (@cardNo, @expiredDate, @accountID, @customerID, @cvv, @cardHolder, @pin, @cardType, @restricted, @failedLogins)";
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = query;
            sqlCmd.Parameters.AddWithValue("@cardNo", this.CardNo);
            sqlCmd.Parameters.AddWithValue("@expiredDate", this.ExpiredDate);
            sqlCmd.Parameters.AddWithValue("@accountID", this.AccountID);
            sqlCmd.Parameters.AddWithValue("@customerID", this.CustomerID);
            sqlCmd.Parameters.AddWithValue("@cvv", this.CVV);
            sqlCmd.Parameters.AddWithValue("@cardHolder", this.CardHolder);
            sqlCmd.Parameters.AddWithValue("@pin", this.PIN);
            sqlCmd.Parameters.AddWithValue("@cardType", this.CardType);
            sqlCmd.Parameters.AddWithValue("@restricted", this.Restricted);
            sqlCmd.Parameters.AddWithValue("@failedLogins", this.FailedLogins);

            DataAccessLayer dal = new DataAccessLayer();

            try
            {
                dal.connectionOpen();
                dal.queryExecution(sqlCmd);
                dal.connectionClose();

                return "Karta kredytowa została dodana.";
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
