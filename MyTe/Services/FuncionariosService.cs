using MyTe.DAL;
using MyTe.Models.Contexts;
using MyTe.Models.Entities;


namespace MyTe.Services
{
    public class FuncionariosService
    {
        public GenericDao<Funcionario> FuncionariosDao { get; set; }
        public MyTeContext Context { get; set; }
        public FuncionariosService(MyTeContext context)
        {
            this.FuncionariosDao = new GenericDao<Funcionario>(context);
            Context = context;
        }
        public IEnumerable<Funcionario> Listar()
        {
            return FuncionariosDao.Listar();
        }
        public Funcionario? Buscar(int id)
        {
            return FuncionariosDao.Buscar(id);
        }
        public void Incluir(Funcionario funcionario)
        {
            FuncionariosDao.Adicionar(funcionario);
        }
        public void Alterar(Funcionario funcionario)
        {
            FuncionariosDao.Alterar(funcionario);
        }
        public void Remover(Funcionario funcionario)
        {
            FuncionariosDao.Remover(funcionario);
        }

        public IEnumerable<FuncionarioDTO> ListarFuncionariosDTO()
        {
            List<FuncionarioDTO> funcionarios = new List<FuncionarioDTO>();
            foreach (var item in FuncionariosDao.Listar())
            {
                funcionarios.Add(new FuncionarioDTO
                {
                    Id = item.Id,
                    Nome = item.Id + " - " + item.Nome
                });
            }
            return funcionarios;
        }
        public Funcionario? BuscarPorEmail(string email)
        {
            return Context.Funcionarios.FirstOrDefault(x => x.Email == email);
        }
    }
}