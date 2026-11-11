using PrjIMV_MVC_CORE.Models.UsuarioModel;

namespace PrjIMV_MVC_CORE.Models
{

    public class CadUserViewModel
    {
        public  List<Usuario> listaUsuarios = new List<Usuario>();
        public Usuario? novoUsuario { get; set; } = new Usuario();

        public String tituloBotaoSalvar = " Cadastrar ";

        public CadUserViewModel() 
        {           
            listaUsuarios = new List<Usuario>();
        }
        
    }
}
