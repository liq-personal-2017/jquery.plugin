using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jquery.plugin.CommonClass
{
    public class CacheUtil
    {

        private readonly static object lockobj = new object();

        /// <summary>
        /// 获取当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <returns></returns>
        public static object GetCache(string CacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[CacheKey];
        }
        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <param name="objObject"></param>
        public static void SetCache(string CacheKey, object objObject)
        {
            lock (lockobj)
            {
                System.Web.Caching.Cache objCache = HttpRuntime.Cache;
                objCache.Insert(CacheKey, objObject);
            }
        }

        public static void Clear()
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            var enumrator = HttpRuntime.Cache.GetEnumerator();
            while (enumrator.MoveNext())
            {
                objCache.Remove(enumrator.Key.ToString());
            }
        }


    }
}