namespace surgical_reports.helpers;

    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
           
         
            CreateMap<Class_Suggestion, Class_Preview_Operative_report>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.regel_1, opt => opt.MapFrom(src => src.regel_1_a + src.regel_1_b + src.regel_1_c))
            .ForMember(dest => dest.regel_2, opt => opt.MapFrom(src => src.regel_2_a + src.regel_2_b + src.regel_2_c))
            .ForMember(dest => dest.regel_3, opt => opt.MapFrom(src => src.regel_3_a + src.regel_3_b + src.regel_3_c))
            .ForMember(dest => dest.regel_4, opt => opt.MapFrom(src => src.regel_4_a + src.regel_4_b + src.regel_4_c))
            .ForMember(dest => dest.regel_5, opt => opt.MapFrom(src => src.regel_5_a + src.regel_5_b + src.regel_5_c))
            .ForMember(dest => dest.regel_6, opt => opt.MapFrom(src => src.regel_6_a + src.regel_6_b + src.regel_6_c))
            .ForMember(dest => dest.regel_7, opt => opt.MapFrom(src => src.regel_7_a + src.regel_7_b + src.regel_7_c))
            .ForMember(dest => dest.regel_8, opt => opt.MapFrom(src => src.regel_8_a + src.regel_8_b + src.regel_8_c))
            .ForMember(dest => dest.regel_9, opt => opt.MapFrom(src => src.regel_9_a + src.regel_9_b + src.regel_9_c))
            .ForMember(dest => dest.regel_10, opt => opt.MapFrom(src => src.regel_10_a + src.regel_10_b + src.regel_10_c))
            .ForMember(dest => dest.regel_11, opt => opt.MapFrom(src => src.regel_11_a + src.regel_11_b + src.regel_11_c))
            .ForMember(dest => dest.regel_12, opt => opt.MapFrom(src => src.regel_12_a + src.regel_12_b + src.regel_12_c))
            .ForMember(dest => dest.regel_13, opt => opt.MapFrom(src => src.regel_13_a + src.regel_13_b + src.regel_13_c))
            .ForMember(dest => dest.regel_14, opt => opt.MapFrom(src => src.regel_14_a + src.regel_14_b + src.regel_14_c));

            

            
        }
    }
