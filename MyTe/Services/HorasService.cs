using Microsoft.CodeAnalysis.CSharp.Syntax;
using MyTe.DAL;
using MyTe.Models.Contexts;
using MyTe.Models.DTO;
using MyTe.Models.Entities;


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
                            EmailFuncionario = f.Email,
                            WBSTipo = w.Tipo == 1? "Alocado" : "Não Alocado"
                        };
            if (idFuncionario > 0)
            {
                return lista.Where(p => p.FuncionarioId == idFuncionario).ToList();
            }
            return lista.ToList();

        }
    }
}