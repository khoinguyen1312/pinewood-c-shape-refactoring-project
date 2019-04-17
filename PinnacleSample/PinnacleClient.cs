namespace PinnacleSample
{
    public class PinnacleClient
    {
        private readonly PartInvoiceController __Controller;

        public PinnacleClient()
        {
            ICustomerRepositoryDB _CustomerRepository = new CustomerRepositoryDB();
            IPartInvoiceRepositoryDB _PartInvoiceRepository = new PartInvoiceRepositoryDB();
            IPartAvailabilityService _PartAvailabilityService = new PartAvailabilityServiceClient();

            __Controller = new PartInvoiceController(_CustomerRepository, _PartInvoiceRepository, _PartAvailabilityService);
        }

        public CreatePartInvoiceResult CreatePartInvoice(string stockCode, int quantity, string customerName)
        {
            return __Controller.CreatePartInvoice(stockCode, quantity, customerName);
        }
    }
}
