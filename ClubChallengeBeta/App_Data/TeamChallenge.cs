//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClubChallengeBeta.App_Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class TeamChallenge
    {
        public int TeamChallengeId { get; set; }
        public string User1Id { get; set; }
        public bool User1Accepted { get; set; }
        public string User2Id { get; set; }
        public bool User2Accepted { get; set; }
        public string User3Id { get; set; }
        public bool User3Accepted { get; set; }
        public string User4Id { get; set; }
        public bool User4Accepted { get; set; }
        public string Winner1Id { get; set; }
        public string Result { get; set; }
        public bool Confirmed { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual AspNetUser AspNetUser1 { get; set; }
        public virtual AspNetUser AspNetUser2 { get; set; }
        public virtual AspNetUser AspNetUser3 { get; set; }
        public virtual AspNetUser AspNetUser4 { get; set; }
    }
}