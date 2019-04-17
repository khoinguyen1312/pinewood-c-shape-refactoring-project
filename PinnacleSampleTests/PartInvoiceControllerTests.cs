using Microsoft.VisualStudio.TestTools.UnitTesting;
using PinnacleSample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace PinnacleSample.Tests
{
    [TestClass()]
    public class PartInvoiceControllerTests
    {
        private static int QUANTITY = 2;
        private static int AVAILABILITY = 1;
        private static int CUSTOMER_ID = 1;

        private static string CUSTOMER_NAME = "John";
        private static string STOCK_CODE = "123";

        private static Customer CUSTOMER = new Customer
        {
            ID = CUSTOMER_ID,
            Name = CUSTOMER_NAME,
            Address = "some where"
        };

        private Mock<ICustomerRepositoryDB> _CustomerRepository;
        private Mock<IPartAvailabilityService> _PartAvailabilityService;
        private Mock<IPartInvoiceRepositoryDB> _PartInvoiceRepository;

        private PartInvoiceController controller;

        [TestInitialize]
        public void TestInitialize()
        {
            _CustomerRepository = new Mock<ICustomerRepositoryDB>();
            _PartAvailabilityService = new Mock<IPartAvailabilityService>();
            _PartInvoiceRepository = new Mock<IPartInvoiceRepositoryDB>();

           controller = new PartInvoiceController(
                    _CustomerRepository.Object,
                    _PartInvoiceRepository.Object,
                    _PartAvailabilityService.Object);
        }

        [TestMethod()]
        public void CreatePartInvoice_should_return_false_when_stock_code_is_null()
        {
            CreatePartInvoiceResult result = controller.CreatePartInvoice(null, QUANTITY, CUSTOMER_NAME);

            Assert.IsFalse(result.Success);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void CreatePartInvoice_should_return_false_when_stock_code_not_null_but_quantity_is_smaller_or_equals_zero(int _quantity)
        {
            CreatePartInvoiceResult result = controller.CreatePartInvoice(STOCK_CODE, _quantity, CUSTOMER_NAME);

            Assert.IsFalse(result.Success);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void CreatePartInvoice_should_return_false_when_customer_id_is_smaller_or_equal_than_zero(int _customerId)
        {
            Customer customer = new Customer
            {
                ID = _customerId,
                Name = CUSTOMER_NAME,
                Address = "some where"
            };

            _CustomerRepository.Setup(repo => repo.GetByName(CUSTOMER_NAME)).Returns(customer);

            CreatePartInvoiceResult result = controller.CreatePartInvoice(STOCK_CODE, QUANTITY, CUSTOMER_NAME);

            Assert.IsFalse(result.Success);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void CreatePartInvoice_should_return_false_when_PartAvailabilityService_return_availability_smaller_than_zero(int _availability)
        {
            _CustomerRepository.Setup(repo => repo.GetByName(CUSTOMER_NAME)).Returns(CUSTOMER);
            _PartAvailabilityService.Setup(service => service.GetAvailability(STOCK_CODE)).Returns(_availability);

            CreatePartInvoiceResult result = controller.CreatePartInvoice(STOCK_CODE, QUANTITY, CUSTOMER_NAME);

            Assert.IsFalse(result.Success);
        }

        [TestMethod()]
        public void CreatePartInvoice_should_return_true_when_PartInvoiceRepository_add_successfully()
        {
            _CustomerRepository.Setup(repo => repo.GetByName(CUSTOMER_NAME)).Returns(CUSTOMER);
            _PartAvailabilityService.Setup(service => service.GetAvailability(STOCK_CODE)).Returns(AVAILABILITY);

            CreatePartInvoiceResult result = controller.CreatePartInvoice(STOCK_CODE, QUANTITY, CUSTOMER_NAME);

            _PartInvoiceRepository
                .Verify
                (repo => 
                    repo.Add
                    (It.Is<PartInvoice>
                        (part => 
                            part.StockCode == STOCK_CODE
                                && part.Quantity == QUANTITY
                                && part.CustomerID == CUSTOMER_ID)
                    )
                );

            Assert.IsTrue(result.Success);
        }
    }
}