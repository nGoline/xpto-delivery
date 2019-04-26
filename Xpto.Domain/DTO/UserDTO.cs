using System;
using System.Collections.Generic;
using Xpto.Domain.Entities;

namespace Xpto.Domain.DTO
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
        public UserDTO(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
        }

        public User ToEntity()
        {
            return new User
            {
                Name = Name,
                Id = Id,
                Password = Password,
                Email = Email
            };
        }
    }
}