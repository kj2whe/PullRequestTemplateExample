using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LuhnAlgorithim.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Type {
        American_Express, //	34, 37	15
        China_Union_Pay, // 62 16-19
        Diners_Club_Carte_Blanche, //	300, 301, 302, 303, 304, 305	14
        Diners_Club_International, //	36	14
        Diners_Club_USA_And_Canada, //	54	16
        Discover, //	6011, 622126 to 622925, 644, 645, 646, 647, 648, 649, 65	16-19
        InstaPayment, //	637, 638, 639	16
        JCB, //	3528 to 3589	16-19
        Maestro, //	5018, 5020, 5038, 5893, 6304, 6759, 6761, 6762, 6763	16-19
        MasterCard, //	51, 52, 53, 54, 55, 222100-272099	16
        Visa, //	4	13-16-19
        Visa_Electron,
    };
}