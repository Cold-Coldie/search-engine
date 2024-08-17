using SearchApi.Repositories.Interfaces;
using SearchApi.Services.Interfaces;
using SearchApi.Utills;

namespace SearchApi.Services.Implementations
{
    /// <summary>
    /// Implementation of the IIndexService interface.
    /// This service provides functionality to build an inverted index from a CSV file.
    /// </summary>
    public class IndexService : IIndexService
    {
        // Private fields to hold references to the repositories and file service.
        private readonly IIndexRepository invertedIndexRepository;
        private readonly IDocumentRepository documentRepository;
        private readonly IFileService fileService;

        /// <summary>
        /// Constructor to inject dependencies for the repositories and file service.
        /// </summary>
        /// <param name="invertedIndexRepository">Repository to store and retrieve the inverted index.</param>
        /// <param name="documentRepository">Repository to store and retrieve documents.</param>
        /// <param name="fileService">Service to handle file storage operations.</param>
        public IndexService(
            IIndexRepository invertedIndexRepository,
            IDocumentRepository documentRepository,
            IFileService fileService)
        {
            // Assigning the injected dependencies to the local fields.
            this.invertedIndexRepository = invertedIndexRepository;
            this.documentRepository = documentRepository;
            this.fileService = fileService;
        }

        /// <summary>
        /// Builds the inverted index from a CSV file.
        /// </summary>
        /// <param name="file">The CSV file containing the documents.</param>
        /// <param name="tokenizerType">Type of tokenizer to use (e.g., word, character).</param>
        /// <param name="isAllowedFrequency">Flag to determine if frequency information should be included.</param>
        /// <param name="isWithStemming">Flag to determine if stemming should be applied to the terms.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task BuildInvertedIndexByCSVFile(
            IFormFile file,
            string tokenizerType,
            bool isAllowedFrequency,
            bool isWithStemming)
        {
            // Store the uploaded file locally and get the file path.
            string filePath = await fileService.StoreImageToLocalFolder(file);

            // Read the CSV file and store the documents in a dictionary with the document ID as the key.
            Dictionary<int, string> documents = CSVReader.Read(filePath);

            // Store the documents in the document repository.
            documentRepository.Set(documents);

            // Initialize the dictionary to hold the inverted index.
            var invertedIndex = new Dictionary<string, List<int>>();

            // Iterate over each document in the collection.
            foreach (var key in documents.Keys)
            {
                // Tokenize the document's text based on the specified tokenizer type and frequency option.
                var terms = EnglishTokenizer.Tokenize(documents[key], tokenizerType, isAllowedFrequency);

                // If stemming is enabled, apply stemming to each term.
                if (isWithStemming)
                {
                    terms = terms.Select(term => EnglishStemmer.Stem(term)).ToArray();
                }

                // Iterate over each term in the document.
                foreach (string term in terms)
                {
                    // If the term already exists in the inverted index, add the document ID to its list.
                    if (invertedIndex.ContainsKey(term))
                    {
                        invertedIndex[term].Add(key);
                    }
                    else
                    {
                        // If the term doesn't exist, create a new entry with the document ID.
                        invertedIndex[term] = new List<int> { key };
                    }
                }
            }

            // Store the built inverted index in the index repository with the relevant options.
            invertedIndexRepository.Set(invertedIndex, isWithStemming, tokenizerType, isAllowedFrequency);
        }
    }
}
