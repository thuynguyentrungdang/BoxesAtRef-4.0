using System.Reflection;
using BoxesAtRef.Models;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services;

namespace BoxesAtRef;

public record ModMetadata : AbstractModMetadata
{
    public override string ModGuid { get; init; } = "com.SomeoneNamedAdam.barf";
    public override string Name { get; init; } = "Boxes At Ref";
    public override string Author { get; init; } = "SomoneNamedAdam";
    public override List<string>? Contributors { get; init; }
    public override SemanticVersioning.Version Version { get; init; } = new("2.0.0");
    public override SemanticVersioning.Range SptVersion { get; init; } = new("~4.0.0");
    public override List<string>? Incompatibilities { get; init; }
    public override Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
    public override string? Url { get; init; } = "";
    public override bool? IsBundleMod { get; init; } = false;
    public override string? License { get; init; } = "MIT";
}

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
public class BoxesAtRef (
    ISptLogger<BoxesAtRef> logger,
    DatabaseService databaseService,
    ConfigServer configServer,
    ModHelper modHelper) : IOnLoad
{
    private ModItemsToAdd _modItemsToAdd = null!;
    private ModCrateContents _modCrateContents = null!;
    private Dictionary<MongoId, TemplateItem>? _itemDb;
    private readonly InventoryConfig _inventoryConfig = configServer.GetConfig<InventoryConfig>();
    
    public Task OnLoad()
    {
        string pathToMod = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
        
        _modItemsToAdd = modHelper.GetJsonDataFromFile<ModItemsToAdd>(pathToMod, "database\\itemsToAdd.json");
        _modCrateContents = modHelper.GetJsonDataFromFile<ModCrateContents>(pathToMod, "database\\crateContents.json");
        _itemDb = databaseService.GetItems();

        AddBoxesToRef();

        logger.Success("[BoxesAtRef] Mod loaded successfully.");
        return Task.CompletedTask;
    }

    private void AddBoxesToRef()
    {
        Trader refTrader = databaseService.GetTrader(Traders.REF)!;
        
        foreach (ModItemsToAdd.ItemsToAdd item in _modItemsToAdd.ListItemsToAdd)
        {
            string crateId = item.Id;
            
            refTrader.Assort.Items.Add(new Item
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
            
            refTrader.Assort.BarterScheme[crateId] = [];
            
            refTrader.Assort.BarterScheme[crateId].Add([
                new BarterScheme
                {
                    Count = item.Price,
                    Template = Money.GP
                }
            ]);

            refTrader.Assort.LoyalLevelItems[crateId] = item.LoyaltyLevel;

            if (item.OpenId is null)
                continue;
            
            ModCrateContents.CrateContents crateContents = _modCrateContents.ModContents[item.OpenId];
            
            // Add to inventory config with custom item pool
            _inventoryConfig.RandomLootContainers[crateId] = new RewardDetails
            {
                RewardCount = crateContents.RewardCount,
                FoundInRaid = crateContents.FoundInRaid,
                RewardTplPool = crateContents.RewardTemplatePool
            };
        }
    }
}