﻿using NLayer.Core.Concreate;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.GenericManager;

namespace NLayer.Service.Services
{
    public class OwnerService : Service<Owner>, IOwnerService
    {
        public OwnerService(GenericRepository<Owner> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
        }
    }
}
