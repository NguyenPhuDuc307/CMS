using AutoMapper;
using CMS.Data.Entities.Catalog;
using CMS.ViewModels.Catalog;

namespace Chat.Web.Mappings
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<Room, RoomViewModel>()
                .ForMember(dst => dst.Admin, opt => opt.MapFrom(x => x.Admin!.UserName));

            CreateMap<RoomViewModel, Room>();
        }
    }
}
