using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Decouverte;
using Microsoft.Xna.Framework.Graphics;
using static Decouverte.Game1;

namespace Decouverte
{
    public class FixedBackground : IBackground
    {
        private Texture2D _textureMap;
        private Rectangle _screenRectangle;

        public FixedBackground(Texture2D texture)
        {
            _textureMap = texture;
        }

        
        public void Update(Rectangle screenRectangle)
        {
            _screenRectangle = screenRectangle;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_textureMap, new Rectangle(0, 0, _screenRectangle.Width, _screenRectangle.Height), Color.White);
        }

        
    }
}
