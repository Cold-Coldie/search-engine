using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace SearchApi.Utills
{
    /// <summary>
    /// A class that provides methods for tokenizing English text.
    /// </summary>
    public static class EnglishTokenizer
    {
        /// <summary>
        /// A set of common English stop words to exclude from tokenization.
        /// </summary>
        private static readonly HashSet<string> StopWords = new HashSet<string>()
        {
            "a", "an", "the", "is", "and", "its", "of", "to", "in", "for", "on", "with",
            "as", "at", "by", "from", "up", "but", "so", "not", "only", "too", "here",
            "there", "when", "why", "how", "all", "any", "both", "each", "more", "most",
            "other", "some", "such", "no", "own", "same", "than", "that", "these",
            "this", "which", "you", "he", "him", "his", "she", "her", "it", "we",
            "us", "our", "they", "them", "their", "i", "me", "my", "myself",
            "your", "yourself", "he", "himself", "she", "herself", "we", "ourselves", "us",
            "ourselves", "they", "themselves"
        };

        /// <summary>
        /// Tokenizes the given text into a collection of words based on the specified tokenizer type.
        /// </summary>
        /// <param name="text">The text to tokenize.</param>
        /// <param name="tokenizerType">The type of tokenization to apply. If "without-stop-words", stop words will be excluded.</param>
        /// <param name="isAllowedFrequency">Determines whether word frequency is allowed. If false, duplicate words will be removed.</param>
        /// <returns>A collection of tokenized words.</returns>
        public static IEnumerable<string> Tokenize(string text, string tokenizerType, bool isAllowedFrequency)
        {
            // Define the pattern to split the text into tokens.
            string pattern = @"[\W_]+";

            // Split the text into tokens, remove whitespace, and convert to lowercase.
            IEnumerable<string> tokens = Regex.Split(text, pattern)
                        .Where(word => !string.IsNullOrWhiteSpace(word))
                        .Select(word => word.ToLower())
                        .ToList();

            // If the tokenizer type specifies to exclude stop words, filter them out.
            if (tokenizerType == "without-stop-words")
            {
                tokens = tokens
                    .Where(word => !StopWords.Contains(word))
                    .ToList();
            }

            // If frequency is not allowed, convert tokens to a HashSet to remove duplicates.
            if (!isAllowedFrequency)
            {
                tokens = tokens.ToHashSet();
            }

            return tokens;
        }
    }
}
