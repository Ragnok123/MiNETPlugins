using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NovaPlay.Object.ModalForm.Elements
{
    public class NovaDropdownElement : NovaElement
    {

        public string Text;
        public List<string> Options = new List<string>();
        public int Default = 0;

        public NovaDropdownElement(string text, List<string> options)
        {
            this.Text = text;
            this.Options = options;
        }

        public NovaDropdownElement(string text, List<string> options, int defaulte)
        {
            this.Text = text;
            this.Options = options;
            this.Default = defaulte;
        }

        public JObject ToJson()
        {
            int svar = 0;
            if(Default == 0)
            {
                svar = 0;
            }
            else
            {
                svar = Default;
            }
            var obj = new JObject
            {
                { "type", "dropdown" },
                { "text", Text },
                { "options", GetOptions()},
                { "default", svar }
            };
            return obj;
        }

        public JToken GetOptions()
        {
            var token = new JArray();
            foreach(var opt in Options)
            {
                token.Add(opt);
            }
            return token;
        }

    }
}
