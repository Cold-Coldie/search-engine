using SearchApi.Dtos; // Importing data transfer objects (DTOs) used for passing data.
using SearchApi.Repositories.Interfaces; // Importing interfaces for the repositories.
using SearchApi.Services.Interfaces; // Importing the interface for the search service.
using SearchApi.Utills; // Importing utilities like EnglishTokenizer and EnglishStemmer.

namespace SearchApi.Services.Implementations
{
    // Implementation of the ISearchService interface.
    public class SearchService : ISearchService
    {
        // Private fields to hold references to the repositories.
        private readonly IIndexRepository invertedIndexRepository;
        private readonly IDocumentRepository documentRepository;

        // Constructor to inject dependencies for the repositories.
        public SearchService(IIndexRepository invertedIndexRepository, IDocumentRepository documentRepository)
        {
            this.invertedIndexRepository = invertedIndexRepository; // Assigning the injected index repository to the local field.
            this.documentRepository = documentRepository; // Assigning the injected document repository to the local field.
        }

        // Method to search for documents based on the search text and pagination parameters.
        public SearchResultDto Search(string searchText, int pageSize, int pageNumber)
        {
            // Retrieve the tokenizer type, frequency allowance, and stemming option from the inverted index repository.
            var tokenizerType = invertedIndexRepository.GetTokenizerType();
            var isALlowedFrequency = invertedIndexRepository.GetIsAllowedFrequency();
            var isWithStemming = invertedIndexRepository.GetIsWithStemming();

            // Tokenize the search text using the retrieved tokenizer type and frequency option.
            var terms = EnglishTokenizer.Tokenize(searchText, tokenizerType, isALlowedFrequency);

            // If stemming is enabled, apply stemming to each term.
            if (isWithStemming)
            {
                terms = terms.Select(term => EnglishStemmer.Stem(term)).ToArray();
            }

            // Retrieve the inverted index from the repository.
            var invertedIndex = invertedIndexRepository.GetIndex();

            // Dictionary to keep track of document scores (i.e., how many search terms each document contains).
            var documentScores = new Dictionary<int, int>();

            // Iterate over each term in the tokenized search text.
            foreach (string term in terms)
            {
                // If the term exists in the inverted index, get the list of document IDs (postings) that contain the term.
                if (invertedIndex.ContainsKey(term))
                {
                    var postings = invertedIndex[term];
                    foreach (var documentKey in postings)
                    {
                        // Increment the document's score if it already exists in the dictionary.
                        if (documentScores.ContainsKey(documentKey))
                        {
                            documentScores[documentKey]++;
                        }
                        else
                        // Otherwise, add the document to the dictionary with an initial score of 1.
                        {
                            documentScores[documentKey] = 1;
                        }
                    }
                }
            }

            // Calculate the total number of relevant documents found.
            var relevantDocumentsCount = documentScores.Count();

            // Calculate the number of pages needed to display all results based on the page size.
            var numOfPages = Math.Ceiling(relevantDocumentsCount / (pageSize * 1f));

            // Retrieve the documents from the repository.
            Dictionary<int, string> documents = documentRepository.GetDocuments();

            // Sort the documents by score in descending order, apply pagination, and convert to a list of DTOs.
            var resultDocuments = documentScores.OrderByDescending(k => k.Value)
                                                  .Select(k => new DocumentResponseDto { Score = k.Value, Text = documents[k.Key] })
                                                  .Skip(pageSize * (pageNumber - 1)) // Skip the documents that belong to previous pages.
                                                  .Take(pageSize) // Take only the documents for the current page.
                                                  .ToList();

            // Return the search results, including the documents, current page, total pages, and total relevant documents count.
            return new SearchResultDto()
            {
                Documents = resultDocuments,
                CurrentPage = pageNumber,
                NumOfPages = (int)numOfPages,
                RelevantDocumentsCount = relevantDocumentsCount,
            };
        }
    }
}
