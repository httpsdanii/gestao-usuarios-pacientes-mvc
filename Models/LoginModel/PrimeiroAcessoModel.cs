using System.ComponentModel.DataAnnotations;

namespace PrjIMV_MVC_CORE.Models.LoginModel
{
    public class PrimeiroAcessoModel
    {
        [DataType(DataType.Password)]
        public string? SenhaA { get; set; } = "";

        [DataType(DataType.Password)]
        public string? SenhaB { get; set; } = "";
    }
}
