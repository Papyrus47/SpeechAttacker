using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;

namespace SpeechAttacker.Content.Items.Weapons
{
    public class NormalChat : BasicChatItem
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            AddTextLength = 300;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
