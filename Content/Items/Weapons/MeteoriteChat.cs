using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace SpeechAttacker.Content.Items.Weapons
{
    public class MeteoriteChat : BasicChatItem,IColor
    {
        public int Index;
        public Color Color => Color.Lerp(Color.MediumPurple, Color.Orange, (float)Math.Sin(Main.timeForVisualEffects * 0.03));
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 1;
            DamageBones *= 1.1f;
            DamageBones += 0.2f;
            AddTextLength = 250;
            Item.rare = ItemRarityID.Blue;
        }
        public override string GetInputText(string Text)
        {
            string FuckYou = "Fuck you ";
            Player player = Main.LocalPlayer;
            if (player.CheckMana(50, true))
            {
                if (Index >= FuckYou.Length)
                    Index = 0;
                return Text + FuckYou[Index++].ToString();
            }
            return base.GetInputText(Text);
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MeteoriteBar, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
