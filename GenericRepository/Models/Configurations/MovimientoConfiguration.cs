using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenericRepository.Models.Configurations
{
    public class MovimientoConfiguration : IEntityTypeConfiguration<Movimiento>
    {
        public void Configure(EntityTypeBuilder<Movimiento> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.TipoMovimiento)
                .HasColumnType("varchar(1)")
                .IsRequired();

            builder.Property(t => t.Fecha)
                .HasColumnType("datetime")
                .HasDefaultValueSql("getdate()")
                .IsRequired();

            builder.Property(t => t.Valor)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(t => t.Saldo)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(t => t.Estado)
                .HasDefaultValue(true);
        }
    }
}
