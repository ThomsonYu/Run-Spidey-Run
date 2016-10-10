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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //TEXTURE AND RECTANGLE VARIABLES VARIABLES
        Texture2D spMnText, barrelText, bombText, explosionText, HowToPlayText, venom, venom2, venomSprite, backGround, titleTex;
        Rectangle spMnRect, sourceRectRunning, sourceRectJumping, sourceRectDucking, barrelRect, barrelSourceRect, bombRect, bombSourceRect, explosionSourceRect,
                  explosionRect, HowToPlayRect, backGroundRec, backGroundRec2, venomRec, venom2Rec, venomRun, venomRun2, titleRec;

        //Keyboard and Gamepad Variables
        KeyboardState keys;
        KeyboardState oldKeys;

        GamePadState pad1;
        GamePadState oldpad1;

        //Game state is originally set to false
        bool startGame = false;
        bool reset = false; //Reset variable to be called when player wants to reset the game
        bool isSpiderManDead = false; //Variable declaring if player is dead or not
        int selectState = 1; //Select state of Menu options
        bool HowToPlayMenu = false; //Variable to check if player wants to access the How To Play Menu

        //Timer variables
        int timer = 0;
        int timerVenom = 0;
        //Current frame holder (start at 1) (Spider Man)
        int currentFrameRunning = 1;
        int currentFrameJumping = 1;
        int currentFrameDucking = 1;
        //Width of a single sprite image, not the whole Sprite Sheet (Spider Man)
        int spriteWidthRunning = 31;
        int spriteHeightRunning = 35;

        int spriteWidthJumping = 24;
        int spriteHeightJumping = 40;

        int spriteWidthDucking = 33;
        int spriteHeightDucking = 33;

        //How much time has passed since SM has been moved back
        int movedBackTimer = 0;

        bool isBeingPushed = false;

        bool running = true;
        bool jump = false;
        bool ducking = false;
        bool duckReverse = false;

        bool bombExploded = false;
        bool explosionTime = false;

        //Barrel current frame
        int barrelCurrentFrame = 1;
        int bombCurrentFrame = 4;
        int explosionCurrentFrame = 1;
        //Width of a single sprite image, not the whole Sprite Sheet
        int spriteWidthBarrel = 12;
        int spriteWidthBomb = 12;
        int spriteWidthExplosion = 71;
        //Height of a single sprite image, not the whole Sprite Sheet
        int spriteHeightBarrel = 12;
        int spriteHeightBomb = 12;
        int spriteHeightExplosion = 64;
        //Barrel Variables
        int barrelTimer = 0;
        int barrelSpeed = 4;
        //Bomb Variables
        int bombTimer = 0;
        int bombGravity = 0;
        int bombSpeed = 6;
        //Bomb and Barrel variables
        int explosionTimer = 0;
        int spawnTimer = 0;
        int spawnTime = 0;

        int chosenBarrelOrBomb = 0;
        //Random Variables to get random numbers for spawning the barrels and bombs
        Random spawnWhen;
        Random barrelOrBomb;

        //High Score variable
        int score = 0;
        //Venom's current frame in sprite animation
        int VenomCurrentFrame = 0;
        //Venom sprite width and height
        int VenomSpriteWidth = 90;
        int VenomSpriteHeight = 110;

        //Color Variables for Menu
        Color playGameColour = Color.Red;
        Color howToPlayColour = Color.White;
        Color exitColor = Color.White;

        SpriteFont font;

        //Variables for calling classes
        MenuClass runMenu;

        DrawClass drawBackground;
        DrawClass drawBackground2;

        DrawClass drawTitle;

        DrawClass venomDraw;
        DrawClass venomDraw2;
        DrawClass venomRunDraw;

        DrawClass drawHowToPlay;

        BarrelandBomb randomSpawner;

        //Sound Effect Variables
        SoundEffect explosionSE;
        SoundEffect selectBackSE;
        SoundEffect selectUpSE;
        SoundEffect jumpSE;

        //Array for Songs
        Song[] themes = new Song[5];

        //Class variables to call music player class
        PlayMusicClass playExplosion;
        PlayMusicClass playSelectBack;
        PlayMusicClass playSelectUp;
        PlayMusicClass playJump;
        PlayMusicClass playThemes;
        int selectSong = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //WINDOW SIZE INITIALIZE
            graphics.PreferredBackBufferHeight = 400;
            graphics.PreferredBackBufferWidth = 1000;
            graphics.ApplyChanges();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //BACKGROUND LOAD
            backGroundRec = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            backGroundRec2 = new Rectangle(graphics.PreferredBackBufferWidth, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            backGround = Content.Load<Texture2D>("Objects&Background/citySprite");
            backGround = Content.Load<Texture2D>("Objects&Background/citySprite");
            drawBackground = new DrawClass(timerVenom, backGround, backGroundRec, graphics.PreferredBackBufferWidth);
            drawBackground2 = new DrawClass(timerVenom, backGround, backGroundRec2, graphics.PreferredBackBufferWidth);

            titleTex = Content.Load<Texture2D>("Objects&Background/Title");
            titleRec = new Rectangle(graphics.PreferredBackBufferWidth / 2 - 125, 20, titleTex.Width, titleTex.Height);
            drawTitle = new DrawClass(timer, titleTex, titleRec, graphics.PreferredBackBufferWidth);

            //MENU LOAD
            HowToPlayRect = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            font = Content.Load<SpriteFont>("font");

            //SPIDERMAN LOAD AND BOMB/BARREL LOAD
            spMnText = Content.Load<Texture2D>("Spiderman/SpiderManSpriteSheet");
            barrelText = Content.Load<Texture2D>("Objects&Background/Barrels Cropped");
            bombText = Content.Load<Texture2D>("Objects&Background/bomb");
            explosionText = Content.Load<Texture2D>("Objects&Background/ExplosionStrip");
            HowToPlayText = Content.Load<Texture2D>("Objects&Background/HowToPlay");

            spMnRect = new Rectangle(100, 290, 80, 80);
            barrelRect = new Rectangle(1000, 340, 30, 30);
            bombRect = new Rectangle(1040, 290, 20, 20);
            explosionRect = new Rectangle(1000, 290, 60, 50);

            barrelOrBomb = new Random();
            spawnWhen = new Random();


            //Call Random fuction to get random numbers for spawning barrel and bomb
            spawnTime = spawnWhen.Next(240, 420);
            chosenBarrelOrBomb = barrelOrBomb.Next(1, 3);
            
            randomSpawner = new BarrelandBomb();

            //VENOM LOAD
            venom = Content.Load<Texture2D>("Venom/venom");
            venom2 = Content.Load<Texture2D>("Venom/venom2");

            venomRec = new Rectangle(-20, 0, 46, 145);
            venom2Rec = new Rectangle(-5, 300, 55, 67);

            venomRun = new Rectangle(0, 280, 85, 100);

            venomSprite = Content.Load<Texture2D>("Venom/SpriteSheet");
            venomDraw = new DrawClass(timerVenom, venom, venomRec, graphics.PreferredBackBufferWidth);
            venomDraw2 = new DrawClass(timerVenom, venom2, venom2Rec, graphics.PreferredBackBufferWidth);
            venomRunDraw = new DrawClass(timerVenom, venomSprite, venomRun, graphics.PreferredBackBufferWidth);

            //MENU LOAD
            runMenu = new MenuClass(startGame, selectState);
            drawHowToPlay = new DrawClass(timer, HowToPlayText, HowToPlayRect, graphics.PreferredBackBufferWidth);

            //Sound/Music LOAD
            explosionSE = Content.Load<SoundEffect>("Sounds&Music/explosion1");
            selectUpSE = Content.Load<SoundEffect>("Sounds&Music/selectUp");
            selectBackSE = Content.Load<SoundEffect>("Sounds&Music/selectBack");
            jumpSE = Content.Load<SoundEffect>("Sounds&Music/jump1");

            themes[0] = Content.Load<Song>("Sounds&Music/theme1");
            themes[1] = Content.Load<Song>("Sounds&Music/theme2");
            themes[2] = Content.Load<Song>("Sounds&Music/theme3");
            themes[3] = Content.Load<Song>("Sounds&Music/theme4");
            themes[4] = Content.Load<Song>("Sounds&Music/theme5");

            playExplosion = new PlayMusicClass();
            playSelectUp = new PlayMusicClass();
            playSelectBack = new PlayMusicClass();
            playJump = new PlayMusicClass();
            playThemes = new PlayMusicClass();

            //Play Music by calling music player class
            playThemes.PlayMusic(themes[selectSong]);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            pad1 = GamePad.GetState(PlayerIndex.One);
            keys = Keyboard.GetState();

            //Press Space on Keyboard or X on Gamepad to change Music
            if (oldKeys.IsKeyUp(Keys.Space) && keys.IsKeyDown(Keys.Space) || oldpad1.Buttons.X == ButtonState.Released && pad1.Buttons.X == ButtonState.Pressed)
            {
                selectSong++;
                if (selectSong == 5)
                {
                    selectSong = 0;
                }
                //Play Music by calling music player class
                playThemes.PlayMusic(themes[selectSong]);
            }

            playThemes.MusicState(themes[selectSong]); //Call music class to control pause and resume of song

            if (startGame == true && isSpiderManDead == false)
            {
                //If game is running, then draw back ground
                drawBackground.updateBackground(gameTime);
                drawBackground2.updateBackground(gameTime);


                venomDraw.update(gameTime); //Call the update method in the DrawClass

                VenomCurrentFrame = venomRunDraw.updateVenom(gameTime); //return current frame using the updateVenom method from DrawClass

                venomRun2 = new Rectangle(VenomCurrentFrame * VenomSpriteWidth, 130, VenomSpriteWidth - 10, VenomSpriteHeight); //Update Venom's current frame to the rectangle

                if (timerVenom > 80) //Being counting score only until Venom starts running
                {
                    score++; //Highscore
                }

                //Bomb bouncing
                bombGravity += 1;
                bombRect.Y += (int)bombGravity;

                if (bombRect.Y >= 345)
                {
                    bombGravity = -20;
                }

                //Checks if the space bar is pressed
                if (!oldKeys.IsKeyDown(Keys.Up) && keys.IsKeyDown(Keys.Up) || oldpad1.Buttons.A == ButtonState.Released && pad1.Buttons.A == ButtonState.Pressed)
                {
                    playJump.PlaySoundEffect(jumpSE);
                    jump = true;
                    running = false;
                    ducking = false;
                    timer = 0;
                }

                //Checks if the down arrow is pressed
                if (!oldKeys.IsKeyDown(Keys.Down) && keys.IsKeyDown(Keys.Down) && jump == false || oldpad1.Buttons.B != ButtonState.Pressed && pad1.Buttons.B == ButtonState.Pressed && jump == false)
                {
                    jump = false;
                    running = false;
                    ducking = true;
                    timer = 0;
                }

                //RUNNING WHICH IS THE DEFAULT
                if (running == true && jump == false && ducking == false)
                {
                    if (timer == 5)
                    {
                        if (currentFrameRunning < 9) //Animate SM by increasing the frame number
                        {
                            currentFrameRunning++;
                        }
                        else
                        {
                            currentFrameRunning = 1;
                        }
                        timer = 0;
                    }

                    if (currentFrameRunning > 0 && currentFrameRunning < 6)
                    {
                        sourceRectRunning = new Rectangle(currentFrameRunning * spriteWidthRunning, 35, spriteWidthRunning, spriteHeightRunning);
                    }
                    else
                    {
                        sourceRectRunning = new Rectangle((currentFrameRunning * spriteWidthRunning) + 3, 35, spriteWidthRunning, spriteHeightRunning);
                    }
                    timer++;
                }

                //ONCE THE SPACEBAR IS PRESSED SM JUMPS
                if (jump == true)
                {
                    running = false;
                    ducking = false;

                    if (timer == 5)
                    {
                        if (currentFrameJumping < 10) //Animate SM to jump
                        {
                            currentFrameJumping++;
                        }
                        else
                        {
                            jump = false;
                            running = true;
                            ducking = false;
                            currentFrameJumping = 1;
                            currentFrameRunning = 1;
                            currentFrameDucking = 1;
                        }
                        timer = 0;
                    }
                    
                    if (currentFrameJumping != 1 && currentFrameJumping != 10 && currentFrameJumping != 3)
                    {
                        sourceRectJumping = new Rectangle((currentFrameJumping * spriteWidthJumping) + 4, 75, spriteWidthJumping, spriteHeightJumping);
                    }
                    else
                    {
                        sourceRectJumping = new Rectangle((currentFrameJumping * spriteWidthJumping) + 5, 75, spriteWidthJumping, spriteHeightJumping);
                    }
                    //Move SM up and down (Make it look like he is jumping)
                    if (currentFrameJumping == 3)
                    {
                        spMnRect.Y = 90;
                    }
                    if (currentFrameJumping == 4)
                    {
                        spMnRect.Y = 60;
                    }
                    if (currentFrameJumping == 6)
                    {
                        sourceRectJumping = new Rectangle((5 * spriteWidthJumping) + 4, 75, spriteWidthJumping, spriteHeightJumping);
                        spMnRect.Y = 40;
                    }
                    if (currentFrameJumping == 7)
                    {
                        sourceRectJumping = new Rectangle((5 * spriteWidthJumping) + 4, 75, spriteWidthJumping, spriteHeightJumping);
                        spMnRect.Y = 20;
                    }
                    if (currentFrameJumping == 8)
                    {
                        sourceRectJumping = new Rectangle((5 * spriteWidthJumping) + 4, 75, spriteWidthJumping, spriteHeightJumping);
                        spMnRect.Y = 40;
                    }
                    if (currentFrameJumping == 9)
                    {
                        sourceRectJumping = new Rectangle((6 * spriteWidthJumping) + 4, 75, spriteWidthJumping, spriteHeightJumping);
                        spMnRect.Y = 160;
                    }
                    if (currentFrameJumping == 10)
                    {
                        sourceRectJumping = new Rectangle((7 * spriteWidthJumping) + 5, 75, spriteWidthJumping, spriteHeightJumping);
                        spMnRect.Y = 290;
                    }

                    timer++;
                }


                //Code to make SM duck
                if (ducking == true)
                {
                    running = false;
                    jump = false;


                    sourceRectDucking = new Rectangle((currentFrameDucking * spriteWidthDucking), 162, spriteWidthDucking, spriteHeightDucking);

                    if (timer == 3)
                    {
                        if (currentFrameDucking < 6 && duckReverse == false) //Animate SM ducking
                        {
                            currentFrameDucking++;
                        }
                        else
                        {
                            duckReverse = true;
                        }

                        if (duckReverse == true)
                        {
                            currentFrameDucking--;
                        }

                        if (currentFrameDucking <= 0)
                        {
                            jump = false;
                            running = true;
                            ducking = false;
                            duckReverse = false;
                            currentFrameJumping = 1;
                            currentFrameRunning = 1;
                            currentFrameDucking = 1;
                        }
                        timer = 0;
                    }

                    if (currentFrameDucking == 4)
                    {
                        sourceRectDucking = new Rectangle((4 * spriteWidthDucking), 162, spriteWidthDucking, spriteHeightDucking);
                    }
                    if (currentFrameDucking == 5)
                    {
                        sourceRectDucking = new Rectangle((4 * spriteWidthDucking), 162, spriteWidthDucking, spriteHeightDucking);
                    }
                    if (currentFrameDucking == 6)
                    {
                        sourceRectDucking = new Rectangle((4 * spriteWidthDucking), 162, spriteWidthDucking, spriteHeightDucking);
                    }

                    timer++;
                }


                //Checking if player has hit the barrel
                if (spMnRect.Intersects(barrelRect))
                {
                    if (running == true || currentFrameJumping == 1 || currentFrameJumping == 2)
                    {
                        spMnRect.X = (barrelRect.X - 65);
                        isBeingPushed = true; //If he is, push SM back
                    }
                }
                else
                {
                    isBeingPushed = false;
                }

                //Checking if player has hit the bomb
                if (spMnRect.Intersects(bombRect))
                {
                    if (ducking == false || running == true) //Bomb effects SM if he is not ducking
                    {
                        explosionTime = true;
                        bombExploded = true;
                        explosionRect.X = bombRect.X;
                        explosionRect.Y = bombRect.Y;
                        bombRect.X = -10;
                        bombRect.Y = -10;
                        spMnRect.X = spMnRect.X - 80;
                        isBeingPushed = true; //Push SM back
                        playExplosion.PlaySoundEffect(explosionSE); //Play explosion sound
                    }
                }
                else
                {
                    isBeingPushed = false;
                    bombExploded = false;
                }
                //Code to display explosion sprite animation
                if (explosionTime == true)
                {
                    if (explosionCurrentFrame < 20)
                    {
                        explosionSourceRect = new Rectangle(explosionCurrentFrame * spriteWidthExplosion - 71, 0, spriteWidthExplosion, spriteHeightExplosion);
                    }
                    else
                    {
                        explosionTime = false;
                    }
                }

                if (isBeingPushed == true) //If SM is to be pushed back, set the timer to zero
                {
                    movedBackTimer = 0;
                }

                //If SM x position is less than 400, start the move back timer
                if (spMnRect.X < 400)
                {
                    movedBackTimer++;
                }

                if (movedBackTimer >= 120 && spMnRect.X < 400) //After 2 seconds, start moving SM x position back to 400
                {
                    spMnRect.X++;
                    if (spMnRect.X >= 400)
                    {
                        movedBackTimer = 0; //set timer to zero if he is at x position of 400
                    }
                }

                //BARREL STUFF MOVING
                if (spawnTimer >= spawnTime && chosenBarrelOrBomb == 1)
                {
                    barrelSourceRect = new Rectangle(barrelCurrentFrame * spriteWidthBarrel - 12, 0, spriteWidthBarrel, spriteHeightBarrel);

                    if (barrelRect.X < 0) //Basically the idea is to reuse the barrel once it goes off screen. Though the barrel is only called if the variable for choosing Barrel or Bomb is 1
                    {
                        barrelRect.X = 1050;
                        if (score < 3000) //If score is less than 3000, use slower speeds
                        {
                            if (randomSpawner.returnSpawn() == 1)
                            {
                                spawnTime = spawnWhen.Next(220, 260);
                                barrelSpeed = 8;
                            }
                            if (randomSpawner.returnSpawn() == 2)
                            {
                                spawnTime = spawnWhen.Next(160, 200);
                                barrelSpeed = 10;
                            }
                            if (randomSpawner.returnSpawn() == 3)
                            {
                                spawnTime = spawnWhen.Next(100, 140);
                                barrelSpeed = 12;
                            }
                            if (randomSpawner.returnSpawn() == 4)
                            {
                                spawnTime = spawnWhen.Next(40, 80);
                                barrelSpeed = 14;
                            }
                            if (randomSpawner.returnSpawn() == 5)
                            {
                                spawnTime = spawnWhen.Next(30, 60);
                                barrelSpeed = 16;
                            }
                        }
                        else if (score > 3000) //Once the score is past 3000, use faster speeds
                        {
                            if (randomSpawner.returnSpawnHard() == 1)
                            {
                                spawnTime = spawnWhen.Next(20, 50);
                                barrelSpeed = 16;
                            }
                            if (randomSpawner.returnSpawnHard() == 2)
                            {
                                spawnTime = spawnWhen.Next(10, 40);
                                barrelSpeed = 17;
                            }
                            if (randomSpawner.returnSpawnHard() == 3)
                            {
                                spawnTime = spawnWhen.Next(0, 30);
                                barrelSpeed = 18;
                            }
                        }

                        spawnTimer = 0;
                        chosenBarrelOrBomb = barrelOrBomb.Next(1, 3);
                    }

                    if (barrelRect.X > -20)
                    {
                        barrelRect.X -= barrelSpeed; //Move the barrel to the left of the screen
                    }
                }

                //BOMB STUFF MOVING
                if (spawnTimer >= spawnTime && chosenBarrelOrBomb == 2 && bombExploded == false) //The same logic used for barrels is used for bombs
                {
                    bombSourceRect = new Rectangle(bombCurrentFrame * spriteWidthBomb - 12, 0, spriteWidthBomb, spriteHeightBomb); 

                    if (bombRect.X < 0)
                    {
                        bombRect.X = 1020;
                        if (randomSpawner.returnSpawn() == 1)
                        {
                            spawnTime = spawnWhen.Next(220, 260);
                            bombSpeed = 4;
                        }
                        if (randomSpawner.returnSpawn() == 2)
                        {
                            spawnTime = spawnWhen.Next(160, 200);
                            bombSpeed = 6;
                        }
                        if (randomSpawner.returnSpawn() == 3)
                        {
                            spawnTime = spawnWhen.Next(100, 140);
                            bombSpeed = 8;
                        }
                        if (randomSpawner.returnSpawn() == 4)
                        {
                            spawnTime = spawnWhen.Next(40, 80);
                            bombSpeed = 10;
                        }
                        if (randomSpawner.returnSpawn() == 5)
                        {
                            spawnTime = spawnWhen.Next(10, 50);
                            bombSpeed = 12;
                        }
                        spawnTimer = 0;
                        chosenBarrelOrBomb = barrelOrBomb.Next(1, 3);
                    }

                    if (bombRect.X > -20)
                    {
                        bombRect.X -= bombSpeed;
                    }
                }

                //Animates Barrel
                if (barrelTimer == 8)
                {
                    if (barrelCurrentFrame < 4)
                    {
                        barrelCurrentFrame++;
                    }
                    else
                    {
                        barrelCurrentFrame = 1;
                    }
                    barrelTimer = 0;
                }

                //Animated bomb
                if (bombTimer == 8)
                {
                    if (bombCurrentFrame > 1)
                    {
                        bombCurrentFrame--;
                    }
                    else
                    {
                        bombCurrentFrame = 4;
                    }
                    bombTimer = 0;
                }

                //Animate explosion
                if (explosionTimer >= 2 && explosionTime == true)
                {
                    if (explosionCurrentFrame < 20)
                    {
                        explosionCurrentFrame++;
                    }
                    else
                    {
                        explosionTime = false;
                        explosionCurrentFrame = 1;
                    }
                    explosionTimer = 0;
                }
                if (explosionTime == false)
                {
                    explosionCurrentFrame = 1;
                    explosionRect.X = 1000;
                }

                //Increase Timer variables
                timerVenom++;
                explosionTimer++;
                barrelTimer++;
                bombTimer++;
                oldpad1 = pad1;
                oldKeys = keys;
                spawnTimer++;

            }

            //If the left of the Spiderman touches Venom, the player loses
            if (spMnRect.Left <= (venomRunDraw.returnRect().Right - 35))
            {
                isSpiderManDead = true;
                //Options for the player to restart the game by pressing Enter or Start
                if (oldKeys.IsKeyUp(Keys.Enter) && keys.IsKeyDown(Keys.Enter) || oldpad1.Buttons.Start == ButtonState.Released && pad1.Buttons.Start == ButtonState.Pressed) //If the enter key is pressed, start the game
                {
                    isSpiderManDead = false;
                    reset = true;
                }
                //Options for the player to return to main menu by pressing Backspace or Back button
                if (oldKeys.IsKeyUp(Keys.Back) && keys.IsKeyDown(Keys.Back) || oldpad1.Buttons.Back == ButtonState.Released && pad1.Buttons.Back == ButtonState.Pressed) //If enter is pressed, exit the game
                {
                    startGame = false;
                    isSpiderManDead = false;
                    reset = true;
                }

                oldpad1 = pad1;
                oldKeys = keys;
            }
            //If the player chooses to reset the game, set everything back to default value
            if (reset == true)
            {
                score = 0;
                timer = 0;
                timerVenom = 0;
                explosionTimer = 0;
                barrelTimer = 0;
                bombTimer = 0;
                spawnTime = 0;
                spawnTimer = 0;
                movedBackTimer = 0;
                isBeingPushed = false;

                LoadContent(); //Call LoadContent to restart all classes and rectangle variables

                isSpiderManDead = false;
                reset = false;
            }

            //Menu and Starting the game

            //If the game is not running, display the menu
            if (startGame == false)
            {
                selectState = runMenu.displayMenu(selectState, selectUpSE, selectBackSE); //Call menu selecting class (Up and Down on the keyboard to change the selection)

                if (selectState == 1) //If selection is 1 then it is on Start Game
                {
                    playGameColour = Color.Red;
                    howToPlayColour = Color.White;
                    exitColor = Color.White;

                    if (HowToPlayMenu == false) //If how to play is not being displayed, and player is currently on start game, start the game
                    {
                        if (oldKeys.IsKeyUp(Keys.Enter) && keys.IsKeyDown(Keys.Enter) || oldpad1.Buttons.Start == ButtonState.Released && pad1.Buttons.Start == ButtonState.Pressed) //If the enter key is pressed, start the game
                        {
                            startGame = true;
                        }
                    }
                }

                if (selectState == 2) //If the selection is 2 then it is on How to Play
                {
                    playGameColour = Color.White;
                    howToPlayColour = Color.Red;
                    exitColor = Color.White; 
                    //if player presses enter or start, display how to play
                    if (oldKeys.IsKeyUp(Keys.Enter) && keys.IsKeyDown(Keys.Enter) || oldpad1.Buttons.Start == ButtonState.Released && pad1.Buttons.Start == ButtonState.Pressed) //If enter is pressed, exit the game
                    {
                        HowToPlayMenu = true;
                        selectUpSE.Play(); //Play Sound effect to go in to how to play
                    }

                }

                if (HowToPlayMenu == true)
                { //If back is pressed, stop displaying the how to play screen
                    if (oldKeys.IsKeyUp(Keys.Back) && keys.IsKeyDown(Keys.Back) || oldpad1.Buttons.Back == ButtonState.Released && pad1.Buttons.Back == ButtonState.Pressed) //If enter is pressed, exit the game
                    {
                        HowToPlayMenu = false;
                        selectBackSE.Play(); //play sound effect to go out out of how to play
                    }
                }

                if (selectState == 3) //If the selection is 3 then it is on Exit
                {
                    playGameColour = Color.White;
                    howToPlayColour = Color.White;
                    exitColor = Color.Red;

                    if (HowToPlayMenu == false) //If how to play is not being displayed, and player is currently on exit, exit the game
                    {
                        if (oldKeys.IsKeyUp(Keys.Enter) && keys.IsKeyDown(Keys.Enter) || oldpad1.Buttons.Start == ButtonState.Released && pad1.Buttons.Start == ButtonState.Pressed) //If enter is pressed, exit the game
                        {
                            this.Exit();
                        }
                    }
                }
            }

            oldKeys = keys;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            //Draw background
            drawBackground.draw(spriteBatch);
            drawBackground2.draw(spriteBatch);

            if (startGame == false)
            {
                drawTitle.draw(spriteBatch);
            }

            //If the game is running, draw Venom
            if (startGame == true && isSpiderManDead == false)
            {
                if (venomDraw.returnRect().Y < 280)
                {
                    venomDraw.draw(spriteBatch);
                }
                else if (timerVenom < 80)
                {
                    venomDraw2.draw(spriteBatch);
                }
            }
            //If the timer is greater than 80, draw Venom animated
            if (timerVenom > 80 && startGame == true && isSpiderManDead == false)
            {
                venomRunDraw.drawSpriteAnimation(spriteBatch, venomRun2);
            }

            spriteBatch.Begin(); //Draw font stuff onto the screen like menu and high score

            if (startGame == true && isSpiderManDead == false)
            {
                //SPIDERMAN DRAW STUFF
                spriteBatch.Draw(barrelText, barrelRect, barrelSourceRect, Color.White);
                spriteBatch.Draw(bombText, bombRect, bombSourceRect, Color.White);
                spriteBatch.Draw(explosionText, explosionRect, explosionSourceRect, Color.White);

                if (jump == true)
                {
                    spriteBatch.Draw(spMnText, spMnRect, sourceRectJumping, Color.White);
                }

                if (jump == false && ducking == false)
                {
                    spriteBatch.Draw(spMnText, spMnRect, sourceRectRunning, Color.White);
                }

                if (ducking == true)
                {
                    spriteBatch.Draw(spMnText, spMnRect, sourceRectDucking, Color.White);
                }
            }
            //END OF SPIDERMAN DRAW STUFF

            //Draw menu stuff
            if (startGame == false && isSpiderManDead == false)
            {
                spriteBatch.DrawString(font, "START GAME", new Vector2((graphics.PreferredBackBufferWidth / 2) - 65, (graphics.PreferredBackBufferHeight / 2) - 60), playGameColour);
                spriteBatch.DrawString(font, "HOW TO PLAY", new Vector2((graphics.PreferredBackBufferWidth / 2) - 70, (graphics.PreferredBackBufferHeight / 2) - 20), howToPlayColour);
                spriteBatch.DrawString(font, "EXIT", new Vector2((graphics.PreferredBackBufferWidth / 2) - 25, (graphics.PreferredBackBufferHeight / 2) + 20), exitColor);
            }

            if (startGame == true)
            {
                spriteBatch.DrawString(font, "Score: " + score / 4, new Vector2(graphics.PreferredBackBufferWidth/2-65, 0), Color.White);
            }

            if (isSpiderManDead == true)
            {
                spriteBatch.DrawString(font, "GAME OVER", new Vector2((graphics.PreferredBackBufferWidth / 2) - 60, (graphics.PreferredBackBufferHeight / 2) - 60), Color.White);
                spriteBatch.DrawString(font, "PRESS START/ENTER TO RESET", new Vector2((graphics.PreferredBackBufferWidth / 2) - 160, (graphics.PreferredBackBufferHeight / 2) - 20), Color.White);
                spriteBatch.DrawString(font, "PRESS BACK FOR MENU", new Vector2((graphics.PreferredBackBufferWidth / 2) - 125, (graphics.PreferredBackBufferHeight / 2) + 20), Color.White);
            }
            spriteBatch.End();

            //Draw the how to play screen, placed at the end so that it is drawn over top of everything
            if (HowToPlayMenu == true)
            {
                drawHowToPlay.draw(spriteBatch);
            }
            //Draw how to handle the music in the how to play screen
            spriteBatch.Begin();
            if (HowToPlayMenu == true)
            {
                spriteBatch.DrawString(font, "Press Space or X on gamepad change to music", new Vector2((graphics.PreferredBackBufferWidth / 2) - 190, (graphics.PreferredBackBufferHeight / 2) + 35), Color.White);
                spriteBatch.DrawString(font, "Press P or Right Shoulder button to pause music", new Vector2((graphics.PreferredBackBufferWidth / 2) - 190, (graphics.PreferredBackBufferHeight / 2)+80), Color.White);
                spriteBatch.DrawString(font, "Press L or Left Shoulder button to resume music", new Vector2((graphics.PreferredBackBufferWidth / 2) - 190, (graphics.PreferredBackBufferHeight / 2) + 140), Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
