using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoHub.Controllers.V2;
using MotoHub.Data;
using MotoHub.Dto.V2.Requests;
using MotoHub.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MotoHub.Tests
{
    public class MotoV2ControllerTests
    {
        private MotoHubContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<MotoHubContext>()
                .UseInMemoryDatabase(databaseName: "MotoHubTestDb_" + System.Guid.NewGuid())
                .Options;

            var context = new MotoHubContext(options);
            context.Motos.AddRange(
                new Moto { Id = 1, Modelo = "CB 500", Marca = "Honda", Ano = 2020, Placa = "ABC-1234", Preco = 35000 },
                new Moto { Id = 2, Modelo = "MT-07", Marca = "Yamaha", Ano = 2021, Placa = "XYZ-5678", Preco = 42000 }
            );
            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task Get_DeveRetornarListaDeMotos()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new MotoV2Controller(context);

            // Act
            var resultado = await controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var lista = Assert.IsType<List<Moto>>(okResult.Value);
            Assert.Equal(2, lista.Count);
        }

        [Fact]
        public async Task GetById_DeveRetornarMoto_QuandoExistir()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new MotoV2Controller(context);

            // Act
            var resultado = await controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var moto = Assert.IsType<Moto>(okResult.Value);
            Assert.Equal("CB 500", moto.Modelo);
        }

        [Fact]
        public async Task GetById_DeveRetornarNotFound_QuandoNaoExistir()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new MotoV2Controller(context);

            // Act
            var resultado = await controller.GetById(999);

            // Assert
            Assert.IsType<NotFoundResult>(resultado);
        }

        [Fact]
        public async Task Post_DeveCriarMotoENaoRetornarErro()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new MotoV2Controller(context);
            var dto = new MotoRequestDto
            {
                Modelo = "Ninja 400",
                Marca = "Kawasaki",
                Ano = 2022,
                Placa = "JKL-9876",
                Preco = 39000
            };

            // Act
            var resultado = await controller.Post(dto);

            // Assert
            var created = Assert.IsType<CreatedAtActionResult>(resultado);
            var motoCriada = Assert.IsType<Moto>(created.Value);
            Assert.Equal("Ninja 400", motoCriada.Modelo);
        }

        [Fact]
        public async Task Put_DeveAtualizarMotoExistente()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new MotoV2Controller(context);
            var dto = new MotoRequestDto
            {
                Modelo = "CB 500X",
                Marca = "Honda",
                Ano = 2023,
                Placa = "ABC-1234",
                Preco = 36000
            };

            // Act
            var resultado = await controller.Put(1, dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var motoAtualizada = Assert.IsType<Moto>(okResult.Value);
            Assert.Equal("CB 500X", motoAtualizada.Modelo);
        }

        [Fact]
        public async Task Delete_DeveRemoverMotoExistente()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new MotoV2Controller(context);

            // Act
            var resultado = await controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(resultado);
            Assert.False(context.Motos.Any(m => m.Id == 1));
        }

        [Fact]
        public async Task Delete_DeveRetornarNotFound_QuandoMotoNaoExistir()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new MotoV2Controller(context);

            // Act
            var resultado = await controller.Delete(999);

            // Assert
            Assert.IsType<NotFoundResult>(resultado);
        }
    }
}
