namespace PinnacleSample
{
    public class PartInvoiceController
    {
        private readonly ICustomerRepositoryDB CustomerRepository;
        private readonly IPartInvoiceRepositoryDB PartInvoiceRepository;
        private readonly IPartAvailabilityService PartAvailabilityService;

        public PartInvoiceController(ICustomerRepositoryDB _CustomerRepository,
            IPartInvoiceRepositoryDB _PartInvoiceRepository,
            IPartAvailabilityService _PartAvailabilityService)
        {
            CustomerRepository = _CustomerRepository;
            PartInvoiceRepository = _PartInvoiceRepository;
            PartAvailabilityService = _PartAvailabilityService;
        }

        public CreatePartInvoiceResult CreatePartInvoice(string stockCode, int quantity, string customerName)
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

            if (PartAvailabilityService.GetAvailability(stockCode) <= 0)
            {
                return new CreatePartInvoiceResult(false);
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
