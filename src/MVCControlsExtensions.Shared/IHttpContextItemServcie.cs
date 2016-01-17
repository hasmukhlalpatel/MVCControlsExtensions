using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCControlsExtensions.Shared
{
    public interface IHttpContextItemServcie
    {
        T GetItem<T>(object key);
    }

    public interface IHttpContextCacheServcie
    {
        T GetItem<T>(string key);

        void SetItem<T>(string key, T value);

    }

    public interface ICacheDataProvider<TV>
    {
        TV GetValue(string key, Func<TV> defaultValueFactory = null);
        void SetValue(string key, TV value);
    }
}
