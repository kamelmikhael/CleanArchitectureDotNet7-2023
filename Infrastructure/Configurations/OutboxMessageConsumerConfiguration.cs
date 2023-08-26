using Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public partial class OutboxMessageConsumerConfiguration : IEntityTypeConfiguration<OutboxMessageConsumer>
{
    public void Configure(EntityTypeBuilder<OutboxMessageConsumer> builder)
    {
        builder.ToTable(TableNames.OutboxMessageConsumers, SchemaNames.OBox);

        OnConfigurePartial(builder);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<OutboxMessageConsumer> entity);
}