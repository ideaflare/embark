using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Embark.Interaction
{
    /// <summary>
    /// Helper methods dealing with serialized byte[] and query IO
    /// </summary>
    public static class TypeConversion
    {
        /// <summary>
        /// Deserialize a serialized byte[] 
        /// </summary>
        public static byte[] ToByteArray(this object blob)
        {
            var bx = blob as IEnumerable;

            if (bx == null)
                throw new NotImplementedException(blob.GetType().Name + " object does not implement IEnumerable");

            var bytes = bx.Cast<int>()
                .Select(i => Convert.ToByte(i))
                .ToArray();

            return bytes;

            //var list = ((ArrayList)blob);
            //int[] deserialized = (int[])list.ToArray(typeof(int));
            //return deserialized
            //    .Select(i => Convert.ToByte(i))
            //    .ToArray();
        }

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

        //internal static IEnumerable<T> WithAutoSaveEnabled<T>(this IEnumerable<T> dataEntries, DataEntryCollection<T> dataEntryCollection)
        //    where T : class, IDataEntry
        //{
        //    return dataEntries.Select(entry =>
        //    {
        //        if(entry.dataEntryCollection == null)
        //            entry.dataEntryCollection = dataEntryCollection
        //        entry.Autosave = true;
        //        return entry;
        //    });
        //}
    }
}
