using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace HouseOfKings.Web.DAL.Repository
{
    public abstract class Repository<TModel> : IDisposable where TModel : class
    {
        protected DbSet<TModel> Set { get; set; }

        protected HouseOfKingsContext Context { get; set; }

        public Repository(HouseOfKingsContext context)
        {
            this.Context = context;
            this.Set = context.Set<TModel>();
        }

        public virtual async Task<TModel> GetAsync(int id)
        {
            return await this.Set.FindAsync(id);
        }

        public virtual async Task<TModel> GetAsync(string id)
        {
            return await this.Set.FindAsync(id);
        }

        public virtual async Task<List<TModel>> GetAllAsync()
        {
            return await this.Set.ToListAsync();
        }

        public virtual void Create(TModel model)
        {
            this.Set.Add(model);
        }

        public virtual void Update(TModel model)
        {
            this.Set.Attach(model);
            this.Context.Entry(model).State = EntityState.Modified;
        }

        public virtual void Delete(TModel model)
        {
            this.Set.Remove(model);
        }

        public virtual async Task SaveAsync()
        {
            await this.Context.SaveChangesAsync();
        }

        public void Dispose()
        {
        }
    }
}