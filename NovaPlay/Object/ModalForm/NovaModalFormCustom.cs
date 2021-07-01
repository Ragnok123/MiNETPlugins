using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NovaPlay.Object.ModalForm.Elements;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;

namespace NovaPlay.Object.ModalForm
{
    public class NovaModalFormCustom : NovaModalForm
    {

        public string Title;
        public List<NovaElement> Content = new List<NovaElement>();

        public NovaModalFormCustom(string title)
        {
            this.Title = title;
        }

        public void AddElement(NovaElement element)
        {
            if(!(element is NovaButtonElement))
            {
                Content.Add(element);
            }
        }

        public JArray GetContent()
        {
            var content = new JArray();
            foreach(var things in Content)
            {
                content.Add(things.ToJson());
            }
            return content;
        }

        public string ToJson()
        {
            var json = new JObject
            {
                { "type", "custom_form" },
                { "title", Title },
                { "content", GetContent() }
            };
            return json.ToString();
        }

    }
}
