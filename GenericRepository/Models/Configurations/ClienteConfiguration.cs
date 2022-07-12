using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenericRepository.Models.Configurations
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Nombre)
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.Property(t => t.Genero)
                .HasColumnType("varchar(20)");

            builder.Property(t => t.Identificacion)
                .HasColumnType("varchar(20)");

            builder.Property(t => t.Direccion)
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.Property(t => t.Edad);

            builder.Property(t => t.Telefono)
                .HasColumnType("varchar(20)")
                .IsRequired();

            builder.Property(t => t.Contrasena)
                .HasColumnType("varchar(20)")
                .IsRequired();

            builder.Property(t => t.Estado)
                .HasDefaultValue(true);
        }
    }
}
