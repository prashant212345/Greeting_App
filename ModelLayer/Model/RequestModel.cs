using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model
{
    public class RequestModel
    {
        public string Key {  get; set; }
        public string Value { get; set; }

        public RequestModel() { }

        public RequestModel(string key, string value)
        {
            Key = key;
            Value = value ?? throw new ArgumentNullException(nameof(value), "Value cannot be null");
        }
    }
}
