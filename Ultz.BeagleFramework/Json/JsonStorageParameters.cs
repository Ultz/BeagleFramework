using Newtonsoft.Json;

namespace Ultz.BeagleFramework.Json
{
    public class JsonStorageParameters
    {
        public string Encoding { get; set; }
        public string Store { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}