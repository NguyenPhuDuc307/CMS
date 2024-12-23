﻿using AutoMapper;
using CMS.Data.Entities.Catalog;
using CMS.Helpers;
using CMS.ViewModels.Catalog;

namespace CMS.ViewModels.Mappings
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<Message, MessageViewModel>()
                .ForMember(dst => dst.FromUserName, opt => opt.MapFrom(x => x.FromUser!.UserName))
                .ForMember(dst => dst.FromFullName, opt => opt.MapFrom(x => x.FromUser!.FirstName))
                .ForMember(dst => dst.Room, opt => opt.MapFrom(x => x.ToRoom!.Name))
                .ForMember(dst => dst.Avatar, opt => opt.MapFrom(x => x.FromUser!.Avatar))
                .ForMember(dst => dst.Content, opt => opt.MapFrom(x => BasicEmojis.ParseEmojis(x.Content!)));

            CreateMap<MessageViewModel, Message>();
        }
    }
}
