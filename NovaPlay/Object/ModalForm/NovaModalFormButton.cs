using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NovaPlay.Object.ModalForm
{
    public class NovaModalFormButton : NovaModalForm
    {
        public string Title;
        public string Content;
        public string Button1;
        public string Button2;

        public NovaModalFormButton(string title, string content, string but1, string but2)
        {
            this.Title = title;
            this.Content = content;
            this.Button1 = but1;
            this.Button2 = but2;
        }

        public string ToJson()
        {
            var json = new JObject
            {
                { "type", "modal" },
                { "title", Title },
                { "content", Content },
                { "button1", Button1 },
                { "button2", Button2 }
            };
            return json.ToString();
        }

    }
}
