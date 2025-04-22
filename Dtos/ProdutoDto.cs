namespace APICatalago.Dtos;

public record ProdutoDto
{
    public int ProdutoId { get; set; }
    public string Descricao { get; set; }
    public decimal Preco { get; set; }
}