using SearchApi.Repositories.Interfaces;

namespace SearchApi.Repositories.Implementations
{
    /// <summary>
    /// The DocumentRepository class provides methods to manage a collection of documents.
    /// It implements the IDocumentRepository interface.
    /// </summary>
    public class DocumentRepository : IDocumentRepository
    {
        // A dictionary to store documents, where the key is the document ID and the value is the document content.
        private Dictionary<int, string>? documents;

        /// <summary>
        /// Retrieves the collection of documents.
        /// </summary>
        /// <returns>A dictionary containing all the documents with their IDs.</returns>
        public Dictionary<int, string> GetDocuments()
        {
            return documents;
        }

        /// <summary>
        /// Sets the collection of documents to a new set of documents.
        /// </summary>
        /// <param name="newDocuments">A dictionary of documents to replace the current collection.</param>
        public void Set(Dictionary<int, string> newDocuments)
        {
            documents = newDocuments;
        }
    }
}
