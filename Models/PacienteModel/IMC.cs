using Microsoft.Extensions.Options;
using System.Reflection.PortableExecutable;
using System.Text.Json.Serialization;

namespace PrjIMV_MVC_CORE.Models.PacienteModel
{
    [Serializable]
    public class IMC
    {       
        public IMC(float peso, float altura)
        {
            Peso=peso;
            Altura=altura;
        }
        public IMC()  
        {           
        }         
        public float Peso { get; set; } = 0;       
       
        public float Altura { get; set; } = 0;

        public float? Valor ()
        {
            return Peso / (Altura * Altura);
        }
        public string? Diagnostico()
        {
            float imc = (float)Valor();
            if (imc < 17) return "Muito abaixo do peso";
            if (imc < 18.5) return "Abaixo do peso";
            if (imc < 25) return "Peso Normal";
            if (imc < 30) return "Acima do peso";
            if (imc < 35) return "Obesidade Grau I";
            if (imc < 40) return "Obesidade Grau II";
            return "Obesidade Grau III";
        }
    }
}
