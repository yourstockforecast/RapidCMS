﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using RapidCMS.Common.Data;
using RapidCMS.Common.Enums;
using RapidCMS.Common.Extensions;
using RapidCMS.Common.Models.Metadata;

namespace RapidCMS.Common.Validation
{
    // TODO: fix memory leak due to events
    public sealed class EditContext
    {
        private readonly Dictionary<IPropertyMetadata, PropertyState> _fieldStates = new Dictionary<IPropertyMetadata, PropertyState>();
        private readonly IServiceProvider _serviceProvider;

        public EditContext(UsageType usageType)
        {
            // TODO: fix this constructor (either defaults, or fix via inheritance of simpler base type)
            Entity = null;
            UsageType = usageType;
            _serviceProvider = null;
        }

        public EditContext(IEntity entity, UsageType usageType, IServiceProvider serviceProvider)
        {
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
            UsageType = usageType;
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public IEntity Entity { get; private set; }
        public UsageType UsageType { get; private set; }

        public event EventHandler<FieldChangedEventArgs> OnFieldChanged;

        public event EventHandler<ValidationStateChangedEventArgs> OnValidationStateChanged;

        public void NotifyPropertyStartedListening(IPropertyMetadata property)
        {
            GetPropertyState(property);
        }

        public void NotifyPropertyChanged(IPropertyMetadata property)
        {
            ValidateProperty(property);

            GetPropertyState(property).IsModified = true;
            OnFieldChanged?.Invoke(this, new FieldChangedEventArgs(property));
        }

        public bool IsValid()
        {
            ValidateModel();

            return !HasValidationMessages();
        }

        public bool IsModified()
        {
            return _fieldStates.Any(x => x.Value.IsModified);
        }

        public bool IsValid(IPropertyMetadata property)
        {
            return !GetPropertyState(property).GetValidationMessages().Any();
        }

        public bool WasValidated(IPropertyMetadata property)
        {
            return GetPropertyState(property).WasValidated;
        }

        public IEnumerable<string> GetValidationMessages()
        {
            foreach (var state in _fieldStates)
            {
                foreach (var message in state.Value.GetValidationMessages())
                {
                    yield return message;
                }
            }
        }

        public IEnumerable<string> GetValidationMessages(IPropertyMetadata property)
        {
            if (_fieldStates.TryGetValue(property, out var state))
            {
                foreach (var message in state.GetValidationMessages())
                {
                    yield return message;
                }
            }
        }

        private bool HasValidationMessages()
        {
            return GetValidationMessages().Any();
        }

        private PropertyState GetPropertyState(IPropertyMetadata property)
        {
            if (!_fieldStates.TryGetValue(property, out var fieldState))
            {
                fieldState = new PropertyState(property);
                _fieldStates.Add(property, fieldState);
            }

            return fieldState;
        }

        private PropertyState? GetFieldState(string propertyName)
        {
            return _fieldStates.SingleOrDefault(field => field.Key.PropertyName == propertyName).Value;
        }

        private void ClearAllFieldStates()
        {
            foreach (var fieldState in _fieldStates)
            {
                fieldState.Value.ClearMessages();
            }
        }

        private void ValidateModel()
        {
            var context = new ValidationContext(Entity, _serviceProvider, null);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(Entity, context, results, true);

            ClearAllFieldStates();

            foreach (var result in results)
            {
                result.MemberNames.ForEach(name => GetFieldState(name)?.AddMessage(result.ErrorMessage));
            }

            _fieldStates.ForEach(kv => kv.Value.WasValidated = true);

            OnValidationStateChanged?.Invoke(this, new ValidationStateChangedEventArgs(isValid: !HasValidationMessages()));
        }

        private void ValidateProperty(IPropertyMetadata property)
        {
            var context = new ValidationContext(Entity, _serviceProvider, null)
            {
                MemberName = property.PropertyName
            };
            var results = new List<ValidationResult>();

            Validator.TryValidateProperty(property.Getter(Entity), context, results);

            var state = GetPropertyState(property);
            state.ClearMessages();
            state.WasValidated = true;

            foreach (var result in results)
            {
                state.AddMessage(result.ErrorMessage);
            }

            OnValidationStateChanged?.Invoke(this, new ValidationStateChangedEventArgs(isValid: !HasValidationMessages()));
        }
    }
}