using System.Linq;
using System.Web.Http.ModelBinding;
using FluentValidation;
using System;

using WebApp.Service.Interface;

namespace WebApp.Service
{
    public abstract class BaseDocumentValidator<TDocumentDTO> : AbstractValidator<TDocumentDTO>
        where TDocumentDTO : class, IDocumentDTO, new()
    {
        public virtual Boolean Validate(TDocumentDTO entity, ModelStateDictionary modelState)
        {
            return Validate(entity, modelState, null);
        }

        public virtual Boolean Validate(TDocumentDTO entity, ModelStateDictionary modelState, string ruleset)
        {
            var __results = ruleset == null ? this.Validate(entity) : this.Validate(entity, ruleSet: ruleset);

            foreach (var __error in __results.Errors)
            {
                modelState.AddModelError(__error.PropertyName, __error.ErrorMessage);
            }

            return modelState.IsValid;
        }
    }
}
