using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalago.Migrations
{
    /// <inheritdoc />
    public partial class PopulaProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" + 
                   "Values ('Coca-Cola','Refrigerante Coca-Cola 350ml',5.45,'Coca-cola.jpg',50,GETDATE(),1)");
            
            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" + 
                      "Values ('Lanche-Atum','Lanche de Atum com maionese',8.45,'lanche-atum.jpg',10,GETDATE(),2)");
            
            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId)" + 
                   "Values ('Pudim 100g','Pudim leite condesado 100g',6.75,'pudim.jpg',20,GETDATE(),3)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Produtos");
        }
    }
}
