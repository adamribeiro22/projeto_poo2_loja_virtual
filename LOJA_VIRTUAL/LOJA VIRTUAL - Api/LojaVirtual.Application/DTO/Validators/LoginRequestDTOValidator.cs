using FluentValidation;
using LojaVirtual.Application.DTO.Auth;

namespace LojaVirtual.Application.DTO.Validators
{
    public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDTO>
    {
        public LoginRequestDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O e-mail é obrigatório.")
                .EmailAddress().WithMessage("O formato do e-mail é inválido.");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("A senha é obrigatória.");
        }
    }
}
