using System.ComponentModel;

namespace Pamaxie.Data
{
    /// <summary>
    /// Language code used for SMS and website. Basically what we use to send out specific things.
    /// </summary>
    public enum LanguageCode
    {
        [Description("German")] DE = 1,
        [Description("English")] EN = 2,
        [Description("Danish")] DK = 3,
        [Description("Spanish")] ES = 4
    }
}