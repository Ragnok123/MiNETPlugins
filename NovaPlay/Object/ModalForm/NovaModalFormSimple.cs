using System;
using System.Collections;
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
    public class NovaModalFormSimple : NovaModalForm
    {
		public string Type = "form";
		public string Title = "";
		public string Content = "";
        public List<NovaButtonElement> Buttons = new List<NovaButtonElement>();

        public NovaModalFormSimple(string title, string content)
        {
            this.Title = title;
            this.Content = content;
        }

        public void SetTitle(string text)
        {
            this.Title = text;
        }

        public void SetContent(string text)
        {
            this.Content = text;
        }

        public void AddButton(NovaButtonElement button)
        {
            this.Buttons.Add(button);
        }

        public JArray GetButtons()
        {
            var arr = new JArray();
            foreach(var but in Buttons)
            {
                arr.Add(but.ToJson());
            }
            return arr;
        }

        public string ToJson()
        {
            var data = new JObject
            {
                { "type", "form" },
                { "title", Title },
                { "content", Content },
                { "buttons", GetButtons()
                }
            };
            string json = data.ToString();
            return json;
        }

    }
}
