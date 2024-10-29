using Dashboard.Models;
using Dashboard.Models.DTOs.Response;
using Dashboard.Repository.Interfaces;
using Dashboard.Services;
using Dashboard.Utility;
using Microsoft.Extensions.Configuration;
using Moq;

namespace DashboardUnitTest
{

    public class RevenueServiceTests
    {
        private readonly Mock<IRevenueRepository> _mockRevenueRepository;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly RevenueService _revenueService;

        public RevenueServiceTests()
        {
            _mockRevenueRepository = new Mock<IRevenueRepository>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockConfiguration = new Mock<IConfiguration>();

            _revenueService = new RevenueService(
                _mockRevenueRepository.Object,
                _mockProductRepository.Object,
                _mockConfiguration.Object
            );
        }

        [Fact]
        public void GetProductCostById_ShouldReturnProductList()
        {
            int productId = 1;
            var products = new List<Product> { new () { Id = productId, Name = "TestProduct" } };
            _mockProductRepository.Setup(repo => repo.GetProductById(productId)).Returns(products);

            var result = _revenueService.GetProductCostById(productId);

            Assert.Equal(products, result);
        }

        [Fact]
        public void GetProductCostByName_ShouldReturnProductsContainingName()
        {
            string name = "Test";
            var products = new List<Product>
            {
                new () { Id = 1, Name = "TestProduct1" },
                new () { Id = 2, Name = "TestProduct2" }
            };
            _mockProductRepository.Setup(repo => repo.GetProductsThatContainsName(name)).Returns(products);

            var result = _revenueService.GetProductCostByName(name);

            Assert.Equal(products, result);
        }

        [Fact]
        public void GetAllSearchValuesByPagination_ShouldReturnPaginatedResponse()
        {
            var searchValues = new List<CustomerSearch>
        {
            new () { Id = 1, SearchTerm = "Search1", Count = 10 },
            new () { Id = 2, SearchTerm = "Search2", Count = 9 },
            new () { Id = 3, SearchTerm = "Search3", Count = 8 }
        };
            _mockRevenueRepository.Setup(repo => repo.GetAllOrderedSearchValues()).Returns(searchValues.AsQueryable().OrderBy( s => s.Count));

            int pageNumber = 1;
            int pageSize = 2;

            var result = _revenueService.GetAllSearchValuesByPagination(pageNumber, pageSize);

            Assert.Equal(2, result.Data.Count);
            Assert.Equal(2, result.MaxPages);
        }
    }

}
