using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace Decouverte
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public const int TAILLE_FENETRE = 1000;

        //RANDOM
        public Random aleatoire = new Random();

        //CARROTTES ALEATOIRE DANS CHAQUE ETAGE
        List<Rectangle> _etage1 = new List<Rectangle>(HAUTEUR_LADDER * 1 / 3);
        List<Rectangle> _etage2 = new List<Rectangle>(HAUTEUR_LADDER * 1 / 3);
        List<Rectangle> _etage3 = new List<Rectangle>(HAUTEUR_LADDER * 2 / 3);


        //LAPIN
        public const int LARGEUR_LAPIN = 200;
        public const int HAUTEUR_LAPIN = 154;
        private Texture2D _textureLapin;
        private Vector2 _positionLapin;
        private int _sensLapin;
        private int _vitesseLapin;

        //CARROT
        public const int LARGEUR_CARROT = 65;
        public const int HAUTEUR_CARROT = 150;
        private Vector2[] _LespositionCarottes = new Vector2[NB_CARROT];
        private Rectangle[] _positionCarotteRect = new Rectangle[NB_CARROT];
        private Texture2D _textureCarotte;
        private int _vitesseCarotte;
        public const int NB_CARROT = 10;

        //ESCALIER
        public const int LARGEUR_LADDER = 222;
        public const int HAUTEUR_LADDER = 492;
        public const int LARGEUR_LADDERSKY = 566;
        public const int HAUTEUR_LADDERSKY = 702;
        private Texture2D _textureLadder;
        private Texture2D _textureLadderSky;
        private Vector2 _positionLadderLeft;
        private Vector2 _positionLadderMilieu;
        private Vector2 _positionLadderRight;


        //ETOILE
        private Texture2D _textureEtoile;
        private Vector2 _positionEtoile;
        private bool etoile = false;

        //COLLISIONS
        private Rectangle _rectangleEtoile;
        private Rectangle _rectangleLapin;

        private KeyboardState _keyboardState;
        private MouseState _mouseState;

        //SCORE
        private int _score;
        private SpriteFont _police;
        private Vector2 _positionScore;
        private Vector2 _positionChrono;


        //CHRONO
        private float _chrono;
        private float _compteur;

        //PAUSE
        private bool pause = false;
        private String _rejouer;

        //ACCELERATION
        private static int _acceleration;

        //SON
        private SoundEffect _rabbitEat;
        private SoundEffect _rabbitLose;
        private SoundEffect _songEtoile;
        private Song _musique;

        //MAP
        private Texture2D _textureMap;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //TAILLE FENETRE
            _graphics.PreferredBackBufferWidth = TAILLE_FENETRE;
            _graphics.PreferredBackBufferHeight = TAILLE_FENETRE;
            Window.Title = "Bouing";

            //LAPIN
            _positionLapin = new Vector2(0, TAILLE_FENETRE - HAUTEUR_LAPIN * 3 / 2);
            _rectangleLapin.X = (int)_positionLapin.X;
            _rectangleLapin.Y = (int)_positionLapin.Y;
            _rectangleLapin.Width = LARGEUR_LAPIN;
            _rectangleLapin.Height = HAUTEUR_LAPIN;

            _vitesseLapin = 250;
            _vitesseCarotte = 200;

            //ETOILE
            _positionEtoile = new Vector2(-TAILLE_FENETRE, -TAILLE_FENETRE);
            _rectangleEtoile.X = (int)_positionEtoile.X;
            _rectangleEtoile.Y = (int)_positionEtoile.Y;
            _rectangleEtoile.Width = 30;
            _rectangleEtoile.Height = 29;

            //ECHELLE
            _positionLadderLeft = new Vector2(TAILLE_FENETRE - LARGEUR_LADDER, -HAUTEUR_LADDER * 1 / 4);
            _positionLadderMilieu = new Vector2(0, HAUTEUR_LADDER * 1 / 3);
            _positionLadderRight = new Vector2(TAILLE_FENETRE - LARGEUR_LADDER, TAILLE_FENETRE / 2);



            _acceleration = 50;
            _graphics.ApplyChanges();

            //SCORE
            _score = 0;
            _positionScore = new Vector2(0, 0);

            //CHRONO
            _chrono = 60;
            _positionChrono = new Vector2(TAILLE_FENETRE - TAILLE_FENETRE * 0.05f, 0);

            //CHRONO
            _compteur = 0;

            for (int i = 0; i < NB_CARROT; i++)
            {
                _LespositionCarottes[i] = new Vector2(aleatoire.Next(0, GraphicsDevice.Viewport.Width - 50), aleatoire.Next(-GraphicsDevice.Viewport.Width - 50, 0));
                _positionCarotteRect[i].X = (int)_LespositionCarottes[i].X;
                _positionCarotteRect[i].Y = (int)_LespositionCarottes[i].Y;
                _positionCarotteRect[i].Width = 50;
                _positionCarotteRect[i].Height = 57;
            }
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here

            //TEXTURE
            _textureLapin = Content.Load<Texture2D>("bunny_droite");
            _textureCarotte = Content.Load<Texture2D>("carrot");
            _textureEtoile = Content.Load<Texture2D>("etoile");
            _textureLadder = Content.Load<Texture2D>("LadderS");
            _textureLadderSky = Content.Load<Texture2D>("LadderSky");

            //MAP
            _textureMap = Content.Load<Texture2D>("Underground");

            //FONT
            _police = Content.Load<SpriteFont>("Font");

            //MUSIC
            _musique = Content.Load<Song>("musique");
            _rabbitEat = Content.Load<SoundEffect>("rabbitEat");
            _rabbitLose = Content.Load<SoundEffect>("rabbitLose");
            _songEtoile = Content.Load<SoundEffect>("sonEtoile");
            MediaPlayer.Play(_musique);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _keyboardState = Keyboard.GetState();
            _mouseState = Mouse.GetState();

            // TODO: Add your update logic here

            if (_chrono >= 0)
            {
                if (!pause)
                {
                    _rejouer = "";
                    // deltaTime stocke le temps écoulé entre 2 tours de boucles
                    // permet de rationaliser le deplacement en pixel en fonction du temps écoulé

                    //SI ON CLIQUE AVEC LA SOURIS
                    if (_mouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (_rectangleEtoile.Contains(_mouseState.X, _mouseState.Y))
                        {
                            _vitesseCarotte += 50;
                            _vitesseLapin += 50;
                            _positionEtoile = new Vector2(-TAILLE_FENETRE, -TAILLE_FENETRE);
                            _rectangleEtoile.X = (int)_positionEtoile.X;
                            _rectangleEtoile.Y = (int)_positionEtoile.Y;
                            _songEtoile.Play();
                        }
                    }

                    // SI FLECHE DROITE
                    if (_keyboardState.IsKeyDown(Keys.Right) && !(_keyboardState.IsKeyDown(Keys.Left)) && _positionLapin.X < TAILLE_FENETRE - LARGEUR_LAPIN)
                    {
                        _sensLapin = 1;
                        _positionLapin.X += _sensLapin * _vitesseLapin * deltaTime;
                        _rectangleLapin.X = (int)_positionLapin.X;
                        _rectangleLapin.Y = (int)_positionLapin.Y;
                        _textureLapin = Content.Load<Texture2D>("bunny_droite");
                    }
                    if (_keyboardState.IsKeyDown(Keys.Left) && !(_keyboardState.IsKeyDown(Keys.Right)) && _positionLapin.X > 0)
                    {
                        _sensLapin = -1;
                        _positionLapin.X += _sensLapin * _vitesseLapin * deltaTime;
                        _rectangleLapin.X = (int)_positionLapin.X;
                        _rectangleLapin.Y = (int)_positionLapin.Y;
                        _textureLapin = Content.Load<Texture2D>("bunny_gauche");
                    }
                    for (int i = 0; i < NB_CARROT; i++)
                    {
                        _LespositionCarottes[i].Y += _vitesseCarotte * deltaTime;
                        _positionCarotteRect[i].X = (int)_LespositionCarottes[i].X;
                        _positionCarotteRect[i].Y = (int)_LespositionCarottes[i].Y;
                    }
                    for (int i = 0; i < NB_CARROT; i++)
                    {
                        if (_LespositionCarottes[i].Y > TAILLE_FENETRE)
                        {
                            _LespositionCarottes[i] = new Vector2(aleatoire.Next(0, GraphicsDevice.Viewport.Width - 50), 0);
                            _positionCarotteRect[i].X = (int)_LespositionCarottes[i].X;
                            _positionCarotteRect[i].Y = (int)_LespositionCarottes[i].Y;
                            _rabbitLose.Play();
                        }
                    }
                    for (int i = 0; i < NB_CARROT; i++)
                    {
                        if (_positionCarotteRect[i].Intersects(_rectangleLapin))
                        {
                            _LespositionCarottes[i] = new Vector2(aleatoire.Next(0, GraphicsDevice.Viewport.Width - 50), 0);
                            _positionCarotteRect[i].X = (int)_LespositionCarottes[i].X;
                            _positionCarotteRect[i].Y = (int)_LespositionCarottes[i].Y;
                            _score += 1;
                            _vitesseCarotte += 1;
                            _vitesseLapin += 1;
                            _rabbitEat.Play();
                        }
                    }

                    //CHRONO
                    _chrono = _chrono - deltaTime;

                    //COMPTEUR
                    _compteur += deltaTime;

                    //SI LE COMPTEUR DEPASSE
                    if (_compteur >= 10)
                    {
                        etoile = true;
                        _compteur = 0;
                        _positionEtoile = new Vector2(aleatoire.Next(0, GraphicsDevice.Viewport.Width - 50), aleatoire.Next(0, GraphicsDevice.Viewport.Width - 50));
                        _rectangleEtoile.X = (int)_positionEtoile.X;
                        _rectangleEtoile.Y = (int)_positionEtoile.Y;
                    }

                    //L'ÉTOILE EST APPARUE
                    if (_compteur > 2 && etoile == true)
                    {
                        etoile = false;
                        _positionEtoile = new Vector2(-TAILLE_FENETRE, -TAILLE_FENETRE);
                        _rectangleEtoile.X = (int)_positionEtoile.X;
                        _rectangleEtoile.Y = (int)_positionEtoile.Y;
                    }

                    //SI ON APPUIE SUR SPACE => PAUSE
                    if (_keyboardState.IsKeyDown(Keys.Space))
                    {
                        pause = true;
                    }
                }

                //SI ON APPUIE SUR ENTER => PAS PAUSE
                if (_keyboardState.IsKeyDown(Keys.Enter))
                {
                    pause = false;
                }
            }
            else
            {
                _rejouer = "P pour rejouer ou Q pour quitter";
                if (_keyboardState.IsKeyDown(Keys.P))
                {
                    Initialize();
                }
                else if (_keyboardState.IsKeyDown(Keys.Q))
                {
                    Exit();
                }
            }

            //LAPIN ET ÉCHELLE
            Rectangle _rectLapin = new Rectangle((int)_positionLapin.X, (int)_positionLapin.Y, LARGEUR_LAPIN, HAUTEUR_LAPIN);
            Rectangle _rectLadderMilieu = new Rectangle((int)_positionLadderMilieu.X, (int)_positionLadderMilieu.Y, LARGEUR_LADDER, HAUTEUR_LADDER);
            Rectangle _rectLadderRight = new Rectangle((int)_positionLadderRight.X, (int)_positionLadderRight.Y, LARGEUR_LADDER, HAUTEUR_LADDER);


            //POUR MONTER SUR ÉCHELLE
            if ((_rectLapin.Intersects(_rectLadderRight) || _rectLapin.Intersects(_rectLadderMilieu)) && _keyboardState.IsKeyDown(Keys.Up))
            {
                _positionLapin.Y -= 5;
            }
            
            //POUR DESCENDRE DE ÉCHELLE
            if ((_rectLapin.Intersects(_rectLadderRight) || _rectLapin.Intersects(_rectLadderMilieu)) && _keyboardState.IsKeyDown(Keys.Down))
            {
                _positionLapin.Y += 5;
            }

            //CARROTTES ALEATOIRE DANS CHAQUE ETAGE
            int _positionCarrotX = aleatoire.Next(0, TAILLE_FENETRE - LARGEUR_CARROT);
            int _positionCarrotY = aleatoire.Next(0, TAILLE_FENETRE - HAUTEUR_CARROT);
            Rectangle _rectCarrot = new Rectangle((int)_positionCarrotX, (int)_positionCarrotY, LARGEUR_CARROT, HAUTEUR_CARROT);

            //if (_compteur > 2)
            //{
            //    while (pause)
            //    {
            //        _etage1.Add(_rectCarrot);
            //        _etage2.Add(_rectCarrot);
            //        _etage3.Add(_rectCarrot);
            //    }

            //}



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            _spriteBatch.Draw(_textureMap, new Rectangle(0, 0, TAILLE_FENETRE, TAILLE_FENETRE), Color.White);
            _spriteBatch.Draw(_textureLadder, _positionLadderRight, Color.White);
            _spriteBatch.Draw(_textureLadder, _positionLadderMilieu, Color.White);
            _spriteBatch.Draw(_textureLapin, _positionLapin, Color.White);
            //_spriteBatch.Draw(_textureLadderSky, _positionLadderLeft, Color.White);


            //APPARAITRE PLUSIEURS CARROTTES
            for (int i = 0; i < NB_CARROT; i++)
            {
                _spriteBatch.Draw(_textureCarotte, _LespositionCarottes[i], Color.White);
            }
            _spriteBatch.DrawString(_police, $"Score : {_score}", _positionScore, Color.White);
            _spriteBatch.DrawString(_police, $"{(float)Math.Round(_chrono, 0)}", _positionChrono, Color.White);
            _spriteBatch.DrawString(_police, _rejouer, new Vector2(0, 50), Color.White);
            _spriteBatch.Draw(_textureEtoile, _positionEtoile, Color.White);
            _spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}