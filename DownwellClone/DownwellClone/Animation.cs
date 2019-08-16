using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace DownwellClone
{
    public class Animation
    {

        /// <summary>
        /// List of frames to display
        /// </summary>
        List<AnimationFrame> frames = new List<AnimationFrame>();
        /// <summary>
        /// Keeps track of how much time the animation has been running for
        /// </summary>
        TimeSpan timeIntoAnimation;

        /// <summary>
        /// Adds the duration of all frames from list to calculate duration of animation
        /// </summary>
        TimeSpan Duration
        {
            get
            {
                double totalSeconds = 0;
                foreach (var frame in frames)
                {
                    totalSeconds += frame.Duration.TotalSeconds;
                }

                return TimeSpan.FromSeconds(totalSeconds);
            }
        }
        /// <summary>
        /// Adds new frame to list with rectangle as size and duration of frame
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="duration"></param>
        public void AddFrame(Rectangle rectangle, TimeSpan duration)
        {
            AnimationFrame newFrame = new AnimationFrame()
            {
                SourceRectangle = rectangle,
                Duration = duration
            };

            frames.Add(newFrame);
        }

        /// <summary>
        /// Compares elapsed game time to animation time to calculate how long the animation still has to play until end, then optionally loops it
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="loop"></param>
        public void Update(GameTime gameTime, bool loop)
        {
            double secondsIntoAnimation =
                timeIntoAnimation.TotalSeconds + gameTime.ElapsedGameTime.TotalSeconds;

            if (loop)
            {
                double remainder = secondsIntoAnimation % Duration.TotalSeconds;

                timeIntoAnimation = TimeSpan.FromSeconds(remainder);
            }
            else
            {
                //do something
            }
            
        }

        public Rectangle CurrentRectangle
        {
            get
            {
                AnimationFrame currentFrame = null;

                // See if we can find the frame
                TimeSpan accumulatedTime = new TimeSpan();
                foreach (var frame in frames)
                {
                    if (accumulatedTime + frame.Duration >= timeIntoAnimation)
                    {
                        currentFrame = frame;
                        break;
                    }
                    else
                    {
                        accumulatedTime += frame.Duration;
                    }
                }

                // If no frame was found, then try the last frame, 
                // just in case timeIntoAnimation somehow exceeds Duration
                if (currentFrame == null)
                {
                    currentFrame = frames.LastOrDefault();
                }

                // If we found a frame, return its rectangle, otherwise
                // return an empty rectangle (one with no width or height)
                if (currentFrame != null)
                {
                    return currentFrame.SourceRectangle;
                }
                else
                {
                    return Rectangle.Empty;
                }
            }
        }
    }
}
