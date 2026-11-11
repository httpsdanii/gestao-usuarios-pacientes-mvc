using System.Text.Json.Serialization;

namespace PrjIMV_MVC_CORE.Models.UsuarioModel
{
    public static class Permissao
    {       
        private static readonly HashSet<string> PermissoesMedico =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "ManPaciente",
            "CadPaciente",            
        };
        private static readonly HashSet<string> PermissoesAdministrador =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "CadPaciente",
            "CadUser",
            "ManPaciente",
            "ManUser",
        };


        public static bool OkMedico(string form)
        {
            if (form == null || string.IsNullOrWhiteSpace(form))
                return false;
            return PermissoesMedico.Contains(form);
        }
        public static bool OkAdministrador(string form)
        {
            if (form == null || string.IsNullOrWhiteSpace(form))
                return false;
            return PermissoesAdministrador.Contains(form);
        }
    }
    [Serializable]
    public enum TipoPerfil
    {
       
        TMedico,
        TPaciente,
        TAdministrador,
        TDesenvolvedor,
        TVisitante,
        TFuncionario,
        TVazio,
    }
    [Serializable]
    public class Perfil
    {
        [JsonInclude]
        public TipoPerfil TipoPerfil { get; set; } = TipoPerfil.TVazio;

        public Perfil(TipoPerfil tipoPerfil)
        {
            TipoPerfil=tipoPerfil;
        }

        public bool ok(string form)
        {

            switch (TipoPerfil)
            {
                case TipoPerfil.TMedico: return Permissao.OkMedico(form);  
                case TipoPerfil.TAdministrador : return Permissao.OkAdministrador(form);  
                case TipoPerfil.TDesenvolvedor: return true;               
                default: throw new ArgumentException("Erro de sistema: Perfil não existe!");
            }             
        }
        public string nomeForm { get; set; } = ""; 
        
    }
}
