using System;
using System.Collections.Generic;
using domain.entities;

namespace domain.dto
{
    [Serializable]
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public UserDTO(UserEntity user)
        {
            Id = user.Id;
            Name = user.Name;
        }
    }
}