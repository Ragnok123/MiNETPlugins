using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NovaPlay.Object.ModalForm.Elements
{
    public class NovaLabelElement : NovaElement
    {

        public string Text;

        public NovaLabelElement(string text)
        {
            this.Text = text;
        }

        public JObject ToJson()
        {
            var obj = new JObject
            {
                { "type", "label" },
                { "text", Text }
            };
            return obj;
        }

    }
}
