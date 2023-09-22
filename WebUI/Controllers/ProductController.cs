using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NLayer.Core.Concreate;
using NLayer.Core.DTOs.IPAddressDtos;
using NLayer.Core.DTOs.ProductDtos;
using NLayer.Core.Services;
using NLayer.Service.Services;

namespace NLayer.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIPAddressService _ipAddressService;
       
        private readonly IMapper _mapper;


        public ProductController(IProductService productService, IHttpContextAccessor httpContextAccessor, IIPAddressService ipAddressService, IMapper mapper)
        {
            _productService=productService;
            _httpContextAccessor=httpContextAccessor;
            _ipAddressService=ipAddressService;
            _mapper=mapper;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _productService.GetAllAsycn();
            return View(values);
        }




        public async Task<IActionResult> Detail(int id)
        {
            var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();

            // X-Forwarded-For başlığını kontrol et
            var xForwardedForHeader = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"];
            if (!string.IsNullOrEmpty(xForwardedForHeader))
            {
                ipAddress = xForwardedForHeader.FirstOrDefault();
            }

            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = "IP adresi bulunamadı.";
            }

            var ipAdressValues = new IPAddressDto()
            {
                IPAdress = ipAddress,
                ProductId = id

            };

            await _ipAddressService.AddAsycn(_mapper.Map<IPAddress>(ipAdressValues));

            var product = await _productService.GetByUserProduct(id);
            var valuesDto = _mapper.Map<GetWithProductDto>(product);
            return View(valuesDto);

        }

        public async Task<IActionResult> Update(int id)
        {
            var values = await _productService.GetByIdAsycn(id);
            values.Condition = true;

            await _productService.UpdateAsycn(values);
            return RedirectToAction("Index", "Product");
        }

        //public async Task<IActionResult> ViewPhoto(int id)
        //{
        //    var values = await _productService.GetByIdAsycn(id);
        //    if (values!=null)
        //    {
        //        return RedirectToAction();
        //    }


        //    return View();
        //}



    }
}
