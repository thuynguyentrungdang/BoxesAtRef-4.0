using System.Text.Json.Serialization;

namespace BoxesAtRef.Models;

public class ModItemsToAdd
{
    [JsonPropertyName("items")] 
    public List<ItemsToAdd> ListItemsToAdd { get; set; } = [];
    
    public class ItemsToAdd
    {
        [JsonPropertyName("_id")]
        public required string Id { get; set; }
        [JsonPropertyName("_id_old")]
        public required string IdOld { get; set; }
        [JsonPropertyName("_tpl")]
        public required string Template { get; set; }
        [JsonPropertyName("openId")]
        public string? OpenId { get; set; }
        [JsonPropertyName("price")]
        public int Price { get; set; }
        [JsonPropertyName("buyRestrictionMax")]
        public int BuyRestrictionMax { get; set; }
        [JsonPropertyName("loyaltyLevel")]
        public int LoyaltyLevel { get; set; }
    }
    
}