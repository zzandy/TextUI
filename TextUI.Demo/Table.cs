using System;
using System.Text.Json.Serialization;

namespace TextUI.Demo
{
    public sealed class Match
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("red")]
        public TeamResult Red { get; set; }

        [JsonPropertyName("blue")]
        public TeamResult Blue { get; set; }

        [JsonPropertyName("startDate")]
        public DateTimeOffset Start { get; set; }

        [JsonPropertyName("endDate")]
        public DateTimeOffset End { get; set; }
    }

    public class Team
    {
        [JsonPropertyName("offense")]
        public Player Offence { get; set; }

        [JsonPropertyName("defense")]
        public Player Defence { get; set; }
    }

    public sealed class TeamResult : Team
    {
        [JsonPropertyName("score")]
        public int Score { get; set; }
    }

    public sealed class Player
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }
        
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }
    }
}