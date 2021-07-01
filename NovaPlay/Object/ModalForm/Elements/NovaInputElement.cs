using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace NovaPlay.Object.ModalForm.Elements
{
    public class NovaInputElement : NovaElement
    {

        public string Text;
        public string Value;

        public NovaInputElement(string text, string value)
        {
            this.Text = text;
            this.Value = value;
        }

        public JObject ToJson()
        {
            var obj = new JObject
            {
                { "type", "input" },
                { "text", Text },
                { "placeholder", Value },
                { "default", "" }
            };
            return obj;
        }

    }
}
