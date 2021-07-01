using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NovaPlay.Object.ModalForm.Elements
{
    public class NovaCheckBoxElement : NovaElement
    {
        public string Text;
        public bool Value;

        public NovaCheckBoxElement(string text, bool value)
        {
            this.Text = text;
            this.Value = value;
        }

        public JObject ToJson()
        {
            var obj = new JObject
            {
                { "type", "toggle" },
                { "text", Text },
                { "default", Value }
            };
            return obj;
        }

    }
}
