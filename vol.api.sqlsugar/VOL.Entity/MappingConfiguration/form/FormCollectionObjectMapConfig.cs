using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VOL.Entity.DomainModels;
using VOL.Entity.MappingConfiguration;

namespace VOL.Entity.MappingConfiguration
{
    public class FormCollectionObjectMapConfig : EntityMappingConfiguration<FormCollectionObject>
    {
        public override void Map(EntityTypeBuilder<FormCollectionObject> builderTable)
        {
            //b.Property(x => x.StorageName).HasMaxLength(45);
        }
    }
}
