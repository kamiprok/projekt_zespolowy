using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WinFormBankomat_N_19.Models;

namespace WinFormBankomat_N_19.Tests
{
    [TestClass]
    public class CustomerTests
    {
        private static long correctPesel = 84062811111;
        private static long incorrectPesel = 124;

        [TestMethod]
        public void GetCustomerID_Test_With_Valid_Data()
        {
            Customer customer = new Customer();
            int result = customer.getCustomerID(correctPesel);

            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void GetCustomerID_Test_With_Invalid_Data()
        {
            Customer customer = new Customer();
            int result = customer.getCustomerID(incorrectPesel);

            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        public void GetCustomerInfo_Test_With_Valid_Data()
        {
            Customer customer = new Customer();
            int result = customer.getCustomerInfo(correctPesel);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void GetCustomerInfo_Test_With_Invalid_Data()
        {
            Customer customer = new Customer();
            int result = customer.getCustomerInfo(incorrectPesel);

            Assert.AreEqual(-1, result);
        }
    }
}
