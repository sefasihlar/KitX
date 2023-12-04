using AutoMapper;
using NLayer.Core.Concreate;
using NLayer.Core.DTOs.AdminDtos.AdminAccountDtos;
using NLayer.Core.DTOs.AnimalDtos;
using NLayer.Core.DTOs.AnimalPhotoDtos;
using NLayer.Core.DTOs.CategoryDtos;
using NLayer.Core.DTOs.FeatureDtos.AnimalProductFeatureDtos;
using NLayer.Core.DTOs.FeatureProductUserDtos;
using NLayer.Core.DTOs.FeatureWithUserDtos.AnimalProductFeatureDtos;
using NLayer.Core.DTOs.FeatureWithUserDtos.BelongingProductFeatureDtos;
using NLayer.Core.DTOs.FeatureWithUserDtos.PersonProductFeatureDtos;
using NLayer.Core.DTOs.FeatureWithUserDtos.SpecialProductFeatureDtos;
using NLayer.Core.DTOs.IPAddressDtos;
using NLayer.Core.DTOs.LocationDtos;
using NLayer.Core.DTOs.ProductDtos;
using NLayer.Core.DTOs.QRCodeDtos;
using NLayer.Core.DTOs.RoleDtos;
using NLayer.Core.DTOs.UserDtos;
using NLayer.Core.DTOs.UserProduct;
using NLayer.Core.DTOs.VersionDtos;
using Version = NLayer.Core.Concreate.Version;


namespace NLayer.Service.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {

            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<QrCode, ProductDto>().ReverseMap();
            CreateMap<QrCode, GetAnimalWithProductId>().ReverseMap();
            CreateMap<QrCode, GetByIdWithAnimalDto>().ReverseMap();
            CreateMap<AppUser, GetByIdWithDto>().ReverseMap();
            CreateMap<UserProduct, UserProductDto>().ReverseMap();
            CreateMap<AppUser, UserRegisterDto>().ReverseMap();
            CreateMap<AppUser, AppUserDto>().ReverseMap();
            CreateMap<AppUser, UserListDto>().ReverseMap();
            CreateMap<UserProduct, UserProductDto>().ReverseMap();
            CreateMap<UserProduct, AddToUserProductDto>().ReverseMap();
            CreateMap<Product, GetWithProductDto>().ReverseMap();
            CreateMap<Product, UpdateAnimalDto>().ReverseMap();
            CreateMap<IPAddress, IPAddressDto>().ReverseMap();
            CreateMap<ProductPhoto, ProductPhotoDto>().ReverseMap();

            CreateMap<AppRole, AppRoleDto>().ReverseMap();
            CreateMap<AppRole, AppRoleListDto>().ReverseMap();
            CreateMap<AppRole, UpdateRoleDto>().ReverseMap();
            CreateMap<AppRole, RoleAssingDto>().ReverseMap();
            CreateMap<AppRole, AppUserRoleDto>();
            CreateMap<AppUser,AdminAccountDto>().ReverseMap();

            CreateMap<string, AppUserRoleDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src));


            CreateMap<AnimalProductFeature, AnimalProductFeatureDto>().ReverseMap();
            CreateMap<BelongingProductFeature, BelongingProductFeatureDto>().ReverseMap();
            CreateMap<PersonProductFeature, PersonProductFeatureDto>().ReverseMap();
            CreateMap<SpecialProductFeature, SpecialProductFeatureDto>().ReverseMap();
            CreateMap<BelongingProductFeature, BelongingFeatureUserDto>().ReverseMap();





            CreateMap<AnimalProductFeature, AnimalFeatureProductUserDto>().ReverseMap();
            CreateMap<AnimalProductFeature, AnimalFeatureUserDto>().ReverseMap();
   
            
            CreateMap<PersonProductFeature, PersonFeatureUserDto>().ReverseMap();
            CreateMap<SpecialProductFeature, SpecialFeatureUserDto>().ReverseMap();


            CreateMap<Category, CategoryDto>().ReverseMap();

            CreateMap<QrCode, QRCodeDto>().ReverseMap();

            CreateMap<Location,LocationDto>().ReverseMap();


            CreateMap<Version, VersionDto>().ReverseMap();






        }
    }
}