using System.ComponentModel;

namespace Pamaxie.Data
{
    public enum DomainType
    {
        [Description("Advertising")] Advertising,
        [Description("Hostile")] Hostile,
        [Description("Banking")] Banking,
        [Description("Bitcoin")] Bitcoin,
        [Description("CryptoJacking")] CryptoJacking,
        [Description("DDos")] DDos,
        [Description("Drug")] Drug,
        [Description("Gambling")] Gambling,
        [Description("Hacking")] Hacking,
        [Description("Marketing")] Marketing,
        [Description("MixedAdult")] MixedAdult,
        [Description("Phishing")] Phishing,
        [Description("Pornographic")] Pornographic,
        [Description("Redirector")] Redirector,
        [Description("Warez")] Warez,
        [Description("IpGrabber")] IpGrabber,
        [Description("Weapons")] Weapons,
        [Description("Military")] Military,
        [Description("Mallicious")] Mallicious
    }
}