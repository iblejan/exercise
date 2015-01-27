using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Results;
using System.Web.Http.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApi1.Controllers;
using WebApi1.Models;

namespace WebApi1.Tests.Controllers
{
    [TestClass]
    public class ItemsControllerTest
    {
        [TestMethod]
        public void EmptyWarehouseReturnsNoItems()
        {
            var c = new ItemsController(new Repository(new List<Item>()));
            c.Request = new HttpRequestMessage();
            c.Configuration = new HttpConfiguration();

            IHttpActionResult actionResult = c.GetAllItems();
            var contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<Item>>;

            Assert.AreEqual(0, contentResult.Content.Count());
        }

        [TestMethod]
        public void WarehouseReturnsInjectedItems()
        {
            var items = TestData.CreateTestData();
            var cntItems = items.Count();
            var c = new ItemsController(new Repository(items));
            setupItemsController(c);

            IHttpActionResult actionResult = c.GetAllItems();
            
            var contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<Item>>;
            var resultItems = contentResult.Content;

            Assert.AreEqual(cntItems, resultItems.Count());
            Assert.IsTrue(resultItems.All(
                s => items.First(
                    t => s.Name == t.Name
                        && s.Description == t.Description
                        && s.Price == t.Price) != null));
        }

        [TestMethod]
        public void WarehouseReturnsPagedResults()
        {
            var items = TestData.CreateLargeTestDataSet();
            var cntItems = items.Count();
            var c = new ItemsController(new Repository(items));
            setupItemsController(c);

            IHttpActionResult actionResult = c.GetAllItems();

            var contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<Item>>;
            var resultItems = contentResult.Content;

            Assert.AreEqual(10, resultItems.Count());
            Assert.IsTrue(resultItems.All(
                s => items.Take(10).First(
                    t => s.Name == t.Name
                        && s.Description == t.Description
                        && s.Price == t.Price) != null));
        }

        [TestMethod]
        public void WarehouseReturnsNotFoundWhenBuyingItemsOutOfStock()
        {
            var items = TestData.CreateTestData();
            var cntItems = items.Count();
            var c = new ItemsController(new Repository(items));
            setupItemsController(c);

            var request = new AcquisitionRequest { 
                Account = null,
                ProductName = "Item d",
                Quantity = 1
            };

            IHttpActionResult actionResult = c.Buy(request);

            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void WarehouseReturnsPendingWhenBuyingItemsInStock()
        {
            var items = TestData.CreateTestData();
            var cntItems = items.Count();
            var c = new ItemsController(new Repository(items));
            setupItemsController(c);

            var request = new AcquisitionRequest
            {
                Account = null,
                ProductName = "Item a",
                Quantity = 1
            };

            IHttpActionResult actionResult = c.Buy(request);
            var contentResult = actionResult as OkNegotiatedContentResult<AcquisitionResponse>;

            Assert.IsNotNull(contentResult);

            var result = contentResult.Content;

            Assert.AreEqual(RequestStatus.Pending, result.RepositoryResponse.Status);
            Assert.IsNotNull(result.RepositoryResponse.ReceiptID);
            Assert.IsNotNull(result.UrlToCheckStatusAt);
        }

        [TestMethod]
        public void GetStatusReturnsExpectedValues()
        {
            var items = TestData.CreateTestData();
            var cntItems = items.Count();
            var c = new ItemsController(new Repository(items));
            setupItemsController(c);

            var request = new AcquisitionRequest
            {
                Account = null,
                ProductName = "Item a",
                Quantity = 1
            };

            IHttpActionResult actionResult = c.Buy(request);
            var contentResult = actionResult as OkNegotiatedContentResult<AcquisitionResponse>;
            var receiptId = contentResult.Content.RepositoryResponse.ReceiptID;

            IHttpActionResult statusResult = c.GetStatus(receiptId);

            var statusResponse = statusResult as OkNegotiatedContentResult<AcquisitionResponse>;
            var content = statusResponse.Content;

            Assert.AreEqual(RequestStatus.Pending, content.RepositoryResponse.Status);
            Assert.IsNotNull(content.RepositoryResponse.ReceiptID);
            Assert.IsNotNull(content.UrlToCheckStatusAt);
        }

        [TestMethod]
        public void WarehouseReturnsNotFoundForItemNotInTheBackend()
        {
            var items = TestData.CreateTestData();
            var cntItems = items.Count();
            var c = new ItemsController(new Repository(items));
            setupItemsController(c);

            IHttpActionResult actionResult = c.GetItem("Item 23");

            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void WarehouseReturnsExpectedValueForItemInTheBackend()
        {
            var items = TestData.CreateTestData();
            var expectedValue = items.First(s => s.Name == "Item b");

            var c = new ItemsController(new Repository(items));
            setupItemsController(c);

            IHttpActionResult actionResult = c.GetItem("Item b");
            var actionResponse = actionResult as OkNegotiatedContentResult<Item>;
            var response = actionResponse.Content;

            Assert.AreEqual(expectedValue.Description, response.Description);
            Assert.AreEqual(expectedValue.Name, response.Name);
            Assert.AreEqual(expectedValue.Description, response.Description);
        }


        // This is needed as Url gets lost in MVC5 when running tests.
        private static void setupItemsController(ApiController controller)
        {
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/items");
            var route = config.Routes.MapHttpRoute("items", "api/{controller}/{name}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary
            {
                {"name", String.Empty},
                {"controller", "items"}
            });

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            controller.Request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;

            UrlHelper urlHelper = new UrlHelper(request);
            controller.Url = urlHelper;
        }
    }
}
