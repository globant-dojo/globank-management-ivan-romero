using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenericRepository.Models.Configurations
{
    public class CuentaConfiguration : IEntityTypeConfiguration<Cuenta>
    {

        public void Configure(EntityTypeBuilder<Cuenta> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.NumeroCuenta)
                .HasColumnType("varchar(20)")
                .IsRequired();

            builder.Property(t => t.TipoCuenta)
                .HasColumnType("varchar(20)")
                .IsRequired();

            builder.Property(t => t.SaldoInicial)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(t => t.Estado)
                .HasDefaultValue(true);
        }
    }
}
