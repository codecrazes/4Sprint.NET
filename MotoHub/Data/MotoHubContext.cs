using Microsoft.EntityFrameworkCore;
using MotoHub.Models;

namespace MotoHub.Data
{
    public class MotoHubContext : DbContext
    {
        public MotoHubContext(DbContextOptions<MotoHubContext> options)
            : base(options)
        {
        }

        public DbSet<Moto> Motos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Aluguel> Alugueis { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<Manutencao> Manutencoes { get; set; }
        public DbSet<Estoque> Estoques { get; set; }

    }
}
