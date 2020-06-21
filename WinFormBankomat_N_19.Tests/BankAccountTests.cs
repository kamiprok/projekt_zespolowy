using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WinFormBankomat_N_19.Models;

namespace WinFormBankomat_N_19.Tests
{
    [TestClass]
    public class BankAccountTests
    {
        private static long correctPesel = 84062811111;
        private static long incorrectPesel = 124;

        [TestMethod]
        public void WithdrawMoney_Test_With_Valid_Data()
        {
            int result = BankAccount.WithdrawMoney(1000, 2, 123456.00);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void WithdrawMoney_Test_With_Invalid_Data()
        {
            int result = BankAccount.WithdrawMoney(100000000, 2, 123456.00);

            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        public void DepositMoney_Test_With_Valid_Data()
        {
            int result = BankAccount.DepositMoney(1000, 2, 123456.00);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void DepositMoney_Test_With_Invalid_Data()
        {
            int result = BankAccount.DepositMoney(-2000, 142, 123456.00);

            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        public void GetAccountID_Test_With_Valid_Data()
        {
            BankAccount account = new BankAccount();
            int result = account.GetAccountID(correctPesel);

            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void GetAccountID_Test_With_Invalid_Data()
        {
            BankAccount account = new BankAccount();
            int result = account.GetAccountID(incorrectPesel);

            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        public void GetAccountInfo_Test_With_Valid_Data()
        {
            BankAccount account = new BankAccount();
            int result = account.GetAccountInfo(correctPesel);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void GetAccountInfo_Test_With_Invalid_Data()
        {
            BankAccount account = new BankAccount();
            int result = account.GetAccountID(incorrectPesel);

            Assert.AreEqual(-1, result);
        }
    }
}
