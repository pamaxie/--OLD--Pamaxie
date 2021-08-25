using System;
using Pamaxie.Data;
using Pamaxie.Database.Design;

namespace Pamaxie.Database.Extensions.Client
{
    /// <summary>
    /// TODO
    /// </summary>
    /// <typeparam name="T">TODO</typeparam>
    internal class ClientDataServiceBase<T> : IDataServiceBase<T> where T : IDatabaseObject
    {
        /// <summary>
        /// Data Context responsible for connecting to Pamaxie
        /// </summary>
        internal PamaxieDataContext DataContext { get; set; }

        /// <summary>
        /// The Service that should be used to connect to the database
        /// </summary>
        internal DatabaseService Service { get; init; }

        /// <inheritdoc/>
        public T Get(string key)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public T Create(T value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool TryCreate(T value, out T createdValue)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public T Update(T value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool TryUpdate(T value, out T updatedValue)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool UpdateOrCreate(T value, out T databaseValue)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool Delete(T value)
        {
            throw new NotImplementedException();
        }
    }
}