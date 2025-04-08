using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using SpeechAttacker.Content.Items.Skills;
using SpeechAttacker.Content.Items.Weapons;
using System;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace SpeechAttacker.Content.Projs
{
    /// <summary>
    /// AI0是前一个字符
    /// </summary>
    public class ChatProj : ModProjectile
    {
        public string Text;
        public BasicChatItem chatItem;
        public bool Summon;
        public bool Trace;
        public bool Boom;
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 25;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.timeLeft = 1800;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 1;
            Projectile.scale = 1.75f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void OnSpawn(IEntitySource source)
        {
            if (source is EntitySource_ItemUse entitySource_ItemUse && entitySource_ItemUse.Item.ModItem is BasicChatItem chatItem)
            {
                this.chatItem = chatItem;
                if (chatItem is ITrace)
                {
                    Projectile.damage /= 2;
                    Trace = true;
                }
                if (chatItem is ISummon)
                    Summon = true;
                if (chatItem is IBoom)
                    Boom = true;
                if (chatItem is IPenetrate)
                    Projectile.penetrate = 5;
            }
            if(source is EntitySource_OnHit source_OnHit && source_OnHit.Entity is Projectile proj && proj.ModProjectile is ChatProj chatProj)
            {
                this.chatItem = chatProj.chatItem;
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float r = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(),targetHitbox.Size(),Projectile.Center - Projectile.velocity.SafeNormalize(default) * ChatSize.X, Projectile.Center + Projectile.velocity.SafeNormalize(default) * ChatSize.X, ChatSize.Y,ref r);
        }
        public override bool? CanDamage()
        {
            if (chatItem != null && Summon)
                return false;
            return base.CanDamage();
        }
        public override void AI()
        {
            if(Projectile.damage <= 0 || Text.Length == 0)
            {
                Projectile.Kill();
                return;
            }
            Player player = Main.player[Projectile.owner];
            Projectile.timeLeft -= player.ownedProjectileCounts[Type] / 100;
            //Projectile.spriteDirection = Projectile.velocity.X > 0 ? 1 : -1;
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.direction == -1)
                Projectile.rotation += MathHelper.Pi;
            Projectile.velocity *= 0.98f;
            #region 特殊AI
            if (chatItem != null)
            {
                if(Summon)
                {
                    Projectile.timeLeft = 100;
                    Projectile.minion = true;
                    if (Projectile.ai[0] != -1)
                    {
                        Projectile proj = Main.projectile[(int)Projectile.ai[0]];
                        while ((int)proj.ai[0] != -1)
                        {
                            proj = Main.projectile[(int)proj.ai[0]];
                        }
                        if (proj.ModProjectile is ChatProj chat)
                        {
                            chat.Text += Text;
                        }
                        Projectile.Kill();
                        return;
                    }
                    NPC target = Projectile.FindTargetWithinRange(600);

                    if (target != null)
                    {
                        Projectile.velocity = (Projectile.velocity * 10 + (target.Center - Projectile.Center) * 0.02f) / 11f;
                        if (target.Center.Distance(Projectile.Center) < 500)
                            Projectile.velocity *= 0.1f;
                        if (Projectile.ai[1]++ > 20)
                        {
                            Projectile.ai[1] = 0;
                            var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, (target.Center - Projectile.Center).SafeNormalize(default) * 20, Type, (int)(Projectile.damage + (Text.Length * 1.5f)), 0f, Projectile.owner,-1);
                            (proj.ModProjectile as ChatProj).Text = Text[0].ToString();
                            Text = Text.Remove(0, 1);
                            proj.CritChance = Projectile.CritChance;
                            proj.timeLeft /= 5;
                            if (Text.Length == 0)
                                Projectile.Kill();
                        }
                    }
                    else
                    {
                        Projectile.ai[1] = 0;
                        Projectile.rotation = Projectile.rotation.AngleLerp(0, 0.9f);
                        Projectile.spriteDirection = 1;

                        Vector2 center = player.Center + new Vector2(0, -100 - Projectile.minionPos * 50);
                        if (center.Distance(Projectile.Center) > 10)
                            Projectile.velocity = (center - Projectile.Center).SafeNormalize(default) * 5;
                        else
                            Projectile.velocity *= 0.6f;
                    }
                }
                else if (Trace)
                {
                    NPC target = Projectile.FindTargetWithinRange(1200);
                    if (target != null)
                    {
                        Projectile.velocity = (Projectile.velocity * 20 + (target.Center - Projectile.Center).SafeNormalize(default) * 50) / 21f;
                    }
                }
            }
            #endregion
            if (Projectile.velocity.Length() < 0.5f && !Summon && !Trace)
            {
                Rectangle bottom = player.Hitbox;
                //Rectangle top = player.Hitbox;
                bottom.Y += bottom.Height;
                bottom.Height = 5;
                //top.Height = 5;
                bool isBottom = false;
                if (bottom.Intersects(Projectile.Hitbox))
                {
                    isBottom = true;
                    player.GetModPlayer<SpeechAttackerPlayer>().StandOnSpeech = true;
                    //player.velocity.X -= player.direction * player.velocity.X * 0.1f;
                }
                //if (top.Intersects(Projectile.Hitbox))
                //{
                //    player.GetModPlayer<SpeechAttackerPlayer>().StandOnSpeech = true;
                //    player.position.Y += 2f;
                //}

                Rectangle left = player.Hitbox;
                Rectangle right = player.Hitbox;
                left.Width = 5;
                right.X += right.Width - 5;
                right.Width = 5;
                left.Height -= left.Height - 10;
                right.Height -= right.Height - 10;
                left.Y += 30;
                right.Y += 30;
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
                        player.velocity.X -= Math.Clamp(Projectile.width / (Projectile.Center.X - player.Center.X + 0.1f * player.direction), -3f, 3f);
                    }
                }
            }

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (chatItem != null)
            {
                if (Boom)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var proj = Projectile.NewProjectileDirect(Projectile.GetSource_OnHit(target), Projectile.Center, -Projectile.velocity.RotatedByRandom(3.14) * 2, Type, Projectile.damage / 2, 0f, Projectile.owner, -1);
                        (proj.ModProjectile as ChatProj).Text = "Boom";
                        if (Trace)
                            (proj.ModProjectile as ChatProj).Trace = true;
                        proj.CritChance = Projectile.CritChance;
                        proj.timeLeft /= 10;
                    }
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Text == null)
                return false;
            SpriteBatch sb = Main.spriteBatch;
            DynamicSpriteFont value = GetDynamicSpriteFont();
            sb.DrawString(value, Text, Projectile.Center - Main.screenPosition, Color.Black, Projectile.rotation, ChatSize * 0.5f + new Vector2(1, 1), Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
            sb.DrawString(value, Text, Projectile.Center - Main.screenPosition, Color.Black, Projectile.rotation, ChatSize * 0.5f + new Vector2(-1, 1), Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
            sb.DrawString(value, Text, Projectile.Center - Main.screenPosition, Color.Black, Projectile.rotation, ChatSize * 0.5f + new Vector2(1, -1), Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
            sb.DrawString(value, Text, Projectile.Center - Main.screenPosition, Color.Black, Projectile.rotation, ChatSize * 0.5f + new Vector2(-1, -1), Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
            Color color = Color.White;
            if(chatItem != null && chatItem is IColor iColor)
            {
                color = iColor.Color;
            }
            sb.DrawString(value, Text, Projectile.Center - Main.screenPosition, color, Projectile.rotation, ChatSize * 0.5f, Projectile.scale,Projectile.spriteDirection == 1? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
            return false;
        }
        public virtual Vector2 ChatSize => GetDynamicSpriteFont().MeasureString(Text);
        public DynamicSpriteFont GetDynamicSpriteFont()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                return SpeechAttacker.PixelFont.Value;
            return FontAssets.MouseText.Value;
        }
    }
}
