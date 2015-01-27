using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApi1.Models
{
    public class Repository
    {
        public static Repository SampleData
        {
            get {
                return new Repository(new List<Item>
                {
                    new Item {
                        Description = "Description a",
                        Price = 1,
                        Name = "Item a"
                    },
    
                    new Item {
                        Description = "Description b",
                        Price = 2,
                        Name = "Item b"
                    }
                });
            }
        }

        IEnumerable<Item> _items;
        public Repository(IEnumerable<Item> items)
        {
            if (items == null)
            {
                _items = new Item[] {}.AsEnumerable();
            }

            _items = items.ToList();
        }

        // The number of records to return could be configurable.
        // For the interest of this setting it to 10.
        // The idea is to always return paged result sets as the cost of returning
        // all items could be ridiculous. We want a responsive service without using a lot of resources.
        // Sorting requirement could cause a problem as sorting requires getting whole recordset
        // from backend. This could be dealt with by using clustered indexes on product name in SQL Server
        // for example.
        public IEnumerable<Item> GetAllItems()
        {
            return GetItems(0, 10);
        }

        public RepositoryResponse GetStatus(string receiptId)
        { 
            // This again involves a call to the backend. A stand-in here.
            return new RepositoryResponse
            {
                Description = "Your order is being processed.",
                Status = RequestStatus.Pending,
                ReceiptID = "SomeUniqueValue"
            };
        }

        public Item Find(string itemName)
        {
            // This is a good candidate for an async call since this involves querying the backend.
            // A stand-in.
            return _items.FirstOrDefault((s) => String.Equals(s.Name, itemName, StringComparison.InvariantCultureIgnoreCase));
        }

        // The details of financial transaction belong in a different module.
        // That's why this function does not deal with whether buyer is initiating a transaction.
        // The buy "workflow" would require a financial transaction approval.
        // That is not dealt with here, as I don't think it's important.
        // Also, I'm not removing item effectively from _items member as this is a backend issue.
        // The backend represents a state machine here. From Pending to Fulfilled et cetera.
        // I could represent it, but it's a mock anyway. Besides, this knowledge shouldn't be coded here.
        // This belongs in the backend.
        public RepositoryResponse Buy(AcquisitionRequest request)
        {
            // This is a good candidate for an async call since this involves querying the backend.
            var item = Find(request.ProductName);

            if (item == null)
            {
                return new RepositoryResponse
                {
                    Description = "Element with name " + request.ProductName + " could not be found.",
                    Status = RequestStatus.OutOfStock
                };
            }

            return new RepositoryResponse
            {
                Description = "Your order is being processed.",
                Status = RequestStatus.Pending,
                ReceiptID = "SomeUniqueValue" 
            };
        }

        #region private methods

        IEnumerable<Item> GetItems(int from, int nrOfRecords)
        {
            return _items.Skip(from).Take(nrOfRecords);
        }

        #endregion
    }
}