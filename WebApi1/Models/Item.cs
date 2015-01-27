using System.Collections.Generic;

namespace WebApi1.Models
{
    public class PagedResult
    {
        public List<Item> Items
        {
            get;
            set;
        }

        public int From
        {
            get;
            set;
        }

        public int Next
        {
            get;
            set;
        }
    }

    public class Item
    {
        public string Name
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public int Price
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }
    }

    // The object modeling the Buy request.
    public class AcquisitionRequest
    {
        public string ProductName
        {
            get;
            set;
        }

        public int Quantity
        {
            get;
            set;
        }

        public Account Account
        {
            get;
            set;
        }
    }

    public enum RequestStatus
    { 
        None,
        Pending,
        Refused,
        Cancelled,
        Fulfilled,
        OutOfStock,
        OnHold
    }

    public class RepositoryResponse
    {
        public RequestStatus Status
        {
            get;
            set;
        }

        // Needed in order to offer more detail with refards to order.
        public string Description
        {
            get;
            set;
        }

        // A transaction id, Guid, system generated, whatever.
        // Used in the backend to identify order.
        public string ReceiptID
        {
            get;
            set;
        }
    }

    public class AcquisitionResponse
    {
        public RepositoryResponse RepositoryResponse
        {
            get;
            set;
        }

        // Used to build the link in the controller. Read hypermedia.
        // In fact this is necessary, the client should be able to check on the status of her order.
        public string UrlToCheckStatusAt
        {
            get;
            set;
        }
    }

    // The object describing financial information supporting acquisition request.
    public class Account
    { 
        // Leaving it empty as from the API's perspective is irrelevant.
        // Does it really matter what this contains?
        // It could be an account id, for previously saved account information
        // or CC number data et cetera
    }
}