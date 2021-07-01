using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;

namespace NovaPlay.Object.ModalForm
{
    public interface NovaModalForm
    {

        string ToJson();

        /*public string ToJsonClass()
        {
            var settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.DefaultValueHandling = DefaultValueHandling.Include;
            settings.MissingMemberHandling = MissingMemberHandling.Error;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var content = JsonConvert.SerializeObject(this, settings);
            return content;
        }*/

    }
}
