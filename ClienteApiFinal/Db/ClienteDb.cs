using ClienteApiFinal.Models;
using Microsoft.EntityFrameworkCore;

namespace ClienteApiFinal.Db
{
    public class ClienteDb : DbContext
    {
        public ClienteDb(DbContextOptions<ClienteDb> options) : base(options) { }

        public DbSet<Cliente> Clientes => Set<Cliente>();

        public DbSet<Endereco> Enderecos => Set<Endereco>();

        public DbSet<Contato> Contatos => Set<Contato>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Cliente>().OwnsOne(c => c.Endereco);


            modelBuilder
                .Entity<Cliente>()
                .HasMany(e => e.Contatos)
                .WithOne()
                .OnDelete(DeleteBehavior.ClientCascade);

        }
    }
}
