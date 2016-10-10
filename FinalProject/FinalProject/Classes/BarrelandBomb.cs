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
    class BarrelandBomb
    {
        int spawnSpeed;
        Random randomBombandBarrelSpawn;

        public BarrelandBomb()
        {

        }
        //Return values to randomly spawn barrel
        public int returnSpawn()
        {
            randomBombandBarrelSpawn = new Random();
            spawnSpeed = randomBombandBarrelSpawn.Next(1, 6);
            return spawnSpeed;
        }
        //Return values to randomly spawn bomb
        public int returnSpawnHard()
        {
            randomBombandBarrelSpawn = new Random();
            spawnSpeed = randomBombandBarrelSpawn.Next(1, 4);
            return spawnSpeed;
        }
    }
}
