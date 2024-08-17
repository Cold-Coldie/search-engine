namespace SearchApi.Utills
{
    /// <summary>
    /// A utility class for reading CSV files and storing the content in a dictionary.
    /// </summary>
    public class CSVReader
    {
        /// <summary>
        /// Reads a CSV file from the specified file path and returns the content as a dictionary.
        /// </summary>
        /// <param name="filePath">The path to the CSV file.</param>
        /// <returns>A dictionary where the key is the line number (starting from 0) and the value is the line content.</returns>
        public static Dictionary<int, string> Read(string filePath)
        {
            // Initialize a dictionary to hold the line number and corresponding line content.
            var values = new Dictionary<int, string>();

            // Create a StreamReader to read the file from the specified path.
            using (StreamReader reader = new StreamReader(filePath))
            {
                int lineNumber = 0; // Variable to track the current line number.

                // Continue reading until the end of the file is reached.
                while (!reader.EndOfStream)
                {
                    // Read a line from the file.
                    string line = reader.ReadLine();

                    // Add the line to the dictionary with the line number as the key.
                    values.Add(lineNumber++, line);
                }
            }

            // Return the populated dictionary.
            return values;
        }
    }
}
