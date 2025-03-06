using BusinessLayer.Interface;
using ModelLayer.Model.Entities;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class GreetingBL : IGreetingBL
    {
        public readonly IGreetingRL _greetingRL;
        public GreetingBL(IGreetingRL greetingRL)
        {
            _greetingRL = greetingRL;
        }
        public string GetGreetingMessage(string? firstName, string? lastName)
        {
            return _greetingRL.GetGreetingMessage(firstName, lastName);
        }
        public string SaveGreetingMessage(string message)
        {
            return _greetingRL.SaveGreetingMessage(message);
        }
        public Greeting GetGreetingById(int id)
        {
            return _greetingRL.GetGreetingById(id);
        }
        public List<Greeting> GetAllGreetings()
        {
            return _greetingRL.GetAllGreetings();
        }
        public bool EditGreetingMessage(int id, string newMessage)
        {
            return _greetingRL.EditGreetingMessage(id, newMessage);
        }
        public bool DeleteGreetingMessage(int id)
        {
            return _greetingRL.DeleteGreetingMessage(id);
        }
    }
}
