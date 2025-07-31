namespace APICatalago.Models
{
    public class Empresa
    {
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }

        public ICollection<Empresa>? Empresas { get; set; }
    }
}
