using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;

namespace SpeechAttacker.Content.Items.Acc.TimeStops
{
    public class MeteoriteTimeStop : BasicTimeStop
    {
        public override int TimeStopMax => 30;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 8;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MeteoriteBar, 100);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
