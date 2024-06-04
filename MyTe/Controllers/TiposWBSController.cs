using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTe.Models.Entities;
using MyTe.Services;
using System.Diagnostics;

namespace MyTe.Controllers
{
    public class TiposWBSController : Controller
    {
        private readonly WBSsService wbssService;
        public TiposWBSController(WBSsService wbssService)
        {
            this.wbssService = wbssService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Route("wbs")]
        public IActionResult ListarWBSs()
        {
            var lista = wbssService.Listar();
            return View(lista);
        }

        [HttpGet("wbs/adicionar/{id?}")]
        public IActionResult IncluirWBS()
        {
            return View();
        }

        [HttpPost("wbs/adicionar/{id?}")]

        public IActionResult IncluirWBS(WBS wbs)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                wbssService.Incluir(wbs);

                // Definindo a mensagem de sucesso na TempData
                TempData["SuccessMessage"] = "WBS incluída com sucesso!";

                return RedirectToAction("IncluirWBS");//Requisição GET
            }
            catch (Exception)
            {

                throw;
            };
        }
        // Action para alterar a descrição de uma área
        [HttpGet("wbs/alterar/{id?}")]

        public IActionResult AlterarWBS(int id)
        {
            try
            {
                //verificando se o id informado é válido
                if (id <= 0)
                {
                    throw new ArgumentException($"O valor informado na URL ({id}) é inválido");
                }

                WBS? wbs = wbssService.Buscar(id);

                if (wbs == null)
                {
                    throw new ArgumentException($"Nenhum objeto com este id : {id}");
                }
                return View(wbs);
            }
            catch (Exception e)
            {
                return View("_Erro", e);
            };
        }

        [HttpPost("wbs/alterar/{id?}")]

        public IActionResult AlterarWBS(WBS wbs)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                wbssService.Alterar(wbs);

                // Definindo a mensagem de sucesso na TempData
                TempData["SuccessMessage"] = "WBS alterada com sucesso!";

                return RedirectToAction("AlterarWBS");//Requisição GET
            }
            catch (Exception)
            {

                throw;
            };

        }

        [HttpGet("wbs/remover/{id?}")]

        public IActionResult RemoverWBS(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException($"O valor informado na URL ({id}) é inválido");
                }
                WBS? wbs = wbssService.Buscar(id);
                if (wbs == null)
                {
                    throw new ArgumentException($"Nenhum objeto com este id : {id}");
                }
                return View(wbs);
            }
            catch (Exception e)
            {
                return View("_Erro", e);
            }
        }

        [HttpPost("wbs/remover/{id?}")]
        public IActionResult RemoverWBS(WBS wbs)
        {
            try
            {
                wbssService.Remover(wbs);
                return RedirectToAction("ListarWBSs");
            }
            catch (Exception e)
            {
                return View("_Erro", e);
            }
        }
    }
}