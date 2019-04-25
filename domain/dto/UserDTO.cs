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
        public string Token { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }        

        public UserDTO()
        { }
        public UserDTO(UserEntity user)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
        }

        public UserEntity ToEntity()
        {
            return new UserEntity
            {
                Name = Name,
                Id = Id,
                Password = Password,
                Email = Email
            };
        }
    }
}