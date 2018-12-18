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
    public class FormatController : Controller
    {
        private List<FormatType> _formatTypes;

        public FormatController() => _formatTypes = new FormatType().GetFormatTypes();

        [HttpGet("[action]")]
        public string GetAll()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(_formatTypes);
        }  

        [Route("GetSpecificFormat/{formatType}")]
        [HttpGet]
        public string GetSpecificFormat(string formatType)
        {
            var _specificFormatType = _formatTypes.SingleOrDefault(x=>x.abbr.Equals(formatType, StringComparison.CurrentCultureIgnoreCase));

            if(_specificFormatType == null){
                return $"You must specify a an appropriate formatType";
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(_specificFormatType);
           
        }

        [HttpGet("[action]")]
        public string GetRandomFormat()
        {
            Random random = GenerateRandom();
            
            //  Since no formatType was passed in, randomly choose one from the available choices
            var _specificFormatType = _formatTypes.OrderBy(item => random.Next()).First();

            return Newtonsoft.Json.JsonConvert.SerializeObject(_specificFormatType);
        } 

        private Random GenerateRandom(){
            return  new Random((int)DateTime.Now.Ticks);
        }


    }

}