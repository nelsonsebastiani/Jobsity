using JobsityChatAPI.Data;
using JobsityChatAPI.Identity.Models;
using JobsityChatAPI.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace JobsityChatAPI.Identity
{
    public class IdentityInitializer
    {
        private readonly BaseContext _context;
        private readonly UserManager<LocalUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public IdentityInitializer(
            BaseContext context,
            UserManager<LocalUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            if (_context.Database.EnsureCreated())
            {
                if (!_roleManager.RoleExistsAsync(Roles.ROLE_CHAT_API).Result)
                {
                    var resultado = _roleManager.CreateAsync(
                        new IdentityRole(Roles.ROLE_CHAT_API)).Result;
                    if (!resultado.Succeeded)
                    {
                        throw new Exception(
                            $"Error during role {Roles.ROLE_CHAT_API} creation.");
                    }
                }
            }
        }
    }
}
