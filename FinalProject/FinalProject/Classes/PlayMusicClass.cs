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
    class PlayMusicClass
    {
        private float volume = 0.5f;
        private float pitch = 0.0f;
        private float pan = 0.0f;

        KeyboardState keys;
        KeyboardState oldKeys;

        GamePadState pad1;
        GamePadState oldpad1;

        public PlayMusicClass()
        {

        }
        //Play sound effect
        public void PlaySoundEffect(SoundEffect sound)
        {
            sound.Play(volume, pitch, pan);
        }

        public void PlayMusic(Song song)
        {
            //When this method is called, stop the music
            MediaPlayer.Stop();
            //If the music is stopped, play the music
            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Play(song);
            }
            MediaPlayer.IsRepeating = true; //Repeats the song when finished
            
        }

        public void MusicState(Song song)
        {
            pad1 = GamePad.GetState(PlayerIndex.One);
            keys = Keyboard.GetState();
            //If L or the Left shoulder button is pressed, resume music
            if (oldKeys.IsKeyUp(Keys.L) && keys.IsKeyDown(Keys.L) || oldpad1.Buttons.LeftShoulder == ButtonState.Released && pad1.Buttons.LeftShoulder == ButtonState.Pressed)
            {
                if (MediaPlayer.State == MediaState.Paused) //if the music is already paused, then resume the music
                {
                    MediaPlayer.Resume();
                }
            }
            //If P or the right shoulder button is pressed and the music is playing, pause the music
            if (oldKeys.IsKeyUp(Keys.P) && keys.IsKeyDown(Keys.P) || oldpad1.Buttons.RightShoulder == ButtonState.Released && pad1.Buttons.RightShoulder == ButtonState.Pressed)
            {
                if (MediaPlayer.State == MediaState.Playing)
                {
                    MediaPlayer.Pause();
                }
            }

            oldpad1 = pad1;
            oldKeys = keys;
        }
    }
}
