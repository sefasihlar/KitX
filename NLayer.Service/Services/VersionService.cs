using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.GenericManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Version = NLayer.Core.Concreate.Version;

namespace NLayer.Service.Services
{
    public class VersionService : Service<Version>, IVersionService
    {
        public VersionService(GenericRepository<Version> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
        }
    }
}
