using LuhnAlgorithim.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LuhnAlgorithim.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class FormatController : ControllerBase
    {
        private readonly List<FormatType> _formatTypes;

        private readonly ILogger<FormatController> _logger;

        public FormatController(ILogger<FormatController> logger)
        {
            _logger = logger;
            _formatTypes = new FormatType().GetFormatTypes();
        }

        [HttpGet("[action]")]
        public string GetAll()
        {
            _logger.LogInformation($"Inside method GetAll()");
            return Newtonsoft.Json.JsonConvert.SerializeObject(_formatTypes);
        }  

        [Route("GetSpecificFormat/{formatType}")]
        [HttpGet]
        public string GetSpecificFormat(string formatType)
        {
            var _specificFormatType = _formatTypes.SingleOrDefault(x=>x.abbr.Equals(formatType, StringComparison.CurrentCultureIgnoreCase));

            if(_specificFormatType == null){
                return $"You must specify an appropriate formatType";
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