namespace Pratz.Web.Data
{
    using System;
    using System.Collections.Generic;

    public class Vote
    {
        public Guid Id { get; set; }
        public string RoomId { get; set; }
        public string Name { get; set; }
        
        public IDictionary<string, string> MemberVotes { get; set; }
    }
}