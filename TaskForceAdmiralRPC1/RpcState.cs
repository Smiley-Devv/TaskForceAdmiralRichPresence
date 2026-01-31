using System;

namespace TaskForceAdmiralLiveRPC
{
    public class RpcState
    {
        public DateTime WhenUpdated { get; set; }
        public string SaveFileLocation { get; set; }
        public string ScenarioName { get; set; }
        public int FriendlyAircraft { get; set; }
        public int EnemyAircraft { get; set; }
        public int EnemyTracks { get; set; }
        public int AttackingGroups { get; set; }
        public string LastGameUpdate { get; set; }
    }
}
