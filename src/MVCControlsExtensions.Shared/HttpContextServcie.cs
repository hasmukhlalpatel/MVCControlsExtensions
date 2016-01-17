
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace MVCControlsExtensions.Shared
{
    using System.Web;
    using System.Web.Caching;

    public abstract class HttpContextServcie
    {
        protected HttpContext Context { get { return HttpContext.Current; } }
    }

    public class HttpContextItemServcie : HttpContextServcie, IHttpContextItemServcie
    {
        public T GetItem<T>(object key)
        {
            return (T)Context.Items[key];
        }
    }

    public abstract class CacheDataProviderBase : HttpContextServcie
    {
        protected object GetValue(string key)
        {
            return Context.Cache[key];
        }

        protected void SetValue(string key, object value)
        {
            Context.Cache.Insert(key, value, Dependencies, AbsoluteExpiration, SlidingExpiration);
        }

        protected object RemoveValue(string key)
        {
            return Context.Cache.Remove(key);
        }

        protected virtual CacheDependency Dependencies
        {
            get { return null; }
        }

        protected virtual DateTime AbsoluteExpiration
        {
            get { return DateTime.Now.AddMinutes(30); }
        }

        protected virtual TimeSpan SlidingExpiration
        {
            get { return Cache.NoSlidingExpiration; }
        }
    }


    public class HttpContextCacheServcie : CacheDataProviderBase, IHttpContextCacheServcie
    {
        public T GetItem<T>(string key)
        {
            return (T)GetValue(key);
        }

        public void SetItem<T>(string key, T value)
        {
            SetValue(key, value);
        }
    }


    public abstract class CacheDataProviderBase<TS, TV> : HttpContextCacheServcie, ICacheDataProvider<TV>
    {
        protected readonly Lazy<TS> Service;// = new Lazy<TS>(() => DependencyResolver.Current.Resolve<TS>());

        protected string GetCombinedKey(string key)
        {
            return string.Format("{0}_{1}", ProviderKey, key);
        }

        public virtual TV GetValue(string key, Func<TV> defaultValueFactory = null)
        {
            var combinedKey = GetCombinedKey(key);
            var cachedvalue = base.GetValue(combinedKey);

            if (cachedvalue == null && defaultValueFactory != null)
            {
                var newValue = defaultValueFactory();
                base.SetValue(combinedKey, newValue);
                return newValue;
            }

            return (TV)cachedvalue;
        }

        public virtual void SetValue(string key, TV value)
        {
            base.SetValue(GetCombinedKey(key), value);
        }

        protected abstract string ProviderKey { get; }

    }

    public class CustomCacheDataProvider : CacheDataProviderBase<IMyService, MyData>
    {
        protected override string ProviderKey { get { return "Uniqe name"; } }

        public override MyData GetValue(string key, Func<MyData> defaultValueFactory = null)
        {
            defaultValueFactory = defaultValueFactory ?? (() => Service.Value.GetData(key));
            return base.GetValue(key, defaultValueFactory);
        }
    }

    public class MyData
    {
    }

    public interface IMyService
    {
        MyData GetData(string id);
    }
    //implement your service and inject in mvc
}
