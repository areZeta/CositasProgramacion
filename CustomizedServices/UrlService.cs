using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CustomizedServices
{
    public class UrlService
    {
        public UrlService() { }
        public string ChangeUrlParamsToPath(string Url)
        {
            string pattern = @"\?\w*=|\&\w*=";
            string replacement = "/";
            Url = Regex.Replace(Url, pattern, replacement);
            return Url;
        }
    }
}
