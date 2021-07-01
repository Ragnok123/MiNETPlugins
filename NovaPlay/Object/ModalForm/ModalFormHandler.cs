using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NovaPlay.Object.ModalForm
{
    public class ModalFormHandler
    {

        public enum FormIds
        {
            Register = 1,
            Login = 2,
            RegisterError = 3,
            LoginError = 4,
            GadgetsOpen = 5
        }

        private string FixJson(string json)
        {
            var regex = new Regex(@"^\[(.+)\]$", RegexOptions.Singleline);
            var match = regex.Match(json);

            if (match.Groups.Count > 1)
            {
                var split = new Regex(@"(?<=(?:""(?:\\""|[^""])*""|)),");
                var parts = split.Split(match.Groups[1].Value);

                for (var i = 0; i < parts.Length; i++)
                {
                    if (parts[i].Trim() == "") parts[i] = "\"\"";
                }

                var fix = "[" + string.Join(",", parts) + "]";
                return fix;
            }

            return json;
        }

        public JArray GetCustom(string data)
        {
            return JsonConvert.DeserializeObject<JArray>(data);
        }
        

    }
}
