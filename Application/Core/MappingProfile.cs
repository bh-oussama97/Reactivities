using Application.Activities;
using AutoMapper;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Activity, Activity>();

            CreateMap<Activity, ActivitiesDTO>()
                .ForMember(d => d.HostUsername, o => o.MapFrom
                (s => s.Attendees.FirstOrDefault(x => x.isHost).User.UserName)
                );

            CreateMap<ActivityAttendee, Profiles.Profile>()
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.User.DisplayName))
               .ForMember(d => d.Username, o => o.MapFrom(s => s.User.UserName))
             .ForMember(d => d.Bio, o => o.MapFrom(s => s.User.Bio));



        }
    }
}
