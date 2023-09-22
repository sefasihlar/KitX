
using NLayer.Core.Repositories;
using NLayer.Repository.Concreate;
using UserProduct = NLayer.Core.Concreate.UserProduct;

namespace NLayer.Repository.Repositories
{
    public class UserProductRepository : GenericRepositoy<UserProduct>, IUserProductRepository
    {
        private readonly IIPAddressRepository _ipAddressRepository;

        public UserProductRepository(AppDbContext context) : base(context)
        {
        }

        public Task<List<Core.Concreate.IPAddress>> GetWithProductListAsync()
        {
            throw new NotImplementedException();
        }
    }
}
