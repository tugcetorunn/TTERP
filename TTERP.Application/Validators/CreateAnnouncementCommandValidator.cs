using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.Announcements.Commands;

namespace TTERP.Application.Validators
{
    public class CreateAnnouncementCommandValidator : AbstractValidator<CreateAnnouncementCommand>
    {
        public CreateAnnouncementCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık boş olamaz")
                .MaximumLength(100).WithMessage("Başlık en fazla 100 hane olmalı");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("İçerik boş olamaz")
                .MaximumLength(400).WithMessage("İçerik en fazla 400 hane olmalı");

            RuleFor(x => x.StartDate)
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Yayın başlangıç tarihi bugün veya bugünden sonra olmalı.");

            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Yayın bitiş tarihi bugün veya bugünden sonra olmalı.");

            RuleFor(x => x.StartDate)
                .LessThan(x => x.EndDate).WithMessage("Başlangıç tarihi bitiş tarihinden önce olmalı");
        }
    }
}
