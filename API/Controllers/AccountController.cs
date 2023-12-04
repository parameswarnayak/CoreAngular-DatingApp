using API.Controllers;
using API.Core.Extensions;
using API.Core.Interfaces;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API.Controller;

public class AccountController : BaseApiController
{
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;

    public AccountController(DataContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        using var hmac = new HMACSHA512();
        AppUser appUser = new()
        {
            UserName = registerDto.UserName,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };

        _context.Users.Add(appUser);
        await _context.SaveChangesAsync();
        return new UserDto
        {
            Username = appUser.UserName,
            Token = _tokenService.CreateToken(appUser)
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        AppUser appUser = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);
        
        if(appUser == null)
        {
            return Unauthorized("Invalid User Name");
        }

        using var hmac = new HMACSHA512(appUser.PasswordSalt);
        var hashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
        
        if(!appUser.PasswordHash.CompareEqual(hashedPassword))
        {
            return Unauthorized("Invalid Password");
        }
        return new UserDto
        {
            Username = appUser.UserName,
            Token = _tokenService.CreateToken(appUser)
        };
    }
}