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

        private static string CUSTOMER_NAME = "John";
        private static string STOCK_CODE = "123";

        private Mock<ICustomerRepositoryDB> _CustomerRepository;
        private Mock<PartAvailabilityServiceClient> _PartAvailabilityService;
        private Mock<IPartInvoiceRepositoryDB> _PartInvoiceRepository;

        private PartInvoiceController controller;

        [TestInitialize]
        public void TestInitialize()
        {
            _CustomerRepository = new Mock<ICustomerRepositoryDB>();
            _PartAvailabilityService = new Mock<PartAvailabilityServiceClient>();
            _PartInvoiceRepository = new Mock<IPartInvoiceRepositoryDB>();

           controller = new PartInvoiceController(
                    _CustomerRepository.Object,
                    _PartInvoiceRepository.Object);
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

    }
}