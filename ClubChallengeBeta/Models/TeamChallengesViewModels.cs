using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClubChallengeBeta.App_Data;

namespace ClubChallengeBeta.Models
{
    public class TeamChallengesViewModel
    {
        public int TeamChallengeId { get; set; }
        public string User1Name { get; set; }
        public string User1Id { get; set; }
        public bool User1Accepted { get; set; }
        public string User2Name { get; set; }
        public string User2Id { get; set; }
        public bool User2Accepted { get; set; }
        public string User3Name { get; set; }
        public string User3Id { get; set; }
        public bool User3Accepted { get; set; }
        public string User4Name { get; set; }
        public string User4Id { get; set; }
        public string Winner1Id { get; set; }
        public bool User4Accepted { get; set; }
        public string Result { get; set; }
        
        public bool CanAcceptReject { get; set; }
        public bool CanDeclareWin { get; set; }
        public bool PendingAcceptance { get; set; }
        public bool PendingApproval { get; set; }
        public bool IsWinner { get; set; }
        public string ScoreText { get; set; }

        public TeamChallengesViewModel(TeamChallenge tc, AspNetUser currentUser)
        {
            TeamChallengeId = tc.TeamChallengeId;
            User1Name = tc.AspNetUser.UserName;
            User1Id = tc.User1Id;
            User1Accepted = tc.User1Accepted;
            User2Name = tc.AspNetUser1.UserName;
            User2Id = tc.User2Id;
            User2Accepted = tc.User2Accepted;
            User3Name = tc.AspNetUser2.UserName;
            User3Id = tc.User3Id;
            User3Accepted = tc.User3Accepted;
            User4Name = tc.AspNetUser3.UserName;
            User4Id = tc.User4Id;
            User4Accepted = tc.User4Accepted;
            Winner1Id = tc.Winner1Id;
            Result = tc.Result;
            CanAcceptReject = false;
            CanDeclareWin = false;
            PendingAcceptance = false;
            PendingApproval = false;
            IsWinner = false;
            if (currentUser.Id == User1Id)
            {
                PendingAcceptance = String.IsNullOrEmpty(Result);
            }
            if (currentUser.Id == User2Id)
            {
                CanAcceptReject = !User2Accepted && String.IsNullOrEmpty(Result);
                PendingAcceptance = User2Accepted && String.IsNullOrEmpty(Result);
            }
            else if (currentUser.Id == User3Id)
            {
                CanAcceptReject = !User3Accepted && String.IsNullOrEmpty(Result);
                PendingAcceptance = User3Accepted && String.IsNullOrEmpty(Result);
            }
            else if (currentUser.Id == User4Id)
            {
                CanAcceptReject = !User4Accepted && String.IsNullOrEmpty(Result);
                PendingAcceptance = User4Accepted && String.IsNullOrEmpty(Result);
            }

            if(tc.Winner1Id != null)
            {
                if (tc.Winner1Id == tc.User1Id){
                    IsWinner = tc.User1Id == currentUser.Id || tc.User2Id == currentUser.Id;
                }
                else
                {
                    IsWinner = tc.User3Id == currentUser.Id || tc.User4Id == currentUser.Id;
                }

                ScoreText = tc.Sets1 + tc.Sets2 == 2 ?
                    String.Format("{0}:{1}, {2}:{3}", tc.Games11, tc.Games12, tc.Games21, tc.Games22) :
                    String.Format("{0}:{1}, {2}:{3}, {4}:{5}", tc.Games11, tc.Games12, tc.Games21, tc.Games22, tc.Games31, tc.Games32);
            }

            if (Result == "Accepted")
            {
                CanDeclareWin = true;
            }
            else if (Result == "Waiting approval")
            {
                PendingApproval = true;
            }
        }
    }

    public class TeamChallengesVictoryModel
    {
        public int TeamChallengeId { get; set; }
        public string User1Name { get; set; }
        public string User2Name { get; set; }
        public string User3Name { get; set; }
        public string User4Name { get; set; }
        public string Result { get; set; }
        public int? Sets1 { get; set; }
        public int? Sets2 { get; set; }

        public List<GameScore> GameScores { get; set; }

    }
}