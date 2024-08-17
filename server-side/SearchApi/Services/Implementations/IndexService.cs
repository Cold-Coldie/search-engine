using SearchApi.Repositories.Interfaces; // Importing the interfaces for the repositories.
using SearchApi.Services.Interfaces; // Importing the interface for the service.
using SearchApi.Utills; // Importing utilities like CSVReader, EnglishTokenizer, and EnglishStemmer.

namespace SearchApi.Services.Implementations
{
    // Implementation of the IIndexService interface.
    public class IndexService : IIndexService
    {
        // Private fields to hold references to the repositories and file service.
        private readonly IIndexRepository invertedIndexRepository;
        private readonly IDocumentRepository documentRepository;
        private readonly IFileService fileService;

        // Constructor to inject dependencies for the repositories and file service.
        public IndexService(IIndexRepository invertedIndexRepository, IDocumentRepository documentRepository, IFileService fileService)
        {
            this.invertedIndexRepository = invertedIndexRepository; // Assigning the injected index repository to the local field.
            this.documentRepository = documentRepository; // Assigning the injected document repository to the local field.
            this.fileService = fileService; // Assigning the injected file service to the local field.
        }

        // Method to build the inverted index from a CSV file.
        public async Task BuildInvertedIndexByCSVFile(IFormFile file, string tokenizerType, bool isAllowedFrequency, bool isWithStemming)
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
                    // If the term doesn't exist, create a new entry with the document ID.
                    {
                        invertedIndex[term] = new List<int> { key };
                    }
                }
            }

            // Store the built inverted index in the index repository with the relevant options.
            invertedIndexRepository.Set(invertedIndex, isWithStemming, tokenizerType, isAllowedFrequency);
        }
    }
}
