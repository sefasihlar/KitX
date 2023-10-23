using Microsoft.EntityFrameworkCore;
using NLayer.Core.Concreate;
using NLayer.Core.Repositories;
using NLayer.Repository.Concreate;

namespace NLayer.Repository.Repositories
{
    public class QRCodeRepository : GenericRepositoy<QrCode>, IQRCodeRepository
    {
        public QRCodeRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<QrCode> GetByQrCodeWithProductId(int productId)
        {
            return await _context.QrCodes
                 .FirstOrDefaultAsync(x => x.ProductId == productId);
        }

        public async Task<List<QrCode>> GetUserProduct(int userId)
        {
            // Veritabanı sorgusunu asenkron olarak yapmak için "await" kullanın.
            return await _context.QrCodes
             .Include(x => x.Product)
             .ThenInclude(x => x.UserProduct)
             .ToListAsync();



        }




        //public async Task<QrCode> GetByUserProduct(int productId)
        //{

        //    return await _context.QrCodes
        //        .Include(x => x.UserProducts)
        //        .ThenInclude(x => x.User)
        //        .Include(x => x.Product).FirstOrDefaultAsync(x => x.Id == productId);
        //}

        //public async Task<List<QrCode>> GetProductWithUserId(int id)
        //{
        //    return await _context.QrCodes
        //        .Include(x => x.UserProducts)
        //        .ThenInclude(x => x.User)
        //        .ToListAsync();

        //}

        //public Task<byte[]> QrCodeToProductAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
