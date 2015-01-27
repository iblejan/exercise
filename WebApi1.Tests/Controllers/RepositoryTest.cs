using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApi1.Models;

namespace WebApi1.Tests.Controllers
{
    [TestClass]
    public class RepositoryTest
    {
        [TestMethod]
        public void GetAllItemsForEmptyRepositoryReturnsEmptyCollection()
        {
            var repository = new Repository(new List<Item>());

            var items = repository.GetAllItems();

            Assert.AreEqual(0, items.Count());
        }

        [TestMethod]
        public void RepositoryReturnsInjectedValues()
        {
            var items = TestData.CreateTestData();
            int cntItems = items.Count();
            var repository = new Repository(items);

            var resultItems = repository.GetAllItems();

            Assert.AreEqual(cntItems, resultItems.Count());
            Assert.IsTrue(resultItems.All(
                s => items.First(
                    t => s.Name == t.Name 
                        && s.Description == t.Description 
                        && s.Price == t.Price ) != null));
        }

        [TestMethod]
        public void BuyingNonExistingItemReturnsProperValue()
        {
            var items = TestData.CreateTestData();
            int cntItems = items.Count();
            var repository = new Repository(items);

            var request = new AcquisitionRequest
            {
                Account = new Account(),
                ProductName = "Item c",
                Quantity = 1
            };

            var response = repository.Buy(request);

            Assert.AreEqual(RequestStatus.OutOfStock, response.Status);
            Assert.AreEqual("Element with name Item c could not be found.", response.Description);
        }

        [TestMethod]
        public void BuyingExistingItemReturnsProperValues()
        {
            var items = TestData.CreateTestData();
            int cntItems = items.Count();
            var repository = new Repository(items);

            var request = new AcquisitionRequest
            {
                Account = new Account(),
                ProductName = "Item a",
                Quantity = 1
            };

            var response = repository.Buy(request);

            Assert.AreEqual(RequestStatus.Pending, response.Status);
            Assert.AreEqual("Your order is being processed.", response.Description);
            Assert.IsNotNull(response.ReceiptID);
        }

        [TestMethod]
        public void QueryingForStatusReturnsProperValue()
        {
            var items = TestData.CreateTestData();
            int cntItems = items.Count();
            var repository = new Repository(items);
            string receiptId = "SomeUniqueValue";

            var response = repository.GetStatus(receiptId);

            Assert.AreEqual(RequestStatus.Pending, response.Status);
            Assert.AreEqual("Your order is being processed.", response.Description);
            Assert.IsNotNull(response.ReceiptID);
        }

        [TestMethod]
        public void SearchingNonExistentItemReturnsNull()
        {
            var items = TestData.CreateTestData();
            var repository = new Repository(items);

            var response = repository.Find("Item 23");

            Assert.IsNull(response);
        }

        [TestMethod]
        public void SearchingExistentItemReturnsExpectedValue()
        {
            var items = TestData.CreateTestData();
            var expectedValue = items.First(s => s.Name == "Item a");
            var repository = new Repository(items);

            var response = repository.Find("Item a");

            Assert.AreEqual(expectedValue.Description, response.Description);
            Assert.AreEqual(expectedValue.Name, response.Name);
            Assert.AreEqual(expectedValue.Price, response.Price);
        }
    }
}
