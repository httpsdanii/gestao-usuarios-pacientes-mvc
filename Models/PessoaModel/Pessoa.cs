using System.Text.Json.Serialization;

namespace PrjIMV_MVC_CORE.Models.PessoaModel
{
    [Serializable]
    public class Pessoa  
    {       
        protected  Pessoa(String nome, String sobrenome, Cpf? cpf, Email? email)
        {
            try
            {
                Nome = nome;
                Sobrenome = sobrenome;
                Cpf = cpf==null ? new Cpf(): cpf;
                Email = email == null ? new Email() : email;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public String? Nome { get;  set; }
        public String? Sobrenome { get; set; } 

        [JsonInclude]
        public Cpf Cpf { get; set; } = new Cpf();
        [JsonInclude]
        public Email Email { get; set; } = new Email();
       
    }
}