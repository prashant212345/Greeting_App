using BusinessLayer.Interface;
using Microsoft.Extensions.Configuration;
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
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.DTO;

namespace BusinessLayer.Service
{
    public class GreetingBL : IGreetingBL
    {
        private readonly IGreetingRL _greetingRL;
        private readonly IConfiguration _configuration;
        public GreetingBL(IGreetingRL greetingRL, IConfiguration configuration)
        {
            _greetingRL = greetingRL;
            _configuration = configuration;
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

        public async Task<string> Login(LoginDTO model)
        {
            var user = await _greetingRL.GetUserByEmail(model.Email);
            if (user == null || !verifyPassword(model.Password, user.Password))
            {
                return "Invalid email or password";
            }
            return GenerateJwtToken(user);
        }

        private bool verifyPassword(string password, string storedHash)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32);

                for (int i = 0; i < 32; i++)
                {
                    if (hashBytes[i + 16] != hash[i])
                        return false;
                }
            }
            return true;
        }



        public async Task<string> ForgotPassword(ForgotPasswordDTO model)
        {
            var user = await _greetingRL.GetUserByEmail(model.Email);
            if (user == null)
                return "User not found";

            // Generate JWT Token for Password Reset
            var token = GenerateJwtToken(user);

            // Send Email
            bool emailSent = SendEmail(user.Email, token);
            if (!emailSent)
                return "Error sending email";

            return "Reset password link has been sent to your email.";
        }

        public async Task<string> ResetPassword(ResetPasswordDTO model)
        {
            var email = ValidateJwtToken(model.Token);
            if (email == null)
                return "Invalid or expired token";

            var user = await _greetingRL.GetUserByEmail(email);
            if (user == null)
                return "User not found";

            user.Password = HashPassword(model.NewPassword);
            await _greetingRL.UpdateUser(user);

            return "Password reset successful!";
        }

        private string GenerateJwtToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                return jwtToken.Subject;
            }
            catch
            {
                return null;
            }
        }

        private bool SendEmail(string toEmail, string token)
        {
            try
            {
                string resetLink = $"https://yourapp.com/reset-password?token={token}";

                MailMessage message = new MailMessage
                {
                    From = new MailAddress(_configuration["Smtp:FromEmail"]),
                    Subject = "Password Reset Request",
                    Body = $"Click the link below to reset your password:\n\n{resetLink}",
                    IsBodyHtml = false
                };
                message.To.Add(toEmail);

                SmtpClient smtpClient = new SmtpClient
                {
                    Host = _configuration["Smtp:Host"],
                    Port = int.Parse(_configuration["Smtp:Port"]),
                    Credentials = new NetworkCredential(
                        _configuration["Smtp:Username"],
                        _configuration["Smtp:Password"]
                    ),
                    EnableSsl = true
                };

                smtpClient.Send(message);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}