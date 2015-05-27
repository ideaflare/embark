namespace Embark.TextConversion
{
    /// <summary>
    /// Convert between objects and text.
    /// </summary>
    public interface ITextConverter
    {
        /// <summary>
        /// Convert an object to text
        /// </summary>
        /// <param name="obj">Object to convert</param>
        /// <returns>Text representing the object</returns>
        string ToText(object obj);

        /// <summary>
        /// Convert text to object
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="text">String containing object data</param>
        /// <returns>Object of type T</returns>
        T ToObject<T>(string text);

        /// <summary>
        /// Convert text to an object used in GetLike and GetBetween matches methods
        /// </summary>
        /// <param name="text">Object text serialized with ToText method</param>
        /// <returns>Object used for comparison in search queries</returns>
        object ToComparisonObject(string text);
        
        /// <summary>
        /// Get matches that have similar property values to an example object.
        /// </summary>
        /// <param name="searchObject">Object to compare against</param>
        /// <param name="comparisonObject">Candidate match agianst search object</param>
        /// <returns>True if comparisonObject matches the search criterea</returns>
        bool IsMatch(object searchObject, object comparisonObject);

        /// <summary>
        /// Get matches whose property values are between values of a start and end example object
        /// </summary>
        /// <param name="startRange">The first object to compare against</param>
        /// <param name="endRange">A second object to comare values agianst to check if search is between example values</param>
        /// <param name="compareValue">Compare value to match between startRange and endRange</param>
        /// <returns>True if compareValue matches the search criterea</returns>
        bool IsBetweenMatch(object startRange, object endRange, object compareValue);
    }
}
