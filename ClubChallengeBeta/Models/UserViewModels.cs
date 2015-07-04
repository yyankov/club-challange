using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClubChallengeBeta.App_Data;

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

    public class UserDetailsViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int? ClubId { get; set; }
        public string ClubName { get; set; }
        public int TrophyCount { get; set; }
        public int ChallengeCount { get; set; }
        public bool CanChallenge { get; set; }

        public UserDetailsViewModel(AspNetUser dbUser, AspNetUser user)
        {
            UserId = dbUser.Id;
            UserName = dbUser.UserName;
            Phone = dbUser.PhoneNumber;
            Email = dbUser.Email;
            ClubId = dbUser.ClubId;
            ClubName = dbUser.Club != null? dbUser.Club.Name : "none";
            ChallengeCount = dbUser.SingleChallenges.Count
                + dbUser.SingleChallenges1.Count
                + dbUser.TeamChallenges.Count
                + dbUser.TeamChallenges1.Count
                + dbUser.TeamChallenges2.Count
                + dbUser.TeamChallenges3.Count;
            TrophyCount = dbUser.Trophies + dbUser.TeamTrophies;
            CanChallenge = dbUser.Id != user.Id && dbUser.ClubId == user.ClubId && dbUser.ClubId != null;
        }
    }
}