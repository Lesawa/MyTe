using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyTe.Models.Entities;
using MyTe.Services;

namespace MyTe.Controllers
{
    public class HorasController : Controller
    {
        private readonly WBSsService wbssService;
        private readonly FuncionariosService funcionariosService;
        private readonly HorasService horasService;
        public HorasController(
            HorasService horasService,
            FuncionariosService funcionariosService,
            WBSsService wbssService)
        {
            this.wbssService = wbssService;
            this.funcionariosService = funcionariosService;
            this.horasService = horasService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("horas/adicionar/{id?}")]
        public IActionResult AdicionarHora()
        {
            var listaDeWBS = wbssService.Listar().Select(wbs => new {
                Id = wbs.Id,
                CodigoDescricao = $"{wbs.CodigoWBS} - {wbs.Descricao}"
            }).ToList();

            ViewBag.ListaDeWBS = new SelectList(listaDeWBS, "Id", "CodigoDescricao");

            return View();
        }

        [HttpPost("horas/adicionar/{id?}")]
        public IActionResult AdicionarHora(LancamentoDeHora hora)
        {
            try
            {
                if (hora.WBSId == 0)
                {
                    ModelState.AddModelError("WBSId", "Nenhuma WBS foi selecionada");
                }

                if (!ModelState.IsValid)
                {
                    return AdicionarHora();
                }

                horasService.Adicionar(hora);
                return RedirectToAction("ListarHoras");
            }
            catch (Exception e)
            {
                return View("_Erro", e);
            }
        }

        [HttpGet]
        [Route("horas")]
        public IActionResult ListarHoras(int idWBS)
        {
            // o parametro id se refere ao id da área
            try
            {
                ViewBag.ListaDeWBS = new SelectList(wbssService.ListarWBSsDTO(), "Id", "Descricao");
                return View(horasService.ListarHoras(idWBS));
            }
            catch (Exception e)
            {
                return View("_Erro", e);
            }
        }


        public IActionResult ListarHorasLINQ(int idFuncionario)//ajustar
        {
            try
            {
                ViewBag.ListaDeFuncionario =
                    new SelectList(funcionariosService.ListarFuncionariosDTO(), "Id", "Nome");
                return View(horasService.ListarHorasLINQ(idFuncionario));
            }
            catch (Exception e)
            {
                return View("_Erro", e);
            }
        }

        [HttpGet("horas/remover/{id?}")]
        public IActionResult RemoverHora(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException($"O valor informado na URL ({id}) é inválido");
                }
                LancamentoDeHora? hora = horasService.Buscar(id);
                if (hora == null)
                {
                    throw new ArgumentException($"Nenhum objeto com este id : {id}");
                }
                return View(hora);
            }
            catch (Exception e)
            {
                return View("_Erro", e);
            }
        }

        [HttpPost("horas/remover/{id?}")]
        public IActionResult RemoverHora(LancamentoDeHora hora)
        {
            try
            {
                horasService.Remover(hora);
                return RedirectToAction("ListarHoras");
            }
            catch (Exception e)
            {
                return View("_Erro", e);
            }
        }
    }
}
