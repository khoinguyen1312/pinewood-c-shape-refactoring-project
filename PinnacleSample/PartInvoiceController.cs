namespace PinnacleSample
{
    public class PartInvoiceController
    {
        ICustomerRepositoryDB CustomerRepository;
        IPartInvoiceRepositoryDB PartInvoiceRepository;

        public PartInvoiceController(ICustomerRepositoryDB _CustomerRepository,
            IPartInvoiceRepositoryDB _PartInvoiceRepository)
        {
            CustomerRepository = _CustomerRepository;
            PartInvoiceRepository = _PartInvoiceRepository;
        }

        public CreatePartInvoiceResult CreatePartInvoice(
            string stockCode, int quantity, string customerName)
        {
            if (string.IsNullOrEmpty(stockCode))
            {
                return new CreatePartInvoiceResult(false);
            }

            if (quantity <= 0)
            {
                return new CreatePartInvoiceResult(false);
            }

            Customer _Customer = CustomerRepository.GetByName(customerName);
            if (_Customer.ID <= 0)
            {
                return new CreatePartInvoiceResult(false);
            }

            using (PartAvailabilityServiceClient _PartAvailabilityService = new PartAvailabilityServiceClient())
            {
                int _Availability = _PartAvailabilityService.GetAvailability(stockCode);
                if (_Availability <= 0)
                {
                    return new CreatePartInvoiceResult(false);
                }
            }

            PartInvoice _PartInvoice = new PartInvoice
            {
                StockCode = stockCode,
                Quantity = quantity,
                CustomerID = _Customer.ID
            };


            PartInvoiceRepository.Add(_PartInvoice);

            return new CreatePartInvoiceResult(true);
        }
    }
}
