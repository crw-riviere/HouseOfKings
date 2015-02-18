using HouseOfKings.Web.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HouseOfKings.Web.DAL.Repository
{
    public class RuleRepository : Repository<Rule>
    {
        public static RuleRepository Create()
        {
            return new RuleRepository(HttpContext.Current.GetOwinContext().Get<HouseOfKingsContext>());
        }

        public RuleRepository(HouseOfKingsContext context)
            : base(context)
        {
        }

        public Rule Get(int id)
        {
            return this.Set.FirstOrDefault(x => x.Number == id);
        }

        public override async System.Threading.Tasks.Task<Rule> GetAsync(int id)
        {
            return await this.Set.FirstOrDefaultAsync(x => x.Number == id);
        }
    }
}