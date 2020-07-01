using LuhnAlgorithim.Models;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LuhnAlgorithim.Models.Tests
{
    [TestClass()]
    public class FormatType
    {
        private readonly Models.FormatType _formatType;

        private readonly List<Models.FormatType> _formatTypes;

        public FormatType()
        {
            _formatType = new Models.FormatType();
            _formatTypes = GetFormatTypes_All();
        }

        [TestMethod()]
        public void ExplodeIINRange_CreatesCorrectNumberOfIINNumbersForDiscover_AreEqual()
        {
            var discoverCard = _formatTypes.Single(x => x.abbr.Equals("d"));

            var originalIINRange = discoverCard.IINRange.Count();

            var metaRangeTotal = discoverCard.IINMetaRangeEnd - discoverCard.IINMetaRangeStart + 1;

            discoverCard.ExplodeIINRange();

            var expectedIINRanceTotal = originalIINRange + metaRangeTotal;

            var actualIINRangeTotal = discoverCard.IINRange.Count();

            Assert.AreEqual(expectedIINRanceTotal, actualIINRangeTotal);
        }

        [TestMethod()]
        public void ExplodeIINRange_CreatesCorrectNumberOfIINNumbersForDinersClubNorthAmerica_AreEqual()
        {
            var dcucCard = _formatTypes.Single(x => x.abbr.Equals("dcuc"));

            var originalIINRange = dcucCard.IINRange.Count();

            var metaRangeTotal = dcucCard.IINMetaRangeEnd - dcucCard.IINMetaRangeStart + 1;

            dcucCard.ExplodeIINRange();

            var expectedIINRanceTotal = originalIINRange + metaRangeTotal;

            var actualIINRangeTotal = dcucCard.IINRange.Count();

            Assert.AreEqual(expectedIINRanceTotal, actualIINRangeTotal);
        }

        [TestMethod()]
        public void ExplodeIINRange_CreatesCorrectNumberOfIINNumbersForVisa_AreEqual()
        {
            var visaCard = _formatTypes.Single(x => x.abbr.Equals("v"));

            var originalIINRange = visaCard.IINRange.Count();

            var metaRangeTotal = visaCard.IINMetaRangeEnd - visaCard.IINMetaRangeStart;

            visaCard.ExplodeIINRange();

            var expectedIINRanceTotal = originalIINRange + metaRangeTotal;

            var actualIINRangeTotal = visaCard.IINRange.Count();

            Assert.AreEqual(expectedIINRanceTotal, actualIINRangeTotal);
        }

        [TestMethod()]
        public void ExplodeIINRange_CreatesCorrectNumberOfIINNumbersForJCB_AreEqual()
        {
            var jcbCard = _formatTypes.Single(x => x.abbr.Equals("jcb"));

            jcbCard.ExplodeIINRange();

            var expectedIINRangeTotal = jcbCard.IINMetaRangeEnd - jcbCard.IINMetaRangeStart + 1;

            var actualIINRangeTotal = jcbCard.IINRange.Count();

            Assert.AreEqual(expectedIINRangeTotal, actualIINRangeTotal);
        }

        private List<Models.FormatType> GetFormatTypes_All()
        {

            var results = new List<Models.FormatType>
            {
                new Models.FormatType()
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

                new Models.FormatType()
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

                new Models.FormatType()
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

                new Models.FormatType()
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

                new Models.FormatType()
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

                new Models.FormatType()
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

                new Models.FormatType()
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

                new Models.FormatType()
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

                new Models.FormatType()
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

                new Models.FormatType()
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

                new Models.FormatType()
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

                new Models.FormatType()
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