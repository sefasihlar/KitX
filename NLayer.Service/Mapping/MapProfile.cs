using AutoMapper;
using NLayer.Core.Concreate;
using NLayer.Core.DTOs.AnimalDtos;
using NLayer.Core.DTOs.AnimalPhotoDtos;
using NLayer.Core.DTOs.IPAddressDtos;
using NLayer.Core.DTOs.ProductDtos;
using NLayer.Core.DTOs.RoleDtos;
using NLayer.Core.DTOs.UserDtos;
using NLayer.Core.DTOs.UserProduct;

namespace NLayer.Service.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {

            CreateMap<Animal, AnimalDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, GetAnimalWithProductId>().ReverseMap();
            CreateMap<Product, GetByIdWithAnimalDto>().ReverseMap();
            CreateMap<AppUser, GetByIdWithDto>().ReverseMap();
            CreateMap<UserProduct, UserProductDto>().ReverseMap();
            CreateMap<AppUser, UserRegisterDto>().ReverseMap();
            CreateMap<AppUser, AppUserDto>().ReverseMap();
            CreateMap<AppUser, UserListDto>().ReverseMap();
            CreateMap<UserProduct, UserProductDto>().ReverseMap();
            CreateMap<UserProduct, AddToUserProductDto>().ReverseMap();
            CreateMap<Product, GetWithProductDto>().ReverseMap();
            CreateMap<Animal, UpdateAnimalDto>().ReverseMap();
            CreateMap<IPAddress, IPAddressDto>().ReverseMap();
            CreateMap<AnimalPhoto, AnimalPhotoDto>().ReverseMap();

            CreateMap<AppRole, AppRoleDto>().ReverseMap();
            CreateMap<AppRole, AppRoleListDto>().ReverseMap();
            CreateMap<AppRole, UpdateRoleDto>().ReverseMap();
            CreateMap<AppRole, RoleAssingDto>().ReverseMap();
            CreateMap<AppRole, AppUserRoleDto>();
            CreateMap<string, AppUserRoleDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src));



        }
    }
}