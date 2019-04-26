using System;
using System.Collections.Generic;
using Xpto.Domain.Entities.Common;
using Xpto.Domain.Entities.Validations;
using Xpto.Domain.Interfaces.Validation;
using Xpto.Domain.Validation;

namespace Xpto.Domain.Entities
{
    public class User : Entity, ISelfValidation
    {
        /// <summary>
        /// User name
        /// </summary>
        /// <value></value>
        public string Name { get; set; }

        /// <summary>
        /// User email (login)
        /// </summary>
        /// <value></value>
        public string Email { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        /// <value></value>
        public string Password { get; set; }

        public ValidationResult ValidationResult { get; private set; }

        public User()
        {        }
        public User(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }

        public bool IsValid
        {
            get
            {
                var fiscal = new UserIsValidValidation();
                ValidationResult = fiscal.Valid(this);
                return ValidationResult.IsValid;
            }
        }
    }
}