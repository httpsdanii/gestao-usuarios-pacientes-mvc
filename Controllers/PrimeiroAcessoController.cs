using Microsoft.AspNetCore.Mvc;
using PrjIMV_MVC_CORE.Models.LoginModel;
using PrjIMV_MVC_CORE.Models.PessoaModel;
using PrjIMV_MVC_CORE.Models.UsuarioModel;
using System.Reflection;
using System.Text.Json;

namespace PrjIMV_MVC_CORE.Controllers
{
    public class PrimeiroAcessoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(PrimeiroAcessoModel model)
        {
            if (model.SenhaA == model.SenhaB && model.SenhaA != null)
            {
                if (model.SenhaA.Length < 6)
                {
                    TempData["ErrosModal"]  =  "Utilize pelo menos 6 dígitos para senha";
                    return View(model);
                }
                if (Cpf.Valida(model.SenhaA))
                {
                    TempData["ErrosModal"]  =  "Cpf é inválido como senha!";
                    return View(model);
                }
                else
                {
                    List<Usuario> listaUsuarios = CadUserController.RecuperaLista();
                    var id = HttpContext.Session.GetString("idUsuario");
                    Usuario? Logado = CadUserController.Busca(listaUsuarios, id??"");

                    if (Logado != null)
                    { 
                        Logado.Hash = MyHash.GerarSha256(model.SenhaA);
                        String json = JsonSerializer.Serialize<List<Usuario>>(listaUsuarios);
                        System.IO.File.WriteAllText("usuarios.json", json);
                    } 
                    else
                    {
                        TempData["ErrosModal"]  =  "Erro salvando nova senha";
                        return View(model);
                    }
                }
                TempData["AvisosModal"] = "  Senha trocada com sucesso!<br/>  Faça login no sistema com sua nova senha!";
                HttpContext.Session.SetString("idUsuario","0000");
                return RedirectToAction("Index", "Home");

            } else
            {
                TempData["ErrosModal"]  =  "Senhas não conferem";
            }

           return View(model);
        }
    }
}
