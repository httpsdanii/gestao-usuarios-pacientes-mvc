using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace PrjIMV_MVC_CORE.Models.PessoaModel
{
    [Serializable]
    public class Email
    {
        [JsonInclude]
        public String? emailLocal { get; set; } 

        [JsonIgnore]
        public String? email
        {
            get { return emailLocal; }
            set
            {
                emailLocal =  value ?? "";

                if (!Valida())
                    throw new ArgumentException($"Erro de sistema: O email passado '{email}' é Inválido!");
            }
        }

        public Email()
        {
            emailLocal="";
        }      
        public bool Valida()
        {
            if (string.IsNullOrWhiteSpace(emailLocal))
                return false;

            int arroba = emailLocal.IndexOf('@');
           
            if (arroba <= 0 || arroba != emailLocal.LastIndexOf('@'))
                return false;
           
            string dominio = emailLocal.Substring(arroba + 1);

            if (dominio.Length < 3)
                return false;

            if (!dominio.Contains("."))
                return false;
           
            if (dominio.EndsWith("."))
                return false;

            return true;
        }
    }
}
