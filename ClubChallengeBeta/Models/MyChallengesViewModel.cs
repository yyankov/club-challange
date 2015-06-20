using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClubChallengeBeta.Models
{
    public class MyChallengesViewModel
    {
        public IEnumerable<SingleChallengesViewModel> SingleChallenges { get; set; }
        public IEnumerable<TeamChallengesViewModel> TeamChallenges { get; set; }
    }
}