using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VOL.Entity.DomainModels;
using VOL.Entity.MappingConfiguration;

namespace VOL.Entity.MappingConfiguration
{
    public class Sys_RoleMapConfig : EntityMappingConfiguration<Sys_Role>
    {
        public override void Map(EntityTypeBuilder<Sys_Role> builderTable)
        {
            //b.Property(x => x.StorageName).HasMaxLength(45);
        }
    }
}
