using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using OffersHub.Application.Exceptions.Products;
using OffersHub.Application.Models.Offers;
using OffersHub.Application.Repositories;
using OffersHub.Application.Services.Offers;
using OffersHub.Domain.Contracts;
using OffersHub.Domain.Models;
using System.Linq.Expressions;

namespace OffersHub.Application.Tests.Offers
{
    public class OfferServiceTests
    {
        private readonly Mock<IOfferRepository> _offerRepository;
        private readonly OfferService _service;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<ICompanyRepository> _companyRepository;
        private readonly Mock<ICategoryRepository> _categoryRepository;

        public OfferServiceTests()
        {
            _offerRepository = new Mock<IOfferRepository>();
            _companyRepository = new Mock<ICompanyRepository>();
            _categoryRepository = new Mock<ICategoryRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _service = new OfferService(_offerRepository.Object, _unitOfWork.Object, _categoryRepository.Object, _companyRepository.Object);  
        }

        [Fact]
        public async Task GetOffer_WhenIdExists_ShouldReturnOffer()
        {
            // Arrange
            int id = 2;
            var offer = new Offer { Id = id, Title = "Sample Offer", Price = 10 };
            _offerRepository.Setup(x => x.Get(id, It.IsAny<CancellationToken>())).ReturnsAsync(offer);

            // Act
            var result = await _service.Get(id, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal("Sample Offer", result.Title);
        }

        [Fact]
        public async Task GetAll_ShouldReturnListOfOffers()
        {
            // Arrange
            var offers = new List<Offer>
    {
        new Offer { Id = 1, Title = "Offer1", Price = 10 },
        new Offer { Id = 2, Title = "Offer2", Price = 20 }
    };
            _offerRepository.Setup(r => r.GetAll(It.IsAny<CancellationToken>())).ReturnsAsync(offers);

            // Act
            var result = await _service.GetAll(CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task CreateOffer_WithValidData_ShouldReturnCreatedOffer()
        {
            // Arrange
            var offerRequest = new OfferRequestModel
            {
                Company_UserName = "testcompany",
                CategoryName = "Electronics",
                Title = "Discount TV",
                Price = 299.99m
            };

            var mockCompany = new Company { Id = 1, UserId = 42, UserName = "testcompany" };
            var mockCategory = new Category { Id = 2, Name = "Electronics" };

            _companyRepository.Setup(repo => repo.Get("testcompany", It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCompany);

            _categoryRepository.Setup(repo => repo.Get("Electronics", It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCategory);

            var mockTransaction = new Mock<IDbContextTransaction>();
            mockTransaction.Setup(t => t.Commit());
            mockTransaction.Setup(t => t.Rollback());
            mockTransaction.Setup(t => t.DisposeAsync()).Returns(ValueTask.CompletedTask); 

            _unitOfWork.Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockTransaction.Object);

            _offerRepository.Setup(repo => repo.Create(It.IsAny<Offer>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Offer offer, CancellationToken _) => offer);


            _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.Create(offerRequest, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(offerRequest.Title, result.Title);
            Assert.Equal(offerRequest.Price, result.Price);
            _offerRepository.Verify(repo => repo.Create(It.IsAny<Offer>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
        }


        [Fact]
        public async Task Delete_WithInvalidId_ShouldThrowException()
        {
            // Arrange
            _offerRepository.Setup(r => r.Exists(It.IsAny<Expression<Func<Offer, bool>>>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<OfferDoesNotExist>(() => _service.Delete(999, CancellationToken.None));
        }

        [Fact]
        public async Task CancelOffer_Within10Minutes_ShouldReturnTrue()
        {
            // Arrange
            var offer = new Offer { Id = 1, CreatedAt = DateTime.UtcNow.AddMinutes(-5) };

            _offerRepository.Setup(r => r.Get(1, It.IsAny<CancellationToken>())).ReturnsAsync(offer);

            // Act
            var result = await _service.CancelOffer(1, CancellationToken.None);

            // Assert
            Assert.True(result);
            _offerRepository.Verify(r => r.Delete(1, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }


        [Fact]
        public async Task CancelOffer_After10Minutes_ShouldReturnFalse()
        {
            // Arrange
            var offer = new Offer { Id = 1, CreatedAt = DateTime.UtcNow.AddMinutes(-15) };

            _offerRepository.Setup(r => r.Get(1, It.IsAny<CancellationToken>())).ReturnsAsync(offer);

            // Act
            var result = await _service.CancelOffer(1, CancellationToken.None);

            // Assert
            Assert.False(result);
            _offerRepository.Verify(r => r.Delete(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }


    }
}
