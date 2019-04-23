using System;
using System.Collections.Generic;

namespace domain.entities
{
    public class UserEntity : EntityBase
    {
        /// <summary>
        /// User Name
        /// </summary>
        /// <value></value>
        public string Name { get; set; }

        /// <summary>
        /// Default User constructor for Entity Framework
        /// </summary>
        public UserEntity()
        {
        }

        /// <summary>
        /// User constructor
        /// </summary>
        /// <param name="name">User Name</param>
        public UserEntity(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Validate an User object
        /// </summary>
        /// <param name="isNew">True if object should not be found in the Database</param>
        public override void Validate(bool isNew = true)
        {
            base.Validate(isNew);

            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException(nameof(Name));
        }
    }
}