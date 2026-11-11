using Microsoft.AspNetCore.Mvc;
using PrjIMV_MVC_CORE.Models;
using PrjIMV_MVC_CORE.Models.PessoaModel;
using PrjIMV_MVC_CORE.Models.UsuarioModel;

namespace PrjIMV_MVC_CORE.Controllers
{
    public class ManUserController : Controller
    {
        public IActionResult Index()
        {
            ManUserViewModel vm = new ManUserViewModel();

            try
            {
                vm.listaUsuarios = CadUserController.RecuperaLista();
            }
            catch (Exception ex)
            {
                TempData["ErrosModal"] = "Erro ao desserializar: " + ex.Message;
            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(ManUserViewModel vm)
        {
            try
            {
                vm.listaUsuarios = CadUserController.RecuperaLista(); // já vem ordenada por Id

                // Os valores exibidos devem vir do Model e não do que foi postado
                ModelState.Clear();

                if (vm.listaUsuarios.Count == 0)
                {
                    TempData["ErrosModal"] = "Nenhum usuário cadastrado!";
                    return View(vm);
                }

                if (vm.acao == "salvar")
                {
                    return Salvar(vm);
                }

                return Buscar(vm);
            }
            catch (Exception ex)
            {
                TempData["ErrosModal"] = "Erro ao Editar: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // A) Busca do usuário pelo ID usando BinarySearch
        private ActionResult Buscar(ManUserViewModel vm)
        {
            if (!int.TryParse(vm.codigo, out int iCodigo))
            {
                TempData["ErrosModal"] = "Id do usuário deve ser numérico!";
                vm.selecionado = new Usuario();
                return View(vm);
            }

            vm.codigo = iCodigo.ToString("D4");

            Usuario usuarioBusca = new Usuario(vm.codigo);
            int pos = vm.listaUsuarios.BinarySearch(usuarioBusca);

            if (pos >= 0)
            {
                vm.selecionado = vm.listaUsuarios[pos];
            }
            else
            {
                TempData["ErrosModal"] = $"Nenhum usuário id {vm.codigo} encontrado!";
                vm.selecionado = new Usuario();
            }
            return View(vm);
        }

        // B) e C) Alteração de todos os dados (exceto Login) e do campo Ativo
        private ActionResult Salvar(ManUserViewModel vm)
        {
            if (vm.selecionado == null || String.IsNullOrEmpty(vm.selecionado.Id))
            {
                TempData["ErrosModal"] = "Nenhum usuário para editar! Busque um usuário pelo ID.";
                vm.selecionado = new Usuario();
                return View(vm);
            }

            Usuario editado = vm.selecionado;
            vm.codigo = editado.Id;

            int pos = vm.listaUsuarios.BinarySearch(new Usuario(editado.Id));
            if (pos < 0)
            {
                TempData["ErrosModal"] = $"Nenhum usuário id {editado.Id} encontrado!";
                return View(vm);
            }

            Usuario usuario = vm.listaUsuarios[pos];
            editado.Login = usuario.Login; // Login não é postado nem alterado, só exibido

            String erro = "";

            if (String.IsNullOrEmpty(editado.Nome))
            {
                erro = String.Concat(erro, "Nome não pode estar em branco! <br/>");
            }
            if (String.IsNullOrEmpty(editado.Sobrenome))
            {
                erro = String.Concat(erro, "Sobrenome não pode estar em branco! <br/>");
            }

            String cpfDigitado = editado.Cpf.cpfLocal ?? "";
            String cpfFormatado = "";
            bool cpfMudou = false;

            if (!Cpf.Valida(cpfDigitado))
            {
                erro = String.Concat(erro, "CPF digitado inválido! <br/>");
            }
            else
            {
                cpfFormatado = Cpf.Formata(cpfDigitado);
                cpfMudou = cpfFormatado != Cpf.Formata(usuario.Cpf.cpfLocal ?? "");

                if (cpfMudou)
                {
                    foreach (Usuario u in vm.listaUsuarios)
                    {
                        if (u.Id != usuario.Id && Cpf.Formata(u.Cpf.cpfLocal ?? "") == cpfFormatado)
                        {
                            erro = String.Concat(erro, "CPF Já cadastrado <br/>");
                            break;
                        }
                    }
                }
            }

            bool emailMudou = (editado.Email.emailLocal ?? "") != (usuario.Email.emailLocal ?? "");

            if (emailMudou && !editado.Email.Valida())
            {
                erro = String.Concat(erro, "Email digitado inválido! <br/>");
            }

            if (editado.Perfil.TipoPerfil == TipoPerfil.TVazio)
            {
                erro = String.Concat(erro, "Selecione um perfil para usuário! <br/>");
            }

            if (erro != "")
            {
                TempData["ErrosModal"] = erro;
                return View(vm); // mantém na tela o que foi digitado
            }

            // Enquanto o usuário não trocou a senha inicial (o CPF), a senha acompanha o novo CPF
            bool senhaInicial = usuario.Hash == MyHash.GerarSha256(usuario.Cpf.cpfLocal ?? "")
                             || usuario.Hash == MyHash.GerarSha256(Cpf.Formata(usuario.Cpf.cpfLocal ?? ""));

            // Login e Id não são alterados
            usuario.Nome = editado.Nome;
            usuario.Sobrenome = editado.Sobrenome;
            usuario.Email.emailLocal = editado.Email.emailLocal;
            usuario.Cpf.cpfLocal = cpfFormatado;
            usuario.Perfil.TipoPerfil = editado.Perfil.TipoPerfil;
            usuario.Ativo = editado.Ativo;

            if (cpfMudou && senhaInicial)
            {
                usuario.Hash = MyHash.GerarSha256(cpfFormatado);
            }

            CadUserController.SalvaLista(vm.listaUsuarios); // grava em usuarios.json

            vm.selecionado = usuario;
            TempData["AvisosModal"] = "Usuário alterado com sucesso!";
            return View(vm);
        }
    }
}
