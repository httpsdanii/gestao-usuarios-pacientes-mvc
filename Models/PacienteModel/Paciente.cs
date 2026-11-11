using PrjIMV_MVC_CORE.Models.PessoaModel;
using PrjIMV_MVC_CORE.Models.UsuarioModel;
using System.Text.Json.Serialization;

namespace PrjIMV_MVC_CORE.Models.PacienteModel
{
    public class PacienteCpfComparer : IComparer<Paciente>
    {
        public int Compare(Paciente? x, Paciente? y)
        {
            return x != null && y != null ? x.Cpf.cpf.CompareTo(y.Cpf.cpf) : -1;
        }
    }

    [Serializable]
    public class Paciente : Pessoa, IComparable <Paciente>
    {
        public static int contador = 0;

        public Usuario Responsavel { get; set; } = new Usuario();

        [JsonInclude]
        public string Id { get; set; } = string.Empty;
        [JsonInclude]
        public IMC Imc { get; set; }
        [JsonInclude]
        public string Sexo { get; set; } = "N";


        [JsonConstructor]
        public Paciente() : base("", "", new Cpf(), new Email())  
        {           
            Imc = new IMC();
            Sexo="N";
        }

        public Paciente(String id) : base("", "", new Cpf(), new Email())  
        {
            Imc = new IMC();
            Id= id;
            Sexo="N";
        }

        public Paciente(string nome, string sobrenome, Cpf? cpf, Email? email, float peso, float altura, string sexo) : 
            base(nome, sobrenome, cpf, email)
        {
            Imc=new IMC(peso, altura);           
            Sexo = sexo;
        }
        int IComparable<Paciente>.CompareTo(Paciente? other)
        {
            return  other != null ? Id.CompareTo(other.Id):-1;
        }
    }
}
