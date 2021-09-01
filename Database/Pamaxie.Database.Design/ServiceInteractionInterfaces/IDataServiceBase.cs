using System;

namespace Pamaxie.Database.Design
{
    /// <summary>
    /// Interface that defines integrations to a service
    /// </summary>
    public interface IDataServiceBase<T>
    {
        /// <summary>
        /// Gets a <see cref="T"/> value from the service
        /// </summary>
        /// <param name="key">The key of the <see cref="T"/> value in the service (unique value like a hash to identify the value)</param>
        /// <returns>Returns a <see cref="T"/> value</returns>
        public T Get(string key);

        /// <summary>
        /// Creates a new <see cref="T"/> value in the service, throws exception inside the service if the value already exists
        /// </summary>
        /// <param name="value">The value that should be created</param>
        /// <returns>The created <see cref="T"/> value</returns>
        /// <exception cref="ArgumentException">The value already exist in the service</exception>
        public T Create(T value);

        /// <summary>
        /// Creates a new <see cref="T"/> value to the service
        /// </summary>
        /// <param name="value">The <see cref="T"/> value that should be created</param>
        /// <param name="createdValue">How the <see cref="T"/> value looks like inside the service</param>
        /// <returns><see cref="bool"/> if the operation was successful and the <see cref="T"/> value was created</returns>
        public bool TryCreate(T value, out T createdValue);

        /// <summary>
        /// Updates a <see cref="T"/> value inside the service,
        /// throws an exception if no <see cref="T"/> value with the key of <see cref="value"/> exists
        /// </summary>
        /// <param name="value">The <see cref="T"/> value that should be updated</param>
        /// <returns>The updated <see cref="T"/> value of the service</returns>
        /// <exception cref="ArgumentException">The <see cref="T"/> value does not exist in the service</exception>
        public T Update(T value);

        /// <summary>
        /// Updates a <see cref="T"/> value inside the service
        /// </summary>
        /// <param name="value">The <see cref="T"/> value that should be updated</param>
        /// <param name="updatedValue">The updated <see cref="T"/> value of the service</param>
        /// <returns><see cref="bool"/> if the operation was successful and the <see cref="T"/> value was updated</returns>
        public bool TryUpdate(T value, out T updatedValue);

        /// <summary>
        /// Updates or creates a <see cref="T"/> value inside the service,
        /// returns a <see cref="bool"/> depending if a new <see cref="T"/> value was updated or created inside the service.
        /// </summary>
        /// <param name="value">The <see cref="T"/> value that should be updated or created in the service</param>
        /// <param name="updatedOrCreatedValue">The updated or created <see cref="T"/> value of the service</param>
        /// <returns><see cref="bool"/> if a new value was created</returns>
        /// <exception cref="ArgumentException">if <see cref="value"/> did not contain a valid key</exception>
        public bool UpdateOrCreate(T value, out T updatedOrCreatedValue);

        /// <summary>
        /// Deletes a <see cref="T"/> value inside the service,
        /// returns a <see cref="bool"/> depending if the <see cref="T"/> value was deleted or not
        /// </summary>
        /// <param name="value">The <see cref="T"/> value that should be deleted</param>
        /// <returns><see cref="bool"/> if the operation was successful and the <see cref="T"/> value was deleted</returns>
        public bool Delete(T value);
    }
}