using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LuhnAlgorithim.Models;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Serialization;

namespace LuhnAlgorithim.Controllers
{
    [Route("api/[controller]")]
    public class LuhnController : Controller
    {
        const string pool = "0123456789";

        private List<FormatType> _formatTypes;

        private int _lengthOfDigits;
        private int _IIN;
        private FormatType _specificFormatType; 

        public LuhnController()
        {
            _formatTypes = this.GetFormatTypes();
        }


        [Route("GenerateNumber/{formatType}/{formatTypelength}")]
        [HttpGet]
        public string GenerateNumber(string formatType, int formatTypelength)
        {
            _specificFormatType = _formatTypes.SingleOrDefault(x=>x.abbr.Equals(formatType, StringComparison.CurrentCultureIgnoreCase));

            if(_specificFormatType == null){
                return $"You must specify a an appropriate formatType";
            }
            else if(Array.IndexOf(_specificFormatType.LengthOfDigits, formatTypelength) == -1){
                return $"You must specify a valid length for a {_specificFormatType.Issuer}";                    
            }

            _lengthOfDigits = _specificFormatType.LengthOfDigits[Array.IndexOf(_specificFormatType.LengthOfDigits, formatTypelength)];
            _IIN = _specificFormatType.IINRange[GenerateRandom().Next(_specificFormatType.IINRange.Length)]; 

            return GenerateLunhNumber();
        }
    
        [Route("GenerateNumber/{formatType}")]
        [HttpGet]
        public string GenerateNumber(string formatType)
        {
            Random random = GenerateRandom();

            _specificFormatType = _formatTypes.SingleOrDefault(x=>x.abbr.Equals(formatType, StringComparison.CurrentCultureIgnoreCase));

            if(_specificFormatType == null){
                return $"You must specify a an appropriate formatType";
            }

            //  Since no formatTypelength was passed in, randomly choose one from the available choices
            int formatTypelength = _specificFormatType.LengthOfDigits[random.Next(_specificFormatType.LengthOfDigits.Length)]; 

            _lengthOfDigits = _specificFormatType.LengthOfDigits[Array.IndexOf(_specificFormatType.LengthOfDigits, formatTypelength)];
            _IIN = _specificFormatType.IINRange[GenerateRandom().Next(_specificFormatType.IINRange.Length)]; 

            return GenerateLunhNumber();
           
        }

        [HttpGet("[action]")]
        public string GenerateNumber()
        {
            Random random = GenerateRandom();

            //  Since no formatType was passed in, randomly choose one from the available choices
            _specificFormatType = _formatTypes.OrderBy(item => random.Next()).First();

            //  Since no formatTypelength was passed in, randomly choose one from the available choices
            int formatTypelength = _specificFormatType.LengthOfDigits[random.Next(_specificFormatType.LengthOfDigits.Length)]; 

            _lengthOfDigits = _specificFormatType.LengthOfDigits[Array.IndexOf(_specificFormatType.LengthOfDigits, formatTypelength)];
            _IIN = _specificFormatType.IINRange[GenerateRandom().Next(_specificFormatType.IINRange.Length)]; 

            return GenerateLunhNumber();


        }        

        private Random GenerateRandom(){
            return  new Random((int)DateTime.Now.Ticks);
        }

        private string GenerateLunhNumber(){

            int IINValueLength = _IIN.ToString().Length;

            Random random = GenerateRandom();

            var chars = _IIN.ToString() + new string(Enumerable.Range(0, _lengthOfDigits-IINValueLength+1)
                .Select(x => pool[random.Next(0, pool.Length)])
                .Append('0').ToArray());

            var luhnSum = CalculateLunhSum(chars);

            //  Multiply luhnSum by 9
            var productResult = luhnSum * 9;

            //  Take last digit of product
            var lastDigitOfProduct = productResult % 10;

            chars = chars.Remove(chars.Length -1, 1) + lastDigitOfProduct;

            var t = new 
            {
                CardNumber = chars,
                CardIssuer = _specificFormatType.Issuer,
                CardLength = _lengthOfDigits,
                CardIID = _IIN
            };
          
            return Newtonsoft.Json.JsonConvert.SerializeObject(t);
        }

