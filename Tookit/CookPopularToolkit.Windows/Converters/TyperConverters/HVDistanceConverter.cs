/*
 *Description: HVDistanceConverter
 *Author: Chance.zheng
 *Creat Time: 2023/8/25 23:06:19
 *.Net Version: 4.6
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CookPopularToolkit.Windows
{
    /// <summary>
    /// Converts instances of other types to and from instances of <see cref="HVDistance"/>
    /// </summary>
    public class HVDistanceConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            // We can only handle strings, integral and floating types
            TypeCode tc = Type.GetTypeCode(sourceType);
            switch (tc)
            {
                case TypeCode.String:
                case TypeCode.Decimal:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;

                default:
                    return false;
            }
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object source)
        {
            if (source != null)
            {
                if (source is string) { return FromString((string)source, culture); }
                else if (source is double) { return new HVDistance((double)source); }
                else { return new HVDistance(Convert.ToDouble(source, culture)); }
            }
            throw GetConvertFromException(source);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            // We can convert to an InstanceDescriptor or to a string.
            if (destinationType == typeof(InstanceDescriptor) || destinationType == typeof(string))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (null == value)
            {
                throw new ArgumentNullException("value");
            }

            if (null == destinationType)
            {
                throw new ArgumentNullException("destinationType");
            }

            if (!(value is HVDistance hv))
            {
                throw new ArgumentException(Error.Format("UnexpectedParameterType", "value"));
            }

            if (destinationType == typeof(string)) { return ToString(hv, culture); }
            if (destinationType == typeof(InstanceDescriptor))
            {
                ConstructorInfo ci = typeof(HVDistance).GetConstructor(new Type[] { typeof(double), typeof(double) });
                return new InstanceDescriptor(ci, new object[] { hv.H, hv.V });
            }

            throw new ArgumentException(Error.Format("CannotConvertType", destinationType.FullName));
        }

        private string ToString(HVDistance hv, CultureInfo cultureInfo)
        {
            char listSeparator = TokenizerHelper.GetNumericListSeparator(cultureInfo);

            // Initial capacity [64] is an estimate based on a sum of:
            // 48 = 4x double (twelve digits is generous for the range of values likely)
            //  8 = 4x Unit Type string (approx two characters)
            //  4 = 4x separator characters
            StringBuilder sb = new StringBuilder(64);

            sb.Append(LengthConverterToString(hv.H, cultureInfo));
            sb.Append(listSeparator);
            sb.Append(LengthConverterToString(hv.V, cultureInfo));
            sb.Append(listSeparator);
            return sb.ToString();
        }

        private string LengthConverterToString(double l, CultureInfo cultureInfo)
        {
            if (double.IsNaN(l)) return "Auto";
            return Convert.ToString(l, cultureInfo);
        }


        private static HVDistance FromString(string s, CultureInfo cultureInfo)
        {
            TokenizerHelper th = new TokenizerHelper(s, cultureInfo);
            double[] lengths = new double[4];
            int i = 0;

            // Peel off each double in the delimited list.
            while (th.NextToken())
            {
                if (i >= 4)
                {
                    i = 5;    // Set i to a bad value. 
                    break;
                }

                lengths[i] = LengthConverterFromString(th.GetCurrentToken(), cultureInfo);
                i++;
            }

            // We have a reasonable interpreation for one value (all four edges), two values (horizontal, vertical),
            // and four values (left, top, right, bottom).
            switch (i)
            {
                case 1:
                    return new HVDistance(lengths[0]);
                case 2:
                    return new HVDistance(lengths[0], lengths[1]);
            }

            throw new FormatException(Error.Format("InvalidStringHVDistance", s));
        }

        // Parse a Length from a string given the CultureInfo.
        // Formats: 
        //"[value][unit]"
        //   [value] is a double
        //   [unit] is a string specifying the unit, like 'in' or 'px', or nothing (means pixels)
        // NOTE - This code is called from FontSizeConverter, so changes will affect both.
        private static double LengthConverterFromString(string? s, CultureInfo cultureInfo)
        {
            string valueString = s!.Trim();
            string goodString = valueString.ToLowerInvariant();
            int strLen = goodString.Length;
            int strLenUnit = 0;
            double unitFactor = 1.0;

            //Auto is represented and Double.NaN
            //properties that do not want Auto and NaN to be in their ligit values,
            //should disallow NaN in validation callbacks (same goes for negative values)
            if (goodString == "auto") return Double.NaN;

            for (int i = 0; i < PixelUnitStrings.Length; i++)
            {
                // NOTE: This is NOT a culture specific comparison.
                // This is by design: we want the same unit string table to work across all cultures.
                if (goodString.EndsWith(PixelUnitStrings[i], StringComparison.Ordinal))
                {
                    strLenUnit = PixelUnitStrings[i].Length;
                    unitFactor = PixelUnitFactors[i];
                    break;
                }
            }

            //  important to substring original non-lowered string 
            //  this allows case sensitive ToDouble below handle "NaN" and "Infinity" correctly. 
            //  this addresses windows bug 1177408
            valueString = valueString.Substring(0, strLen - strLenUnit);

            // FormatException errors thrown by Convert.ToDouble are pretty uninformative.
            // Throw a more meaningful error in this case that tells that we were attempting
            // to create a Length instance from a string.  This addresses windows bug 968884
            try
            {
                double result = Convert.ToDouble(valueString, cultureInfo) * unitFactor;
                return result;
            }
            catch (FormatException)
            {
                throw new FormatException(Error.Format("LengthFormatError", valueString));
            }
        }

        // This array contains strings for unit types 
        // These are effectively "TypeConverter only" units.
        // They are all expressable in terms of the Pixel unit type and a conversion factor.
        private static string[] PixelUnitStrings = { "px", "in", "cm", "pt" };
        private static double[] PixelUnitFactors =
        {
            1.0,              // Pixel itself
            96.0,             // Pixels per Inch
            96.0 / 2.54,      // Pixels per Centimeter
            96.0 / 72.0,      // Pixels per Point
        };
    }

    internal class Error
    {
        private static readonly bool s_usingResourceKeys = AppContext.TryGetSwitch("System.Resources.UseSystemResourceKeys", out var isEnabled) && isEnabled;

        public static string Format(string resourceFormat, object? p1)
        {
            if (s_usingResourceKeys)
            {
                return string.Join(", ", resourceFormat, p1);
            }

            return string.Format(resourceFormat, p1);
        }

        private static global::System.Resources.ResourceManager s_resourceManager;
        internal static global::System.Resources.ResourceManager ResourceManager => s_resourceManager ?? (s_resourceManager = new global::System.Resources.ResourceManager(typeof(HVDistanceConverter)));
        private static string? GetResourceString(string resourceKey)
        {
            string? resourceString = null;
            try { resourceString = ResourceManager.GetString(resourceKey); }
            catch (MissingManifestResourceException) { }

            return resourceString;
        }
    }

    internal class TokenizerHelper
    {
        /// <summary>
        /// Constructor for TokenizerHelper which accepts an IFormatProvider.
        /// If the IFormatProvider is null, we use the thread's IFormatProvider info.
        /// We will use ',' as the list separator, unless it's the same as the
        /// decimal separator.  If it *is*, then we can't determine if, say, "23,5" is one
        /// number or two.  In this case, we will use ";" as the separator.
        /// </summary>
        /// <param name="str"> The string which will be tokenized. </param>
        /// <param name="formatProvider"> The IFormatProvider which controls this tokenization. </param>
        internal TokenizerHelper(string str, IFormatProvider formatProvider)
        {
            char numberSeparator = GetNumericListSeparator(formatProvider);

            Initialize(str, '\'', numberSeparator);
        }

        /// <summary>
        /// Initialize the TokenizerHelper with the string to tokenize,
        /// the char which represents quotes and the list separator.
        /// </summary>
        /// <param name="str"> The string to tokenize. </param>
        /// <param name="quoteChar"> The quote char. </param>
        /// <param name="separator"> The list separator. </param>
        internal TokenizerHelper(string str,
                                 char quoteChar,
                                 char separator)
        {
            Initialize(str, quoteChar, separator);
        }

        /// <summary>
        /// Initialize the TokenizerHelper with the string to tokenize,
        /// the char which represents quotes and the list separator.
        /// </summary>
        /// <param name="str"> The string to tokenize. </param>
        /// <param name="quoteChar"> The quote char. </param>
        /// <param name="separator"> The list separator. </param>
        private void Initialize(string str,
                                char quoteChar,
                                char separator)
        {
            _str = str;
            _strLen = str == null ? 0 : str.Length;
            _currentTokenIndex = -1;
            _quoteChar = quoteChar;
            _argSeparator = separator;

            // immediately forward past any whitespace so
            // NextToken() logic always starts on the first
            // character of the next token.
            while (_charIndex < _strLen)
            {
                if (!Char.IsWhiteSpace(_str, _charIndex))
                {
                    break;
                }

                ++_charIndex;
            }
        }

        internal string? GetCurrentToken()
        {
            // if no current token, return null
            if (_currentTokenIndex < 0)
            {
                return null;
            }

            return _str.Substring(_currentTokenIndex, _currentTokenLength);
        }

        /// <summary>
        /// Throws an exception if there is any non-whitespace left un-parsed.
        /// </summary>
        internal void LastTokenRequired()
        {
            if (_charIndex != _strLen)
            {
                throw new System.InvalidOperationException(Error.Format("TokenizerHelperExtraDataEncountered", _str));
            }
        }

        /// <summary>
        /// Advances to the NextToken
        /// </summary>
        /// <returns>true if next token was found, false if at end of string</returns>
        internal bool NextToken()
        {
            return NextToken(false);
        }

        /// <summary>
        /// Advances to the NextToken, throwing an exception if not present
        /// </summary>
        /// <returns>The next token found</returns>
        internal string? NextTokenRequired()
        {
            if (!NextToken(false))
            {
                throw new System.InvalidOperationException(Error.Format("TokenizerHelperPrematureStringTermination", _str));
            }

            return GetCurrentToken();
        }

        /// <summary>
        /// Advances to the NextToken, throwing an exception if not present
        /// </summary>
        /// <returns>The next token found</returns>
        internal string? NextTokenRequired(bool allowQuotedToken)
        {
            if (!NextToken(allowQuotedToken))
            {
                throw new System.InvalidOperationException(Error.Format("TokenizerHelperPrematureStringTermination", _str));
            }

            return GetCurrentToken();
        }

        /// <summary>
        /// Advances to the NextToken
        /// </summary>
        /// <returns>true if next token was found, false if at end of string</returns>
        internal bool NextToken(bool allowQuotedToken)
        {
            // use the currently-set separator character.
            return NextToken(allowQuotedToken, _argSeparator);
        }

        /// <summary>
        /// Advances to the NextToken.  A separator character can be specified
        /// which overrides the one previously set.
        /// </summary>
        /// <returns>true if next token was found, false if at end of string</returns>
        internal bool NextToken(bool allowQuotedToken, char separator)
        {
            _currentTokenIndex = -1; // reset the currentTokenIndex
            _foundSeparator = false; // reset

            // If we're at end of the string, just return false.
            if (_charIndex >= _strLen)
            {
                return false;
            }

            char currentChar = _str[_charIndex];

            Debug.Assert(!Char.IsWhiteSpace(currentChar), "Token started on Whitespace");

            // setup the quoteCount
            int quoteCount = 0;

            // If we are allowing a quoted token and this token begins with a quote,
            // set up the quote count and skip the initial quote
            if (allowQuotedToken &&
                currentChar == _quoteChar)
            {
                quoteCount++; // increment quote count
                ++_charIndex; // move to next character
            }

            int newTokenIndex = _charIndex;
            int newTokenLength = 0;

            // loop until hit end of string or hit a , or whitespace
            // if at end of string ust return false.
            while (_charIndex < _strLen)
            {
                currentChar = _str[_charIndex];

                // if have a QuoteCount and this is a quote
                // decrement the quoteCount
                if (quoteCount > 0)
                {
                    // if anything but a quoteChar we move on
                    if (currentChar == _quoteChar)
                    {
                        --quoteCount;

                        // if at zero which it always should for now
                        // break out of the loop
                        if (0 == quoteCount)
                        {
                            ++_charIndex; // move past the quote
                            break;
                        }
                    }
                }
                else if ((Char.IsWhiteSpace(currentChar)) || (currentChar == separator))
                {
                    if (currentChar == separator)
                    {
                        _foundSeparator = true;
                    }
                    break;
                }

                ++_charIndex;
                ++newTokenLength;
            }

            // if quoteCount isn't zero we hit the end of the string
            // before the ending quote
            if (quoteCount > 0)
            {
                throw new System.InvalidOperationException(Error.Format("TokenizerHelperMissingEndQuote", _str));
            }

            ScanToNextToken(separator); // move so at the start of the nextToken for next call

            // finally made it, update the _currentToken values
            _currentTokenIndex = newTokenIndex;
            _currentTokenLength = newTokenLength;

            if (_currentTokenLength < 1)
            {
                throw new System.InvalidOperationException(Error.Format("TokenizerHelperEmptyToken", _str));
            }

            return true;
        }

        // helper to move the _charIndex to the next token or to the end of the string
        void ScanToNextToken(char separator)
        {
            // if already at end of the string don't bother
            if (_charIndex < _strLen)
            {
                char currentChar = _str[_charIndex];

                // check that the currentChar is a space or the separator.  If not
                // we have an error. this can happen in the quote case
                // that the char after the quotes string isn't a char.
                if (!(currentChar == separator) &&
                    !Char.IsWhiteSpace(currentChar))
                {
                    throw new System.InvalidOperationException(Error.Format("TokenizerHelperExtraDataEncountered", _str));
                }

                // loop until hit a character that isn't
                // an argument separator or whitespace.
                int argSepCount = 0;
                while (_charIndex < _strLen)
                {
                    currentChar = _str[_charIndex];

                    if (currentChar == separator)
                    {
                        _foundSeparator = true;
                        ++argSepCount;
                        _charIndex++;

                        if (argSepCount > 1)
                        {
                            throw new System.InvalidOperationException(Error.Format("TokenizerHelperEmptyToken", _str));
                        }
                    }
                    else if (Char.IsWhiteSpace(currentChar))
                    {
                        ++_charIndex;
                    }
                    else
                    {
                        break;
                    }
                }

                // if there was a separatorChar then we shouldn't be
                // at the end of string or means there was a separator
                // but there isn't an arg

                if (argSepCount > 0 && _charIndex >= _strLen)
                {
                    throw new System.InvalidOperationException(Error.Format("TokenizerHelperEmptyToken", _str));
                }
            }
        }

        // Helper to get the numeric list separator for a given IFormatProvider.
        // Separator is a comma [,] if the decimal separator is not a comma, or a semicolon [;] otherwise.
        static internal char GetNumericListSeparator(IFormatProvider provider)
        {
            char numericSeparator = ',';

            // Get the NumberFormatInfo out of the provider, if possible
            // If the IFormatProvider doesn't not contain a NumberFormatInfo, then
            // this method returns the current culture's NumberFormatInfo.
            NumberFormatInfo numberFormat = NumberFormatInfo.GetInstance(provider);

            Debug.Assert(null != numberFormat);

            // Is the decimal separator is the same as the list separator?
            // If so, we use the ";".
            if ((numberFormat!.NumberDecimalSeparator.Length > 0) && (numericSeparator == numberFormat.NumberDecimalSeparator[0]))
            {
                numericSeparator = ';';
            }

            return numericSeparator;
        }

        internal bool FoundSeparator
        {
            get
            {
                return _foundSeparator;
            }
        }

        char _quoteChar;
        char _argSeparator;
        string _str;
        int _strLen;
        int _charIndex;
        internal int _currentTokenIndex;
        internal int _currentTokenLength;
        bool _foundSeparator;
    }
}
