using NLayer.Core.Concreate;
using NLayer.Core.Repositories;
using NLayer.Repository.Concreate;

namespace NLayer.Repository.Repositories
{
    public class AnimalRepository : GenericRepositoy<Product>, IProductRepository
    {
        public AnimalRepository(AppDbContext context) : base(context)
        {
        }

        public Task<Product> GetAnimalWithProductId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetByUserProduct(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetProductWithUserId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> QrCodeToProductAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
