using System.Text.Json.Serialization;
using SPTarkov.Server.Core.Models.Common;

namespace BoxesAtRef.Models;

public class ModCrateContents
{
    public Dictionary<MongoId, CrateContents> ModContents { get; set; } = new();

    public class CrateContents
    {
        [JsonPropertyName("rewardCount")]
        public int RewardCount { get; set; }
        [JsonPropertyName("foundInRaid")]
        public bool FoundInRaid { get; set; }
        [JsonPropertyName("rewardTplPool")]
        public required Dictionary<MongoId, double> RewardTemplatePool { get; set; }
    }
}