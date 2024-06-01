using Microsoft.AspNetCore.Identity;
using MyTe.Models.Entities;
using MyTe.Models.Common;

namespace MyTe.Services
{
    public class AutenticacaoService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AutenticacaoService(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public List<string?> ListRoles()
        {
            return roleManager.Roles
                .Select(p => p.Name).ToList();
        }

        public async Task<NovoUsuario> CreateUser(UsuarioViewModel model)
        {
            var novoUsuario = new NovoUsuario();

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };
            var result = await userManager.CreateAsync(user, model.Senha!);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.Perfil))
                {
                    var appRole = await roleManager.FindByNameAsync(model.Perfil);
                    if (appRole != null)
                    {
                        await userManager.AddToRoleAsync(user, model.Perfil);
                    }
                }
                await signInManager.SignInAsync(user, isPersistent: false);
                novoUsuario.Success = true;
            }
            else
            {
                novoUsuario.Errors = result.Errors;
            }
            return novoUsuario;
        }

        public async Task<bool> LoginUser(LogonViewModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.Email!, model.Senha!, model.RememberMe, false);

            if (result.Succeeded)
            {
                Utils.USERNAME = model.Email;
                return true;
            }
            return false;
        }

        public async Task LogoutUser()
        {
            await signInManager.SignOutAsync();
            Utils.UsuarioLogado!.IniciarPropriedades();
        }
    }
}