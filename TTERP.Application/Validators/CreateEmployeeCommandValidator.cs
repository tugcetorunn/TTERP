using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Employees.Commands;

namespace TTERP.Application.Validators
{
    public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeCommandValidator() 
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("İsim alanı boş bırakılamaz.")
                .Length(2, 50).WithMessage("İsim en az 2, en fazla 50 karakter olabilir.")
                .Matches(@"^[\p{L}\s]+$").WithMessage("İsim sadece harf içerebilir.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Soyisim alanı boş bırakılamaz.")
                .Length(2, 50).WithMessage("Soyisim en az 2, en fazla 50 karakter olabilir.")
                .Matches(@"^[\p{L}\s]+$").WithMessage("Soyisim sadece harf içerebilir.");

            RuleFor(x => x.NationalId)
                .NotEmpty().WithMessage("TCKN boş olamaz.")
                .Length(11).WithMessage("TCKN 11 haneli olmalıdır.")
                .Matches(@"^\d+$").WithMessage("TCKN sadece rakamlardan oluşmalıdır.")
                .Must(x => int.TryParse(x[^2..], out int lastTwo) && lastTwo % 2 == 0)
                .WithMessage("TCKN çift rakamla bitmelidir.");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Doğum tarihi boş olamaz.")
                .Must(date => CalculateAge(date) >= 18 && CalculateAge(date) <= 65)
                .WithMessage("Çalışan yaşı 18 ile 65 arasında olmalıdır.");

            RuleFor(x => x)
                .Must(x =>
                {
                    var birthDate = x.DateOfBirth;
                    var startDate = x.HireDate;

                    var ageAtStart = startDate!.Year - birthDate.Year;
                    if (birthDate.Date > startDate.AddYears(-ageAtStart)) ageAtStart--;

                    return ageAtStart >= 18;
                })
                .WithMessage("Çalışanın işe giriş tarihindeki yaşı en az 18 olmalıdır.");


            RuleFor(x => x.HireDate)
                .NotEmpty().WithMessage("İşe başlama tarihi boş olamaz.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Telefon numarası zorunludur.")
                .Length(10).WithMessage("Telefon numarası 10 haneli olmalıdır.")
                .Matches(@"^[1-9][0-9]{9}$").WithMessage("Telefon numarası başında 0 olamaz ve sadece rakam içermelidir.");

            RuleFor(x => x.TitleId)
                .GreaterThan(0).WithMessage("Geçerli bir unvan seçilmelidir.");

            RuleFor(x => x.RoleId)
                .GreaterThan(0).WithMessage("Geçerli bir rol seçilmelidir.");
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
