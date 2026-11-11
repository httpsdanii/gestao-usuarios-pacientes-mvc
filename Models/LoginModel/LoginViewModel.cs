using System.ComponentModel.DataAnnotations;

namespace PrjIMV_MVC_CORE.Models.LoginModel
{
    public class LoginViewModel
    {     
        
        public string Login { get; set; } = "";      
        [DataType(DataType.Password)]       
        public string Senha { get; set; } = "";      
      
    }
}
