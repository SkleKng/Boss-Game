using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Boss_Game_2._0.Screens.Bosses.Attacks;
using System;
using Microsoft.Xna.Framework.Input;

namespace Boss_Game_2._0.Screens.Bosses
{
    class Boss2Screen : BossScreen
    {
        private Random rng;

        private int playerSelection;

        //gui shit
        private TimeSpan selectTimer;
        bool showSelectionUnderline;

        public Boss2Screen(SpriteBatch spriteBatch) : base(spriteBatch)
        {
            bossAttack = new RandomCircles(spriteBatch, 0, 0, (float)Math.PI / 12, (float)Math.PI / 12, 250, 0, 0, 15, 10);
            rng = new Random();
            bossHP = 20;
            playerSelection = 0;
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

            if (playerTurn)
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
                        base.bossHP -= rng.Next(2, 6);

                    }
                    else if (playerSelection == 1)//Heal
                    {
                        if (base.player.GetHealsUsed() >= 3)
                        {
                            return;
                        }
                        base.player.Heal();
                    }
                    else if (playerSelection == 2)
                    {
                        ScreenManager.SetScreen(ScreenManager.BossMenu);
                    }
                    timer = TimeSpan.Zero;
                    playerSelection = 0;
                    playerTurn = false;
                    switchAttack();
                }
            }
            else // boss turn >:)
            {
                player.Update();
                bossAttack.Start();
                if (timer.TotalSeconds >= bossAttack.attackTime)
                { // Switch back to player's turn
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
                    bossAttack = new Spiral(spriteBatch, 400, 200, (float)Math.PI / 33, (float)Math.PI / 1.5f, 250, 0, 0, 15, 10, 16);
                    break;
                case 2:
                    bossAttack = new MovingCircles(spriteBatch, 0, 0, (float)Math.PI / 12, (float)Math.PI / 2, 250, 0, 0, 15, 7);
                    break;
                case 3:
                    bossAttack = new RandomCircles(spriteBatch, 0, 0, (float)Math.PI / 12, (float)Math.PI / 12, 250, 0, 0, 15, 10);
                    break;
            }
            bossAttack = new Spiral(spriteBatch, 400, 200, (float)Math.PI / 33, (float)Math.PI / 1.5f, 250, 0, 0, 15, 10, 16);
        }

        public override void Draw()
        {
            spriteBatch.Draw(TextureManager.Textures.Placeholder2, Window, Color.White * 0.2f);

            //DEBUG ONLY
            spriteBatch.DrawString(TextureManager.Message.Font, " Boss HP :" + bossHP.ToString() + "\n Player HP :" + base.player.GetHp() + "\n Heals used :" + base.player.GetHealsUsed().ToString(), new Vector2(0, 0), Color.White);

            if (playerTurn)
            {
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
                player.DrawPlayer(spriteBatch);
            }
            base.Draw();
        }
    }
}
