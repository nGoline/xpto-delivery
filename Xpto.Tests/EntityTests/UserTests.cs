using System;
using Xunit;
using GeoCoordinatePortable;
using Xpto.Domain.Entities;
using Xpto.Domain.DTO;

namespace Xpto.Tests.EntityTests
{
    public class UserTests
    {
        private const string _name = "Test Point 1";
        private const string _email = "admin@xpto.com";
        private const string _password = "123456";

        [Fact]
        public void MustCreateNewBlankUser()
        {
            var User = new User();

            Assert.NotNull(User);
        }

        [Fact]
        public void MustCreateNewUserAndSetProps()
        {
            var User = new User();
            User.Name = _name;
            User.Email = _email;
            User.Password = _password;

            Assert.Equal(_name, User.Name);
            Assert.Equal(_email, User.Email);
            Assert.Equal(_password, User.Password);
        }

        [Fact]
        public void MustCreateNewUserFromValues()
        {
            var User = new User(_name, _email, _password);

            Assert.Equal(_name, User.Name);
            Assert.Equal(_email, User.Email);
            Assert.Equal(_password, User.Password);
        }

        [Fact]
        public void ShouldNotCreateFromInvalidValues()
        {
            Assert.False(new User(_name, "_email", _password).IsValid);
            Assert.False(new User("", _email, _password).IsValid);
            Assert.False(new User(_name, _email, "").IsValid);
            var User = new User();
            User.Name = _name;
            Assert.False(User.IsValid);
            User.Email = _email;
            Assert.False(User.IsValid);
            User.Password = _password;            
            User.Name = string.Empty;
            Assert.False(User.IsValid);
            User.Name = " ";
            Assert.False(User.IsValid);
        }

        [Fact]
        public void MustCreateDto()
        {
            var User = new User(_name, _email, _password);
            var dto = new UserDTO(User);

            Assert.Equal(_name, dto.Name);
            Assert.Equal(_email, dto.Email);
        }

        [Fact]
        public void MustCreateFromDto()
        {
            var dto = new UserDTO
            {
                Name = _name,
                Email = _email,
                Password = _password
            };

            var User = dto.ToEntity();

            Assert.Equal(_name, User.Name);
            Assert.Equal(_email, User.Email);
            Assert.Equal(_password, User.Password);
        }
    }
}
