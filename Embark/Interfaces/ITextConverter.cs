using Embark.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embark.Interfaces
{
    /// <summary>
    /// Convert between POCO objects and text.
    /// </summary>
    public interface ITextConverter
    {
        /// <summary>
        /// Convert text to object
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="text">String containing POCO data</param>
        /// <returns>Object of type T</returns>
        T ToObject<T>(string text);

        /// <summary>
        /// Convert a POCO object to text
        /// </summary>
        /// <param name="obj">POCO object to convert</param>
        /// <returns>Text representing the object</returns>
        string ToText(object obj);

        /// <summary>
        /// Get matches that have similar property values to an example object.
        /// </summary>
        /// <param name="searchObject">Object to compare against</param>
        /// <param name="compareValues">Compare values collected for the <see cref="ITextDataStore"/></param>
        /// <returns>Objects from the collection that match the search criterea</returns>
        IEnumerable<string> GetLikeMatches(string searchObject, IEnumerable<string> compareValues);

        /// <summary>
        /// Get matches whose property values are between values of a start and end example object
        /// </summary>
        /// <param name="startRange">The first object to compare against</param>
        /// <param name="endRange">A second object to comare values agianst to check if search is between example values</param>
        /// <param name="compareValues">Compare values collected for the <see cref="ITextDataStore"/></param>
        /// <returns>Objects from the collection that match the search criterea</returns>
        IEnumerable<DataEnvelope> GetBetweenMatches(string startRange, string endRange, IEnumerable<DataEnvelope> compareValues);
    }
}
