using FluentValidation;
using FluentValidation.Results;
using Justpharm.Web.Models;

namespace Justpharm.Web.Validator
{

    public class PerfilValidator : AbstractValidator<PerfilUsuario>
    {
        public PerfilValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es requerido");
            RuleFor(x => x.UidMaestroPerfil).NotEmpty().WithMessage("Debe seleccionar un tipo de perfil para la aplicación");
        }
    }

    public class PacienteValidator : AbstractValidator<Paciente>
    {
        public PacienteValidator()
        {
            RuleFor(x => x.Nombre).MinimumLength(2).NotEmpty().WithMessage("El nombre es requerido");
            RuleFor(x => x.Apellidos).MinimumLength(2).NotEmpty().WithMessage("El apellido es requerido");
            RuleFor(x => x.DNI).NotEmpty().WithMessage("El DNI es requerido").Must(ValidarDNI!).WithMessage("El formato del DNI no es válido");
            RuleFor(x => x.SexoMasculino).NotNull().WithMessage("Debe seleccionar el sexo del paciente");
            RuleFor(x => x.Edad).GreaterThan(0).WithMessage("La edad debe ser mayor que 0");
            RuleFor(x => x.Peso).GreaterThan(1).WithMessage("El paciente debe tener al menos 1kg");
        }

        private bool ValidarDNI(string dni)
        {
            // Validación del formato de DNI español
            if (string.IsNullOrEmpty(dni) || dni.Length != 9)
                return false;

            string letrasValidas = "TRWAGMYFPDXBNJZSQVHLCKE";
            string numero = dni.Substring(0, 8);
            int resto = int.Parse(numero) % 23;
            char letraCalculada = letrasValidas[resto];

            return dni[8] == letraCalculada;
        }
    }


    public static class PacienteValidatorExtensions
    {
        public static bool ValidatePaciente(this Paciente paciente)
        {
            PacienteValidator? validator = new PacienteValidator();  // Crea una instancia del validador
            ValidationResult? result = validator.Validate(paciente); // Realiza la validación
            return result.IsValid; // Devuelve true si la validación es exitosa
        }
    }

}
