using Microsoft.Xna.Framework;
using SpeechAttacker.Content.Projs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace SpeechAttacker.Content.Items.Acc.TimeStops
{
    public abstract class BasicTimeStop : ModItem, ITimeStop
    {
        /// <summary>
        /// 用于暂停的时间
        /// </summary>
        public static int TimeStopTimeMax;
        public static int CD;
        public static int TimeStopTime;
        /// <summary>
        /// 最大的暂停时间
        /// </summary>
        public abstract int TimeStopMax { get; }

        public override void Load()
        {
            base.Load();
            On_Main.CanPauseGame += CanPauseGame;
            On_Main.DoUpdate_HandleChat += HandleChat;
        }

        public static bool CanPauseGame(On_Main.orig_CanPauseGame orig)
        {
            if (TimeStopTime < TimeStopTimeMax && !Main.gameMenu && CD <= 0)
            {
                if (CD > 0)
                    CD--;
                if (TimeStopTimeMax != 0)
                    TimeStopTimeMax = 0;
                else
                    TimeStopTime = 0;
                return true;
            }
            
            if (CD > 0)
                CD--;
            if (TimeStopTimeMax != 0)
                TimeStopTimeMax = 0;
            else
                TimeStopTime = 0;
            return orig.Invoke();
        }
        public static void HandleChat(On_Main.orig_DoUpdate_HandleChat orig)
        {
            if (Main.drawingPlayerChat)
            {
                TimeStopTime++;
                Player player = Main.LocalPlayer;
                foreach (var item in player.armor)
                {
                    if (item.ModItem is ITimeStop time)
                        TimeStopTimeMax += time.TimeStopMax;
                }
                if (TimeStopTime > TimeStopTimeMax)
                    CD = 120;
            }
            orig.Invoke();
        }


        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = Item.height = 5;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
            if(CD > 0 && CD % 5 == 0)
            {
                string Text = CD.ToString();
                var proj = Projectile.NewProjectileDirect(player.GetSource_FromThis(), player.Center, (Main.MouseWorld - player.Center).SafeNormalize(default) * 20, ModContent.ProjectileType<ChatProj>(), (int)(Item.damage + (Text.Length * 1.5f)), 0f, player.whoAmI, -1);
                (proj.ModProjectile as ChatProj).Text = Text;
                proj.timeLeft /= 5;
            }
        }
    }
}
