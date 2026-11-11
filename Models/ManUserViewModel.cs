using PrjIMV_MVC_CORE.Models.UsuarioModel;

namespace PrjIMV_MVC_CORE.Models
{
    public class ManUserViewModel
    {
        public List<Usuario> listaUsuarios = new List<Usuario>();
        public Usuario? selecionado { get; set; } = new Usuario();

        public String codigo { get; set; } = "0000";

        // "buscar" ou "salvar" (botão que foi clicado)
        public String acao { get; set; } = "";

        public String tituloBotaoSalvar = " Atualizar ";

        public ManUserViewModel()
        {
            listaUsuarios = new List<Usuario>();
        }
    }
}
