namespace PinnacleSample
{
    public class PinnacleClient
    {
        private PartInvoiceController __Controller;

        public PinnacleClient()
        {
            ICustomerRepositoryDB _CustomerRepository = new CustomerRepositoryDB();
            IPartInvoiceRepositoryDB _PartInvoiceRepository = new PartInvoiceRepositoryDB();

            __Controller = new PartInvoiceController(_CustomerRepository, _PartInvoiceRepository);
        }

        public CreatePartInvoiceResult CreatePartInvoice(string stockCode, int quantity, string customerName)
        {

            return __Controller.CreatePartInvoice(stockCode, quantity, customerName);
        }
    }
}
