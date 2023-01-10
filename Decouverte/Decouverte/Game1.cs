using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;

namespace Decouverte
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _texturePereNoel;
        private Vector2 _positionPereNoel;
        private Vector2[] _LespositionsCadeaux = new Vector2[NB_CADEAU];
        private Texture2D _textureCadeau;
        private Rectangle[] _positionCadeauRect = new Rectangle[NB_CADEAU];
        private Texture2D _textureEtoile;
        private Vector2 _positionEtoile;
        private KeyboardState _keyboardState;
        private int _sensPereNoel;
        private int _vitessePereNoel;
        private int _vitesseCadeau;
        private int _score;
        private SpriteFont _police;
        private Vector2 _positionScore;
        private Vector2 _positionChrono;
        private Rectangle _rectanglePereNoel;
        private Rectangle _rectangleEtoile;
        private MouseState _mouseState;
        private float _chrono;
        private float _compteur;
        private String _rejouer;
        private bool etoile = false;
        private static int _acceleration;
        private bool pause = false;
        public Random aleatoire = new Random();
        public const int LARGEUR_PERE_NOEL = 200;
        public const int HAUTEUR_PERE_NOEL = 154;
        public const int TAILLE_FENETRE=800;
        public const int NB_CADEAU=10;

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
            _positionPereNoel = new Vector2(TAILLE_FENETRE/2-LARGEUR_PERE_NOEL/2, TAILLE_FENETRE - HAUTEUR_PERE_NOEL);
            _positionEtoile = new Vector2(-TAILLE_FENETRE, - TAILLE_FENETRE);
            _rectangleEtoile.X = (int)_positionEtoile.X;
            _rectangleEtoile.Y = (int)_positionEtoile.Y;
            _rectangleEtoile.Width =30;
            _rectangleEtoile.Height = 29;
            _acceleration = 50;
            _rectanglePereNoel.X = (int)_positionPereNoel.X;
            _rectanglePereNoel.Y = (int)_positionPereNoel.Y;
            _rectanglePereNoel.Width = LARGEUR_PERE_NOEL;
            _rectanglePereNoel.Height = HAUTEUR_PERE_NOEL;
            _graphics.PreferredBackBufferWidth = TAILLE_FENETRE;
            _graphics.PreferredBackBufferHeight = TAILLE_FENETRE;
            _graphics.ApplyChanges();
            _vitessePereNoel = 250;
            _score = 0;
            _chrono = 60;
            _compteur = 0;
            _positionChrono = new Vector2(TAILLE_FENETRE-TAILLE_FENETRE*0.05f, 0);
            _vitesseCadeau = 200;
            _positionScore = new Vector2(0, 0);
            for(int i = 0; i < NB_CADEAU; i++)
            {
                _LespositionsCadeaux[i] = new Vector2(aleatoire.Next(0, GraphicsDevice.Viewport.Width - 50), aleatoire.Next(-GraphicsDevice.Viewport.Width - 50,0));
                _positionCadeauRect[i].X = (int)_LespositionsCadeaux[i].X;
                _positionCadeauRect[i].Y = (int)_LespositionsCadeaux[i].Y;
                _positionCadeauRect[i].Width = 50;
                _positionCadeauRect[i].Height = 57;
            }
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
            _texturePereNoel = Content.Load<Texture2D>("bunny_droite");
            _textureCadeau = Content.Load<Texture2D>("cadeau");
            _textureEtoile = Content.Load<Texture2D>("etoile");

            //MAP
            _textureMap = Content.Load<Texture2D>("Underground");


            _police = Content.Load<SpriteFont>("Font");
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

                    //si on clique avec la souris
                    if (_mouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (_rectangleEtoile.Contains(_mouseState.X, _mouseState.Y))
                        {
                            _vitesseCadeau += 50;
                            _vitessePereNoel += 50;
                            _positionEtoile = new Vector2(-TAILLE_FENETRE, -TAILLE_FENETRE);
                            _rectangleEtoile.X = (int)_positionEtoile.X;
                            _rectangleEtoile.Y = (int)_positionEtoile.Y;
                            _songEtoile.Play();
                        }
                    }

                    // si fleche droite
                    if (_keyboardState.IsKeyDown(Keys.Right) && !(_keyboardState.IsKeyDown(Keys.Left)) && _positionPereNoel.X < TAILLE_FENETRE - LARGEUR_PERE_NOEL)
                    {
                        _sensPereNoel = 1;
                        _positionPereNoel.X += _sensPereNoel * _vitessePereNoel * deltaTime;
                        _rectanglePereNoel.X = (int)_positionPereNoel.X;
                        _rectanglePereNoel.Y = (int)_positionPereNoel.Y;
                        _texturePereNoel = Content.Load<Texture2D>("bunny_droite");
                    }
                    if (_keyboardState.IsKeyDown(Keys.Left) && !(_keyboardState.IsKeyDown(Keys.Right)) && _positionPereNoel.X > 0)
                    {
                        _sensPereNoel = -1;
                        _positionPereNoel.X += _sensPereNoel * _vitessePereNoel * deltaTime;
                        _rectanglePereNoel.X = (int)_positionPereNoel.X;
                        _rectanglePereNoel.Y = (int)_positionPereNoel.Y;
                        _texturePereNoel = Content.Load<Texture2D>("bunny_gauche");
                    }
                    for (int i = 0; i < NB_CADEAU; i++)
                    {
                        _LespositionsCadeaux[i].Y += _vitesseCadeau * deltaTime;
                        _positionCadeauRect[i].X = (int)_LespositionsCadeaux[i].X;
                        _positionCadeauRect[i].Y = (int)_LespositionsCadeaux[i].Y;
                    }
                    for (int i = 0; i < NB_CADEAU; i++)
                    {
                        if (_LespositionsCadeaux[i].Y > TAILLE_FENETRE)
                        {
                            _LespositionsCadeaux[i] = new Vector2(aleatoire.Next(0, GraphicsDevice.Viewport.Width - 50), 0);
                            _positionCadeauRect[i].X = (int)_LespositionsCadeaux[i].X;
                            _positionCadeauRect[i].Y = (int)_LespositionsCadeaux[i].Y;
                            _songCadeauP.Play();
                        }
                    }
                    for (int i = 0; i < NB_CADEAU; i++)
                    {
                        if (_positionCadeauRect[i].Intersects(_rectanglePereNoel))
                        {
                            _LespositionsCadeaux[i] = new Vector2(aleatoire.Next(0, GraphicsDevice.Viewport.Width - 50), 0);
                            _positionCadeauRect[i].X = (int)_LespositionsCadeaux[i].X;
                            _positionCadeauRect[i].Y = (int)_LespositionsCadeaux[i].Y;
                            _score += 1;
                            _vitesseCadeau += 1;
                            _vitessePereNoel += 1;
                            _songCadeauG.Play();
                        }
                    }

                    //chrono
                    _chrono = _chrono - deltaTime;

                    //compteur
                    _compteur += deltaTime;
                    //si le compteur depasse 10 sec
                    if (_compteur >= 10)
                    {
                        etoile = true;
                        _compteur = 0;
                        _positionEtoile = new Vector2(aleatoire.Next(0, GraphicsDevice.Viewport.Width - 50), aleatoire.Next(0, GraphicsDevice.Viewport.Width - 50));
                        _rectangleEtoile.X = (int)_positionEtoile.X;
                        _rectangleEtoile.Y = (int)_positionEtoile.Y;
                    }

                    //si l'étoile est apparue depuis plus de 2sec
                    if (_compteur > 2 && etoile == true)
                    {
                        etoile = false;
                        _positionEtoile = new Vector2(-TAILLE_FENETRE, -TAILLE_FENETRE);
                        _rectangleEtoile.X = (int)_positionEtoile.X;
                        _rectangleEtoile.Y = (int)_positionEtoile.Y;
                    }

                    //si on appuie sur space = pause
                    if (_keyboardState.IsKeyDown(Keys.Space))
                    {
                        pause = true;
                    }
                }
                
                //si on appuie sur entrer = pas pause
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
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.MidnightBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            _spriteBatch.Draw(_texturePereNoel, _positionPereNoel, Color.White);
            
            for(int i = 0; i < NB_CADEAU; i++)
            {
                _spriteBatch.Draw(_textureCadeau, _LespositionsCadeaux[i], Color.White);
            }
            _spriteBatch.DrawString(_police, $"Score : {_score}", _positionScore, Color.White);
            _spriteBatch.DrawString(_police, $"{(float)Math.Round(_chrono,0)}", _positionChrono, Color.White);
            _spriteBatch.DrawString(_police, _rejouer, new Vector2(0, 50), Color.White);
            _spriteBatch.Draw(_textureEtoile, _positionEtoile, Color.White);
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public interface IBackground
        {
            void Update(Rectangle screenRectangle);
            void Draw(SpriteBatch spriteBatch);
        }
    }
}