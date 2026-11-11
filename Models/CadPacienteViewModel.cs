using PrjIMV_MVC_CORE.Models.PacienteModel;

namespace PrjIMV_MVC_CORE.Models
{
    public class CadPacienteViewModel
    {
        public List<Paciente> listaPacientes = new List<Paciente>();
        public Paciente? novoPaciente { get; set; } = new Paciente();

        public String tituloBotaoSalvar = " Cadastrar ";

        public CadPacienteViewModel()
        {
            listaPacientes = new List<Paciente>();
        }
    }
}
