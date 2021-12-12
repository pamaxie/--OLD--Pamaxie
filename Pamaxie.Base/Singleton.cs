using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamaxie.Base
{
    /// <summary>
    /// This is used to implement a singleton pattern in a class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : class, new()
    {
        private static readonly T instance = new T();

        static Singleton() { }

        public Singleton(){}

        /// <summary>
        /// Instance of the Singleton item type
        /// </summary>
        public static T Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
