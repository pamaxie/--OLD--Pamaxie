using System;

namespace Pamaxie.Database.Design
{
    /// <summary>
    /// Interface that defines how a database extension has to look like
    /// </summary>
    public interface IDataServiceBase<T>
    {
        /// <summary>
        /// Gets a value from the database
        /// </summary>
        /// <param name="key">The key of the value in the database (unique value like a hash to identify the value)</param>
        /// <returns></returns>
        public T Get(string key);

        /// <summary>
        /// Creates a new Entry in the database, throws exception inside the database if the value already exists
        /// </summary>
        /// <param name="value">The value that should be created</param>
        /// <returns>The created database object</returns>
        /// <exception cref="ArgumentException">The value does already exist in the database</exception>
        public T Create(T value);
        
        /// <summary>
        /// Creates a new Entry in the database
        /// </summary>
        /// <param name="value">The value that should be created</param>
        /// <param name="createdValue">How the value looks like inside the database</param>
        /// <returns><see cref="bool"/> if the operation was successful and the entry was created</returns>
        public bool TryCreate(T value, out T createdValue);
        
        /// <summary>
        /// Updates a value inside the database to the value of <see cref="value"/>,
        /// throws an exception if no value with the key of <see cref="value"/> exists
        /// </summary>
        /// <param name="value">The value that should be updated</param>
        /// <returns>The updated value of the database</returns>
        /// <exception cref="ArgumentException">The value does not exist in the database</exception>
        public T Update(T value);

        /// <summary>
        /// Updates a value inside the database to the value of <see cref="updatedValue"/>
        /// </summary>
        /// <param name="value">The value that should be updated</param>
        /// <param name="updatedValue">The updated value of the database</param>
        /// <returns><see cref="bool"/> if the operation was successful and the entry was updated</returns>
        public bool TryUpdate(T value, out T updatedValue);

        /// <summary>
        /// Updates or creates a value inside the database,
        /// returns a <see cref="bool"/> depending if a new value was created inside the database.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="databaseValue"></param>
        /// <returns><see cref="bool"/> if a new value was created</returns>
        /// <exception cref="ArgumentException">if <see cref="value"/> did not contain a valid key</exception>
        public bool UpdateOrCreate(T value, out T databaseValue);

        /// <summary>
        /// Deletes a value inside the database,
        /// returns a <see cref="bool"/> depending if the value was created or not
        /// </summary>
        /// <param name="value">The value that should be deleted</param>
        /// <returns><see cref="bool"/> if the operation was successful and the value was deleted</returns>
        public bool Delete(T value);
    }
}