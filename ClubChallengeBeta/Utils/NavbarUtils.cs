using ClubChallengeBeta.App_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClubChallengeBeta.Utils
{
    public static class NavbarUtils
    {
        private static ClubChallengeEntities db = new ClubChallengeEntities();
        public static bool IsClubUser(string userid)
        {
            var currentUser = db.AspNetUsers.SingleOrDefault(e => e.Id == userid);
            var clubUser = false;
            foreach (var role in currentUser.AspNetRoles)
            {
                if (role.Name == "Club")
                {
                    clubUser = true;
                }
            }
            return clubUser;
        }
        public static int PendingChallenges(string currentUserId)
        {
            AspNetUser currentUser = db.AspNetUsers.Find(currentUserId);
            int currentUserClubId = currentUser.Club.ClubId;
            var result = db.SingleChallenges.Where(s => s.AspNetUser1.ClubId == currentUserClubId && s.Result == "Waiting approval").Count();
            result += db.TeamChallenges.Where(s => s.AspNetUser1.ClubId == currentUserClubId && s.Result == "Waiting approval").Count();
            return result;
        }
    }
}