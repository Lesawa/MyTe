using MyTe.DAL;
using MyTe.Models.Contexts;
using MyTe.Models.Entities;

namespace MyTe.Services
{
    public class WBSsService
    {
        public GenericDao<WBS> WBSsDao { get; set; }
        public WBSsService(MyTeContext context)
        {
            this.WBSsDao = new GenericDao<WBS>(context);
        }

        public IEnumerable<WBS> Listar()
        {
            return WBSsDao.Listar();
        }

        public IEnumerable<WBS> BuscarWBSsPorIds(List<int> ids)
        {
            return WBSsDao.Listar().Where(w => ids.Contains(w.Id)).ToList();
        }
        public void Incluir(WBS wbs)
        {
            WBSsDao.Adicionar(wbs);
        }

        public WBS? Buscar(int id)
        {
            return WBSsDao.Buscar(id);
        }
        public void Alterar(WBS wbs)
        {
            WBSsDao.Alterar(wbs);
        }

        public void Remover(WBS wbs)
        {
            WBSsDao.Remover(wbs);
        }
        public IEnumerable<WBSDTO> ListarWBSsDTO()
        {
            List<WBSDTO> wbss = new List<WBSDTO>();
            foreach (var item in WBSsDao.Listar())
            {
                wbss.Add(new WBSDTO
                {
                    Id = item.Id,
                    Descricao = item.CodigoWBS + " - " + item.Descricao
                });
            }
            return wbss;
        }
    }
}