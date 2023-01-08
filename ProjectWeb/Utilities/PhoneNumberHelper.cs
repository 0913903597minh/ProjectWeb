using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibData.Utilities
{
    public class PhoneNumberHelper
    {
        public static string ValidateCountryCodePhoneNumber(string phone, string countryCode, string countryCodePrefixLenght)
        {
            try
            {
                if ((!phone.Any(c => c < '0' || c > '9')) || phone.StartsWith("+"))
                {
                    Console.WriteLine("Given string is numeric");
                }
                if (phone.All(char.IsDigit) || phone.StartsWith("+"))
                {
                    if ((phone.Length >= 7 && phone.Length <= 12) || phone.StartsWith("+") || phone.StartsWith("00"))
                    {

                        if (!phone.Substring(0, 1).Equals("0"))
                        {
                            if (phone.Substring(0, 2) == countryCode && phone.Length > Convert.ToInt32(countryCodePrefixLenght))
                            {
                                phone = "0" + phone.Substring(2);
                            }
                            else if (phone.Substring(0, 2) == countryCode && phone.Length == Convert.ToInt32(countryCodePrefixLenght))
                            {
                                phone = "0" + phone;
                            }
                            else if (phone.Substring(0, 3).Equals(("+" + countryCode)))
                            {
                                phone = "0" + phone.Substring(3);
                            }
                            else
                            {
                                phone = "0" + phone;
                            }
                        }
                        else if (phone.Substring(0, 4).Equals(("00" + countryCode)))
                        {
                            phone = "0" + phone.Substring(4);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return phone;
        }
        public static string RemoveHeaderCountryPhoneNumber(string phone, string countryCode)
        {
            try
            {
                if (phone.Substring(0, 1).Equals("0"))
                {

                    if (phone.Substring(0, 4).Equals(("00" + countryCode)))
                    {
                        phone = phone.Substring(4);
                    }
                    else
                    {
                        phone = phone.Substring(1);
                    }

                }
                else
                {
                    if (phone.Substring(0, 2) == countryCode) phone = phone.Substring(2);
                }

            }
            catch (Exception)
            {
            }
            return phone;
        }
        public static bool IsCountryPhoneNumber(string phone, string countryCode)
        {
            try
            {
                if (phone.Substring(0, 1).Equals("0"))
                {

                    if (phone.Substring(0, 4).Equals(("00" + countryCode)))
                    {
                        phone = phone.Substring(4);
                    }
                    else
                    {
                        phone = phone.Substring(1);
                    }

                }
                else
                {
                    if (phone.Substring(0, 2) == countryCode) phone = phone.Substring(2);
                }
                if (phone.Length >= 7 && phone.Length <= 12 ) return true;
                return false;
            }
            catch (Exception ex)
            {

                return false;
            }

        }
    }
}
