using SpeechAttacker.Content.Items.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechAttacker.Content.Items
{
    public class TestChatItem : BasicChatItem,IBoom
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 10;
            DamageBones += 10;
            DamageBones *= 10;
        }
    }
}
