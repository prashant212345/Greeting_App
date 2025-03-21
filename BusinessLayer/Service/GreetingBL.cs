using BusinessLayer.Interface;
using Microsoft.IdentityModel.Tokens;
using Middleware.GlobalExceptionHandler;
using ModelLayer.Model.DTO;
using ModelLayer.Model.Entities;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
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
            var greeting = _greetingRL.GetGreetingById(id);
            if (greeting == null)
            {
                throw new ExceptionHandler("Greeting not found!", 404);
            }
            return greeting;
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

        public async Task<string> Register(RegisterDTO model)
        {
            var existingUser = await _greetingRL.GetUserByEmail(model.Email);
            if (existingUser != null)
            {
                return "User already exists";
            }

            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = HashPassword(model.Password)
            };

            await _greetingRL.RegisterUser(user);
            return "User registered successfully";
        }
        private string HashPassword(string password)
        {
            const int SaltSize = 16;  // 16 bytes salt
            const int HashSize = 32;  // 32 bytes hash
            const int Iterations = 10000; // PBKDF2 iterations

            byte[] salt = new byte[SaltSize];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(HashSize);

                byte[] hashBytes = new byte[SaltSize + HashSize];
                Array.Copy(salt, 0, hashBytes, 0, SaltSize);
                Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

                return Convert.ToBase64String(hashBytes);
            }
        }


    }
}
