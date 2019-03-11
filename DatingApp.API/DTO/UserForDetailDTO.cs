using System;
using System.Collections.Generic;
using DatingApp.API.Model;

namespace DatingApp.API.DTO
{
    public class UserForDetailDTO
    {
         public int Id { get; set; }

        public string UserName { get; set; }
        public string gender { get; set; }
        public int Age { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interest { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhotoUrl { get; set; }
        public ICollection<PhotoForDetailsDto> Photos { get; set; }
    }
}