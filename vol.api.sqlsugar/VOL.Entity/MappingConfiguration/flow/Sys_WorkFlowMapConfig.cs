using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VOL.Entity.DomainModels;
using VOL.Entity.MappingConfiguration;

namespace VOL.Entity.MappingConfiguration
{
    public class Sys_WorkFlowMapConfig : EntityMappingConfiguration<Sys_WorkFlow>
    {
        public override void Map(EntityTypeBuilder<Sys_WorkFlow> builderTable)
        {
            //b.Property(x => x.StorageName).HasMaxLength(45);
        }
    }
}
