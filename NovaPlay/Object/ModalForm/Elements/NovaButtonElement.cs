using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NovaPlay.Object.ModalForm.Elements
{
    public class NovaButtonElement : NovaElement
    {
        public string Text;

        public NovaButtonElement(string text)
        {
            this.Text = text;
        }

        public void SetText(string text)
        {
            this.Text = text;
        }

        public JObject ToJson()
        {
            var obj = new JObject
            {
               { "text", Text }
            };
            return obj;
        }

    }



    public class NovaImage
    {
        public string Type;

        [JsonProperty(propertyName: "data")]
        public string Url;
    }
}
