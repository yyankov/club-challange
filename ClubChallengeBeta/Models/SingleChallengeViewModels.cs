using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClubChallengeBeta.App_Data;

namespace ClubChallengeBeta.Models
{
    public class SingleChallengesViewModel
    {
        public int SinglesChallengeId { get; set; }
        public string User1Name { get; set; }
        public string User1Id { get; set; }
        public bool User1Accepted { get; set; }
        public string User2Name { get; set; }
        public string User2Id { get; set; }
        public string WinnerId { get; set; }
        public bool User2Accepted { get; set; }
        public string Result { get; set; }

        public bool CanAcceptReject { get; set; }
        public bool CanDeclareWin { get; set; }
        public bool PendingAcceptance { get; set; }
        public bool PendingApproval { get; set; }
        public bool IsWinner { get; set; }

        public SingleChallengesViewModel(SingleChallenge sc, AspNetUser currentUser)
        {
            SinglesChallengeId = sc.SinglesChallengeId;
            User1Name = sc.AspNetUser.UserName;
            User1Id = sc.User1Id;
            User1Accepted = sc.User1Accepted;
            User2Name = sc.AspNetUser1.UserName;
            User2Id = sc.User2Id;
            WinnerId = sc.WinnerId;
            User2Accepted = sc.User2Accepted;
            Result = sc.Result;
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
            }

            if (sc.WinnerId != null)
            {
                IsWinner = sc.WinnerId == currentUser.Id;
            }

            if (Result == "Accepted")
            {
                CanDeclareWin = true;
            }
            else if (Result == "Pending")
            {
                PendingApproval = true;
            }
        }
    }
}