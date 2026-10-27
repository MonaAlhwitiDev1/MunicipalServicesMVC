using MunicipalServicesMVC.Application.Dtos;

namespace MunicipalServicesMVC.Application.Services
{
    public interface IUserService
    {
        bool Login(LoginDto loginDto);

        bool Register(RegisterDto registerDto);
    }
}