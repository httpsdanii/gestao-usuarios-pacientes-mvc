using Microsoft.AspNetCore.Mvc;
using PrjIMV_MVC_CORE.Models.LoginModel;
using PrjIMV_MVC_CORE.Models.PessoaModel;
using PrjIMV_MVC_CORE.Models.UsuarioModel;
using System.Text.Json;

namespace PrjIMV_MVC_CORE.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public Usuario? Logado;

        [HttpPost]   

        public IActionResult Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState)
                {
                    foreach (var erro in item.Value.Errors)
                    {
                        TempData["ErrosModal"] += erro.ErrorMessage + "<br />";
                    }
                }
                return View(model);
            }            
                   

            List<Usuario> listaUsuarios = new List<Usuario>();


            if (System.IO.File.Exists("usuarios.json"))
            {
                String json = System.IO.File.ReadAllText("usuarios.json");
                listaUsuarios = JsonSerializer.Deserialize<List<Usuario>>(json) ?? new List<Usuario>();
                listaUsuarios.Sort();

                if (Cpf.Valida(model.Senha))
                {
                    model.Senha = Cpf.Formata(model.Senha);                  
                   
                }

                foreach (Usuario u in listaUsuarios)
                {
                    if (u.ValidaUsuarioSenha(model.Login, model.Senha))
                    {
                        Logado = u;
                        HttpContext.Session.SetString("idUsuario", u.Id);                      
                        HttpContext.Session.SetString("nomeUsuario", u.Nome??"");
                        break;
                    }
                }

                if (Logado == null)
                {
                    TempData["ErrosModal"] = "Login ou senha inválido";
                } 
                else
                {
                    if (Cpf.Valida(model.Senha))
                    {
                        TempData["AvisosModal"]  =  "Este é o seu primeiro acesso. Troque sua senha inicial.";
                        return  RedirectToAction("Index", "PrimeiroAcesso");                        
                    }
                }
            } 
            else
            {
                TempData["ErrosModal"]  =  "Lista de usuários vazia";
            }           

            if (Logado != null)
            {               
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
    }

}
