using ModelLayer.Model.Entities;
using RepositoryLayer.Context;
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
        private readonly GreetingContext _context;

        public GreetingRL(GreetingContext context)
        {
            _context = context;
        }
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
        public string SaveGreetingMessage(string message)
        {
            var greeting = new Greeting { Message = message };
            _context.greetings.Add(greeting);
            _context.SaveChanges();
            return "Greeting message saved successfully!";
        }
        public Greeting GetGreetingById(int id)
        {
            return _context.greetings.FirstOrDefault(g => g.Id == id);
        }
    }
}
