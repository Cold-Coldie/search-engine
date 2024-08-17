using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using SearchApi.Controllers;
using SearchApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class DocumentsControllerTests
{
    private readonly DocumentsController _controller;
    private readonly Mock<IIndexService> _indexServiceMock;
    private readonly Mock<ISearchService> _searchServiceMock;

    public DocumentsControllerTests()
    {
        _indexServiceMock = new Mock<IIndexService>();
        _searchServiceMock = new Mock<ISearchService>();
        _controller = new DocumentsController(_indexServiceMock.Object, _searchServiceMock.Object);
    }

    [Fact]
    public async Task BuildInvertedIndexAndStoreDocuments_ReturnsNoContent()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        string tokenizeType = "simple";
        bool isAllowedFrequency = true;
        bool isWithStemming = true;

        // Act
        var result = await _controller.BuildInvertedIndexAndStoreDocuments(fileMock.Object, tokenizeType, isAllowedFrequency, isWithStemming);

        // Assert
        _indexServiceMock.Verify(s => s.BuildInvertedIndexByCSVFile(fileMock.Object, tokenizeType, isAllowedFrequency, isWithStemming), Times.Once);
        Assert.IsType<NoContentResult>(result);
    }
}
