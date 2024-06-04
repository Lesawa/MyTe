using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using MyTe.DAL;
using MyTe.Models.Contexts;
using MyTe.Models.DTO;
using MyTe.Models.Entities;
using System.Text.Json;


namespace MyTe.Services
{
    public class HorasService
    {
        public GenericDao<LancamentoDeHora> HorasDao { get; set; }
        public MyTeContext Context { get; set; }

        public HorasService(MyTeContext context)
        {
            this.HorasDao = new GenericDao<LancamentoDeHora>(context);
            this.Context = context;
        }

        public void Adicionar(LancamentoDeHora hora)
        {
            HorasDao.Adicionar(hora);
        }

        public IEnumerable<LancamentoDeHora> ListarHoras(int idWBS)
        {
            if (idWBS > 0)
            {
                return HorasDao.Listar().Where(c => c.WBSId == idWBS).ToList();
            }
            return HorasDao.Listar();
        }

        public void Remover(LancamentoDeHora hora)
        {
            HorasDao.Remover(hora);
        }

        public LancamentoDeHora? Buscar(int id)
        {
            return HorasDao.Buscar(id);
        }

        public IEnumerable<LancamentoDeHoraDTO> ListarHorasLINQ(int idFuncionario)
        {
            var lista = from w in Context.WBSs
                        join h in Context.Horas on w.Id equals h.WBSId
                        join f in Context.Funcionarios on h.FuncionarioId equals f.Id
                        select new LancamentoDeHoraDTO
                        {
                            Id = h.Id,
                            WBSId = w.Id,
                            CodigoWBSId = w.CodigoWBS,
                            DescricaoWBS = w.Descricao,
                            RegistroData = h.RegistroData,
                            HorasTrabalhadas = h.HorasTrabalhadas,
                            FuncionarioId = f.Id,
                            NomeFuncionario = f.Nome,
                            EmailFuncionario = f.Email
                        };
            if (idFuncionario > 0)
            {
                return lista.Where(p => p.FuncionarioId == idFuncionario).ToList();
            }
            return lista.ToList();
        }

        public IEnumerable<LancamentoDeHora> ListarHorasPorFuncionario(int funcionarioId)
        {
            // Aqui você faria a consulta no banco de dados para recuperar as horas do funcionário
            // Por exemplo, usando Entity Framework, você pode fazer algo assim:
            return Context.Horas.Where(h => h.FuncionarioId == funcionarioId).ToList();
        }
    }

        //public IEnumerable<LancamentoDeHora> ListarHorasPorPeriodo(int funcionarioId, DateTime startDate, DateTime endDate)
        //{
        //    Console.WriteLine($"Consulta por Funcionario ID: {funcionarioId}, Start Date: {startDate}, End Date: {endDate}");

        //    var resultado = Context.Horas
        //        .Where(hora => hora.FuncionarioId == funcionarioId && hora.RegistroData >= startDate && hora.RegistroData <= endDate)
        //        .ToList();

        //    Console.WriteLine($"Resultado da Consulta: {JsonSerializer.Serialize(resultado)}");

        //    return resultado;
        //}
}