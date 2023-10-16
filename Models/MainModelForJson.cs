using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WpfApp2.Models
{
    public class MainModelForJson
    {
        public string Number1
        {
            get;
            set;
        }
        public string Number2
        {
            get;
            set;
        }
        public string Result
        {
            get;
            set;

        }
        public DateTime Date
        {
            get;
            set;
        }
        public bool BadREsult {  get; set; }

        #region static methods for getting List<MainModelForJson> from json

        public static List<MainModelForJson> getListFromJson(string fileName)
        {
            List<MainModelForJson> list = new List<MainModelForJson>();
            string read=File.ReadAllText(fileName);
            string pattern = "\\\"Number1\\\": \\\"*.+\\\"*\\,\\n\\t\\\"Number2\\\": \\\"*.+\\\"*\\,\\n\\t\\\"Result\\\": \\\"*.+\\\"*\\,\\n\\t\\\"Date\\\": \".+\\\"";

            Regex rg=new Regex(pattern);
            MatchCollection matchCollection = rg.Matches(read);
            for(int i = 0; i < matchCollection.Count; i++)
            {
                list.Add(FromJson(matchCollection[i].Value));
            }
            return list;
        }
        private static string fromRegex(string pattern,string str,int startIndex)
        {
            Regex rg = new Regex(pattern);
            MatchCollection matchCollection = rg.Matches(str);
            string result = matchCollection[0].Value;
            string realResult= result.Substring(startIndex, result.Length-startIndex-1);
            if (realResult.Contains("\""))
            {
                realResult=realResult.Substring(1, realResult.Length-2);
            }
            
            return realResult;
        }
        private static MainModelForJson FromJson(string str)
        {
            MainModelForJson model = new MainModelForJson();
            model.Number1 = fromRegex("\\\"Number1\\\": \\\"*.+\\\"*\\,", str, 11);
            model.Number2 = fromRegex("\\\"Number2\\\": \\\"*.+\\\"*\\,", str, 11);
            model.Result = fromRegex("\\\"Result\\\": \\\"*.+\\\"*\\,", str, 10);
            model.Date =DateTime.Parse( fromRegex("\\\"Date\\\": \\\".+\\\"", str, 9));

            return model;
        }
        #endregion
        public override string ToString()
        {
            if (Number1.Contains(",")&&!BadREsult){
                Number1 = Number1.Replace(',', '.');
            }
            if (Number2.Contains(",") && !BadREsult)
            {
                Number2 = Number2.Replace(",", ".");
            }
            if (Result.Contains(",") && !BadREsult)
            {
                Result = Result.Replace(",", ".");
            }
            if (BadREsult)
            {
                return "{\n\t\"Number1\": " + "\"" + Number1 + "\"" + ",\n\t" +
                   "\"Number2\": " + "\"" + Number2 + "\"" + ",\n\t" +
                   "\"Result\": " + "\""+ Result + "\"" + ",\n\t" +
                   "\"Date\": " + "\"" + Date + "\"" + "\n}";
            }
            return "{\n\t\"Number1\": " + Number1 + ",\n\t" +
                   "\"Number2\": " + Number2 + ",\n\t" +
                   "\"Result\": " + Result + ",\n\t" +
                   "\"Date\": " + "\"" + Date + "\"" + "\n}";
        }
    }
}
