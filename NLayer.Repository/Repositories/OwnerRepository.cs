using NLayer.Core.Concreate;
using NLayer.Core.Repositories;
using NLayer.Repository.Concreate;

namespace NLayer.Repository.Repositories
{
    public class OwnerRepository : GenericRepositoy<Owner>, IOwnerRepository
    {
        public OwnerRepository(AppDbContext context) : base(context)
        {
        }
    }
}
