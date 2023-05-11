using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Boss_Game_2._0.Screens.Bosses.Attacks;
using System;
using Boss_Game_2._0.Screens;
using Microsoft.Xna.Framework.Input;

namespace Boss_Game_2._0
{
    class Player
    {
        Rectangle playerRect;
        Color playerColor;
        int hp;
        int maxhp;
        float speed;
        bool iFrames;
        int iFramesTimer;
        int numHealsUsed;

        public Player(Rectangle _playerRect, int _hp, float _speed, SpriteBatch spriteBatch)
        {
            playerColor = Color.White;
            this.hp = _hp;
            this.maxhp = _hp;
            this.speed = _speed;
            this.playerRect = _playerRect;
            //hp = 20;
            //speed = 2;
            //playerRect = new Rectangle(800 / 2 - 10, 600, 20, 20);
            iFrames = false;
            iFramesTimer = 90;
        }

        public void Update()
        {
            KeyboardState kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.W) || kb.IsKeyDown(Keys.Up))
            {
                playerRect.Y -= (int)speed;
            }
            if (kb.IsKeyDown(Keys.S) || kb.IsKeyDown(Keys.Down))
            {
                playerRect.Y += (int)speed;
            }
            if (kb.IsKeyDown(Keys.D) || kb.IsKeyDown(Keys.Right))
            {
                playerRect.X += (int)speed;
            }
            if (kb.IsKeyDown(Keys.A) || kb.IsKeyDown(Keys.Left))
            {
                playerRect.X -= (int)speed;
            }                        
            if (iFrames == true)
            {
                playerColor = Color.Red * 0.5f;
                iFramesTimer--;

                if (iFramesTimer == 0)
                {
                    iFrames = false;
                    iFramesTimer = 90;
                    playerColor = Color.White;
                }
            }

            if(playerRect.X < 0) { playerRect.X = 0;  }
            if(playerRect.X > 780) { playerRect.X = 780; }
            if(playerRect.Y < 0) { playerRect.Y = 0; }
            if(playerRect.Y > 780) { playerRect.Y = 780; }

        }

        public void ChangePlayerHP(int x)
        {
            if (iFrames == false)
            {
                iFrames = true;
                hp -= x;
            }
        }

        public void Heal()
        {
            hp += 10;
            if(hp > maxhp) { hp = maxhp; }
            numHealsUsed++;
        }

        public Rectangle ReturnRect()
        {
            return playerRect;
        }

        public int ReturnX()
        {
            return playerRect.X;
        }

        public int ReturnY()
        {
            return playerRect.Y;
        }
        public int GetHp()
        {
            return hp;
        }

        public void setX(int x)
        {
            playerRect.X = x;
        }

        public void setY(int y)
        {
            playerRect.Y = y;
        }

        public Rectangle Despawn()
        {
            return playerRect = new Rectangle(playerRect.X, playerRect.Y, 0, 0);
        }

        public int GetHealsUsed()
        {
            return numHealsUsed;
        }

        public void DrawPlayer(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.Player.PlayerModel, playerRect, playerColor);
        }


    }
}
