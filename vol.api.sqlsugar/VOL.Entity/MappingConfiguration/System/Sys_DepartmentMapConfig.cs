using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VOL.Entity.DomainModels;
using VOL.Entity.MappingConfiguration;

namespace VOL.Entity.MappingConfiguration
{
    public class Sys_DepartmentMapConfig : EntityMappingConfiguration<Sys_Department>
    {
        public override void Map(EntityTypeBuilder<Sys_Department> builderTable)
        {
            //b.Property(x => x.StorageName).HasMaxLength(45);
        }
    }
}
