﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClubChallengeBeta.App_Data;
using System.ComponentModel.DataAnnotations;

namespace ClubChallengeBeta.Models
{
    public class UserViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int Score { get; set; }
        public int TeamScore { get; set; }
        public int Trophies { get; set; }
        public int TeamTrophies { get; set; }
        public UserViewModel(AspNetUser user)
        {
            UserId = user.Id;
            UserName = user.UserName;
            Score = user.Score;
            TeamScore = user.TeamScore;
            Trophies = user.Trophies;
            TeamTrophies = user.TeamTrophies;
        }
    }
    public class ClubViewModel
    {
        public int ClubId { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string OwnerName { get; set; }
        public string OwnerId { get; set; }
        public int UsersCount { get; set; }
        public bool OwnClub { get; set; }
        public List<UserViewModel> Users { get; set; }
        public List<UserViewModel> SinglesLeaders { get; set; }
        public List<UserViewModel> TeamLeaders { get; set; }

        public ClubViewModel(Club club, AspNetUser user)
        {
            ClubId = club.ClubId;
            Name = club.Name;
            OwnerName = club.AspNetUser.UserName;
            OwnerId = club.AspNetUser.Id;
            Text = club.Text;
            UsersCount = club.AspNetUsers.Count;
            ClubId = club.ClubId;
            OwnClub = club.ClubId == user.ClubId;
        }
    }

    public class ClubSearchViewModel
    {
        public bool ClubUser { get; set; }
        public bool HasClub { get; set; }
        public List<ClubViewModel> Clubs { get; set; }
    }
}