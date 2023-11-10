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
    [EnableCors("AllowMyOrigin")]
    [Authorize(AuthenticationSchemes = "Roles")]
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
            // Check if the incoming DTO is not null
            if (dto != null)
            {
                // Retrieve the product by its ID
                var product = await _productService.GetByIdAsycn(dto.ProductId);

                // If the product is not found, return an error response
                if (product == null)
                    return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400, "Product not found"));

                // If the product exists and its condition is false, proceed
                if (product != null && product.Condition == false)
                {
                    // Get all QR codes
                    var qrcode = await _qrCodeService.GetAllAsycn();

                    // Find the QR code with the given activation code and false condition
                    var istrue = qrcode.FirstOrDefault(x => x.Code == dto.ActivationCode && x.Condition == false);

                    // If a valid QR code is found
                    if (istrue != null)
                    {
                        // Prepare user product data
                        var userProductValues = new UserProductDto()
                        {
                            UserId = dto.UserId,
                            ProductId = dto.ProductId,
                        };

                        // Check if a relationship already exists
                        var userproductsin = await _userProductService.GetByIdsAsycn(dto.UserId, dto.ProductId);

                        // If no relationship exists
                        if (userproductsin == null)
                        {
                            // Add a new user product relationship
                            var result = await _userProductService.AddAsycn(_mapper.Map<UserProduct>(userProductValues));

                            // Update product condition to true
                            product.Condition = true;
                            await _productService.UpdateAsycn(product);

                            // Prepare category data for the response
                            var catagory = new ActivationCodeDto()
                            {
                                CategoryId = result.Product.CategoryId,
                            };

                            // Update the QR code condition to true
                            istrue.Condition = true;
                            _qrCodeService.UpdateAsycn(istrue);

                            // Return a success response with category information
                            return CreateActionResult(CustomResponseDto<ActivationCodeDto>.Success(200, catagory));
                        }
                    }
                }
                else
                {
                    // Return an error response if the product has been activated before
                    return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400, "This product has been activated before"));
                }
            }

            // Return an error response for unsuccessful activation
            return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(400, "Activation process failed"));
        }


    }
}
