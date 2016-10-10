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
    class MenuClass
    {
        protected int selectState;
        protected bool gameRun;

        private KeyboardState kb;
        private KeyboardState oldKb;
        private GamePadState pad1;
        private GamePadState oldpad1;


        public MenuClass(bool isGameRunning, int currentState)
        {
            gameRun = isGameRunning;
            selectState = currentState;
        }

        public int displayMenu(int currentState, SoundEffect selectUp, SoundEffect selectBack)
        {
            kb = Keyboard.GetState();
            pad1 = GamePad.GetState(PlayerIndex.One);
            //Menu selection, if the game is not running and the player presses up or down on arrow keys or d-pad, change the selection
            //Also play a sound effect for up or down
            if (oldKb.IsKeyUp(Keys.Down) && kb.IsKeyDown(Keys.Down) || oldpad1.DPad.Down == ButtonState.Released && pad1.DPad.Down == ButtonState.Pressed)
            {
                selectState++;
                selectBack.Play(0.5f, 0, 0);
            }

            if (oldKb.IsKeyUp(Keys.Up) && kb.IsKeyDown(Keys.Up) || oldpad1.DPad.Up == ButtonState.Released && pad1.DPad.Up == ButtonState.Pressed)
            {
                selectState--;
                selectUp.Play(0.5f, 0, 0);
            }

            if (selectState < 1)
            {
                selectState = 1;
            }

            if (selectState > 3)
            {
                selectState = 3;
            }

            oldKb = kb;
            oldpad1 = pad1;
            return selectState;
        }
    }
}
