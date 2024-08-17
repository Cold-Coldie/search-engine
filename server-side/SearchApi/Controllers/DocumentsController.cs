using Microsoft.AspNetCore.Mvc; // Importing necessary namespaces for ASP.NET Core MVC.
using SearchApi.Services.Interfaces; // Importing the namespace where the service interfaces are defined.

namespace SearchApi.Controllers
{
    // Setting the base route for all actions in this controller to 'api/documents'.
    [Route("api/documents")]
    [ApiController] // Specifies that this controller will handle HTTP API requests.
    public class DocumentsController : ControllerBase
    {
        // Private fields to hold references to the services used by this controller.
        private readonly IIndexService indexService;
        private readonly ISearchService searchService;

        // Constructor for the controller that initializes the service fields via dependency injection.
        public DocumentsController(IIndexService indexService, ISearchService searchService)
        {
            this.indexService = indexService; // Assigning the injected index service to the local field.
            this.searchService = searchService; // Assigning the injected search service to the local field.
        }

        // HTTP POST action to build the inverted index and store documents from a CSV file.
        [HttpPost]
        public async Task<IActionResult> BuildInvertedIndexAndStoreDocuments(
            IFormFile file, // The uploaded file, expected to be a CSV.
            string tokenizeType, // The type of tokenization to use (e.g., whitespace, punctuation).
            bool isAllowedFrequency, // Indicates whether to allow frequency counting.
            bool isWithStemming) // Indicates whether stemming should be applied to the tokens.
        {
            // Calls the index service to build the inverted index based on the provided file and options.
            await indexService.BuildInvertedIndexByCSVFile(file, tokenizeType, isAllowedFrequency, isWithStemming);
            return NoContent(); // Returns a 204 No Content response, indicating the operation was successful.
        }

        // HTTP GET action to search for relevant documents based on the search text and pagination parameters.
        [HttpGet]
        public IActionResult GetRelevantDocuments(string searchText, int pageSize, int pageNumber)
        {
            // Calls the search service to find relevant documents.
            var result = searchService.Search(searchText, pageSize, pageNumber);
            return Ok(result); // Returns a 200 OK response with the search results.
        }
    }
}
