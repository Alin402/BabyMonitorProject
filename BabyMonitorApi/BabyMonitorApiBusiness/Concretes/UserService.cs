using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BabyMonitorApiBusiness.Abstractions;
using BabyMonitorApiDataAccess.CustomValidationExceptions;
using BabyMonitorApiDataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using BabyMonitorApiDataAccess.Dtos.Users;
using BabyMonitorApiDataAccess.Repositories.Abstractions;

namespace BabyMonitorApiBusiness.Concretes
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _baseRepository;
        private readonly IUserRepository _userRepository;

        public UserService(IRepository<User> baseRepository, IUserRepository userRepository)
        {
            _baseRepository = baseRepository;
            _userRepository = userRepository;
        }

        public async Task<User> PostUserAsync(User user)
        {
			try
			{
                user.Id = Guid.NewGuid();

                PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
                string? password = user.Password;

                if (!string.IsNullOrEmpty(password))
                {
                    user.Password = passwordHasher.HashPassword(user, password);
                }

                return await _baseRepository.PostAsync(user);
			}
			catch (EmailAlreadyInUseException)
			{
                throw;
			}
            catch (UsernameAlreadyInUseException)
            {
                throw;
            }
        }

        public async Task<User?> AuthenticateUserAsync(string? email, string? password)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    var user = await _userRepository.FindByEmailAsync(email);

                    if (user != null)
                    {
                        if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(user.Password))
                        {
                            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
                            PasswordVerificationResult result =  passwordHasher.VerifyHashedPassword(user, user.Password, password);

                            if (result == PasswordVerificationResult.Success)
                            {
                                return user;
                            }
                            throw new IncorrectPasswordException("The provided password is incorrect");
                        }
                    }
                }
                return null;
            }
            catch (UserNotFoundException ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            catch (IncorrectPasswordException ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        
        public async Task<User?> AuthenticateUserCompareHashesAsync(string? email, string? password)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    var user = await _userRepository.FindByEmailAsync(email);

                    if (user != null)
                    {
                        if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(user.Password))
                        {
                            if (password == user.Password)
                            {
                                return user;
                            }
                            throw new IncorrectPasswordException("The provided password is incorrect");
                        }
                    }
                }
                return null;
            }
            catch (UserNotFoundException ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            catch (IncorrectPasswordException ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<GetUserDto?> GetUserAsync(string? email)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    User? user = await _userRepository.FindByEmailAsync(email);
                    if (user != null)
                    {
                        return new GetUserDto()
                        {
                            Id = user.Id,
                            Email = user.Email,
                            Name = user.Name,
                            Username = user.Username,
                            Password = user.Password
                        };
                    }
                }

                throw new UserNotFoundException("User not found");
            }
            catch (UserNotFoundException)
            {
                throw;
            }
        }
    }
}
