using System;

namespace Polyblast.scripts.upgrades;

public partial class DamageUpgrade : Upgrade
{
    public DamageUpgrade() : base("strong pew", "Increases player damage by by 1")
    {
    }
    
    public override void OnUpgrade(
        float currentDifficulty, Action<float> difficultySetter, 
        float playerFireRate, Action<float> playerFireRateSetter,
        int playerHealth, Action<int> playerHealthSetter,
        int playerDamage, Action<int> playerDamageSetter,
        float playerSpeed, Action<float> playerSpeedSetter
    )
    {
        playerDamageSetter(playerDamage + 1);
    }
}