using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input.Touch;

namespace DownwellClone
{
    class UI
    {
        //The textures for both health and arrow UI
        static Texture2D healthFill;

        static Texture2D arrowFill;

        //Segments of health to show
        List<Rectangle> healthvalue = new List<Rectangle>();
        //Segments of arrows to show
        List<Rectangle> arrowvalue = new List<Rectangle>();

        Rectangle currentHealth;

        Rectangle currentArrow;

        Vector2 scale;

        float scalefactor = 2f;

        public UI(GraphicsDevice graphicsDevice, Texture2D textureHealth, Texture2D textureArrow)
        {
            scale = new Vector2(scalefactor, scalefactor);

            healthFill = textureHealth;

            arrowFill = textureArrow;

            //Adding all 6 states of arrow parts of the UI (having 0 to having 5)
            Rectangle arrow5 = new Rectangle(0, 0, 80, 16);

            Rectangle arrow4 = new Rectangle(0, 0, 64, 16);

            Rectangle arrow3 = new Rectangle(0, 0, 48, 16);

            Rectangle arrow2 = new Rectangle(0, 0, 32, 16);

            Rectangle arrow1 = new Rectangle(0, 0, 16, 16);

            Rectangle arrow0 = new Rectangle(0, 0, 0, 16);

            arrowvalue.Add(arrow0);
            arrowvalue.Add(arrow1);
            arrowvalue.Add(arrow2);
            arrowvalue.Add(arrow3);
            arrowvalue.Add(arrow4);
            arrowvalue.Add(arrow5);

            currentArrow = arrowvalue[5];

            //Adding all 4 states of health parts of the UI (0 to 3)
            Rectangle health3 = new Rectangle(0, 0, 96, 16);

            Rectangle health2 = new Rectangle(0, 0, 64, 16);

            Rectangle health1 = new Rectangle(0, 0, 32, 16);

            Rectangle health0 = new Rectangle(0, 0, 0, 16);

            healthvalue.Add(health0);
            healthvalue.Add(health1);
            healthvalue.Add(health2);
            healthvalue.Add(health3);

            currentHealth = healthvalue[3];

        }

        //Get the player values to update the UI
        public void Update(CharacterEntity player)
        {
            currentHealth = healthvalue[player.Health];

            currentArrow = arrowvalue[player.Ammo];
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Color tintColor = Color.WhiteSmoke;
            Vector2 drawPosHealth = new Vector2(114, 840 - healthFill.Height);
            Vector2 drawPosArrow = new Vector2(114, 840 - healthFill.Height - arrowFill.Height * 2);

            spriteBatch.Draw(arrowFill, position: drawPosArrow, color: tintColor, sourceRectangle: currentArrow, scale: scale);
            spriteBatch.Draw(healthFill, position: drawPosHealth, color: tintColor, sourceRectangle: currentHealth, scale: scale);
        }
    }
}
