using PrjIMV_MVC_CORE.Models.PacienteModel;

namespace PrjIMV_MVC_CORE.Models
{
    public class ManPacienteViewModel
    {
        public List<Paciente> listaPacientes = new List<Paciente>();
        public Paciente? selecionado { get; set; } = new Paciente();

        public Paciente maior = new Paciente();
        public Paciente menor = new Paciente();

        public String codigo { get; set; } = "0000";

        public float pesoEditado { get; set; } = 0;

        public float alturaEditada { get; set; } = 0;

        public String tituloBotaoSalvar = " Atualizar ";

        public ManPacienteViewModel()
        {
            listaPacientes = new List<Paciente>();
        }
    }
}

