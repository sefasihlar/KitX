using NLayer.Core.Concreate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Version = NLayer.Core.Concreate.Version;

namespace NLayer.Core.Repositories
{
    public interface IVersionRepository:GenericRepository<Version>
    {
    }
}
