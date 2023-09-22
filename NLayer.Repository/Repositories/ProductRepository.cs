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

        public async Task<Product> GetAnimalWithProductId(int id)
        {
            return await _context.Products
                 .Include(x => x.Animal)
                 .FirstOrDefaultAsync(x => x.Id == id);


        }

        public async Task<Product> GetByUserProduct(int productId)
        {

            return await _context.Products
                .Include(x => x.UserProducts)
                .ThenInclude(x => x.User)
                .Include(x => x.Animal).FirstOrDefaultAsync(x => x.Id == productId);
        }

        public async Task<List<Product>> GetProductWithUserId(int id)
        {
            return await _context.Products
                .Include(x => x.UserProducts)
                .ThenInclude(x => x.User)
                .ToListAsync();

        }
    }
}
