using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly DataContext _context;

    public UsersController(DataContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public ActionResult<AppUser> GetUser(int id)
    {
        return _context.Users.Find(id);
    }

    [HttpGet]
    public ActionResult<List<AppUser>> GetUsers(int id)
    {
        return _context.Users.ToList();
    }
}