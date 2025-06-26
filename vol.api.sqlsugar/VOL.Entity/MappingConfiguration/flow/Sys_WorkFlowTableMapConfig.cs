using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VOL.Entity.DomainModels;
using VOL.Entity.MappingConfiguration;

namespace VOL.Entity.MappingConfiguration
{
    public class Sys_WorkFlowTableMapConfig : EntityMappingConfiguration<Sys_WorkFlowTable>
    {
        public override void Map(EntityTypeBuilder<Sys_WorkFlowTable> builderTable)
        {
            //b.Property(x => x.StorageName).HasMaxLength(45);
        }
    }
}
