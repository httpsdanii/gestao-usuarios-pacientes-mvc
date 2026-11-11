using Microsoft.AspNetCore.Mvc;
using PrjIMV_MVC_CORE.Models;
using PrjIMV_MVC_CORE.Models.PacienteModel;
using PrjIMV_MVC_CORE.Models.PessoaModel;
using PrjIMV_MVC_CORE.Models.UsuarioModel;
using System.Text.Json;

namespace PrjIMV_MVC_CORE.Controllers
{
    public class ManPacienteController : Controller
    {
        public IActionResult Index()
        {
            ManPacienteViewModel vm = new ManPacienteViewModel();

            try
            {
                vm.listaPacientes = CadPacienteController.RecuperaLista();
            }
            catch (Exception ex)
            {
                TempData["ErrosModal"] = "Erro ao desserializar: " + ex.Message;

            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(ManPacienteViewModel vm)
        {
            try
            {              

                vm.listaPacientes = CadPacienteController.RecuperaLista();

                if (vm.listaPacientes.Count == 0)
                {
                    TempData["ErrosModal"] = "Nenhum paciente cadastrado!";
                    return View(vm);
                }

                vm.maior = vm.listaPacientes[0];
                vm.menor = vm.listaPacientes[0];

                foreach(Paciente p in vm.listaPacientes)
                {
                    if (p.Imc.Valor() > vm.maior.Imc.Valor()) vm.maior = p;
                    if (p.Imc.Valor() < vm.menor.Imc.Valor()) vm.menor = p;
                }

                if (vm.selecionado == null)
                {
                    TempData["ErrosModal"] = "Nenhum paciente para editar!";
                    return View(vm);
                } 

                vm.listaPacientes.Sort(); 

                if (!int.TryParse(vm.codigo, out int iCodigo))
                {
                    TempData["ErrosModal"] = "Id do usuário deve ser numérico!";
                    return View(vm);
                }

                vm.codigo = iCodigo.ToString("D4");

                List<Usuario> listaUsuarios = CadUserController.RecuperaLista();
                
                if (!String.IsNullOrEmpty(vm.codigo)) // Esta buscando
                {
                    Paciente pacienteBusca = new Paciente(vm.codigo);
                    int pos = vm.listaPacientes.BinarySearch(pacienteBusca);
                    if (pos >=0)
                    {
                        var idUsuario = HttpContext.Session.GetString("idUsuario");
                        vm.selecionado = vm.listaPacientes[pos]; 
                        if (vm.pesoEditado >= 10 && vm.alturaEditada > 1 && (vm.selecionado.Imc.Peso != vm.pesoEditado || vm.selecionado.Imc.Altura != vm.alturaEditada))
                        {
                            vm.selecionado.Imc.Peso = vm.pesoEditado;
                            vm.selecionado.Imc.Altura = vm.alturaEditada;
                            vm.selecionado.Responsavel = CadUserController.Busca(listaUsuarios, idUsuario ?? "0") ?? new Usuario();
                            CadPacienteController.SalvaLista(vm.listaPacientes);
                            TempData["AvisosModal"] = "Paciente editado com sucesso!";
                        }
                        return View(vm);
                    }
                    else
                    {
                        TempData["ErrosModal"] = $"Nenhum paciente id {vm.codigo} encontrado!";
                        return View(vm);
                    }
                } else
                {
                    TempData["ErrosModal"] = "Nenhum paciente para editar!";
                    return View(vm);
                }  
            }
            catch (Exception ex)
            {
                TempData["ErrosModal"] = "Erro ao Editar: " + ex.Message;
                return RedirectToAction("Index");
            }
        }       
    }
}
