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

        public LuhnController() => _formatTypes = new FormatType().GetFormatTypes();

        [Route("GenerateNumber/{formatType}/{formatTypelength}")]
        [HttpGet]
        public string GenerateNumber(string formatType, int formatTypelength)
        {
            var response = new CardResponseObject(){
                Success = false
            };

            _specificFormatType = _formatTypes.SingleOrDefault(x=>x.abbr.Equals(formatType, StringComparison.CurrentCultureIgnoreCase));
            _specificFormatType.ExplodeIINRange();

            if(_specificFormatType == null){
                return $"You must specify a an appropriate formatType";
            }
            else if(Array.IndexOf(_specificFormatType.LengthOfDigits, formatTypelength) == -1){
                return $"You must specify a valid length for a {_specificFormatType.Issuer}";                    
            }

            _lengthOfDigits = _specificFormatType.LengthOfDigits[Array.IndexOf(_specificFormatType.LengthOfDigits, formatTypelength)];
            _IIN = _specificFormatType.IINRange[GenerateRandom().Next(_specificFormatType.IINRange.Count)]; 

            response.CardNumber = GenerateLunhNumber();
            response.CardIssuer = _specificFormatType.Issuer;
            response.CardLength = _lengthOfDigits;
            response.CardIID = _IIN;
            response.CardDisplayFormat = _specificFormatType.DisplayFormat.FirstOrDefault(x=>x.FormatLength.Equals(_lengthOfDigits)).DigitSpacingFormat;
            response.Success = true;

            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        }
    
        [Route("GenerateNumber/{formatType}")]
        [HttpGet]
        public string GenerateNumber(string formatType)
        {
            Random random = GenerateRandom();

            var response = new CardResponseObject(){
                Success = false
            };            

            _specificFormatType = _formatTypes.SingleOrDefault(x=>x.abbr.Equals(formatType, StringComparison.CurrentCultureIgnoreCase));
            _specificFormatType.ExplodeIINRange();

            if(_specificFormatType == null){
                return $"You must specify a an appropriate formatType";
            }

            //  Since no formatTypelength was passed in, randomly choose one from the available choices
            int formatTypelength = _specificFormatType.LengthOfDigits[random.Next(_specificFormatType.LengthOfDigits.Length)]; 

            _lengthOfDigits = _specificFormatType.LengthOfDigits[Array.IndexOf(_specificFormatType.LengthOfDigits, formatTypelength)];
            _IIN = _specificFormatType.IINRange[GenerateRandom().Next(_specificFormatType.IINRange.Count)]; 

            response.CardNumber = GenerateLunhNumber();
            response.CardIssuer = _specificFormatType.Issuer;
            response.CardLength = _lengthOfDigits;
            response.CardIID = _IIN;
            response.CardDisplayFormat = _specificFormatType.DisplayFormat.FirstOrDefault(x=>x.FormatLength.Equals(_lengthOfDigits)).DigitSpacingFormat;
            response.Success = true;

            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
           
        }

        [HttpGet("[action]")]
        public string GenerateNumber()
        {
            Random random = GenerateRandom();
            
            var response = new CardResponseObject(){
                Success = false
            };

            //  Since no formatType was passed in, randomly choose one from the available choices
            _specificFormatType = _formatTypes.OrderBy(item => random.Next()).First();
            _specificFormatType.ExplodeIINRange();

            //  Since no formatTypelength was passed in, randomly choose one from the available choices
            int formatTypelength = _specificFormatType.LengthOfDigits[random.Next(_specificFormatType.LengthOfDigits.Length)]; 

            _lengthOfDigits = _specificFormatType.LengthOfDigits[Array.IndexOf(_specificFormatType.LengthOfDigits, formatTypelength)];
            _IIN = _specificFormatType.IINRange[GenerateRandom().Next(_specificFormatType.IINRange.Count)]; 

            response.CardNumber = GenerateLunhNumber();
            response.CardIssuer = _specificFormatType.Issuer;
            response.CardLength = _lengthOfDigits;
            response.CardIID = _IIN;
            response.CardDisplayFormat = _specificFormatType.DisplayFormat.FirstOrDefault(x=>x.FormatLength.Equals(_lengthOfDigits)).DigitSpacingFormat;
            response.Success = true;

            return Newtonsoft.Json.JsonConvert.SerializeObject(response);
        }        

        private Random GenerateRandom(){
            return  new Random((int)DateTime.Now.Ticks);
        }

        private string GenerateLunhNumber(){

            int IINValueLength = _IIN.ToString().Length;

            Random random = GenerateRandom();

            var chars = _IIN.ToString() + new string(Enumerable.Range(0, _lengthOfDigits-IINValueLength-1)
                .Select(x => pool[random.Next(0, pool.Length)])
                .Append('0').ToArray());

            var luhnSum = CalculateLunhSum(chars);

            //  Multiply luhnSum by 9
            var productResult = luhnSum * 9;

            //  Take last digit of product
            var lastDigitOfProduct = productResult % 10;

            chars = chars.Remove(chars.Length -1, 1) + lastDigitOfProduct;

            return chars;
         
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
 
    }
}
