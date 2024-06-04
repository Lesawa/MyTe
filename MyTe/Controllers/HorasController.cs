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

        public int idWBS { get; private set; }

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
        public IActionResult ListarHorasPorFuncionario(string emailFuncionario)
        {
            try
            {
                // Aqui você filtra as horas do funcionário logado pelo e-mail
                var horasDoFuncionario = horasService.ListarHorasPorFuncionario(emailFuncionario);

                // Retorna a view "ListarHoras" passando a lista de horas registradas do funcionário logado como modelo
                return View("ListarHoras", horasDoFuncionario);
            }
            catch (Exception e)
            {
                // Se ocorrer um erro, retorna a view de erro com a mensagem de erro
                return View("_Erro", e);
            }
        }

        //[HttpGet]
        //public IActionResult ListarHoras()
        //{
        //    try
        //    {
        //        var userLog = Utils.USERNAME;
        //        var funcionario = funcionariosService.BuscarPorEmail(userLog!);

        //        if (funcionario == null)
        //        {
        //            return BadRequest("Funcionário não encontrado.");
        //        }

        //        // Aqui você filtra as horas do funcionário logado pelo e-mail
        //        var horasDoFuncionario = horasService.ListarHorasPorFuncionario(funcionario.Email!);

        //        // Retorna a view "ListarHoras" passando a lista de horas registradas do funcionário logado como modelo
        //        return View(horasDoFuncionario);
        //    }
        //    catch (Exception e)
        //    {
        //        // Se ocorrer um erro, retorna a view de erro com a mensagem de erro
        //        return View("_Erro", e);
        //    }
        //}

        [HttpGet]
        public IActionResult ListarHoras()
        {
            try
            {
                // Busca as horas do funcionário logado
                var userLog = Utils.USERNAME;
                var funcionario = funcionariosService.BuscarPorEmail(userLog!);
                if (funcionario == null)
                {
                    return BadRequest("Funcionário não encontrado.");
                }
                var horasDoFuncionario = horasService.ListarHorasPorFuncionario(funcionario.Email!);

                // Busca as informações de WBS para as horas encontradas
                var wbsIds = horasDoFuncionario.Select(h => h.WBSId).Distinct().ToList();
                var wbsInfo = wbssService.BuscarWBSsPorIds(wbsIds);

                // Combina as informações de WBS com as horas
                var horasComWBSInfo = from hora in horasDoFuncionario
                                      join wbs in wbsInfo on hora.WBSId equals wbs.Id
                                      select new LancamentoDeHora // Crie um novo objeto LancamentoDeHora
                                      {
                                          Id = hora.Id,
                                          FuncionarioId = hora.FuncionarioId,
                                          WBSId = hora.WBSId,
                                          RegistroData = hora.RegistroData,
                                          HorasTrabalhadas = hora.HorasTrabalhadas,
                                          Funcionario = hora.Funcionario,
                                          TipoWBS = wbs // Defina o TipoWBS como a WBS relacionada
                                      };

                // Converta para uma lista de LancamentoDeHora
                var listaHoras = horasComWBSInfo.ToList();

                return View(listaHoras);
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
    }
}