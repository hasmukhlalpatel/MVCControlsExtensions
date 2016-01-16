using System;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace MVCControls.Core
{
    /// <summary>
    /// Generic Cache data provider.
    /// </summary>
    /// <typeparam name="T">service type to get the data</typeparam>
    public class CacheDataProvider<T>
    {
        /// <summary>
        /// The lazy service
        /// </summary>
        protected Lazy<T> LazyService = new Lazy<T>(() => DependencyResolver.Current.GetService<T>());

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <value>
        /// The service.
        /// </value>
        public T Service { get { return LazyService.Value; } }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private object GetData(string key)
        {
            return HttpContext.Current.Cache[key];
        }

        /// <summary>
        /// Sets the data.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="obj">The object.</param>
        private void SetData(string key, object obj)
        {
            HttpContext.Current.Cache.Insert(key, obj, CacheDependency, AbsoluteExpiration, SlidingExpiration);
        }

        /// <summary>
        /// Clears the cache data.
        /// </summary>
        /// <param name="key">The key.</param>
        protected void ClearCacheData(string key)
        {
            HttpContext.Current.Cache.Remove(key);
        }

        /// <summary>
        /// Gets the or add cache data.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="valueFactory">The value factory.</param>
        /// <returns></returns>
        protected TValue GetOrAddCacheData<TValue>(string key, Func<string, TValue> valueFactory)
        {
            var data = GetData(key);
            if (data == null)
            {
                var newData = valueFactory(key);
                SetData(key, newData);
                return newData;
            }
            return (TValue)data;
        }

        /// <summary>
        /// Gets the cache dependency.
        /// </summary>
        /// <value>
        /// The cache dependency.
        /// </value>
        protected virtual CacheDependency CacheDependency
        {
            get { return null; }
        }

        /// <summary>
        /// Gets the absolute expiration.
        /// </summary>
        /// <value>
        /// The absolute expiration.
        /// </value>
        protected virtual DateTime AbsoluteExpiration
        {
            get { return DateTime.Now.AddMinutes(30); }
        }

        /// <summary>
        /// Gets the sliding expiration.
        /// </summary>
        /// <value>
        /// The sliding expiration.
        /// </value>
        protected virtual TimeSpan SlidingExpiration
        {
            get { return Cache.NoSlidingExpiration; }
        }
    }
}