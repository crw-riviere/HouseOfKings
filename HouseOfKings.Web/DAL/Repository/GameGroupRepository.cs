using HouseOfKings.Web.Models;

namespace HouseOfKings.Web.DAL.Repository
{
    public class GameGroupRepository : Repository<GameGroup>
    {
        public GameGroupRepository(HouseOfKingsContext context)
            : base(context)
        {
        }
    }
}