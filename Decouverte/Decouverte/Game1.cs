using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace Decouverte
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;

        public const int TAILLE_FENETRE = 800;

        //RANDOM
        public Random aleatoire = new Random();

        //LAPIN
        public const int LARGEUR_LAPIN = 200;
        public const int HAUTEUR_LAPIN = 154;
        private SpriteBatch _spriteBatch;
        private Texture2D _textureLapin;
        private Vector2 _positionLapin;
        private int _sensLapin;
        private int _vitesseLapin;

        //CARROT
        private Vector2[] _LespositionCarottes = new Vector2[NB_CARROT];
        private Rectangle[] _positionCarotteRect = new Rectangle[NB_CARROT];
        private Texture2D _textureCarotte;
        private int _vitesseCarotte;
        public const int NB_CARROT = 10;

        //ESCALIER
        private Vector2 _positionEscalier;

        //ETOILE
        private Texture2D _textureEtoile;
        private Vector2 _positionEtoile;
        private bool etoile = false;

        private KeyboardState _keyboardState;
        private MouseState _mouseState;

        //SCORE
        private int _score;
        private SpriteFont _police;
        private Vector2 _positionScore;
        private Vector2 _positionChrono;

        private Rectangle _rectangleLapin;
        private Rectangle _rectangleEtoile;

        //CHRONO
        private float _chrono;
        private float _compteur;

        //PAUSE
        private bool pause = false;
        private String _rejouer;

        //ACCELERATION
        private static int _acceleration;

        //SON
        private SoundEffect _songCadeauG;
        private SoundEffect _songCadeauP;
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
            
            //LAPIN
            _positionLapin = new Vector2(TAILLE_FENETRE / 2 - LARGEUR_LAPIN / 2, TAILLE_FENETRE - HAUTEUR_LAPIN);
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
            _textureCarotte = Content.Load<Texture2D>("cadeau");
            _textureEtoile = Content.Load<Texture2D>("etoile");

            //MAP
            _textureMap = Content.Load<Texture2D>("Underground");

            //FONT
            _police = Content.Load<SpriteFont>("Font");

            //MUSIC
            _musique = Content.Load<Song>("musique");
            _songCadeauG = Content.Load<SoundEffect>("cadeauGagné");
            _songCadeauP = Content.Load<SoundEffect>("cadeauPerdu");
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
                            _songCadeauP.Play();
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
                            _songCadeauG.Play();
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
                _rejouer = "Appuyez sur la touche p pour rejouer ou q pour quitter";
                if (_keyboardState.IsKeyDown(Keys.P))
                {
                    Initialize();
                }
                else if (_keyboardState.IsKeyDown(Keys.Q))
                {
                    Exit();
                }
            }

            //LAPIN ET ESCALIER
            Rectangle _rectLapin = new Rectangle((int)_positionLapin.X, (int)_positionLapin.Y, LARGEUR_LAPIN, HAUTEUR_LAPIN);
            Rectangle _rectEscalier = new Rectangle();

            if (_rectLapin.Intersects(_rectEscalier))
            {
                _positionLapin.Y += _vitesseLapin;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            _spriteBatch.Draw(_textureMap, new Rectangle(0, 0, TAILLE_FENETRE, TAILLE_FENETRE), Color.White);
            _spriteBatch.Draw(_textureLapin, _positionLapin, Color.White);

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