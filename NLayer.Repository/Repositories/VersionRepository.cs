using NLayer.Core.Repositories;
using NLayer.Repository.Concreate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Version = NLayer.Core.Concreate.Version;

namespace NLayer.Repository.Repositories
{
    public class VersionRepository : GenericRepositoy<Version>, IVersionRepository
    {
        public VersionRepository(AppDbContext context) : base(context)
        {
        }
    }
}
