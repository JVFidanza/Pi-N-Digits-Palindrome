using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace PalindromePrimePi21
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            int increment = 979;
            int substringLength = 21;
            int PiLength = increment + substringLength;
            int start = 0;
            string Pi = GetPi(PiLength, start);
            int startIndex = 0;
            string theOneTrueMagicNumber = string.Empty;
            while (true)
            {
                string piSubstring = Pi.Substring(startIndex, substringLength);
                
                bool isPalindrome = piSubstring[0] == piSubstring.Last() && CheckPalindrome(piSubstring);
                
                if (isPalindrome)
                {
                    if (IsPrime(int.Parse(piSubstring)))
                    {
                        theOneTrueMagicNumber = piSubstring;
                        Console.WriteLine(theOneTrueMagicNumber);
                        break;
                    }
                }
                
                startIndex++;
                
                if ((startIndex + substringLength) >= PiLength)
                {
                    start += increment;
                    Pi = GetPi(PiLength, start);
                    startIndex = 0;
                }
            }
            
        }
        
        public static bool IsPrime(int number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (int)Math.Floor(Math.Sqrt(number));
          
            for (int i = 3; i <= boundary; i += 2)
                if (number % i == 0)
                    return false;
    
            return true;        
        }
        
        public static bool CheckPalindrome(string value)
        {
            return value.SequenceEqual(value.Reverse());
        }

        public static string GetPi(int length, int start)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://api.pi.delivery/v1/pi?start={start}&numberOfDigits={length}");
            request.Proxy = WebProxy.GetDefaultProxy();
            Stream objStream;
            StreamReader objReader;
            string Pi = string.Empty;
            try
            {
                objStream = request.GetResponse().GetResponseStream();
                objReader = new StreamReader(objStream);
                Pi = objReader.ReadLine();
            }
            catch (WebException e)
            {
                Thread.Sleep(1000);
                return GetPi(length, start);
            }

            Pi = Pi.Contains("content")? Pi.Substring(12) : Pi;
            return Pi;
        }
    }
}