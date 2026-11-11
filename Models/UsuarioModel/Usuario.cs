using PrjIMV_MVC_CORE.Models.PessoaModel;
using System.Text.Json.Serialization;

namespace PrjIMV_MVC_CORE.Models.UsuarioModel
{
    public class UsuarioCpfComparer : IComparer<Usuario>
    {
        public int Compare(Usuario? x, Usuario? y)
        {
            return x != null && y != null ? x.Cpf.cpf.CompareTo(y.Cpf.cpf):-1;
        }
    }

    [Serializable]
    public class Usuario : Pessoa, IComparable<Usuario>
    {
        [JsonInclude]
        public String Id { get; set; }
        [JsonInclude]
        public String Login { get; set; }
        [JsonInclude]
        public String Hash { get; set; }
        [JsonInclude]
        public Perfil Perfil { get; set; }
        [JsonInclude]
        public bool Ativo { get; set; }

        public static int contador = 1;

        public string logado()
        {
            return Hash == MyHash.GerarSha256(Cpf.cpf) ? "Não":"Sim";
        }

        public Usuario(String id) : base("", "", new Cpf(), new Email())
        {
            Id = id;
            Perfil = new Perfil(TipoPerfil.TVazio);
            Login = "";
            Hash = "";
            Ativo = true;
        }

        public Usuario() : base("","",new Cpf(), new Email())
        {
            Id = "";
            Perfil = new Perfil(TipoPerfil.TVazio);  
            Login = "";
            Hash = "";
            Ativo = true;
        }
        public Usuario(string nome, 
            string sobrenome, 
            Cpf cpf, 
            Email email,
            Perfil perfil,
            String login) : base(nome, sobrenome, cpf, email)
        {
            try
            {
                Id = "";
                Perfil = perfil;
                Login = login;                
                Hash = MyHash.GerarSha256(cpf != null ? cpf.cpf : String.Empty );
                Ativo = true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool ValidaUsuarioSenha(String login, String Senha)
        {
            if (!Ativo || login != Login || Senha == null) return false;
            String var = MyHash.GerarSha256(Senha);
            return var == Hash;
        }
        public int CompareTo(Usuario other)
        {
            return  other != null ? this.Id.CompareTo(other.Id) : -1;
        }

      
    }
}
