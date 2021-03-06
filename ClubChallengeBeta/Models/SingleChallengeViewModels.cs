﻿using System;
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
        public string ScoreText { get; set; }

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

                ScoreText = sc.Sets1 + sc.Sets2 == 2 ?
                    String.Format("{0}:{1}, {2}:{3}",sc.Games11,sc.Games12,sc.Games21,sc.Games22) :
                    String.Format("{0}:{1}, {2}:{3}, {4}:{5}", sc.Games11, sc.Games12, sc.Games21, sc.Games22, sc.Games31, sc.Games32);
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

    public class GameScore
    {
        public int? Games1 { get; set; }
        public int? Games2 { get; set; }
    }

    public class SingleChallengesVictoryModel
    {
        public int SinglesChallengeId { get; set; }
        public string User1Name { get; set; }
        public string User2Name { get; set; }
        public string Result { get; set; }
        public int? Sets1 { get; set; }
        public int? Sets2 { get; set; }

        public List<GameScore> GameScores { get; set; }

    }
}