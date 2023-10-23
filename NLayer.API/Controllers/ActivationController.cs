using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Controllers.BaseController;
using NLayer.Core.Concreate;
using NLayer.Core.DTOs;
using NLayer.Core.DTOs.ActivationDtos;
using NLayer.Core.DTOs.UserProduct;
using NLayer.Core.Repositories;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    [Authorize(AuthenticationSchemes = "Roles")]
    [EnableCors("AllowMyOrigin")]
    [Route("api/[controller]")]
    [ApiController]

    public class ActivationController : CustomBaseController
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IQRCodeService _qrCodeService;
        private readonly IAppUserRepository _appUserRepository;
        private readonly IUserProductService _userProductService;

        public ActivationController(IProductService productService, IMapper mapper, IQRCodeService qrCodeService, IAppUserRepository appUserRepository, IUserProductService userProductService)
        {
            _productService=productService;
            _mapper=mapper;
            _qrCodeService=qrCodeService;
            _appUserRepository=appUserRepository;
            _userProductService=userProductService;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> ActivationProduct(ActivationPoroductDto dto)
        {

            if (dto!=null)
            {
                var product = await _productService.GetByIdAsycn(dto.ProductId);

                if (product!=null && product.Condition==false)
                {
                    var qrcode = await _qrCodeService.GetAllAsycn();
                    var istrue = qrcode.FirstOrDefault(x => x.Code == dto.ActivationCode && x.Condition == false);

                    if (istrue!=null)
                    {
                        var userProductValues = new UserProductDto()
                        {
                            UserId = dto.UserId,
                            ProductId = dto.ProductId,
                        };

                        //daha önce bir ilişki oluşturlmuşmu
                        var userproductsin = await _userProductService.GetByIdsAsycn(dto.UserId, dto.ProductId);

                        if (userproductsin==null)
                        {
                            var result = await _userProductService.AddAsycn(_mapper.Map<UserProduct>(userProductValues));

                            product.Condition = true;
                            await _productService.UpdateAsycn(product);

                            var catagory = new ActivationCodeDto()
                            {
                                CategoryId = result.Product.CategoryId,
                            };

                            istrue.Condition = true;

                            _qrCodeService.UpdateAsycn(istrue);

                            return CreateActionResult(CustomResponseDto<ActivationCodeDto>.Success(200, catagory));
                        }
                    }

                }

                else
                {
                    return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400, "Bu ürün daha önce aktifleştirilmiş"));
                }
            }

            return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400, "Activation işlemi başarısız"));
        }

    }
}
