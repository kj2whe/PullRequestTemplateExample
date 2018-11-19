using System.Collections.Generic;

namespace LuhnAlgorithim.Models
{
    public class FormatType
    {
        public string abbr {get; set;}
        public string Issuer {get; set;}
        public List<int> IINRange {get; set;}
        public int IINMetaRangeStart {get; set;}
        public int IINMetaRangeEnd {get; set;}
        public int[] LengthOfDigits {get; set;}

        public void ExplodeIINRange()
        {
            if(IINMetaRangeStart == 0 && IINMetaRangeEnd == 0)
                return;
                
            if(IINRange == null)
                IINRange = new List<int>();

            for(int i = this.IINMetaRangeStart; i <= this.IINMetaRangeEnd; i++)
            {
                IINRange.Add(i);
            }
        }
    }
};