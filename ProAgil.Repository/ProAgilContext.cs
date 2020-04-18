using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ProAgil.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace ProAgil.Repository
{
    public class ProAgilContext : IdentityDbContext<User, Roles, int, 
        IdentityUserClaim<int>, 
        UserRoles, 
        IdentityUserLogin<int>, 
        IdentityRoleClaim<int>, 
        IdentityUserToken<int>>
    {
        public ProAgilContext(DbContextOptions<ProAgilContext> options) : base(options){}
        public DbSet<Evento> Eventos {get; set;}
        public DbSet<Palestrante> Palestrantes {get; set;}
        public DbSet<PalestranteEvento> PalestrantesEventos {get; set;}
        public DbSet<RedeSocial> RedeSociais {get; set;}
        public DbSet<Lote> Lotes {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRoles>(userRole => {
                userRole.HasKey(ur => new {ur.UserId, ur.RoleId});
                userRole.HasOne(ur => ur.Roles).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleId).IsRequired();
                userRole.HasOne(ur => ur.User).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.UserId).IsRequired();
            });

            modelBuilder.Entity<PalestranteEvento>()
                .HasKey(PE => new { PE.EventoId, PE.PalestranteId });
        }
    }
}