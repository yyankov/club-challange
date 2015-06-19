using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClubChallengeBeta.Models
{
    public class SimpleUserViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsOwner { get; set; }
    }
}