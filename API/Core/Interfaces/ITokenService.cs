using API.Entities;

namespace API.Core.Interfaces;

public interface ITokenService
{
    string CreateToken(AppUser appUser);
}