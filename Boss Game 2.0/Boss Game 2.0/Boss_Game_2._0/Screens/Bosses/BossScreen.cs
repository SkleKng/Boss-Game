using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Boss_Game_2._0.Screens.Bosses.Attacks;
using Microsoft.Xna.Framework.Input;
using System;

namespace Boss_Game_2._0.Screens.Bosses
{
    class BossScreen : Screen
    {
        public BossAttack bossAttack; // TODO: store boss object instead of parts of the boss. 
        public TimeSpan timer;
        public Player player;

        public bool isPaused;
        public bool playerTurn;
        public bool playerKilled;

        public int bossHP;

        public BossScreen(SpriteBatch spriteBatch) : base(spriteBatch)
        {
            player = new Player(new Rectangle(800 / 2 - 10, 600, 20, 20), 20,2, spriteBatch);
            bossHP = 20;

            isPaused = false;
            timer = new TimeSpan(0, 0, 0);
            playerTurn = true;
            playerKilled = false;
        }

        public override void Update(GameTime gameTime)
        {
            Game1.kb = Keyboard.GetState();
            KeyboardState kb = Keyboard.GetState();


            if (Game1.IsKeyPressed(Keys.Escape))
            {
                isPaused = !isPaused;
            }


            if (!isPaused)
            {
                bossAttack.Update(gameTime);

                if (playerKilled)
                {
                    bossAttack.Stop();
                }
            }

            for (int i = bossAttack.getProjectilesCount() - 1; i >= 0; i--)
            {
                if (player.ReturnRect().Intersects(bossAttack.getList(i)))
                {
                    player.ChangePlayerHP(2);
                    bossAttack.remove(i);
                    if (player.GetHp() <= 0)
                    {
                        playerKilled = true;
                        bossAttack.Stop();
                        player.Despawn();
                    }
                }
            }

            Game1.UpdateOldKb();
        }

        public override void Draw()
        {
            if (isPaused)
            {
                bossAttack.Draw();
                spriteBatch.Draw(TextureManager.Bosses.Boss1.Boss, new Rectangle(325, 150, 150, 150), Color.White); // TODO: make a boss class that will have its own .Draw() method with this code
                spriteBatch.Draw(TextureManager.Bosses.PauseOverlay, Window, Color.White);
                return;
            }
            
            bossAttack.Draw();
            spriteBatch.Draw(TextureManager.Bosses.Boss1.Boss, new Rectangle(325, 150, 150, 150), Color.White); // TODO: make a boss class that will have its own .Draw() method with this code
            player.DrawPlayer(spriteBatch);
        }
    }
}
