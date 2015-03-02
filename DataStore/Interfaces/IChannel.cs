using System;
using System.Collections.Generic;

namespace Embark.Interfaces
{
    /// <summary>
    /// All commands to insert/get/update/delete documents
    /// </summary>
    public interface IChannel
    {
        // Basic
        long Insert<T>(string tag, T something) where T : class;
        T Get<T>(string tag, long id) where T : class;
        bool Update<T>(string tag, T something) where T : class;
        bool Delete(string tag, long id);

        // Range
        List<T> GetWhere<T>(string tag, T newValue, T oldValue, T optionalEndrange = null) where T : class;
        int UpdateWhere<T>(string tag, T newValue, T oldValue, T optionalEndrange = null) where T : class;
        int DeleteWhere<T>(string tag, T newValue, T oldValue, T optionalEndrange = null) where T : class;
    }
}
