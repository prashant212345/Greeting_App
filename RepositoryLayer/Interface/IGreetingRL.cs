using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model.Entities;

namespace RepositoryLayer.Interface
{
    public interface IGreetingRL
    {
        string GetGreetingMessage(string firstName, string lastName);
        string SaveGreetingMessage(string message);
        Greeting GetGreetingById(int id);
        List<Greeting> GetAllGreetings();
        bool EditGreetingMessage(int id, string newMessage);
    }
}
