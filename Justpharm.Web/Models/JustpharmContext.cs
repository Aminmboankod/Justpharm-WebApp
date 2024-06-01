using Microsoft.EntityFrameworkCore;

namespace Justpharm.Web.Models;

public partial class JustpharmContext : DbContext
{
    public JustpharmContext()
    {
    }

    public JustpharmContext(DbContextOptions<JustpharmContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }

    public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }

    public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }

    public virtual DbSet<Centros> Centros { get; set; }

    public virtual DbSet<Cuidado> Cuidado { get; set; }

    public virtual DbSet<EfectoSecundario> EfectoSecundario { get; set; }

    public virtual DbSet<Favoritos> Favoritos { get; set; }

    public virtual DbSet<Medicamento> Medicamento { get; set; }

    public virtual DbSet<Notificacion> Notificacion { get; set; }

    public virtual DbSet<Paciente> Paciente { get; set; }

    public virtual DbSet<PerfilUsuario> PerfilUsuario { get; set; }

    public virtual DbSet<Toma> Toma { get; set; }

    public virtual DbSet<Tratamiento> Tratamiento { get; set; }

    public virtual DbSet<_MaestroCuidados> _MaestroCuidados { get; set; }

    public virtual DbSet<_MaestroDietas> _MaestroDietas { get; set; }

    public virtual DbSet<_MaestroNecesidades> _MaestroNecesidades { get; set; }

    public virtual DbSet<_MaestroPerfil> _MaestroPerfil { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRoles>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");
        });

        modelBuilder.Entity<AspNetUsers>(entity =>
        {
            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.HasMany(d => d.Role).WithMany(p => p.User)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRoles",
                    r => r.HasOne<AspNetRoles>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUsers>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<Centros>(entity =>
        {
            entity.Property(e => e.UidCentro).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Cuidado>(entity =>
        {
            entity.Property(e => e.UidCuidado).ValueGeneratedNever();
        });

        modelBuilder.Entity<EfectoSecundario>(entity =>
        {
            entity.HasKey(e => e.UidEfectoSecundario).HasName("EfectoSecundario_PK");

            entity.Property(e => e.UidEfectoSecundario).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.UidPacienteNavigation).WithMany(p => p.EfectoSecundario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("EfectoSecundario_Paciente_FK");
        });

        modelBuilder.Entity<Favoritos>(entity =>
        {
            entity.HasKey(e => e.UidFavoritos).HasName("Favoritos_PK");

            entity.Property(e => e.UidFavoritos).ValueGeneratedNever();

            entity.HasOne(d => d.User).WithMany(p => p.Favoritos).HasConstraintName("Favoritos_AspNetUsers_FK");
        });

        modelBuilder.Entity<Medicamento>(entity =>
        {
            entity.HasKey(e => e.UidMedicamento).HasName("PK__Medicame__3214EC075F449857");

            entity.Property(e => e.UidMedicamento).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Nombre).IsFixedLength();
        });

        modelBuilder.Entity<Notificacion>(entity =>
        {
            entity.HasKey(e => e.UidNotificacion).HasName("Notificacion_PK");

            entity.Property(e => e.UidNotificacion).ValueGeneratedNever();

            entity.HasOne(d => d.IdTratamientoNavigation).WithMany(p => p.Notificacion).HasConstraintName("Notificacion_Tratamiento_FK");
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.UidPaciente).HasName("Paciente_PK");

            entity.Property(e => e.UidPaciente).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Alergias).HasDefaultValueSql("((0))");
            entity.Property(e => e.EnfermedadCronica).HasDefaultValueSql("((0))");
            entity.Property(e => e.SexoMasculino).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.UidMaestroDietaNavigation).WithMany(p => p.Paciente).HasConstraintName("FK_Paciente__MaestroDietas");

            entity.HasOne(d => d.UidUsuarioGestorNavigation).WithMany(p => p.Paciente).HasConstraintName("FK_Paciente_PerfilUsuario");

            entity.HasOne(d => d.User).WithMany(p => p.Paciente).HasConstraintName("Paciente_AspNetUsers_FK");
        });

        modelBuilder.Entity<PerfilUsuario>(entity =>
        {
            entity.Property(e => e.UidPerfil).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.IdAspNetUserNavigation).WithOne(p => p.PerfilUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PerfilUsuario_AspNetUsers");

            entity.HasOne(d => d.UidCentroNavigation).WithMany(p => p.PerfilUsuario).HasConstraintName("FK_PerfilUsuario_Centros");

            entity.HasOne(d => d.UidMaestroPerfilNavigation).WithMany(p => p.PerfilUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PerfilUsuario__MaestroPerfil");
        });

        modelBuilder.Entity<Toma>(entity =>
        {
            entity.Property(e => e.UidToma).ValueGeneratedNever();
            entity.Property(e => e.Ingerido).HasDefaultValueSql("((0))");
            entity.Property(e => e.Mejora).HasDefaultValueSql("((0))");

            entity.HasOne(d => d.UidMedicamentoNavigation).WithMany(p => p.Toma)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Toma_Medicamento");

            entity.HasOne(d => d.UidPacienteNavigation).WithMany(p => p.Toma).HasConstraintName("FK_Toma_Paciente");

            entity.HasOne(d => d.UidTratamientoNavigation).WithMany(p => p.Toma).HasConstraintName("FK_Toma_Tratamiento");
        });

        modelBuilder.Entity<Tratamiento>(entity =>
        {
            entity.HasKey(e => e.UidTratamiento).HasName("Tratamiento_PK");

            entity.Property(e => e.UidTratamiento).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Tomas).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.UidMedicamentoNavigation).WithMany(p => p.Tratamiento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tratamiento_Medicamento");

            entity.HasOne(d => d.UidPacienteNavigation).WithMany(p => p.Tratamiento).HasConstraintName("FK_Tratamiento_Paciente");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Tratamiento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Tratamiento_AspNetUsers_FK");
        });

        modelBuilder.Entity<_MaestroCuidados>(entity =>
        {
            entity.Property(e => e.UidMaestroCuidados).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<_MaestroDietas>(entity =>
        {
            entity.Property(e => e.UidMaestroDieta).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<_MaestroNecesidades>(entity =>
        {
            entity.Property(e => e.UidNecesidades).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<_MaestroPerfil>(entity =>
        {
            entity.Property(e => e.UidMaestroPerfil).HasDefaultValueSql("(newid())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
