<div align="center">

# 🗳️ **BARF – Boxes At Ref (SPT 4.0 Edition)**
### *A modern, fully updated revival of the classic BARF mod — now rebuilt for SPT 4.0*

<img src="https://img.shields.io/badge/SPT-4.0.x-blue?style=for-the-badge" />
<img src="https://img.shields.io/badge/Mod-Type-Server-orange?style=for-the-badge" />
<img src="https://img.shields.io/badge/Status-Active-success?style=for-the-badge" />

</div>

---

## ✨ Overview

**BARF (Boxes At Ref)** adds custom loot crates to the **Ref (Therapist)** trader in SPT.  
This version is a complete rewrite for **SPT 4.0**, using the new server architecture, trader assort format, and runtime item generation.

If you've used the original BARF from SPT Forge — this is the modern continuation.

---

## 🚀 Features

- 🆕 **Fully updated for SPT 4.0**
- 📦 **Adds purchasable loot crates** to the Ref trader
- 🏷️ **Configurable crate data** (price, loyalty level, rewards, open animation IDs)
- 🔄 **Auto-generated assort entries** (items, barter schemes, loyalty levels)
- 🧩 **Supports both list-based & dictionary-based config formats**
- 🔧 **No manual JSON edits required**
- ⚡ **Safe reload on server start**
- 🔒 **Compatible with all existing profiles**

---

## 📂 Installation

### Requirements
- **SPT 4.0.x**
- Server mod only — *no BepInEx plugin required*

### Steps
1. Download the latest release
2. Extract it into your SPT installation
3. You should see:

```
SPT/user/mods/BARF/
    BARF.dll
    config/
```

Restart your server and enjoy.

---

## ⚙️ Configuration

Configuration lives here:

```
SPT/user/mods/BARF/config/items.json
```

Two valid JSON formats:

### **1) List Format**
```json
{
  "items": [
    {
      "_id": "crate_unique_id",
      "_tpl": "template_id",
      "openID": "open_animation_id",
      "price": 50,
      "buyRestrictionMax": 1,
      "loyaltyLevel": 1
    }
  ]
}
```

### **2) Dictionary Format**
```json
{
  "66573310a1657263d816a139": {
    "rewardCount": 3,
    "foundInRaid": true,
    "rewardTplPool": []
  }
}
```

BARF automatically detects and loads either structure.

---

## 🛠️ Building From Source

### Requirements
- .NET **9.0** SDK
- A local SPT 4.0 install (for reference assemblies)

### Configure paths (inside .csproj)
```xml
<PropertyGroup>
    <SPTPath>F:\SPT_4.0\SPT</SPTPath>
</PropertyGroup>
```

### Build command
```bash
dotnet build BARF.sln
```

### Output structure
```
dist/
└── SPT/user/mods/BARF/
    └── BARF.dll
```

---

## 🧠 How the Mod Works

- Loads crate/item definitions from `items.json`
- Injects items into Ref trader’s assort at server startup
- Creates:
    - Item entries
    - Barter scheme entries
    - Loyalty level requirements
- Generates GP-based barter schemes
- Ensures dictionaries are initialized to avoid missing-key errors
- Supports unlimited or restricted counts

Everything is done in-memory (no base game files modified).

---

## ⚠️ Known Limitations

- Crates appear only under the **Ref** trader
- Changing config requires a server restart
- No default item pools included — you define your own

---

## 📜 License

Do whatever you want.  
Feel free to fork, modify, or build on top of this version.

---

## 🙌 Credits

- Original BARF mod creators
- SPT development team
- SPT modding community for tools, docs, and reference implementations

---

<div align="center">

### ⭐ If you enjoy the mod, leave a star on the repo!

</div>
