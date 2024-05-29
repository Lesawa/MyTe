using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTe.Models.Entities;
using MyTe.Services;

namespace MyTe.Controllers
{
    public class FuncionariosController : Controller
    {
        private readonly FuncionariosService funcionariosService;
        private readonly HorasService horasService;
        public FuncionariosController(
            FuncionariosService funcionariosService,
            HorasService horasService)
        {
            this.horasService = horasService;
            this.funcionariosService = funcionariosService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Route("funcionarios")]
        public IActionResult ListarFuncionarios()
        {
            var lista = funcionariosService.Listar();
            return View(lista);
        }

        [HttpGet("funcionarios/incluir/{id?}")]
        public IActionResult IncluirFuncionario()
        {
            return View();
        }

        [HttpPost]

        public IActionResult IncluirFuncionario(Funcionario funcionario)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                funcionariosService.Incluir(funcionario);
                return RedirectToAction("ListarFuncionarios");//Requisição GET
            }
            catch (Exception)
            {

                throw;
            };
        }
        // Action para alterar a descrição de uma área
        [HttpGet("funcionarios/alterar/{id?}")]
        public IActionResult AlterarFuncionario(int id)
        {
            try
            {
                //verificando se o id informado é válido
                if (id <= 0)
                {
                    throw new ArgumentException($"O valor informado na URL ({id}) é inválido");
                }

                Funcionario? funcionario = funcionariosService.Buscar(id);

                if (funcionario == null)
                {
                    throw new ArgumentException($"Nenhum objeto com este id : {id}");
                }
                return View(funcionario);
            }
            catch (Exception e)
            {
                return View("_Erro", e);
            };
        }

        [HttpPost("funcionarios/alterar/{id?}")]

        public IActionResult AlterarFuncionario(Funcionario funcionario)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                funcionariosService.Alterar(funcionario);
                return RedirectToAction("ListarFuncionarios");//Requisição GET
            }
            catch (Exception)
            {

                throw;
            };

        }
        [HttpGet("funcionarios/remover/{id?}")]
        public IActionResult RemoverFuncionario(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException($"O valor informado na URL ({id}) é inválido");
                }
                Funcionario? funcionario = funcionariosService.Buscar(id);
                if (funcionario == null)
                {
                    throw new ArgumentException($"Nenhum objeto com este id : {id}");
                }
                return View(funcionario);
            }
            catch (Exception e)
            {
                return View("_Erro", e);
            }
        }

        [HttpPost("funcionarios/remover/{id?}")]
        public IActionResult RemoverFuncionario(Funcionario funcionario)
        {
            try
            {
                funcionariosService.Remover(funcionario);
                return RedirectToAction("ListarFuncionarios");
            }
            catch (Exception e)
            {
                return View("_Erro", e);
            }
        }
    }
}
