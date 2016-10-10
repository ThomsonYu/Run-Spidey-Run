using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FinalProject
{
    class DrawClass
    {
        protected Texture2D text;
        protected Rectangle rect;
        protected int time;
        protected int screenWidth;

        private int currentFrame = 0;
        
        public DrawClass(int timer, Texture2D tex, Rectangle rec, int ScreenSize)
        {
            time = timer;
            text = tex;
            rect = rec;
            screenWidth = ScreenSize;
        }
        //Update Venom jumping down rectangle
        public void update(GameTime gameTime)
        {
            if (rect.Y < 280)
            {
                rect.X++;
                rect.Y += 8;
            }
        }

        //Update background code, Scroll the background to the left and then set it back to the right of the screen
        public void updateBackground(GameTime gameTimeBackground)
        {
            if (time > 80)
            {
                rect.X -= 3;
            }

            if (rect.Right < 0)
            {
                rect.X = screenWidth - 20;
            }
            time++;
        }
        //Update Venom code (Animate his running sprite)
        //Over a certain amount of time, Venom gets closer to Spider-Man
        public int updateVenom(GameTime gameTimeVenom)
        {
            if (time < 1800)
            {
                if (time % 10 == 0)
                {
                    if (currentFrame < 6)
                    {
                        currentFrame++;
                    }
                    else
                    {
                        currentFrame = 0;
                    }
                }
            }

            if (time > 1800 && time < 3600)
            {
                if (time % 7 == 0)
                {
                    if (currentFrame < 6)
                    {
                        currentFrame++;
                    }
                    else
                    {
                        currentFrame = 0;
                    }
                }
                if (rect.X < 50)
                {
                    rect.X++;
                }
            }

            if (time > 3600 && time < 5400)
            {
                if (time % 5 == 0)
                {
                    if (currentFrame < 6)
                    {
                        currentFrame++;
                    }
                    else
                    {
                        currentFrame = 0;
                    }
                }
                if (rect.X < 100)
                {
                    rect.X++;
                }
            }

            if (time > 5400)
            {
                if (time % 5 == 0)
                {
                    if (currentFrame < 6)
                    {
                        currentFrame++;
                    }
                    else
                    {
                        currentFrame = 0;
                    }
                }

                if (rect.X < 170)
                {
                    rect.X++;
                }
            }

            time++;
            return currentFrame;
        }
        //Return rectangle
        public Rectangle returnRect()
        {
            return rect;
        }
        //Code to draw rectangles when called
        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(text, rect, Color.White);
            spriteBatch.End();
        }
        //Code to draw sprite animation
        public void drawSpriteAnimation(SpriteBatch spriteBatch, Rectangle rect2)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(text, rect, rect2, Color.White);
            spriteBatch.End();
        }
    }

}
