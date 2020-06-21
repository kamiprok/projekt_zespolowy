using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WinFormBankomat_N_19.Models;

namespace WinFormBankomat_N_19.Tests
{
    [TestClass]
    public class TransactionTests
    {
        [TestMethod]
        public void LogTransaction_Tests()
        {
            Transaction transaction = new Transaction(2, OperationType.Withdrawal, 123456);
            int result = transaction.LogTransaction();

            Assert.AreEqual(0, result);
        }
    }
}
