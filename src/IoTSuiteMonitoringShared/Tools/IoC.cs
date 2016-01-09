using System;
using System.Collections.Generic;

namespace Danvy.Tools
{
    public class IoC
    {
        private Dictionary<Type, object> registered = new Dictionary<Type, object>();
        private static readonly IoC instance = new IoC();
        private IoC()
        {

        }
        public static IoC Instance
        {
            get
            {
                return instance;
            }
        }
        public void Register<T>(Func<T> f) where T : class
        {
            if (registered.ContainsKey(typeof(T)))
                return;
            registered.Add(typeof(T), f);
        }
        public void Register<T>(Object o) where T : class
        {
            if (registered.ContainsKey(typeof(T)))
                return;
            registered.Add(typeof(T), o);
        }
        public T Resolve<T>()
        {
            object o = null;
            if (registered.TryGetValue(typeof(T), out o))
            {
                if (o is Func<T>)
                {
                    return (o as Func<T>).Invoke();
                }
                else
                {
                    return (T)o;
                }
            }
            return default(T);
        }
    }
}
