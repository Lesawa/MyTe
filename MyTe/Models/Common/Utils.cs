using Microsoft.AspNetCore.Identity;
using MyTe.Models.Common;

namespace MyTe.Models.Common
{
    public class Utils
    {
        public static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = {"Administrador", "Usuário", "Gerente"};

            IdentityResult result;

            foreach (var role in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    result = await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
        public static string? USERNAME = null;
        public static UsuarioLogado? UsuarioLogado { get; set; } = new UsuarioLogado();

    }
}

