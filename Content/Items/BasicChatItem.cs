using SpeechAttacker.Content.DamageType;
using SpeechAttacker.Content.Items.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SpeechAttacker.Content.Items
{
    public abstract class BasicChatItem : ModItem
    {
        public StatModifier DamageBones;
        public override void SetDefaults()
        {
            Item.damage = 1;
            DamageBones = new StatModifier(1, 1, 0,0);
            Item.DamageType = ChatDamage.Instance;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int index = tooltips.FindIndex(x => x.Name == "Damage" && x.Mod == "Terraria");
            if (index != -1)
            {
                tooltips.Insert(index + 1, new TooltipLine(Mod, "AddDamage", Language.GetTextValue("Mods.SpeechAttacker.Tooltip.AddDamage") + "+" + (DamageBones.Additive * 100 - 100).ToString() + "% *" + DamageBones.Multiplicative.ToString() + " -1bit"));
            }
            if(this is ISkills) // 这里处理各种情况
            {
                if(this is IBoom)
                {
                    tooltips.Add(new TooltipLine(Mod, "Skill", Language.GetTextValue("Mods.SpeechAttacker.Tooltip.Skill.Boom")));
                }
            }
        }
    }
}
