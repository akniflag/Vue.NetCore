using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VOL.Entity.DomainModels;
using VOL.Entity.MappingConfiguration;

namespace VOL.Entity.MappingConfiguration
{
    public class Sys_LogMapConfig : EntityMappingConfiguration<Sys_Log>
    {
        public override void Map(EntityTypeBuilder<Sys_Log> builderTable)
        {
            //b.Property(x => x.StorageName).HasMaxLength(45);
        }
    }
}
