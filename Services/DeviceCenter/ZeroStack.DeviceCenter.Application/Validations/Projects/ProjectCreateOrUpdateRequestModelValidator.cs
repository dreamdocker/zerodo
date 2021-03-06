﻿using FluentValidation;
using Microsoft.Extensions.Localization;
using System.Reflection;
using ZeroStack.DeviceCenter.Application.Models.Projects;

namespace ZeroStack.DeviceCenter.Application.Validations.Projects
{
    public class ProjectCreateOrUpdateRequestModelValidator : AbstractValidator<ProjectCreateOrUpdateRequestModel>
    {
        public ProjectCreateOrUpdateRequestModelValidator(IStringLocalizerFactory factory)
        {
            IStringLocalizer _localizer1 = factory.Create("Models.Projects.ProjectCreateOrUpdateRequestModel", Assembly.GetExecutingAssembly().ToString());
            IStringLocalizer _localizer2 = factory.Create(typeof(ProjectCreateOrUpdateRequestModel));

            RuleFor(m => m.Name).Length(4, 7).WithMessage((m, p) => _localizer1["LengthValidator", _localizer1["ProjectName"], 5, 7, p.Length]);
            RuleFor(m => m.Name).NotNull().NotEmpty().Length(3, 8).WithName(_localizer2["ProjectName"]);
        }
    }
}