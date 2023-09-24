using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DuarteQuestions.Model.Configuration
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(u => u.Name)
                .IsUnique();
            builder.HasIndex(u => u.Email)
                .IsUnique();
            builder.Property(p => p.Name)
                .IsRequired()
                .HasColumnType("varchar(256)");
            builder.Property(p => p.Email)
                .IsRequired()
                .HasColumnType("varchar(256)");
            builder.Property(p => p.Password)
                .IsRequired()
                .HasColumnType("varchar(256)");
        }
    }
}
