using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using SpeechAttacker.Content.Items;
using SpeechAttacker.Content.Items.Skills;
using System;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace SpeechAttacker.Content.Projs
{
    public class ChatProj : ModProjectile
    {
        public string Text;
        public BasicChatItem chatItem;
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 25;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.timeLeft = 1800;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 5;
            Projectile.scale = 1.75f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void OnSpawn(IEntitySource source)
        {
            if(source is EntitySource_ItemUse entitySource_ItemUse && entitySource_ItemUse.Item.ModItem is BasicChatItem chatItem)
            {
                this.chatItem = chatItem;
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float r = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(),targetHitbox.Size(),Projectile.Center - Projectile.velocity.SafeNormalize(default) * ChatSize.X, Projectile.Center + Projectile.velocity.SafeNormalize(default) * ChatSize.X, ChatSize.Y,ref r);
        }
        public override void AI()
        {
            //Projectile.spriteDirection = Projectile.velocity.X > 0 ? 1 : -1;
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.direction == -1)
                Projectile.rotation += MathHelper.Pi;
            Projectile.velocity *= 0.98f;
            if (Projectile.velocity.Length() < 2)
            {
                Player player = Main.player[Projectile.owner];
                Rectangle bottom = player.Hitbox;
                Rectangle top = player.Hitbox;
                bottom.Y += bottom.Height;
                bottom.Height = 5;
                top.Height = 5;
                bool isBottom = false;
                if (bottom.Intersects(Projectile.Hitbox))
                {
                    isBottom = true;
                    player.GetModPlayer<SpeechAttackerPlayer>().StandOnSpeech = true;
                    //player.velocity.X -= player.direction * player.velocity.X * 0.1f;
                }
                if (top.Intersects(Projectile.Hitbox))
                {
                    player.GetModPlayer<SpeechAttackerPlayer>().StandOnSpeech = true;
                    player.position.Y += 2f;
                }

                Rectangle left = player.Hitbox;
                Rectangle right = player.Hitbox;
                left.Width = 5;
                right.X += right.Width - 5;
                right.Width = 5;
                left.Height -= 5;
                right.Height -= 5;
                if (left.Intersects(Projectile.Hitbox) || right.Intersects(Projectile.Hitbox))
                {
                    player.GetModPlayer<SpeechAttackerPlayer>().CollideSpeechX = true;
                    if (!isBottom)
                    {
                        player.position.Y += 8;
                    }
                    else
                    {
                        player.velocity.Y -= Math.Abs(player.velocity.X * 1.5f);
                        player.velocity.X = Math.Clamp(Projectile.width / (Projectile.Center.X - player.Center.X + 0.1f), -5, 5);
                    }
                }
            }
            //NPC target = Projectile.FindTargetWithinRange(1200);
            //if(target != null)
            //{
            //    Projectile.velocity = (Projectile.velocity * 20 + (target.Center - Projectile.Center).SafeNormalize(default) * 50) / 21f;
            //}
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (chatItem != null)
            {
                if (chatItem is IBoom)
                {
                    var proj = Projectile.NewProjectileDirect(Projectile.GetSource_OnHit(target), Projectile.Center, -Projectile.velocity * 2, Type, Projectile.damage / 2, 0f, Projectile.owner);
                    (proj.ModProjectile as ChatProj).Text = "1";
                    proj.CritChance = Projectile.CritChance;
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Text == null)
                return false;
            SpriteBatch sb = Main.spriteBatch;
            sb.DrawString(FontAssets.MouseText.Value, Text, Projectile.Center - Main.screenPosition, Color.Black, Projectile.rotation, ChatSize * 0.5f + new Vector2(1,1), Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
            sb.DrawString(FontAssets.MouseText.Value, Text, Projectile.Center - Main.screenPosition, Color.Black, Projectile.rotation, ChatSize * 0.5f + new Vector2(-1, 1), Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
            sb.DrawString(FontAssets.MouseText.Value, Text, Projectile.Center - Main.screenPosition, Color.Black, Projectile.rotation, ChatSize * 0.5f + new Vector2(1, -1), Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
            sb.DrawString(FontAssets.MouseText.Value, Text, Projectile.Center - Main.screenPosition, Color.Black, Projectile.rotation, ChatSize * 0.5f + new Vector2(-1, -1), Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
            sb.DrawString(FontAssets.MouseText.Value, Text, Projectile.Center - Main.screenPosition, Color.White, Projectile.rotation, ChatSize * 0.5f, Projectile.scale,Projectile.spriteDirection == 1? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
            return false;
        }
        public virtual Vector2 ChatSize => FontAssets.MouseText.Value.MeasureString(Text);
    }
}
