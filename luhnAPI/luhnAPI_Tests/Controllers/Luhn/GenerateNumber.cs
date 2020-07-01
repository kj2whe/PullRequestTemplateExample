using LuhnAlgorithim.Models;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LuhnAlgorithim.Controllers.Tests
{
    [TestClass()]
    public class GenerateNumber
    {
        private readonly LuhnController _luhnController;

        private readonly List<FormatType> _formatTypes;

        public GenerateNumber()
        {
            using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var logger = loggerFactory.CreateLogger<LuhnController>();

            _luhnController = new LuhnController(logger);
            _formatTypes = GetFormatTypes_All();
        }

        #region Generate Number 0 Paramaters

        #region Success

        [TestMethod()]
        public void GenerateNumber_0ParamaterIsFoundInList_True()
        {
            var obj = JsonConvert.DeserializeObject<CardResponseObject>(_luhnController.GenerateNumber());

            var objFound = GetFormatTypes_All().Single(x => x.Issuer.Equals(obj.CardIssuer));

            Assert.IsNotNull(objFound);
        }

        #endregion

        #endregion

        #region Generate Number 1 Paramater

        #region Success

        [TestMethod()]
        public void GenerateNumber_1ParamaterFormatTypeIsFoundInList_True()
        {
            string formatType = "v";

            var expected = GetFormatTypes_All().Single(x => x.abbr.Equals(formatType));

            var actual = JsonConvert.DeserializeObject<CardResponseObject>(_luhnController.GenerateNumber(formatType));

            Assert.AreEqual(expected.Issuer.ToLower(), actual.CardIssuer.ToLower());
        }

        #endregion

        #region Failure

        [TestMethod()]
        public void GenerateNumber_1ParamaterFormatTypeIsNotFoundInList_False()
        {
            string formatType = "q";

            var actual = JsonConvert.DeserializeObject<CardResponseObject>(_luhnController.GenerateNumber(formatType));

            Assert.AreEqual(false, actual.Success);
        }

        [TestMethod()]
        public void GenerateNumber_1ParamaterFormatTypeIsEmptyString_False()
        {
            string formatType = string.Empty;

            var actual = JsonConvert.DeserializeObject<CardResponseObject>(_luhnController.GenerateNumber(formatType));

            Assert.AreEqual(false, actual.Success);
        }

        #endregion

        #endregion

        #region Generate Number 2 Paramaters

        #region Success

        [TestMethod()]
        public void GenerateNumber_2ParamatersFormatTypeAndTypeLengthAreFoundInList_True()
        {
            string formatType = "v";
            int formatTypelength = 16;

            var expected = GetFormatTypes_All().Single(x => x.abbr.Equals(formatType));

            var actual = JsonConvert.DeserializeObject<CardResponseObject>(_luhnController.GenerateNumber(formatType, formatTypelength));

            Assert.AreEqual(expected.Issuer.ToLower(), actual.CardIssuer.ToLower());
            Assert.IsTrue(expected.LengthOfDigits.Contains(actual.CardLength));
        }

        #endregion

        #region Failure

        [TestMethod()]
        public void GenerateNumber_2ParamatersFormatTypeIsNotFoundInList_False()
        {
            string formatType = "q";
            int formatTypelength = 16;

            var actual = JsonConvert.DeserializeObject<CardResponseObject>(_luhnController.GenerateNumber(formatType, formatTypelength));

            Assert.AreEqual(false, actual.Success);
        }

        [TestMethod()]
        public void GenerateNumber_2ParamatersFormatTypeIsEmptyString_False()
        {
            string formatType = string.Empty;
            int formatTypelength = 16;

            var actual = JsonConvert.DeserializeObject<CardResponseObject>(_luhnController.GenerateNumber(formatType, formatTypelength));

            Assert.AreEqual(false, actual.Success);
        }

        [TestMethod()]
        public void GenerateNumber_2ParamatersFormatTypeLengthIs0_False()
        {
            string formatType = "v";
            int formatTypelength = 0;

            var actual = JsonConvert.DeserializeObject<CardResponseObject>(_luhnController.GenerateNumber(formatType, formatTypelength));

            Assert.AreEqual(false, actual.Success);
        }

        [TestMethod()]
        public void GenerateNumber_2ParamatersFormatTypeLengthIsNotFoundInList_False()
        {
            string formatType = "v";
            int formatTypelength = 17;

            var actual = JsonConvert.DeserializeObject<CardResponseObject>(_luhnController.GenerateNumber(formatType, formatTypelength));

            Assert.AreEqual(false, actual.Success);
        }

        #endregion

        #endregion

        #region Generate Number 3 Paramaters

        #region Success

        [TestMethod()]
        public void GenerateNumber_3ParamatersFormatTypeAndFormatTypeLengthAndFormatStartAreFoundInList_True()
        {
            string formatType = "v";
            int formatTypelength = 16;
            int formatStart = 4;

            var expected = GetFormatTypes_All().Single(x => x.abbr.Equals(formatType));

            var actual = JsonConvert.DeserializeObject<CardResponseObject>(_luhnController.GenerateNumber(formatType, formatTypelength, formatStart));

            Assert.AreEqual(expected.Issuer.ToLower(), actual.CardIssuer.ToLower());
            Assert.IsTrue(expected.LengthOfDigits.Contains(actual.CardLength));
            Assert.AreEqual(expected.IINRange[0], Convert.ToInt32(actual.CardNumber.Substring(0, 1)));
        }

        #endregion

        #region Failure

        [TestMethod()]
        public void GenerateNumber_3ParamatersFormatTypeIsEmptyString_False()
        {
            string formatType = string.Empty;
            int formatTypelength = 16;
            int formatStart = 4;

            var actual = JsonConvert.DeserializeObject<CardResponseObject>(_luhnController.GenerateNumber(formatType, formatTypelength, formatStart));

            Assert.AreEqual(false, actual.Success);
        }

        [TestMethod()]
        public void GenerateNumber_3ParamatersFormatLengthIs0_False()
        {
            string formatType = "v";
            int formatTypelength = 0;
            int formatStart = 4;

            var actual = JsonConvert.DeserializeObject<CardResponseObject>(_luhnController.GenerateNumber(formatType, formatTypelength, formatStart));

            Assert.AreEqual(false, actual.Success);
        }

        [TestMethod()]
        public void GenerateNumber_3ParamatersFormatStartIs0_False()
        {
            string formatType = "v";
            int formatTypelength = 16;
            int formatStart = 0;

            var actual = JsonConvert.DeserializeObject<CardResponseObject>(_luhnController.GenerateNumber(formatType, formatTypelength, formatStart));

            Assert.AreEqual(false, actual.Success);
        }

        [TestMethod()]
        public void GenerateNumber_3ParamatersFormatTypeIsNotFoundInList_False()
        {
            string formatType = "q";
            int formatTypelength = 16;
            int formatStart = 4;

            var actual = JsonConvert.DeserializeObject<CardResponseObject>(_luhnController.GenerateNumber(formatType, formatTypelength, formatStart));

            Assert.AreEqual(false, actual.Success);
        }

        [TestMethod()]
        public void GenerateNumber_3ParamatersFormatTypeLengthIsNotFoundInList_False()
        {
            string formatType = "v";
            int formatTypelength = 17;
            int formatStart = 4;

            var actual = JsonConvert.DeserializeObject<CardResponseObject>(_luhnController.GenerateNumber(formatType, formatTypelength, formatStart));

            Assert.AreEqual(false, actual.Success);
        }

        [TestMethod()]
        public void GenerateNumber_3ParamatersFormatStartIsNotFoundInList_False()
        {
            string formatType = "v";
            int formatTypelength = 16;
            int formatStart = 5;

            var actual = JsonConvert.DeserializeObject<CardResponseObject>(_luhnController.GenerateNumber(formatType, formatTypelength, formatStart));

            Assert.AreEqual(false, actual.Success);
        }

        #endregion

        #endregion

        private List<FormatType> GetFormatTypes_All()
        {

            var results = new List<FormatType>
            {
                new FormatType()
                {
                    abbr = "ve",
                    Issuer = "Visa Electron",
                    IINRange = new List<int> { 4026, 417500, 4508, 4844, 4913, 4917 },
                    LengthOfDigits = new int[] { 16 },
                    DisplayFormat = new List<FormatTypeSpacing>(){
                    new FormatTypeSpacing{
                        FormatLength = 16,
                        DigitSpacingFormat = " dddd dddd dddd dddd"
                    }
                }
                },

                new FormatType()
                {
                    abbr = "v",
                    Issuer = "Visa",
                    IINRange = new List<int> { 4 },
                    LengthOfDigits = new int[] { 13, 16, 19 },
                    DisplayFormat = new List<FormatTypeSpacing>(){
                    new FormatTypeSpacing{
                        FormatLength = 13,
                        DigitSpacingFormat = " dddd dddd dddd d"
                    },
                    new FormatTypeSpacing{
                        FormatLength = 16,
                        DigitSpacingFormat = " dddd dddd dddd dddd"
                    },
                    new FormatTypeSpacing{
                        FormatLength = 19,
                        DigitSpacingFormat = " dddd dddd dddd dddd ddd"
                    },
                }
                },

                new FormatType()
                {
                    abbr = "mc",
                    Issuer = "MasterCard",
                    IINRange = new List<int> { 51, 52, 53, 54, 55 },
                    IINMetaRangeStart = 222100,
                    IINMetaRangeEnd = 272099,
                    LengthOfDigits = new int[] { 16 },
                    DisplayFormat = new List<FormatTypeSpacing>(){
                    new FormatTypeSpacing{
                        FormatLength = 16,
                        DigitSpacingFormat = " dddd dddd dddd dddd"
                    }
                }
                },

                new FormatType()
                {
                    abbr = "m",
                    Issuer = "Maestro",
                    IINRange = new List<int> { 5018, 5020, 5038, 5893, 6304, 6759, 6761, 6762, 6763 },
                    LengthOfDigits = new int[] { 13, 15, 16, 19 },
                    DisplayFormat = new List<FormatTypeSpacing>(){
                    new FormatTypeSpacing{
                        FormatLength = 13,
                        IINMetaRangeStart = 500000,
                        IINMetaRangeEnd = 509999,
                        DigitSpacingFormat = " dddd dddd ddddd"
                    },
                    new FormatTypeSpacing{
                        FormatLength = 15,
                        IINMetaRangeStart = 560000,
                        IINMetaRangeEnd = 589999,
                        DigitSpacingFormat = " dddd dddddd ddddd"
                    },
                    new FormatTypeSpacing{
                        FormatLength = 16,
                        IINMetaRangeStart = 600000,
                        IINMetaRangeEnd = 699999,
                        DigitSpacingFormat = " dddd dddd dddd dddd"
                    },
                    new FormatTypeSpacing{
                        FormatLength = 19,
                        DigitSpacingFormat = " dddd dddd dddd dddd ddd"
                    }
                }
                },

                new FormatType()
                {
                    abbr = "jcb",
                    Issuer = "JCB",
                    IINMetaRangeStart = 3528,
                    IINMetaRangeEnd = 3589,
                    LengthOfDigits = new int[] { 16 },
                    DisplayFormat = new List<FormatTypeSpacing>(){
                    new FormatTypeSpacing{
                        FormatLength = 16,
                        DigitSpacingFormat = " dddd dddd dddd dddd"
                    }
                }
                },

                new FormatType()
                {
                    abbr = "ip",
                    Issuer = "InstaPayment",
                    IINRange = new List<int> { 637, 638, 639 },
                    LengthOfDigits = new int[] { 16 },
                    DisplayFormat = new List<FormatTypeSpacing>(){
                    new FormatTypeSpacing{
                        FormatLength = 16,
                        DigitSpacingFormat = " dddd dddd dddd dddd"
                    }
                }
                },

                new FormatType()
                {
                    abbr = "d",
                    Issuer = "Discover",
                    IINRange = new List<int> { 6011, 644, 645, 646, 647, 648, 649, 65 },
                    IINMetaRangeStart = 622126,
                    IINMetaRangeEnd = 622925,
                    LengthOfDigits = new int[] { 16 },
                    DisplayFormat = new List<FormatTypeSpacing>(){
                    new FormatTypeSpacing{
                        FormatLength = 16,
                        DigitSpacingFormat = " dddd dddd dddd dddd"
                    }
                }
                },

                new FormatType()
                {
                    abbr = "cup",
                    Issuer = "China Union Pay",
                    IINRange = new List<int> { 62 },
                    LengthOfDigits = new int[] { 16, 19 },
                    DisplayFormat = new List<FormatTypeSpacing>(){
                    new FormatTypeSpacing{
                        FormatLength = 16,
                        DigitSpacingFormat = " dddd dddd dddd dddd"
                    },
                    new FormatTypeSpacing{
                        FormatLength = 19,
                        DigitSpacingFormat = " dddddd ddddddddddddd"
                    }
                }
                },

                new FormatType()
                {
                    abbr = "dcuc",
                    Issuer = "Diners Club - USA & Canada",
                    IINRange = new List<int> { 54, 644, 645, 646, 647, 648, 649, 65 },
                    IINMetaRangeStart = 622126,
                    IINMetaRangeEnd = 622925,
                    LengthOfDigits = new int[] { 16 },
                    DisplayFormat = new List<FormatTypeSpacing>(){
                    new FormatTypeSpacing{
                        FormatLength = 16,
                        DigitSpacingFormat = " dddd dddd dddd dddd"
                    }
                }
                },

                new FormatType()
                {
                    abbr = "dci",
                    Issuer = "Diners Club - International",
                    IINRange = new List<int> { 36 },
                    LengthOfDigits = new int[] { 14 },
                    DisplayFormat = new List<FormatTypeSpacing>(){
                    new FormatTypeSpacing{
                        FormatLength = 14,
                        DigitSpacingFormat = " dddd dddddd dddd"
                    }
                }
                },

                new FormatType()
                {
                    abbr = "dccb",
                    Issuer = "Diners Club - Carte Blanche",
                    IINRange = new List<int> { 300, 301, 302, 303, 304, 305 },
                    LengthOfDigits = new int[] { 14 },
                    DisplayFormat = new List<FormatTypeSpacing>(){
                    new FormatTypeSpacing{
                        FormatLength = 14,
                        DigitSpacingFormat = " dddd dddddd dddd"
                    }
                }
                },

                new FormatType()
                {
                    abbr = "ae",
                    Issuer = "American Express",
                    IINRange = new List<int> { 34, 37 },
                    LengthOfDigits = new int[] { 15 },
                    DisplayFormat = new List<FormatTypeSpacing>(){
                    new FormatTypeSpacing{
                        FormatLength = 15,
                        DigitSpacingFormat = " dddd dddddd ddddd"
                    }
                }
                }
            };

            return results;
        }
    }
}