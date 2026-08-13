using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Customers.Commands;
using TTERP.Application.CQRS.Employees.Commands;

namespace TTERP.Application.Validators
{
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator() 
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("İsim alanı boş bırakılamaz.")
                .Length(2, 50).WithMessage("İsim en az 2, en fazla 50 karakter olabilir.")
                .Matches(@"^[\p{L}\s]+$").WithMessage("İsim sadece harf içerebilir.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Soyisim alanı boş bırakılamaz.")
                .Length(2, 50).WithMessage("Soyisim en az 2, en fazla 50 karakter olabilir.")
                .Matches(@"^[\p{L}\s]+$").WithMessage("Soyisim sadece harf içerebilir.");

            // Bireysel müşteri
            When(x => x.CustomerType == 1, () =>
            {
                RuleFor(x => x.NationalId)
                    .NotEmpty()
                    .WithMessage("Bireysel müşteriler için T.C. Kimlik No zorunludur.")
                    .Length(11)
                    .Matches(@"^\d{11}$")
                    .WithMessage("T.C. Kimlik No 11 haneli olmalıdır.");

                RuleFor(x => x.TaxNumber)
                    .Empty()
                    .WithMessage("Bireysel müşterilerde Vergi No girilmemelidir.");
            });

            // Kurumsal müşteri
            When(x => x.CustomerType == 2, () =>
            {
                RuleFor(x => x.TaxNumber)
                    .NotEmpty()
                    .WithMessage("Kurumsal müşteriler için Vergi No zorunludur.")
                    .MaximumLength(20);

                RuleFor(x => x.NationalId)
                    .Empty()
                    .WithMessage("Kurumsal müşterilerde T.C. Kimlik No girilmemelidir.");
            });

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Telefon numarası zorunludur.")
                .Length(10).WithMessage("Telefon numarası 10 haneli olmalıdır.")
                .Matches(@"^[1-9][0-9]{9}$").WithMessage("Telefon numarası başında 0 olamaz ve sadece rakam içermelidir.");
        }

        private int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}
