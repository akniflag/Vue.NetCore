using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VOL.Entity.DomainModels;
using VOL.Entity.MappingConfiguration;

namespace VOL.Entity.MappingConfiguration
{
    public class Sys_DictionaryMapConfig : EntityMappingConfiguration<Sys_Dictionary>
    {
        public override void Map(EntityTypeBuilder<Sys_Dictionary> builderTable)
        {
            //b.Property(x => x.StorageName).HasMaxLength(45);
        }
    }
}
