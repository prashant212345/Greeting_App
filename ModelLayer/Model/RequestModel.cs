using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model
{
    public class RequestModel
    {
        public string FirstName {  get; set; }
        public string LastName { get; set; }

        public RequestModel() { }

        public RequestModel(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
