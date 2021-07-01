using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NovaPlay.Object.ModalForm.Elements
{
    public class NovaSliderElement : NovaElement
    {

        public string Text;
        public int Min;
        public int Max;
        public int Step;

        public NovaSliderElement(string text, int min, int max, int step)
        {
            this.Text = text;
            this.Min = min;
            this.Max = max;
            this.Step = step;
        }

        public JObject ToJson()
        {
            var obj = new JObject
            {
                { "type", "slider" },
                { "text", Text },
                { "min", Min },
                { "max", Max },
                { "step", Step }
            };
            return obj;
        }
    }
}
