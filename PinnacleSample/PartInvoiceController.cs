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
            if (IsInValidRequest(stockCode, quantity))
            {
                return new CreatePartInvoiceResult(false);
            }

            Customer _Customer = CustomerRepository.GetByName(customerName);
            if (_Customer.ID <= 0)
            {
                return new CreatePartInvoiceResult(false);
            }

            AddPartInvoiceToRepository(stockCode, quantity, _Customer.ID);

            return new CreatePartInvoiceResult(true);
        }

        private bool IsInValidRequest(string stockCode, int quantity)
        {
            return quantity <= 0
                || string.IsNullOrEmpty(stockCode)
                || PartAvailabilityService.GetAvailability(stockCode) <= 0;
        }

        private void AddPartInvoiceToRepository(string stockCode, int quantity, int customerId)
        {
            PartInvoice _PartInvoice = new PartInvoice
            {
                StockCode = stockCode,
                Quantity = quantity,
                CustomerID = customerId
            };

            PartInvoiceRepository.Add(_PartInvoice);
        }
    }
}
