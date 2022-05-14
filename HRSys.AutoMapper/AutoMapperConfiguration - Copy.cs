using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using eMeeting.DTO.Attachments;
using eMeeting.DTO.Entities;
using eMeeting.DTO.Lookup;
using eMeeting.DTO.Members;
using eMeeting.DTO.Roles;
using eMeeting.DTO.Settings;
using eMeeting.Model.Model;
using eMeeting.ModelView.Attachments;
using eMeeting.ModelView.Entities;
using eMeeting.ModelView.Lookup;
using eMeeting.ModelView.Members;
using eMeeting.ModelView.Roles;
using eMeeting.ModelView.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace eMeeting.AutoMapper
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            ConfigureDTO();
            ConfigureViewModel();
        }
        private void ConfigureDTO()
        {
            CreateMap<Locations, LocationDto>()
                .ReverseMap()
                .ForMember(entity => entity.CreatedBy, opt => opt.OnlyIfNotUpdatable())
                .ForMember(entity => entity.CreatedOn, opt => opt.OnlyIfNotUpdatable())
                .ForMember(entity => entity.IsSystematic, opt => opt.OnlyIfNotUpdatable());
            CreateMap<ExternalEntities, ExternalEntityDto>()
                .ReverseMap()
                .ForMember(entity => entity.CreatedBy, opt => opt.OnlyIfNotUpdatable())
                .ForMember(entity => entity.CreatedOn, opt => opt.OnlyIfNotUpdatable())
                .ForMember(entity => entity.IsSystematic, opt => opt.OnlyIfNotUpdatable());
            CreateMap<InternalEntities, InternalEntityDto>()
                .ReverseMap()
                .ForMember(entity => entity.CreatedBy, opt => opt.OnlyIfNotUpdatable())
                .ForMember(entity => entity.CreatedOn, opt => opt.OnlyIfNotUpdatable())
                .ForMember(entity => entity.IsSystematic, opt => opt.OnlyIfNotUpdatable());
            CreateMap<Cycles, CycleDto>()
                .ReverseMap()
                .ForMember(entity => entity.CreatedBy, opt => opt.OnlyIfNotUpdatable())
                .ForMember(entity => entity.CreatedOn, opt => opt.OnlyIfNotUpdatable())
                .ForMember(entity => entity.IsSystematic, opt => opt.OnlyIfNotUpdatable())
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToString("dd/MM/yyyy")));
            CreateMap<AttachmentCategories, AttachmentCategoryDto>()
                .ReverseMap()
                .ForMember(entity => entity.CreatedBy, opt => opt.OnlyIfNotUpdatable())
                .ForMember(entity => entity.CreatedOn, opt => opt.OnlyIfNotUpdatable())
                .ForMember(entity => entity.IsSystematic, opt => opt.OnlyIfNotUpdatable());
            CreateMap<AttachmentObjectTypes, AttachmentTypeDto>()
              .ReverseMap()
              .ForMember(entity => entity.CreatedBy, opt => opt.OnlyIfNotUpdatable())
              .ForMember(entity => entity.CreatedOn, opt => opt.OnlyIfNotUpdatable())
              .ForMember(entity => entity.IsSystematic, opt => opt.OnlyIfNotUpdatable());
            CreateMap<EntityTypes, EntityTypeDto>()
              .ReverseMap()
              .ForMember(entity => entity.CreatedBy, opt => opt.OnlyIfNotUpdatable())
              .ForMember(entity => entity.CreatedOn, opt => opt.OnlyIfNotUpdatable())
              .ForMember(entity => entity.IsSystematic, opt => opt.OnlyIfNotUpdatable());
            CreateMap<EntityMembers, EntityMemberDto>()
              .ReverseMap()
              .ForMember(entity => entity.CreatedBy, opt => opt.OnlyIfNotUpdatable())
              .ForMember(entity => entity.CreatedOn, opt => opt.OnlyIfNotUpdatable());

            CreateMap<Entities, EntityDto>()
              .ReverseMap()
              .ForMember(entity => entity.CreatedBy, opt => opt.OnlyIfNotUpdatable())
              .ForMember(entity => entity.CreatedOn, opt => opt.OnlyIfNotUpdatable())
              .ForMember(entity => entity.EntityTypeId, opt => opt.OnlyIfNotUpdatable())
              .ForMember(entity => entity.TenantId, opt => opt.OnlyIfNotUpdatable())
              .ForMember(entity => entity.CreatedBy, opt => opt.OnlyIfNotUpdatable())
              .ForMember(entity => entity.CreatedOn, opt => opt.OnlyIfNotUpdatable())
              ;
            CreateMap<Committees, CommitteeDto>()
              .ReverseMap()
              .ForMember(entity => entity.CreatedBy, opt => opt.OnlyIfNotUpdatable())
              .ForMember(entity => entity.CreatedOn, opt => opt.OnlyIfNotUpdatable())
              .ForMember(entity => entity.Entity, opt => opt.OnlyIfNotUpdatable())
              .ForMember(entity => entity.EntityId, opt => opt.OnlyIfNotUpdatable());
            CreateMap<Members, MemberDto>()
             .ReverseMap()
             .ForMember(entity => entity.CreatedBy, opt => opt.OnlyIfNotUpdatable())
             .ForMember(entity => entity.CreatedDate, opt => opt.OnlyIfNotUpdatable());

            CreateMap<RoleGroup, RoleGroupDto>().ReverseMap();
            CreateMap<BusinessRoles, BusinessRoleDto>()
            .ReverseMap()
            .ForMember(entity => entity.CreatedBy, opt => opt.OnlyIfNotUpdatable())
            .ForMember(entity => entity.CreatedOn, opt => opt.OnlyIfNotUpdatable());

            CreateMap<Projects, ProjectDto>()
              .ReverseMap()
              .ForMember(entity => entity.CreatedBy, opt => opt.OnlyIfNotUpdatable())
              .ForMember(entity => entity.CreatedOn, opt => opt.OnlyIfNotUpdatable())
              .ForMember(entity => entity.Entity, opt => opt.OnlyIfNotUpdatable())
              .ForMember(entity => entity.EntityId, opt => opt.OnlyIfNotUpdatable());
            CreateMap<ApplicationUser, MemberDto>().ReverseMap();
            CreateMap<BoardOfDirectors, BoardOfDirectorDto>()
             .ReverseMap()
             .ForMember(entity => entity.CreatedBy, opt => opt.OnlyIfNotUpdatable())
             .ForMember(entity => entity.CreatedOn, opt => opt.OnlyIfNotUpdatable())
             .ForMember(entity => entity.Entity, opt => opt.OnlyIfNotUpdatable())
             .ForMember(entity => entity.EntityId, opt => opt.OnlyIfNotUpdatable());

            CreateMap<Model.Model.AttachmentData, AttachmentDataDto>()
            .ReverseMap();

            CreateMap<Model.Model.Attachments, AttachmentDto>()
             .ReverseMap()
             .ForMember(entity => entity.CreatedBy, opt => opt.OnlyIfNotUpdatable())
             .ForMember(entity => entity.CreatedOn, opt => opt.OnlyIfNotUpdatable());

            CreateMap<Model.Model.BusinessPermissions, BusinessPermissionsDto>().ReverseMap();

        }
        private void ConfigureViewModel()
        {
            CreateMap<LocationDto, LocationViewModel>().ReverseMap();
            CreateMap<ExternalEntityDto, ExternalEntityViewModel>().ReverseMap();
            CreateMap<InternalEntityDto, InternalEntityViewModel>().ReverseMap();
            CreateMap<CycleViewModel, CycleDto>().ReverseMap()
                .ForMember(member => member.StartDate, opt => opt.MapFrom(src => ((DateTime)src.StartDate).ToString("dd/MM/yyyy")))
                .ForMember(member => member.EndDate, opt => opt.MapFrom(src => ((DateTime)src.EndDate).ToString("dd/MM/yyyy")));
            CreateMap<AttachmentCategoryDto, AttachmentCategoryViewModel>().ReverseMap();
            CreateMap<AttachmentTypeDto, AttachmentTypeViewModel>().ReverseMap();
            CreateMap<EntityTypeDto, EntityTypeViewModel>().ReverseMap();
            CreateMap<EntityMemberDto, EntityMemberViewModel>().ReverseMap();
            CreateMap<EntityDto, EntityViewModel>().ReverseMap();
            CreateMap<CommitteeDto, CommitteeViewModel>().ReverseMap();
            CreateMap<MemberDto, MemberViewModel>().ReverseMap();
            CreateMap<RoleGroupDto, RoleGroupViewModel>().ReverseMap();
            CreateMap<BusinessRoleDto, BusinessRoleViewModel>().ReverseMap();
            CreateMap<ProjectDto, ProjectViewModel>().ReverseMap();
            CreateMap<ApplicationUser, MemberViewModel>().ReverseMap();
            CreateMap<BoardOfDirectorDto, BoardOfDirectorViewModel>().ReverseMap();

            CreateMap<AttachmentDataDto, AttachmentDataViewModel>().ReverseMap();
            CreateMap<AttachmentDto, AttachmentViewModel>().ReverseMap();

            CreateMap<BusinessPermissionsDto, BusinessPermissionsViewModel>().ReverseMap();
        }
        //public static void Configure(IMapperConfigurationExpression config)
        //{
        //    config.AllowNullCollections = true;
        //    ConfigureDTO(config);
        //    ConfigureViewModel(config);
        //    config.AddExpressionMapping();
        //}

        //private static void ConfigureViewModel(IMapperConfigurationExpression config)
        //{
        //    config.CreateMap<LocationDto, LocationViewModel>().ReverseMap();

        //}

        //private static void ConfigureDTO(IMapperConfigurationExpression config)
        //{
        //    config.CreateMap<Locations, LocationDto>().ReverseMap();
        //    config.CreateMap<List<LocationDto>, List<LocationViewModel>>().ReverseMap();

        //}
    }
}
