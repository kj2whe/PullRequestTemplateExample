using System;
using System.Collections.Generic;

namespace LuhnAlgorithim.Models
{
    public class CardResponseObject
    {
        public CardResponseObject()
        {
            Success = false;
            CardNumber = "XX";
            CardIssuer = "Error";
            CardLength = 0;
            CardIID = 0;
            CardDisplayFormat = "XX";
        }

        public bool Success { get; set; }
        public string CardNumber { get; set; }
        public string CardIssuer { get; set; }
        public int CardLength { get; set; }
        public int CardIID { get; set; }
        public string CardDisplayFormat { get; set; }
        public string Version
        {
            get
            {
                return "0.1.0";
            }
        }
    }
};