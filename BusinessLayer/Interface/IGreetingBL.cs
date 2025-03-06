using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model.Entities;

namespace BusinessLayer.Interface
{
    public interface IGreetingBL
    {
        string GetGreetingMessage(string firstName, string lastName);
        string SaveGreetingMessage(string message);
        Greeting GetGreetingById(int id);
        List<Greeting> GetAllGreetings();
    }
}
