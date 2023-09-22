using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core.Concreate;

namespace NLayer.Repository.Configurations
{
    public class UserProcutConfiguration : IEntityTypeConfiguration<UserProduct>
    {


        public void Configure(EntityTypeBuilder<UserProduct> builder)
        {
            builder.HasKey(x => new { x.UserId, x.ProductId });
        }
    }
}
