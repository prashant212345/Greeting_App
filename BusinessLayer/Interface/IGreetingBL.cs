using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model.DTO;
using ModelLayer.Model.Entities;

namespace BusinessLayer.Interface
{
    public interface IGreetingBL
    {
        string GetGreetingMessage(string firstName, string lastName);
        string SaveGreetingMessage(string message);
        Greeting GetGreetingById(int id);
        List<Greeting> GetAllGreetings();
        bool EditGreetingMessage(int id, string newMessage);
        bool DeleteGreetingMessage(int id);
        Task<string> Register(RegisterDTO model);
    }
}
