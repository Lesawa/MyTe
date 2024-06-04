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
            // Verifique se há uma mensagem de boas-vindas no TempData
            if (TempData["WelcomeMessage"] != null)
            {
                ViewBag.WelcomeMessage = TempData["WelcomeMessage"]; // Passar a mensagem para a View
                TempData.Remove("WelcomeMessage"); // Limpar o TempData para evitar exibir a mensagem novamente
            }

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
                    // Defina a mensagem de boas-vindas no TempData
                    TempData["WelcomeMessage"] = "Seja bem-vindo(a)! Seu registro foi concluído com sucesso.";
                    return RedirectToAction("Registrar");
                }
                foreach (var error in novo.Errors!)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            //Verificar se há uma mensagem de sucesso na TempData
            if (TempData.ContainsKey("SuccessMessage"))
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LogonViewModel model, string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var loginSuccess = await authService.LoginUser(model);
                if (loginSuccess)
                {
                    //Extrair a primeira parte do email do "@"
                    string userEmail = User.Identity!.Name;
                    string username = userEmail!.Split('@')[0];
                    Utils.UsuarioLogado!.Usuario = username;

                    if (User.IsInRole("Administrador"))
                    {
                        TempData["LoginSuccess"] = "Login bem-sucedido como Administrador!";
                        return RedirectToAction("Login");
                    }
                    else if (User.IsInRole("Gerente"))
                    {
                        TempData["LoginSuccess"] = "Login bem-sucedido como Gerente!";
                        return RedirectToAction("Login");
                    }
                    // Adicione aqui a ação a ser tomada para um gerente
                    else
                    {
                        TempData["LoginSuccess"] = "Login bem-sucedido como Funcionário!";
                        return RedirectToAction("Login");
                    }

                }
                else
                {
                    TempData["LoginError"] = "Credenciais inválidas";
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