using FluentValidation.Results;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FluentValidation
{
    public class FluentValidationValidator : ComponentBase
    {
        [CascadingParameter]
        private EditContext _editContext { get; set; }

        private ValidationMessageStore _validationMessageStore;
        
        [Inject]
        private ValidatorLocator _validatorLocator { get; set; }
        
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            EditContext previousEditContext = _editContext;

            await base.SetParametersAsync(parameters);

            if (_editContext == null)
                throw new NullReferenceException($"{nameof(FluentValidationValidator)} must be placed within an {nameof(EditForm)}");

            if (_editContext != previousEditContext)  EditContextChanged();
        }

        private void EditContextChanged()
        {
            _validationMessageStore = new ValidationMessageStore(_editContext);
            HookUpEditContextEvents();
        }

        private void HookUpEditContextEvents()
        {
            _editContext.OnValidationRequested += (s,a) => Validate();
            _editContext.OnFieldChanged += (s, a) => Validate();
        }

        private void Validate()
        {
            _validationMessageStore.Clear();

            var validators = _validatorLocator.GetValidators(_editContext.Model.GetType());

            var context = new ValidationContext(_editContext.Model);
            var failures = validators
                .SelectMany(v => v.Validate(context).Errors)
                .Where(f => f != null)
                .ToList();

            AddValidationResult(_editContext.Model, new ValidationResult(failures));
        }

        private void AddValidationResult(object model, ValidationResult validationResult)
        {
            foreach (ValidationFailure error in validationResult.Errors)
            {
                var fieldIdentifier = new FieldIdentifier(model, error.PropertyName);
                _validationMessageStore.Add(fieldIdentifier, error.ErrorMessage);
            }
            _editContext.NotifyValidationStateChanged();
        }
    }
}