using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class GreetingRL:IGreetingRL
    {
        public string GetGreetingMessage(string? firstName, string? lastName)
        {
            if(!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                return "Hello " + firstName + " " + lastName;
            }
            else if(!string.IsNullOrEmpty(firstName))
            {
                return "Hello " + firstName;
            }
            else if (!string.IsNullOrEmpty(lastName))
            {
                return "Hello " + lastName;
            }
            else
            {
                return "Hello World";
            }
        }
    }
}
