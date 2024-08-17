using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using SearchApi.Controllers;
using SearchApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SearchApi.Tests
{
    /// <summary>
    /// Unit tests for the <see cref="DocumentsController"/> class.
    /// </summary>
    public class DocumentsControllerTests
    {
        private readonly DocumentsController _controller; // The controller instance to test.
        private readonly Mock<IIndexService> _indexServiceMock; // Mock of the index service.
        private readonly Mock<ISearchService> _searchServiceMock; // Mock of the search service.

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentsControllerTests"/> class.
        /// </summary>
        public DocumentsControllerTests()
        {
            _indexServiceMock = new Mock<IIndexService>();
            _searchServiceMock = new Mock<ISearchService>();
            _controller = new DocumentsController(_indexServiceMock.Object, _searchServiceMock.Object);
        }

        /// <summary>
        /// Tests that the <see cref="DocumentsController.BuildInvertedIndexAndStoreDocuments"/> method returns a <see cref="NoContentResult"/>.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task BuildInvertedIndexAndStoreDocuments_ReturnsNoContent()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>(); // Mock of the file to upload.
            string tokenizeType = "simple"; // Tokenization type.
            bool isAllowedFrequency = true; // Indicates whether frequency counting is allowed.
            bool isWithStemming = true; // Indicates whether stemming should be applied.

            // Act
            var result = await _controller.BuildInvertedIndexAndStoreDocuments(fileMock.Object, tokenizeType, isAllowedFrequency, isWithStemming);

            // Assert
            // Verifies that the index service method was called once with the specified parameters.
            _indexServiceMock.Verify(s => s.BuildInvertedIndexByCSVFile(fileMock.Object, tokenizeType, isAllowedFrequency, isWithStemming), Times.Once);
            // Asserts that the result is of type NoContentResult.
            Assert.IsType<NoContentResult>(result);
        }
    }
}
