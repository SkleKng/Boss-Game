using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Boss_Game_2._0.Screens.Bosses.Attacks;
using System;
using Microsoft.Xna.Framework.Input;

namespace Boss_Game_2._0.Screens.Bosses
{
    class Boss1Screen : BossScreen
    {
        private Random rng;

        private int playerSelection;
        private bool isAttacking;

        Rectangle[] timingZones;

        //gui shit
        private TimeSpan selectTimer;
        bool showSelectionUnderline;

        Rectangle attackBarPos;

        public Boss1Screen(SpriteBatch spriteBatch) : base(spriteBatch)
        {
            bossAttack = new RandomCircles(spriteBatch, 0, 0, (float)Math.PI / 12, (float)Math.PI / 12, 250, 0, 0, 15, 10);
            rng = new Random();
            bossHP = 20;
            playerSelection = 0;

            timingZones = new Rectangle[] { new Rectangle(113, 610, 40, 80), new Rectangle(153, 610, 40, 80), new Rectangle(193, 610, 40, 80), new Rectangle(233, 610, 40, 80), new Rectangle(273, 610, 40, 80) };
            attackBarPos = new Rectangle(700, 620, 20, 60);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();

            if (isPaused)
            {
                base.Update(gameTime);
                return;
            }

            timer += gameTime.ElapsedGameTime;

            if (playerTurn && !isAttacking)
            {
                selectTimer += gameTime.ElapsedGameTime;
                if (selectTimer.Milliseconds >= 500)
                {
                    showSelectionUnderline = !showSelectionUnderline;
                    selectTimer = TimeSpan.Zero;
                }

                if (kb.IsKeyDown(Keys.Right) && Game1.oldKb.IsKeyUp(Keys.Right))
                {
                    playerSelection = Math.Min(2, playerSelection + 1);
                }
                else if (kb.IsKeyDown(Keys.Left) && Game1.oldKb.IsKeyUp(Keys.Left))
                {
                    playerSelection = Math.Max(0, playerSelection - 1);
                }
                else if (kb.IsKeyDown(Keys.Z) && Game1.oldKb.IsKeyUp(Keys.Z))
                {
                    if (playerSelection == 0)//Attack
                    {
                        attackBarPos = new Rectangle(700, 620, 20, 60);
                        isAttacking = true;
                    }
                    else if (playerSelection == 1)//Heal
                    {
                        if (base.player.GetHealsUsed() >= 3)
                        {
                            return;
                        }
                        base.player.Heal();
                        playerTurn = false;
                    }
                    else if (playerSelection == 2)
                    {
                        ScreenManager.SetScreen(ScreenManager.BossMenu);
                    }
                    timer = TimeSpan.Zero;
                    playerSelection = 0;
                    switchAttack();
                }
            }
            else if (isAttacking)
            {
                attackBarPos.X -= 10;
                if (kb.IsKeyDown(Keys.Z) && Game1.oldKb.IsKeyUp(Keys.Z))
                {
                    double multiplier = 0;
                    if (timingZones[2].Contains(attackBarPos)) // Bar is FULLY within green zone
                    {
                        multiplier = 2.5;
                    }
                    else
                    {
                        for(int i = 0; i < timingZones.Length; i++)
                        {
                            if (timingZones[i].Intersects(attackBarPos))
                            {
                                if (i == 0 || i == 4) { multiplier = 1; }
                                else if (i == 1 || i == 3) { multiplier = 1.5; }
                                else { multiplier = 2; }
                            }
                        }
                    }
                    bossHP -= (int)(2 * multiplier);
                    playerTurn = false;
                    isAttacking = false;
                }
            }
            else // boss turn >:)
            {
                player.Update();
                bossAttack.Start();
                if (timer.TotalSeconds >= bossAttack.attackTime) { // Switch back to player's turn
                    bossAttack.Stop();
                    bossAttack.clear();
                    timer = TimeSpan.Zero;
                    player.setX(800 / 2 - 10);
                    player.setY(600);
                    playerTurn = true;
                }
            }
            base.Update(gameTime);
        }

        private void switchAttack()
        {
            int attack = rng.Next(0, 4);
            switch (attack)
            {
                case 0:
                    bossAttack = new TopBottom(spriteBatch, 0, -0, (float)Math.PI / 15, (float)Math.PI / 2, 200, 0, 0, 15, 10);
                    break;
                case 1:
                    bossAttack = new Spiral(spriteBatch, 400, 200, (float)Math.PI / 31, (float)Math.PI / 2.2f, 250, 0, 0, 15, 10, 1);
                    break;
                case 2:
                    bossAttack = new MovingCircles(spriteBatch, 0, 0, (float)Math.PI / 12, (float)Math.PI / 2, 250, 0, 0, 15, 7);
                    break;
                case 3:
                    bossAttack = new RandomCircles(spriteBatch, 0, 0, (float)Math.PI / 12, (float)Math.PI / 12, 250, 0, 0, 15, 10);
                    break;
            }
        }

        public override void Draw()
        {
            spriteBatch.Draw(TextureManager.Textures.Placeholder, Window, Color.White * 0.2f);

            //DEBUG ONLY
            spriteBatch.DrawString(TextureManager.Message.Font, " Boss HP :" + bossHP.ToString() + "\n Player HP :" + base.player.GetHp() + "\n Heals used :" + base.player.GetHealsUsed().ToString(), new Vector2(0, 0), Color.White);

            if (playerTurn)
            {
                if(!isAttacking)
                {
                    spriteBatch.Draw(TextureManager.Textures.TextBox, new Rectangle(100, 550, 600, 200), Color.White);
                    spriteBatch.DrawString(TextureManager.Message.Font, "Attack", new Vector2(185, 625 - 20), Color.White);
                    spriteBatch.DrawString(TextureManager.Message.Font, "Heal", new Vector2(345, 625 - 20), player.GetHealsUsed() < 3 ? Color.White : Color.Gray);
                    spriteBatch.DrawString(TextureManager.Message.Font, "Flee", new Vector2(505, 625 - 20), Color.White);

                    spriteBatch.Draw(TextureManager.Textures.RightArrow, new Rectangle(150 + playerSelection * 160, 600 + 10, 32, 32), Color.White);

                    if (showSelectionUnderline)
                    {
                        spriteBatch.Draw(TextureManager.MainMenu.SelectionUnderline, new Rectangle(185 + playerSelection * 160, 650, 136, 5), Color.White);
                    }
                }
                else
                {
                    spriteBatch.Draw(TextureManager.Textures.AttackBox, new Rectangle(100, 600, 600, 100), Color.White);
                    spriteBatch.Draw(TextureManager.Textures.SolidFill, timingZones[0], new Color(178, 46, 46));
                    spriteBatch.Draw(TextureManager.Textures.SolidFill, timingZones[1], new Color(217, 146, 66));
                    spriteBatch.Draw(TextureManager.Textures.SolidFill, timingZones[2], new Color(68, 186, 87));
                    spriteBatch.Draw(TextureManager.Textures.SolidFill, timingZones[3], new Color(217, 146, 66));
                    spriteBatch.Draw(TextureManager.Textures.SolidFill, timingZones[4], new Color(178, 46, 46));

                    spriteBatch.Draw(TextureManager.Textures.SolidFill, attackBarPos, Color.White);
                }
                
            }
            else
            {
                player.DrawPlayer(spriteBatch);
            }            
            base.Draw();
        }
    }
}
