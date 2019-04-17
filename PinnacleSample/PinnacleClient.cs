namespace PinnacleSample
{
    public class PinnacleClient
    {
        private PartInvoiceController __Controller;

        public PinnacleClient()
        {
            ICustomerRepositoryDB _CustomerRepository = new CustomerRepositoryDB();
            PartAvailabilityServiceClient _PartAvailabilityService = new PartAvailabilityServiceClient();
            IPartInvoiceRepositoryDB _PartInvoiceRepository = new PartInvoiceRepositoryDB();

            __Controller = new PartInvoiceController(_CustomerRepository, _PartAvailabilityService, _PartInvoiceRepository);
        }

        public CreatePartInvoiceResult CreatePartInvoice(string stockCode, int quantity, string customerName)
        {

            return __Controller.CreatePartInvoice(stockCode, quantity, customerName);
        }
    }
}
