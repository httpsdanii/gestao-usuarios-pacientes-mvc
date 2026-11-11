using Microsoft.AspNetCore.Mvc;
using PrjIMV_MVC_CORE.Models;
using PrjIMV_MVC_CORE.Models.PacienteModel;
using PrjIMV_MVC_CORE.Models.PessoaModel;
using PrjIMV_MVC_CORE.Models.UsuarioModel;
using System.Text.Json;

namespace PrjIMV_MVC_CORE.Controllers
{
    public class CadPacienteController : Controller
    {
        public IActionResult Index()
        {
            CadPacienteViewModel vm = new CadPacienteViewModel();

            try
            {
                vm.listaPacientes = RecuperaLista();
            }
            catch (Exception ex)
            {
                TempData["ErrosModal"] = "Erro ao desserializar: " + ex.Message;
                
            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(CadPacienteViewModel vm)
        {
            try
            {
                String erro = "";

                vm.listaPacientes = RecuperaLista();

                if (vm.novoPaciente == null)
                {
                    TempData["ErrosModal"] = "Nenhum dado para cadastrar!";
                    return View(vm);
                }

                if (String.IsNullOrEmpty(vm.novoPaciente.Nome))
                {
                    erro = String.Concat(erro, "Nome não pode estar em branco! <br/>");
                }

                if (String.IsNullOrEmpty(vm.novoPaciente.Sobrenome))
                {
                    erro = String.Concat(erro, "Sobrenome não pode estar em branco! <br/>");
                }               

                if (!Cpf.Valida(vm.novoPaciente.Cpf.cpf))
                {
                    erro = String.Concat(erro, "CPF digitado inválido! <br/>");
                }

                vm.listaPacientes.Sort(new PacienteCpfComparer());
                var procurado = new Paciente();
                procurado.Cpf = vm.novoPaciente.Cpf;
                procurado.Cpf.cpf = Cpf.Formata(procurado.Cpf.cpf);

                int pos = vm.listaPacientes.BinarySearch(
                    procurado,
                    new PacienteCpfComparer()
                );

                if (pos > 0)
                {
                    erro = String.Concat(erro, "CPF Já cadastrado <br/>");
                }

                vm.listaPacientes.Sort();

                if (!vm.novoPaciente.Email.Valida())
                {
                    erro = String.Concat(erro, "Email digitado inválido! <br/>");
                }

              //  vm.novoPaciente.Perfil = new Perfil(TipoPerfil.TDesenvolvedor);

                // Quando o objeto é populado pelo 

                if (erro != "")
                {
                    TempData["ErrosModal"] = erro;                    
                    return View(vm);
                }

                List<Usuario> listaUsuarios = CadUserController.RecuperaLista();
                var idUsuario = HttpContext.Session.GetString("idUsuario");
                vm.novoPaciente.Id = (Paciente.contador++).ToString("D4");
                vm.novoPaciente.Responsavel = CadUserController.Busca(listaUsuarios, idUsuario ?? "0") ?? new Usuario();
                vm.listaPacientes.Add(vm.novoPaciente);

                SalvaLista(vm.listaPacientes);

                vm.novoPaciente = new Paciente();

                TempData["AvisosModal"] = "Paciente cadastrado com sucesso!";

                return View(vm);
            }
            catch (Exception ex)
            {
                TempData["ErrosModal"] = "Erro ao cadastrar: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        public static List<Paciente> RecuperaLista()
        {
            List<Paciente> listaPacientes = new List<Paciente>();

            if (System.IO.File.Exists("Pacientes.json"))
            {
                if (System.IO.File.Exists("Pacientes.json"))
                {
                    String json = System.IO.File.ReadAllText("Pacientes.json");
                    listaPacientes = JsonSerializer.Deserialize<List<Paciente>>(json) ?? new List<Paciente>();
                    listaPacientes.Sort();
                    Paciente.contador = int.Parse(listaPacientes[listaPacientes.Count - 1].Id) + 1;
                }
            }
            return listaPacientes;
        }

        public static void SalvaLista(List<Paciente> listaPacientes)
        {
            listaPacientes.Sort();
            String json = JsonSerializer.Serialize<List<Paciente>>(listaPacientes);
            System.IO.File.WriteAllText("Pacientes.json", json);
            listaPacientes = JsonSerializer.Deserialize<List<Paciente>>(json) ?? new List<Paciente>();
        }
    }
}
