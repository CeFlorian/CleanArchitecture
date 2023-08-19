namespace NorthWind.EFCore.Repositories.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(o => o.Active)
                .IsRequired();

            builder.Property(o => o.Password)
                .IsRequired();

            builder.Property(o => o.Name)
                .HasMaxLength(30);

            builder.Property(o => o.LastName)
                .HasMaxLength(30);

            builder.Property(o => o.CreationDate)
                .IsRequired();

            builder.Property(o => o.Email)
                .IsRequired();
        }
    }
}
