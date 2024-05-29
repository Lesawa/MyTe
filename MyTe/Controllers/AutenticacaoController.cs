using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MyTe.Models.Entities;
using Azure.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyTe.Services;
using MyTe.Models.Common;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MyTe.Controllers
{
    public class AutenticacaoController : Controller
    {
        private readonly AutenticacaoService authService;

        public AutenticacaoController(AutenticacaoService service)
        {
            this.authService = service;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Registrar()
        {
            ViewBag.Roles = new SelectList(authService.ListRoles());
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(UsuarioViewModel model)
        {
            if (ModelState.IsValid)
            {
                var novo = await authService.CreateUser(model);
                if (novo.Success)
                {
                    return RedirectToAction("Login", "Autenticacao");
                }
                foreach (var error in novo.Errors!)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Registrar();
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null) 
        { 
            //Verificar se há uma mensagem de sucesso na TempData
            if(TempData.ContainsKey("SucessMessage"))
            {
                ViewBag.SuccessMessage = TempData["SucessMessage"];
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LogonViewModel model,
            string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var loginSuccess = await authService.LoginUser(model);

                //object LoginResult = null;
                if (loginSuccess)
                {
                    //Extrair a primeira parte do email do "@"
                    string userEmail = User.Identity!.Name;
                    string username = userEmail.Split('@')[0];
                    Utils.UsuarioLogado!.Usuario = username;

                    if (returnUrl != null)
                    {
                        return Redirect(returnUrl);
                    }

                    if (User.IsInRole("Administrador"))
                    {
                        TempData["SucessMessage"] = "Login bem-sucedido como Administrador!";
                        return RedirectToAction("ListarHorasLINQ", "Horas");
                    }
                    else if (User.IsInRole("Gerente"))
                    {
                        TempData["SucessMessage"] = "Login bem-sucedido como Gerente!";
                        return RedirectToAction("ListarHorasLINQ", "Horas");
                    }
                    // Adicione aqui a ação a ser tomada para um gerente
                    else
                    {
                        TempData["SucessMessage"] = "Login bem-sucedido como Usuário!";
                        return RedirectToAction("ListarHoras", "Horas");
                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Credenciais inválidas");
                }
            }
            return View(model);
        }

        [HttpGet]

        public async Task<IActionResult> Logout()
        {
            await authService.LogoutUser();
            return RedirectToAction("Login", "Autenticacao");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            var erro = "Você não tem permissão para acessar este recurso!!";
            return View("_Erro", new Exception(erro));
        }


    }
}
