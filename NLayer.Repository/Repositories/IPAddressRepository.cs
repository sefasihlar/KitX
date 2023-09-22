using Microsoft.EntityFrameworkCore;
using NLayer.Core.Concreate;
using NLayer.Core.Repositories;
using NLayer.Repository.Concreate;

namespace NLayer.Repository.Repositories
{
    public class IPAddressRepository : GenericRepositoy<IPAddress>, IIPAddressRepository
    {
        public IPAddressRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IPAddress> GetByIpAddressWithProductId(int productId)
        {
            return await _context.IPAddresses
                .FirstOrDefaultAsync(x => x.ProductId == productId);


        }

        public async Task<List<IPAddress>> GetWithProductListAsync()
        {
            return await _context.IPAddresses
               .Include(x => x.Product)
             .ToListAsync();
        }

        //public async Task<List<IPAddress>> GetWithList()
        //{
        //    return await _context.IPAddresses
        //        .Include(x => x.Product)
        //        .ToListAsync();
        //}
    }
}
