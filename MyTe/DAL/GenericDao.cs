using Microsoft.EntityFrameworkCore;
using MyTe.Models.Contexts;
using MyTe.Models.Entities;
using System.Runtime.CompilerServices;

namespace MyTe.DAL
{
    public class GenericDao<T> where T : class
    {
        private MyTeContext Context { get; set; }
        public GenericDao(MyTeContext context)
        {
            this.Context = context;
        }

        //Listando todas as entidades
        public IEnumerable<T> Listar()
        {
            return Context.Set<T>().ToList();
        }

        //Buscando uma entidade pelo id
        public T? Buscar(int id)
        {
            return Context.Set<T>().Find(id);
        }

        //Adicionar entidades
        public void Adicionar(T item)
        {
            //Context.Add<T>(item);
            Context.Entry<T>(item).State = EntityState.Added;
            Context.SaveChanges();
        }

        //Alterar Entidades
        public void Alterar(T item)
        {
            Context.Entry<T>(item).State = EntityState.Modified;
            Context.SaveChanges();
        }

        //Remover Entidades
        public void Remover(T item)
        {
            Context.Entry<T>(item).State = EntityState.Deleted;
            Context.SaveChanges();
        }
    }
}
