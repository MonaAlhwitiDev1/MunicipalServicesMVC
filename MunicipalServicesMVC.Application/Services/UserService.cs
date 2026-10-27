using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using MunicipalServicesMVC.Application.Dtos;
using MunicipalServicesMVC.Domain.Models;
using MunicipalServicesMVC.Infrastructure.Repositories;

namespace MunicipalServicesMVC.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool Login(LoginDto loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.Email) ||
                string.IsNullOrEmpty(loginDto.Password))
            {
                return false;
            }

            var user = _unitOfWork.Users
                .GetByEmailWithRoleAndEmployee(
                    loginDto.Email.Trim()
                );

            if (user == null)
            {
                return false;
            }

            var passwordHasher = new PasswordHasher<User>();

            var result = passwordHasher.VerifyHashedPassword(
                user,
                user.Password,
                loginDto.Password
            );

            return result != PasswordVerificationResult.Failed;
        }

        public bool Register(RegisterDto registerDto)
        {
            var email = registerDto.Email.Trim();

            var existingUser = _unitOfWork.Users
                .GetByEmailWithRoleAndEmployee(email);

            if (existingUser != null)
            {
                return false;
            }

            var role = _unitOfWork.Roles.GetAll()
                .FirstOrDefault(r =>
                    r.Name != null &&
                    r.Name.Trim() == "مستخدم التطبيق"
                );

            if (role == null)
            {
                throw new InvalidOperationException(
                    "دور مستخدم التطبيق غير موجود"
                );
            }

            var user = new User
            {
                Name = registerDto.Name.Trim(),
                Email = email,
                RoleId = role.Id,
                EmployeeId = null
            };

            var passwordHasher = new PasswordHasher<User>();

            user.Password = passwordHasher.HashPassword(
                user,
                registerDto.Password
            );

            _unitOfWork.Users.Add(user);
            _unitOfWork.Save();

            return true;
        }
    }
}