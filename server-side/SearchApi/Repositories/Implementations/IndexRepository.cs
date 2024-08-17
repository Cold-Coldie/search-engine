using SearchApi.Repositories.Interfaces;

namespace SearchApi.Repositories.Implementations
{
    /// <summary>
    /// The IndexRepository class manages the inverted index used for document retrieval.
    /// It implements the IIndexRepository interface.
    /// </summary>
    public class IndexRepository : IIndexRepository
    {
        // The inverted index is a dictionary where the key is a token (word)
        // and the value is a list of document IDs in which the token appears.
        private Dictionary<string, List<int>> invertedIndex = new Dictionary<string, List<int>>();

        // Indicates whether stemming is used in the indexing process.
        public bool IsWithStemming { get; private set; }

        // Indicates whether frequency of tokens is considered in the indexing process.
        public bool IsAllowedFrequency { get; private set; }

        // Specifies the type of tokenizer used for tokenizing documents.
        public string TokenizerType { get; private set; } = string.Empty;

        /// <summary>
        /// Sets the inverted index and associated properties.
        /// </summary>
        /// <param name="newInvertedIndex">A dictionary representing the new inverted index.</param>
        /// <param name="isWithStemming">Indicates if stemming is applied.</param>
        /// <param name="tokenizerType">Specifies the type of tokenizer used.</param>
        /// <param name="isAllowedFrequency">Indicates if token frequency is considered.</param>
        public void Set(Dictionary<string, List<int>> newInvertedIndex, bool isWithStemming, string tokenizerType, bool isAllowedFrequency)
        {
            invertedIndex = newInvertedIndex;
            IsWithStemming = isWithStemming;
            TokenizerType = tokenizerType;
            IsAllowedFrequency = isAllowedFrequency;
        }

        /// <summary>
        /// Retrieves the current inverted index.
        /// </summary>
        /// <returns>A dictionary representing the inverted index.</returns>
        public Dictionary<string, List<int>> GetIndex()
        {
            return invertedIndex;
        }

        /// <summary>
        /// Indicates whether stemming is used in the index.
        /// </summary>
        /// <returns>True if stemming is applied, otherwise false.</returns>
        public bool GetIsWithStemming()
        {
            return IsWithStemming;
        }

        /// <summary>
        /// Retrieves the type of tokenizer used in the index.
        /// </summary>
        /// <returns>A string representing the tokenizer type.</returns>
        public string GetTokenizerType()
        {
            return TokenizerType;
        }

        /// <summary>
        /// Indicates whether token frequency is considered in the index.
        /// </summary>
        /// <returns>True if token frequency is considered, otherwise false.</returns>
        public bool GetIsAllowedFrequency()
        {
            return IsAllowedFrequency;
        }
    }
}
