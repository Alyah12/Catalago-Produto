using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalago.Migrations
{
    /// <inheritdoc />
    public partial class PopulaCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Categorias(Nome, ImagemUrl) Values ('bebida', 'bebida.jpg')");
            mb.Sql("Insert into Categorias(Nome, ImagemUrl) Values ('hamburguer', 'hamburguer.jpg')");
            mb.Sql("Insert into Categorias(Nome, ImagemUrl) Values (' marmita', 'marmita.jpg')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Categorias");
        }
    }
}
