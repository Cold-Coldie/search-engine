using Microsoft.AspNetCore.Mvc;
using SearchApi.Services.Interfaces;

namespace SearchApi.Controllers
{
    /// <summary>
    /// Controller for handling document-related operations, such as building an inverted index and searching documents.
    /// </summary>
    [Route("api/documents")]
    [ApiController] // Specifies that this controller will handle HTTP API requests.
    public class DocumentsController : ControllerBase
    {
        // Private fields to hold references to the services used by this controller.
        private readonly IIndexService indexService;
        private readonly ISearchService searchService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentsController"/> class.
        /// </summary>
        /// <param name="indexService">The index service used for building the inverted index.</param>
        /// <param name="searchService">The search service used for searching documents.</param>
        public DocumentsController(IIndexService indexService, ISearchService searchService)
        {
            this.indexService = indexService; // Assigning the injected index service to the local field.
            this.searchService = searchService; // Assigning the injected search service to the local field.
        }

        /// <summary>
        /// Builds the inverted index and stores documents from a CSV file.
        /// </summary>
        /// <param name="file">The uploaded file, expected to be a CSV containing document data.</param>
        /// <param name="tokenizeType">The type of tokenization to use (e.g., whitespace, punctuation).</param>
        /// <param name="isAllowedFrequency">Indicates whether to allow frequency counting in tokenization.</param>
        /// <param name="isWithStemming">Indicates whether stemming should be applied to the tokens.</param>
        /// <returns>A task representing the asynchronous operation, with a 204 No Content response on success.</returns>
        [HttpPost]
        public async Task<IActionResult> BuildInvertedIndexAndStoreDocuments(
            IFormFile file,
            string tokenizeType,
            bool isAllowedFrequency,
            bool isWithStemming)
        {
            // Calls the index service to build the inverted index based on the provided file and options.
            await indexService.BuildInvertedIndexByCSVFile(file, tokenizeType, isAllowedFrequency, isWithStemming);
            return NoContent(); // Returns a 204 No Content response, indicating the operation was successful.
        }

        /// <summary>
        /// Searches for relevant documents based on the provided search text and pagination parameters.
        /// </summary>
        /// <param name="searchText">The text to search for in the documents.</param>
        /// <param name="pageSize">The number of results to return per page.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <returns>A 200 OK response with the search results.</returns>
        [HttpGet]
        public IActionResult GetRelevantDocuments(string searchText, int pageSize, int pageNumber)
        {
            // Calls the search service to find relevant documents.
            var result = searchService.Search(searchText, pageSize, pageNumber);
            return Ok(result); // Returns a 200 OK response with the search results.
        }
    }
}
