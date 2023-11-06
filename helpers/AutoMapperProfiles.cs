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


            CreateMap<Class_Preview_Operative_report, Class_Suggestion>()
            .ForMember(dest => dest.regel_1_a, opt => opt.MapFrom(src => src.regel_1))
            .ForMember(dest => dest.regel_2_a, opt => opt.MapFrom(src => src.regel_2))
            .ForMember(dest => dest.regel_3_a, opt => opt.MapFrom(src => src.regel_3))
            .ForMember(dest => dest.regel_4_a, opt => opt.MapFrom(src => src.regel_4))
            .ForMember(dest => dest.regel_5_a, opt => opt.MapFrom(src => src.regel_5))
            .ForMember(dest => dest.regel_6_a, opt => opt.MapFrom(src => src.regel_6))
            .ForMember(dest => dest.regel_7_a, opt => opt.MapFrom(src => src.regel_7))
            .ForMember(dest => dest.regel_8_a, opt => opt.MapFrom(src => src.regel_8))
            .ForMember(dest => dest.regel_9_a, opt => opt.MapFrom(src => src.regel_9))
            .ForMember(dest => dest.regel_10_a, opt => opt.MapFrom(src => src.regel_10))
            .ForMember(dest => dest.regel_11_a, opt => opt.MapFrom(src => src.regel_11))
            .ForMember(dest => dest.regel_12_a, opt => opt.MapFrom(src => src.regel_12))
            .ForMember(dest => dest.regel_13_a, opt => opt.MapFrom(src => src.regel_13))
            .ForMember(dest => dest.regel_14_a, opt => opt.MapFrom(src => src.regel_14));



            CreateMap<InstitutionalDTO, Class_Preview_Operative_report>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.regel_1, opt => opt.MapFrom(src => src.Regel1A + src.Regel1B + src.Regel1C))
            .ForMember(dest => dest.regel_2, opt => opt.MapFrom(src => src.Regel2A + src.Regel2B + src.Regel2C))
            .ForMember(dest => dest.regel_3, opt => opt.MapFrom(src => src.Regel3A + src.Regel3B + src.Regel3C))
            .ForMember(dest => dest.regel_4, opt => opt.MapFrom(src => src.Regel4A + src.Regel4B + src.Regel4C))
            .ForMember(dest => dest.regel_5, opt => opt.MapFrom(src => src.Regel5A + src.Regel5B + src.Regel5C))
            .ForMember(dest => dest.regel_6, opt => opt.MapFrom(src => src.Regel6A + src.Regel6B + src.Regel6C))
            .ForMember(dest => dest.regel_7, opt => opt.MapFrom(src => src.Regel7A + src.Regel7B + src.Regel7C))
            .ForMember(dest => dest.regel_8, opt => opt.MapFrom(src => src.Regel8A + src.Regel8B + src.Regel8C))
            .ForMember(dest => dest.regel_9, opt => opt.MapFrom(src => src.Regel9A + src.Regel9B + src.Regel9C))
            .ForMember(dest => dest.regel_10, opt => opt.MapFrom(src => src.Regel10A + src.Regel10B + src.Regel10C))
            .ForMember(dest => dest.regel_11, opt => opt.MapFrom(src => src.Regel11A + src.Regel11B + src.Regel11C))
            .ForMember(dest => dest.regel_12, opt => opt.MapFrom(src => src.Regel12A + src.Regel12B + src.Regel12C))
            .ForMember(dest => dest.regel_13, opt => opt.MapFrom(src => src.Regel13A + src.Regel13B + src.Regel13C))
            .ForMember(dest => dest.regel_14, opt => opt.MapFrom(src => src.Regel14A + src.Regel14B + src.Regel14C))
            .ForMember(dest => dest.regel_15, opt => opt.MapFrom(src => src.Regel15))
            .ForMember(dest => dest.regel_16, opt => opt.MapFrom(src => src.Regel16))
            .ForMember(dest => dest.regel_17, opt => opt.MapFrom(src => src.Regel17))
            .ForMember(dest => dest.regel_18, opt => opt.MapFrom(src => src.Regel18))
            .ForMember(dest => dest.regel_19, opt => opt.MapFrom(src => src.Regel19))
            .ForMember(dest => dest.regel_20, opt => opt.MapFrom(src => src.Regel20))
            .ForMember(dest => dest.regel_21, opt => opt.MapFrom(src => src.Regel21))
            .ForMember(dest => dest.regel_22, opt => opt.MapFrom(src => src.Regel22))
            .ForMember(dest => dest.regel_23, opt => opt.MapFrom(src => src.Regel23))
            .ForMember(dest => dest.regel_24, opt => opt.MapFrom(src => src.Regel24))
            .ForMember(dest => dest.regel_25, opt => opt.MapFrom(src => src.Regel25))
            .ForMember(dest => dest.regel_26, opt => opt.MapFrom(src => src.Regel26))
            .ForMember(dest => dest.regel_27, opt => opt.MapFrom(src => src.Regel27))
            .ForMember(dest => dest.regel_28, opt => opt.MapFrom(src => src.Regel28))
            .ForMember(dest => dest.regel_29, opt => opt.MapFrom(src => src.Regel29))
            .ForMember(dest => dest.regel_30, opt => opt.MapFrom(src => src.Regel30))
            .ForMember(dest => dest.regel_31, opt => opt.MapFrom(src => src.Regel31))
            .ForMember(dest => dest.regel_32, opt => opt.MapFrom(src => src.Regel32))
            .ForMember(dest => dest.regel_33, opt => opt.MapFrom(src => src.Regel33))
           
            
            
            
            
            
            
            
            
            
            
            ;



            CreateMap<InstitutionalDTO, Class_Suggestion>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.user, opt => opt.Ignore())
            .ForMember(dest => dest.soort, opt => opt.Ignore());

            CreateMap<PreviewForReturnDTO, Class_Preview_Operative_report>().ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<PreviewForReturnDTO, Class_privacy_model>();

            CreateMap<Class_Hospital, HospitalForReturnDTO>();

            CreateMap<HospitalForReturnDTO, Class_Hospital>()
            .ForMember(dest => dest.RegExpr, opt => opt.Ignore())
            .ForMember(dest => dest.SampleMrn, opt => opt.Ignore())
            .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());


            
        }
    }
