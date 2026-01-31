using System;

namespace TaskForceAdmiralLiveRPC
{
    public class SaveSnapshot
    {
        public string ScenarioName { get; set; }
        public int FriendlyAircraft { get; set; }
        public int EnemyAircraft { get; set; }
        public int EnemyTracks { get; set; }
        public int AttackingGroups { get; set; }
        public string LastUpdate { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is SaveSnapshot other)
            {
                return ScenarioName == other.ScenarioName &&
                       FriendlyAircraft == other.FriendlyAircraft &&
                       EnemyAircraft == other.EnemyAircraft &&
                       EnemyTracks == other.EnemyTracks &&
                       AttackingGroups == other.AttackingGroups &&
                       LastUpdate == other.LastUpdate;
            }
            return false;
        }

        public override int GetHashCode() =>
            HashCode.Combine(ScenarioName, FriendlyAircraft, EnemyAircraft, EnemyTracks, AttackingGroups, LastUpdate);
    }
}