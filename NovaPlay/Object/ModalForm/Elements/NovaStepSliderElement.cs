using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NovaPlay.Object.ModalForm.Elements
{
    public class NovaStepSliderElement : NovaElement
    {

        public string Text;
        public int Value;
        public List<string> Steps = new List<string>();

        public NovaStepSliderElement(string text, int value, List<string> steps)
        {
            this.Text = text;
            this.Value = value;
            this.Steps = steps;
        }

        public JArray GetSteps()
        {
            var arr = new JArray();
            foreach(var elements in Steps)
            {
                arr.Add(elements);
            }
            return arr;
        }

        public JObject ToJson()
        {
            var obj = new JObject
            {
                { "type", "step_slider" },
                { "text", Text },
                { "steps", GetSteps() },
                { "default", Value }
            };
            return obj;
        }

    }
}
