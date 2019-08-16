using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DownwellClone
{
    public class AnimationFrame
    {
        /// <summary>
        /// Displayed area of spritesheet
        /// </summary>
        public Rectangle SourceRectangle { get; set; }

        /// <summary>
        /// Duration of an animation frame
        /// </summary>
        public TimeSpan Duration { get; set; }
    }
}
