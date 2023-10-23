using Microsoft.EntityFrameworkCore;
using NLayer.Core.Concreate;
using NLayer.Core.Repositories;
using NLayer.Repository.Concreate;

namespace NLayer.Repository.Repositories
{
    public class ProductRepository : GenericRepositoy<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public Task<Product> GetAnimalWithProductId(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> GetByUserProduct(int productId)
        {

            return await _context.Products
                .Include(x => x.UserProduct)
                .ThenInclude(x => x.User)
                .Include(x => x.UserProduct).FirstOrDefaultAsync(x => x.Id == productId);
        }

        public async Task<List<Product>> GetProductWithUserId(int id)
        {
            return await _context.Products
                .Include(x => x.UserProduct)
                .ThenInclude(x => x.User)
                .ToListAsync();

        }

        public Task<byte[]> QrCodeToProductAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
