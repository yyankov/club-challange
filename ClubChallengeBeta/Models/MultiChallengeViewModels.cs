using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ClubChallengeBeta.Extensions.ValidationAttributes;

namespace ClubChallengeBeta.Models
{
    public class UserSelectViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
    }

    public class MultiChallengeViewModel : ValidationAttribute
    {
        public string PartnerId { get; set; }

        [NotEqual("PartnerId", ErrorMessage = "Opponent 1 should be different than Partner")]
        public string Opponent1Id { get; set; }

        [NotEqual("PartnerId", ErrorMessage = "Opponent 2 should be different than Partner")]
        [NotEqualTo("Opponent1Id", ErrorMessage = "Opponent 2 should be different than Opponent 1")]
        public string Opponent2Id { get; set; }
        
        [Required]
        public int KZL { get;set; }
    }
}