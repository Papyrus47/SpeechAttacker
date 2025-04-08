using SpeechAttacker.Content.DamageType;
using SpeechAttacker.Content.Items.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SpeechAttacker.Content.Items.Weapons
{
    public abstract class BasicChatItem : ModItem
    {
        /// <summary>
        /// 伤害增量
        /// </summary>
        public StatModifier DamageBones;
        /// <summary>
        /// 语言增加量
        /// </summary>
        public int AddTextNum;
        /// <summary>
        /// 语言长度
        /// </summary>
        public int AddTextLength;
        public override void SetDefaults()
        {
            Item.damage = 1;
            DamageBones = new StatModifier(1, 1, 0,0);
            Item.width = 40;
            Item.height = 20;
            Item.DamageType = ChatDamage.Instance;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int index = tooltips.FindIndex(x => x.Name == "Damage" && x.Mod == "Terraria");
            if (index != -1)
            {
                tooltips.Insert(index + 1, new TooltipLine(Mod, "AddTextLength", Language.GetTextValue("Mods.SpeechAttacker.Tooltip.AddTextLength") + (AddTextLength + 470).ToString()));
                tooltips.Insert(index + 1, new TooltipLine(Mod, "AddText", Language.GetTextValue("Mods.SpeechAttacker.Tooltip.AddText") + AddTextNum.ToString()));
                tooltips.Insert(index + 1, new TooltipLine(Mod, "AddDamage", Language.GetTextValue("Mods.SpeechAttacker.Tooltip.AddDamage") + "+" + (DamageBones.Additive * 100 - 100).ToString("0.00") + "% *" + DamageBones.Multiplicative.ToString() + " -1byte"));
            }
            if(this is ISkills) // 这里处理各种情况
            {
                if(this is IBoom)
                {
                    tooltips.Add(new TooltipLine(Mod, "Boom", Language.GetTextValue("Mods.SpeechAttacker.Tooltip.Skill.Boom")));
                }
                if (this is ITrace)
                {
                    tooltips.Add(new TooltipLine(Mod, "Trace", Language.GetTextValue("Mods.SpeechAttacker.Tooltip.Skill.Trace")));
                }
                if(this is ISummon)
                {
                    tooltips.Add(new TooltipLine(Mod, "Summon", Language.GetTextValue("Mods.SpeechAttacker.Tooltip.Skill.Summon")));
                }
                if(this is IPenetrate)
                {
                    tooltips.Add(new TooltipLine(Mod, "Penetrate", Language.GetTextValue("Mods.SpeechAttacker.Tooltip.Skill.Penetrate")));
                }
            }
        }
        /// <summary>
        /// 输入文本触发
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public virtual string OnInputText(string Text) => null;
        /// <summary>
        /// 正在获取文本时候触发
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public virtual string GetInputText(string Text) => null;
        /// <summary>
        /// 发射的文本
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public virtual string ShootText(string Text) => Text;
        /// <summary>
        /// 正在更新打字界面触发
        /// </summary>
        public virtual void OnUpdateHandleChat() { }
    }
}
