using AutoMapper;
using Ical.Net.CalendarComponents;
using Unidash.TimeTable.Models;
using Unidash.TimeTable.Models.Resources;

namespace Unidash.TimeTable
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CalendarEvent, CalendarEventEntity>(MemberList.Destination)
                .ForMember(dm => dm.Id, mo => mo.MapFrom(x => x.Uid))
                .ForMember(dm => dm.StartsAt, mo => mo.MapFrom(x => x.Start.AsUtc))
                .ForMember(dm => dm.EndsAt, mo => mo.MapFrom(x => x.End.AsUtc))
                .ForMember(dm => dm.Title, mo => mo.MapFrom(x => x.Summary))
                .ForMember(dm => dm.IsHidden, mo => mo.UseDestinationValue())
                .ForMember(dm => dm.Source, mo => mo.UseDestinationValue());

            CreateMap<CalendarEventEntity, CalendarEventResource>(MemberList.Destination);
        }
    }
}