namespace LuhnAlgorithim.Models
{
    public class FormatType
    {
        public string abbr {get; set;}
        public string Issuer {get; set;}
        public int[] IINRange {get; set;}
        public int IINMetaRangeStart {get; set;}
        public int IINMetaRangeEnd {get; set;}
        public int[] LengthOfDigits {get; set;}
    }
};