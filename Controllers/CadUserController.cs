using Microsoft.AspNetCore.Mvc;
using PrjIMV_MVC_CORE.Models;
using PrjIMV_MVC_CORE.Models.PacienteModel;
using PrjIMV_MVC_CORE.Models.PessoaModel;
using PrjIMV_MVC_CORE.Models.UsuarioModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace PrjIMV_MVC_CORE.Controllers
{
    public class CadUserController : Controller
    {
        public IActionResult Index()
        {
            CadUserViewModel vm = new CadUserViewModel();

            try
            {
                vm.listaUsuarios = RecuperaLista();            
            }
            catch (Exception ex)
            {
                TempData["ErrosModal"] = "Erro ao desserializar: " + ex.Message;
                return RedirectToAction("Index");
            }
            return View(vm);
        }

        public static Usuario? Busca(List<Usuario> listaUsuarios, String id)
        {            
            Usuario buscar = new Usuario(id);
            int pos = listaUsuarios.BinarySearch(buscar);
            if (pos >= 0)
            {
                return listaUsuarios[pos];
            }
            return null;
        }

        [HttpPost]
        public ActionResult Index(CadUserViewModel vm)  
        {
            try
            {
                String erro = "";

                vm.listaUsuarios = RecuperaLista();

                if (vm.novoUsuario == null)
                {
                    TempData["ErrosModal"] = "Nenhum dado para cadastrar!";
                    return View(vm);
                }
                if (String.IsNullOrEmpty(vm.novoUsuario.Nome))
                {
                    erro = String.Concat(erro, "Nome não pode estar em branco! <br/>");
                }
                if (String.IsNullOrEmpty(vm.novoUsuario.Sobrenome))
                {
                    erro = String.Concat(erro, "Sobrenome não pode estar em branco! <br />");
                }
                if (String.IsNullOrEmpty(vm.novoUsuario.Login) || vm.novoUsuario.Login.Trim().Length < 6)
                {
                    erro = String.Concat(erro, "Login deve ter no mínimo 6 caracteres! <br />");
                }                
                if (!Cpf.Valida(vm.novoUsuario.Cpf.cpf))
                {
                    erro = String.Concat(erro, "CPF digitado inválido! <br />");
                }

                if (vm.novoUsuario.Perfil.TipoPerfil == TipoPerfil.TVazio)
                {
                    erro = String.Concat(erro, "Selecione um perfil para usuário! <br />");
                }

                vm.listaUsuarios.Sort(new UsuarioCpfComparer());

                var procurado = new Usuario();
                procurado.Cpf.cpf = Cpf.Formata(vm.novoUsuario.Cpf.cpf);

               int pos = vm.listaUsuarios.BinarySearch(
                    procurado,
                    new UsuarioCpfComparer()
                );

                if (pos > 0)
                {
                    erro = String.Concat(erro, "CPF Já cadastrado <br />");
                }                 

                vm.listaUsuarios.Sort();

                if (!vm.novoUsuario.Email.Valida())
                {
                    erro = String.Concat(erro, "Email digitado inválido! <br />");
                } 

                if (erro != "")
                {
                    TempData["ErrosModal"] = erro;                 
                   return View(vm);
                }

                vm.novoUsuario.Id = (Usuario.contador++).ToString("D4"); // Cadastro não esta usando construtor;
                vm.novoUsuario.Cpf.cpf = Cpf.Formata(vm.novoUsuario.Cpf.cpf);
                vm.novoUsuario.Hash = MyHash.GerarSha256(vm.novoUsuario.Cpf.cpf);
                vm.listaUsuarios.Add(vm.novoUsuario);
                SalvaLista(vm.listaUsuarios);

                vm.novoUsuario = new Usuario();

                return View(vm);
            }
            catch (Exception ex)
            {
                TempData["ErrosModal"] = "Erro ao cadastrar: " + ex.Message;
                return RedirectToAction("Index");
            }           
        }

        public static List<Usuario> RecuperaLista()
        {
            List<Usuario> listaUsuarios = new List<Usuario>();

            if (System.IO.File.Exists("usuarios.json"))
            {
                String json = System.IO.File.ReadAllText("usuarios.json");
                listaUsuarios = JsonSerializer.Deserialize<List<Usuario>>(json) ?? new List<Usuario>();
                listaUsuarios.Sort();
                Usuario.contador = int.Parse(listaUsuarios[listaUsuarios.Count - 1].Id) + 1;
            }

            return listaUsuarios;
        }

        public static void SalvaLista(List<Usuario> listaUsuarios)
        {
            listaUsuarios.Sort();

            //foreach(Usuario u in listaUsuarios)
            //{
            //    if (u.logado()=="Não")
            //    {
            //        u.Hash=MyHash.GerarSha256(u.Cpf.cpf);
            //    }
            //}

            String json = JsonSerializer.Serialize<List<Usuario>>(listaUsuarios);
            System.IO.File.WriteAllText("usuarios.json", json);
            listaUsuarios = JsonSerializer.Deserialize<List<Usuario>>(json) ?? new List<Usuario>();
        }
    }
}
