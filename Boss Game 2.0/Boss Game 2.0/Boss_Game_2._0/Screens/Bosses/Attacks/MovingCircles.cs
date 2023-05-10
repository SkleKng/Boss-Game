using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Boss_Game_2._0.Screens.Bosses.Attacks
{
    class MovingCircles : BossAttack
    {
        float angSpeed; // radians per second
        float totalAngDisp;
        float angSpacing;
        int centerX; // going to be set anyway
        int centerY; // going to be set anyway
        float xAccel; // Would make sense for both these to be called xVel and yVel
        float yAccel; //     unless the bullets in the spiral accelerate.
        int projectileSize;

        public MovingCircles(SpriteBatch spriteBatch, int centerX, int centerY, float angSpacing, float angSpeed, int projectileSpeed, float xAccel, float yAccel, int projectileSize) : base(spriteBatch, projectileSpeed)
        {
            this.angSpeed = angSpeed;
            this.angSpacing = angSpacing;
            totalAngDisp = 0;
            this.centerX = 0; 
            this.centerY = 0;
            this.xAccel = xAccel;
            this.yAccel = yAccel;
            this.projectileSize = projectileSize;
        }

        public override void Update(GameTime gameTime)
        {
            float angMoved = Math.Abs((float)timer.TotalSeconds * angSpeed);


            if (angMoved >= angSpacing)
            {
                centerX += 50;
                if (isAttacking)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        projectiles.Add(new Projectile(spriteBatch, TextureManager.Textures.SolidFill, centerX, centerY, projectileSpeed * (float)Math.Cos(totalAngDisp), projectileSpeed * (float)Math.Sin(totalAngDisp), xAccel, yAccel, projectileSize));
                        totalAngDisp += angMoved;
                    }

                }
                timer = TimeSpan.Zero;
            }

            base.Update(gameTime);
        }

        public override void Draw()
        {
            // stuff

            base.Draw();
        }
    }
}
