using System.Linq;
using System.Web.Http;
using WebApi1.Models;

namespace WebApi1.Controllers
{
    public class ItemsController : ApiController
    {
        Repository _repository;
        public ItemsController(Repository repository)
        {
            _repository = repository;
        }

        public ItemsController()
        {
            _repository = Repository.SampleData;
        }

        [HttpGet]
        public IHttpActionResult GetItem(string name)
        {
            var item = _repository.Find(name);

            if (item == null)
            {
                return NotFound();
            }

            item.Url = Url.Link("items", new { controller = "items", name = item.Name });

            return Ok(item);
        }

        [HttpGet]
        [Route("api/items/order/{receiptId}=SomeUniqueValue")]
        public IHttpActionResult GetStatus(string receiptId)
        {
            var status = _repository.GetStatus(receiptId);

            return Ok(new AcquisitionResponse
            {
                RepositoryResponse = status,
                UrlToCheckStatusAt = Url.Link("items", new { controller = "items", action = "GetStatus", receiptId = status.ReceiptID })
            });
        }

        [HttpPost]
        public IHttpActionResult Buy(AcquisitionRequest request)
        {
            var response = _repository.Buy(request);

            if (response.Status == RequestStatus.OutOfStock)
            {
                return NotFound();
            }

            return Ok(new AcquisitionResponse
            {
                RepositoryResponse = response,
                UrlToCheckStatusAt = Url.Link("items", new { controller = "items", action = "GetStatus", receiptId = response.ReceiptID })
            });
        }


        [HttpGet]
        public IHttpActionResult GetAllItems()
        {
            var items = _repository.GetAllItems();

            var uItems = items.Select(s => new Item
            {
                Name = s.Name,
                Price = s.Price,
                Description = s.Description,
                Url = Url.Link("items", new { controller = "items", action = "GetItem", name = s.Name })
            });

            return Ok(uItems);
        }
    }
}
