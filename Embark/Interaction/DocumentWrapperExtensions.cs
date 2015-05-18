using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Embark.Interaction
{
    /// <summary>
    /// IEnumerable DocumentWrapper Extension methods
    /// </summary>
    public static class DocumentWrapperExtensions
    {
        /// <summary>
        /// Return only the deserialized objects within a wrapped document request
        /// </summary>
        /// <typeparam name="T">The type of wrapped documents</typeparam>
        /// <param name="documents">Documents with ID/Timestamp info</param>
        /// <returns>The Object Contents of the DocumentWrappers</returns>
        public static IEnumerable<T> Unwrap<T>(this IEnumerable<DocumentWrapper<T>> documents)
        {
            return documents.Select(doc => doc.Content);
        }

        internal static IEnumerable<T> UnwrapWithIDs<T>(this IEnumerable<DocumentWrapper<T>> documents)
            where T : class, IDataEntry
        {
            return documents.Select(doc =>
                {
                    doc.Content.ID = doc.ID;
                    return doc.Content;
                });
        }
    }
}
