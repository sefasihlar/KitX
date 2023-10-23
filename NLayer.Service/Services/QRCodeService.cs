using AutoMapper;
using NLayer.Core.Concreate;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.GenericManager;

namespace NLayer.Service.Services
{
    public class QRCodeService : Service<QrCode>, IQRCodeService
    {
        private readonly IQRCodeRepository _productRepository;
        readonly IQRGeneratorService _qrCodeService;

        private readonly IQRCodeRepository _qRCodeRepository;
        private readonly IMapper _mapper;


        public QRCodeService(GenericRepository<QrCode> repository, IUnitOfWork unitOfWork, IQRCodeRepository productRepository, IQRGeneratorService qrCodeService, IMapper mapper, IQRCodeRepository qRCodeRepository) : base(repository, unitOfWork)
        {
            _productRepository=productRepository;
            _qrCodeService=qrCodeService;
            _mapper=mapper;
           
            _qRCodeRepository=qRCodeRepository;
        }

        public async Task<QrCode> GetByQrCodeWithProductId(int productId)
        {
            var values = await _qRCodeRepository.GetByQrCodeWithProductId(productId);
            return values;
        }

        public async Task<List<QrCode>> GetUserProduct(int userId)
        {
            var values = await _productRepository.GetUserProduct(userId);
            return values.ToList();
        }


        //public async Task<> GetAnimalWithProductId(int id)
        //{
        //    var values = await _qr.GetAnimalWithProductId(id);
        //    return values;
        //}


        //public async Task<Product> GetByUserProduct(int productId)
        //{

        //    var values = await _productRepository.GetByUserProduct(productId);
        //    return values;
        //}

        //public async Task<List<Product>> GetProductWithUserId(int id)
        //{
        //    var values = await _productRepository.GetProductWithUserId(id);
        //    return values;
        //}

        //public async Task<byte[]> QrCodeToProductAsync(int id)
        //{
        //    var product = await _productRepository.GetByIdAsycn(id);

        //    var plainObject = "https://kitxapp.com/Detail/"+id;

        //    string planText = JsonSerializer.Serialize(plainObject);

        //    return _qrCodeService.GenerateQrCode(planText);


        //}
    }
}
