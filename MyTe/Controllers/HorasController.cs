using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyTe.Models.Common;
using MyTe.Models.Entities;
using MyTe.Services;
using System.Text.Json.Serialization;
using System.Text.Json;

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

        [HttpGet]
        public IActionResult AdicionarHora()
        {
            var listaDeWBS = wbssService.Listar().Select(wbs => new
            {
                Id = wbs.Id,
                CodigoDescricao = $"{wbs.CodigoWBS} - {wbs.Descricao}"
            }).ToList();

            ViewBag.ListaDeWBS = new SelectList(listaDeWBS, "Id", "CodigoDescricao");

            var horasSalvas = horasService.ListarHoras(0); // Carrega todas as horas
            ViewBag.HorasSalvas = horasSalvas;

            return View();
        }

        [HttpPost]
        public IActionResult AdicionarHora([FromBody] List<LancamentoDeHora> horas)
        {
            try
            {
                var userLog = Utils.USERNAME;
                if (string.IsNullOrEmpty(userLog))
                {
                    return BadRequest("User log is not available.");
                }

                var userEmailObject = funcionariosService.BuscarPorEmail(userLog);
                var userIdFuncionario = userEmailObject!.Id;

                if (userEmailObject == null)
                {
                    return BadRequest("User not found.");
                }

                if (horas == null || !horas.Any())
                {
                    ModelState.AddModelError("Horas", "Nenhuma hora foi registrada");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                horas!.ForEach(hora => hora.FuncionarioId = userIdFuncionario);
                horas.ForEach(hora => horasService.Adicionar(hora));

                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };

                return Json(new { success = true, data = horas }, options);
            }
            catch (Exception e)
            {
                return View("_Erro", e);
            }
        }

        [HttpGet]
        public IActionResult ListarHorasDoFuncionario()
        {
            try
            {
                var userLog = Utils.USERNAME;
                var userEmailObject = funcionariosService.BuscarPorEmail(userLog!);
                var funcionarioId = userEmailObject!.Id;

                var horasDoFuncionario = horasService.ListarHorasPorFuncionario(funcionarioId);

                return Json(new { success = true, data = horasDoFuncionario });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        [HttpGet]
        public IActionResult ListarHoras(int idWBS)
        {
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

        public IActionResult ListarHorasLINQ(int idFuncionario)
        {
            try
            {
                ViewBag.ListaDeFuncionario = new SelectList(funcionariosService.ListarFuncionariosDTO(), "Id", "Nome");
                return View(horasService.ListarHorasLINQ(idFuncionario));
            }
            catch (Exception e)
            {
                return View("_Erro", e);
            }
        }

        [HttpGet]
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

        [HttpPost]
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

        // Método para buscar horas salvas por período
        [HttpGet]
        public IActionResult GetHorasSalvas(string startDate, string endDate)
        {
            try
            {
                var userLog = Utils.USERNAME;
                var userEmailObject = funcionariosService.BuscarPorEmail(userLog!);
                var funcionarioId = userEmailObject!.Id;

                // Parse the date strings into DateTime
                DateTime startDateTime = DateTime.Parse(startDate);
                DateTime endDateTime = DateTime.Parse(endDate);

                // Log para verificação
                Console.WriteLine($"Funcionario ID: {funcionarioId}");
                Console.WriteLine($"Start Date: {startDateTime}, End Date: {endDateTime}");

                var horasSalvas = horasService.ListarHorasPorPeriodo(funcionarioId, startDateTime, endDateTime);

                Console.WriteLine($"Horas Salvas: {JsonSerializer.Serialize(horasSalvas)}");

                return Json(new { success = true, data = horasSalvas });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

    }
}
