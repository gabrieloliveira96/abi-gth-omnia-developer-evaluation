using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.CustomerId).IsRequired();
        builder.Property(s => s.CustomerName).IsRequired();
        builder.Property(s => s.BranchId).IsRequired();
        builder.Property(s => s.BranchName).IsRequired();
        builder.Property(s => s.Date).IsRequired();
        builder.Property(s => s.IsCancelled).IsRequired();
        builder
            .HasMany(s => s.Items)
            .WithOne(si => si.Sale)
            .HasForeignKey(si => si.SaleId);

    }
}
