using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RichnessSoft.Entity.Model;


namespace RichnessSoft.Entity.Validations
{
    internal class ModelValidation : AbstractValidator<Models>
    {
        public ModelValidation()
        {
            RuleFor(x => x.code).NotEmpty().WithMessage("รหัสห้ามว่าง");
            RuleFor(x => x.name1).NotEmpty().WithMessage("ชื่อห้ามว่าง");
        }
    }
}