        private int CalculateLunhSum(string tempCh)
        {
            int results = 0;
            bool arrayIsEvenInLength = (tempCh.Length % 2 == 0);
            int even = 0; 
            int odd = 0; 
  
            // Loop to find even, odd sum 
            for(int i = 0; i < tempCh.Length; i++)
            { 
                var digit = Convert.ToInt32(tempCh[i].ToString());

                if (i % 2 == 0){
                    even += (arrayIsEvenInLength) ? CalculateDoubleSum(digit) : digit; 
                }else{
                    odd += (!arrayIsEvenInLength) ? CalculateDoubleSum(digit) : digit; 
                }
                    
            } 

            results = even + odd;

            return results;
        }

        private int CalculateDoubleSum(int i){
            var iDouble = i*2;
            return (iDouble>9) ? iDouble.ToString().Sum(c => c - '0') : iDouble;
        }

        private List<FormatType> GetFormatTypes(){

            var results = new List<FormatType>();

            results.Add(new FormatType(){
                abbr = "ve",
                Issuer = "Visa Electron",
                IINRange = new int[]{4026, 417500, 4508, 4844, 4913, 4917},
                LengthOfDigits = new int[]{16}
            });

            results.Add(new FormatType(){
                abbr = "v",
                Issuer = "Visa",
                IINRange = new int[]{4},
                LengthOfDigits = new int[]{13,16,19}
            });

            results.Add(new FormatType(){
                abbr = "mc",
                Issuer = "MasterCard",
                IINRange = new int[]{51, 52, 53, 54, 55},
                IINMetaRangeStart = 222100,
                IINMetaRangeEnd = 272099,
                LengthOfDigits = new int[]{16}
            });

            results.Add(new FormatType(){
                abbr = "m",
                Issuer = "Maestro",
                IINRange = new int[]{5018, 5020, 5038, 5893, 6304, 6759, 6761, 6762, 6763},
                LengthOfDigits = new int[]{16,19}
            });

            // results.Add(new FormatType(){
            //     abbr = "jcb",
            //     Issuer = "JCB",
            //     IINMetaRangeStart = 3528,
            //     IINMetaRangeEnd = 3589,
            //     LengthOfDigits = new int[]{16,19}
            // });

            results.Add(new FormatType(){
                abbr = "ip",
                Issuer = "InstaPayment",
                IINRange = new int[]{637, 638, 639},
                LengthOfDigits = new int[]{16}
            });

            results.Add(new FormatType(){
                abbr = "d",
                Issuer = "Discover",
                IINRange = new int[]{6011, 644, 645, 646, 647, 648, 649, 65},
                IINMetaRangeStart = 622126,
                IINMetaRangeEnd = 622925,                
                LengthOfDigits = new int[]{16,19}
            });

            results.Add(new FormatType(){
                abbr = "dcuc",
                Issuer = "Diners Club - USA & Canada",
                IINRange = new int[]{54, 644, 645, 646, 647, 648, 649, 65},
                IINMetaRangeStart = 622126,
                IINMetaRangeEnd = 622925,
                LengthOfDigits = new int[]{16, 19}
            });

            results.Add(new FormatType(){
                abbr = "dci",
                Issuer = "Diners Club - International",
                IINRange = new int[]{36},
                LengthOfDigits = new int[]{14}
            });

            results.Add(new FormatType(){
                abbr = "dccb",
                Issuer = "Diners Club - Carte Blanche",
                IINRange = new int[]{300, 301, 302, 303, 304, 305},
                LengthOfDigits = new int[]{14}
            });

            results.Add(new FormatType(){
                abbr = "ae",
                Issuer = "American Express",
                IINRange = new int[]{34, 37},
                LengthOfDigits = new int[]{15}
            });

            return results;
        }
        
    }
}
