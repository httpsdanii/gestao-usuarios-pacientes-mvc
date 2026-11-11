using Microsoft.AspNetCore.Mvc;
using PrjIMV_MVC_CORE.Models;
using PrjIMV_MVC_CORE.Models.PacienteModel;
using PrjIMV_MVC_CORE.Models.PessoaModel;

using System.Text.Json;

namespace PrjIMV_MVC_CORE.Controllers
{
    public class TestePacienteController : Controller
    {
       // public Paciente? Paciente;

        public List<Paciente>? lista;

        public TestePacienteController()
        {
            if (lista == null)
            {
                lista = new List<Paciente>();
            }
        }
        public IActionResult Index()
        {
            try
            {
                if (System.IO.File.Exists("pacientes.json"))
                {
                    String json = System.IO.File.ReadAllText("pacientes.json");
                    lista = JsonSerializer.Deserialize<List<Paciente>>(json) ?? new List<Paciente>();
                    lista.Sort();
                    Paciente.contador = int.Parse(lista[lista.Count - 1].Id) + 1;
                }

                TesteViewModelPaciente vm = new TesteViewModelPaciente();

                Cpf cpf = new Cpf();
                cpf.cpf = "32369280778";
                Email email = new Email();                
                email.email = "halrangel@yahoo.com.br";
                vm.paciente = new Paciente("Helio Augusto","de Lima Rangel", cpf, email, 108, 2.0F,"M");

                if (lista != null)
                {
                    lista.Add(vm.paciente);
                    String json = JsonSerializer.Serialize(lista);
                    System.IO.File.WriteAllText("pacientes.json", json);
                }

                TempData["AvisosModal"] = $"IMC Do paciente:{vm.paciente.Imc.Valor()}, Diagnóstico: {vm.paciente.Imc.Diagnostico()}"; 

                return View(vm);
            }
            catch (Exception ex)
            {
                TempData["ErrosModal"] = "Erro ao serializar: " + ex.Message;
                return RedirectToAction("Index");
            }
        }



    }
}
