using System;
using System.Collections;

namespace Assets._Project.Develop.Runtime.Utilities.DataManagment
{
    public interface ISaveLoadService
    {
        IEnumerator LoadAsync<TData>(Action<TData> onLoad) where TData : ISaveData;
        IEnumerator SaveAsync<TData>(TData data) where TData : ISaveData;
        IEnumerator RemoveAsync<TData>() where TData : ISaveData;
        IEnumerator ExistsAsync<TData>(Action<bool> onExistsResult) where TData : ISaveData;
    }
}
