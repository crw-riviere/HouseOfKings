using HouseOfKings.Web.DAL.Repository;
using HouseOfKings.Web.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace HouseOfKings.Web.Controllers
{
    public class RuleController : ApiController
    {
        private RuleRepository ruleRepository;

        public RuleRepository RuleRepository
        {
            get
            {
                return this.ruleRepository ?? HttpContext.Current.GetOwinContext().Get<RuleRepository>();
            }
            set
            {
                this.ruleRepository = value;
            }
        }

        // GET: api/Rule/5
        public async Task<Rule> Get(int id)
        {
            return await this.RuleRepository.GetAsync(id);
        }
    }
}