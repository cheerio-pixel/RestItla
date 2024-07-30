using RestItla.Application.DTO.User;
using RestItla.Application.Extras.ResultObject;
using RestItla.Domain.Enum;

namespace RestItla.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<Result<LoginResponseDTO>> Login(LoginDTO dto);
        Task<Result<RegisterResponseDTO>> Register(RegisterDTO dto, Role role);
    }
}