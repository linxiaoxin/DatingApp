using System;
using System.Collections.Generic;

namespace DatingApp.API.Model
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }
        public string gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interest { get; set; }
        public string City { get; set; }

        public string Country { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<Like> Liker { get; set; }
        public ICollection<Like> Likee { get; set; }
        public ICollection<Message> MsgSent { get; set; }
        public ICollection<Message> MsgReceived { get; set; }
    }
}