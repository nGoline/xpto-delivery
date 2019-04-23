using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace domain.entities
{
    public class EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        /// <summary>
        /// Table Identity
        /// </summary>
        /// <value></value>
        public Guid Id { get; set; }

        /// <summary>
        /// Validate an Entity object
        /// </summary>
        /// <param name="isNew">True if object should not be found in the Database</param>
        public virtual void Validate(bool isNew = true)
        {
            // If the object is new it should not have an Id
            // If it's not new, it should have an Id
            if (isNew && Id != Guid.Empty)
                throw new ArgumentException(string.Format("{0} should be 0!", nameof(Id)));

            if (!isNew && Id == Guid.Empty)
                throw new ArgumentNullException(nameof(Id));
        }
    }
}