using System.Reflection;
using BoxesAtRef.Models;
using SPTarkov.Common.Models.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Helpers.Server;
using SPTarkov.Server.Core.Helpers.Traders;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Spt.Tables;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services;

namespace BoxesAtRef;

public record ModMetadata : IModMetadata
{
    public string ModGuid { get; init; } = "com.SomeoneNamedAdam.barf";
    public string Name { get; init; } = "Boxes At Ref";
    public string Author { get; init; } = "SomoneNamedAdam";
    public List<string>? Contributors { get; init; }
    public SemanticVersioning.Version Version { get; init; } = new("3.0.0");
    public SemanticVersioning.Range SptVersion { get; init; } = new("~4.1.2");
    public bool HasPrepatcher { get; init; } = false;
    public List<string>? Incompatibilities { get; init; }
    public Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
    public string? Url { get; init; } = "";
    public string? License { get; init; } = "MIT";
}

[Injectable(TypePriority = OnLoadOrder.PostLoad + 1)]
public class BoxesAtRef (
    ISptLogger<BoxesAtRef> logger,
    TraderHelper traderHelper,
    InventoryConfig inventoryConfig,
    ModHelper modHelper) : IOnLoad
{
    private ModItemsToAdd _modItemsToAdd = null!;
    private ModCrateContents _modCrateContents = null!;
    
    public Task OnLoadAsync(CancellationToken cancellationToken)
    {
        string pathToMod = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
        
        _modItemsToAdd = modHelper.GetJsonDataFromFile<ModItemsToAdd>(pathToMod, "database\\itemsToAdd.json");
        _modCrateContents = modHelper.GetJsonDataFromFile<ModCrateContents>(pathToMod, "database\\crateContents.json");

        AddBoxesToRef();

        logger.Success("[BoxesAtRef] Mod loaded successfully.");
        return Task.CompletedTask;
    }

    private void AddBoxesToRef()
    {
        TraderAssort refTrader = traderHelper.GetTraderAssortsByTraderId(Traders.REF)!;
        
        foreach (ModItemsToAdd.ItemsToAdd item in _modItemsToAdd.ListItemsToAdd)
        {
            string crateId = item.Id;
            
            refTrader.Items.Add(new Item
            {
                Id = crateId,
                Template = item.Template,
                ParentId = "hideout",
                SlotId = "hideout",
                Upd = new Upd
                {
                    UnlimitedCount = true,
                    BuyRestrictionMax = item.BuyRestrictionMax,
                    StackObjectsCount = 9999999
                }
            });
            
            refTrader.BarterScheme[crateId] = [];
            
            refTrader.BarterScheme[crateId].Add([
                new BarterScheme
                {
                    Count = item.Price,
                    Template = Money.GP
                }
            ]);

            refTrader.LoyalLevelItems[crateId] = item.LoyaltyLevel;

            if (item.OpenId is null)
                continue;
            
            ModCrateContents.CrateContents crateContents = _modCrateContents.ModContents[item.OpenId];
            
            // Add to inventory config with custom item pool
            inventoryConfig.RandomLootContainers[crateId] = new RewardDetails
            {
                RewardCount = crateContents.RewardCount,
                FoundInRaid = crateContents.FoundInRaid,
                RewardTplPool = crateContents.RewardTemplatePool
            };
        }
    }
}