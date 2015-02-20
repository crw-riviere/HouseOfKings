using AutoMapper;
using HouseOfKings.Web.Models;
using HouseOfKings.Web.ViewModels;

namespace HouseOfKings.Web.App_Start
{
    public static class AutoMapperWebConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile(new PlayerProfile());
            });
        }
    }

    public class PlayerProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Player, PlayerViewModel>();
        }
    }
}