using Newtonsoft.Json;
using PamaxieML.Model;
using StackExchange.Redis;
using System;

namespace Pamaxie.Database.Redis.DataClasses
{
    /// <summary>
    /// Value pair for storing image Values
    /// </summary>
    public class MediaPredictionData
    {

        /// <summary>
        /// Creating a new Medida Pediction Data instance
        /// </summary>
        /// <param name="mediaHash"></param>
        public MediaPredictionData(string mediaHash)
        {
            MediaHash = mediaHash;
        }

        private MediaData _data;

        /// <summary>
        /// Image hash for reading the data from the Database
        /// </summary>
        private string MediaHash { get; }

        /// <summary>
        /// The data for the image
        /// WARNING: Setting values is directly setting values inside of the database.
        /// WARNING: Getting values initially automatically loads the values from the database.
        /// </summary>
        public MediaData Data
        {
            get
            {
                if (_data != null) return _data;

                if (MediaHash == null) throw new InvalidOperationException("Cannot get Data when image hash is null or not set");
                
                IDatabase db = RedisData.Redis.GetDatabase();

                //The hash set does not exist in the Database.
                if (!db.KeyExists(MediaHash)) return null;
                


                RedisValue rawData = db.StringGet(MediaHash);
                MediaData data = JsonConvert.DeserializeObject<MediaData>(rawData);

                //Reset the expiration of the key to 90 days.
                db.KeyExpire(MediaHash, new TimeSpan(90, 0, 0, 0, 0), CommandFlags.FireAndForget);
                _data = data;
                return data;
            }
            set
            {
                if (string.IsNullOrEmpty(MediaHash))
                {
                    throw new InvalidOperationException("Cannot set Data when image Hash is null or not set");
                }
                if (Equals(_data, value)) return;

                IDatabase db = RedisData.Redis.GetDatabase();
                value.LastUpdated = DateTime.Now;
                string mediaData = JsonConvert.SerializeObject(value);

                //Delete the cached files after 90 days to increase performance and prevent bloating
                db.StringSet(MediaHash, mediaData, new TimeSpan(90, 0, 0, 0, 0), When.Always, CommandFlags.FireAndForget);
                _data = value;
            }
        }

        /// <summary>
        /// Tries to load the data for the prediction if it can't returns false
        /// </summary>
        public bool TryLoadData(out MediaData data)
        {
            data = new MediaData();
            try
            {
                IDatabase db = RedisData.Redis.GetDatabase();
                RedisValue rawData = db.StringGet(MediaHash);
                if (string.IsNullOrEmpty(rawData)) return false;

                data = JsonConvert.DeserializeObject<MediaData>(rawData);
                _data = data;
                return true;
            }
            catch (TypeInitializationException exception)
            {
                Console.WriteLine("An exception occured trying to initialise " + exception.Source);
                Console.WriteLine(exception.Message);
                return false;
            }
        }
    }

    public class MediaData
    {

        /// <summary>
        /// Specifies the detected properties in an image / file
        /// </summary>
        public OutputProperties DetectedLabels { get; set; }

        /// <summary>
        /// Set when the key gets updated
        /// </summary>
        public DateTime LastUpdated { get; internal set; }

        /// <summary>
        /// Specifies the version of the neural network / AI used for scanning
        /// </summary>
        public ushort NetworkVersion { get; private set; } = 1;
    }
}
