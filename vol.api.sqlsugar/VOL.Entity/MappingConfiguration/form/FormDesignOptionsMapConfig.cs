using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VOL.Entity.DomainModels;
using VOL.Entity.MappingConfiguration;

namespace VOL.Entity.MappingConfiguration
{
    public class FormDesignOptionsMapConfig : EntityMappingConfiguration<FormDesignOptions>
    {
        public override void Map(EntityTypeBuilder<FormDesignOptions> builderTable)
        {
            //b.Property(x => x.StorageName).HasMaxLength(45);
        }
    }
}
