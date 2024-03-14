using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Kintsugi.Objects.Graphics
{
    public abstract class GraphicsObject
    {
        /// <summary>
        /// Graphic properties of this object.
        /// </summary>
        public ISpriteable? Graphic { get; protected set; }

        /// <summary>
        /// Set the sprite properties for this object.
        /// </summary>
        /// <param name="path">Path to the sprite's graphic.</param>
        /// <param name="tilePivot">
        /// Position on the tile from which the object is rendered. Defined between <see cref="Vector2.Zero"/> and
        /// <see cref="Vector2.One"/> as the upper and lower bounds of the tile width. </param>
        /// <param name="imagePivot">
        /// Position on the sprite which will match positions with the <paramref name="tilePivot"/>.
        /// Defined between <see cref="Vector2.Zero"/> the pixel width and height of the sprite.</param>
        public void SetSpriteSingle(string path, Vector2 tilePivot = default, Vector2 imagePivot = default)
        {
            SetSpriteSingle(new Sprite(path));
            Graphic.Properties.TilePivot = tilePivot;
            Graphic.Properties.ImagePivot = imagePivot;
            Console.WriteLine(Graphic.Properties.TilePivot + "AND" + tilePivot);

            Console.WriteLine(Graphic.Properties.ImagePivot +"AND" + imagePivot);
        }

        /// <summary>
        /// Set a single sprite graphic to this object.
        /// </summary>
        /// <param name="sprite">Image form which to take the sprite.</param>
        public void SetSpriteSingle(Sprite sprite)
        {
            if (Graphic is SpriteSingle spriteSingle)
                spriteSingle.Sprite = sprite;
            else
            {
                Graphic = new SpriteSingle();
                ((SpriteSingle)Graphic).Sprite = sprite;
            }
        }

        /// <summary>
        /// Set a single sprite graphic to this object.
        /// </summary>
        /// <param name="spriteSingle">Sprite graphic to copy from.</param>
        public void SetSpriteSingle(SpriteSingle spriteSingle)
            => SetSpriteSingle(spriteSingle.Sprite);

        /// <summary>
        /// Sets an animation graphic for this object.
        /// </summary>
        /// <param name="spriteSheet">Sprite sheet from which to take frames.</param>
        /// <param name="timeLength">Duration of the animation.</param>
        /// <param name="frames">Frame indexes in the sprite sheet that make up the animation.</param>
        /// <param name="repeats">Amount of repetitions. Set to 0 if it should loop indefinitely.</param>
        /// <param name="bounces">Determines if the animation plays front then back once it reaches its last frame.</param>
        /// <param name="autoStart">Start the animation once this method ends?</param>
        public void SetAnimation(SpriteSheet spriteSheet, double timeLength, IEnumerable<int> frames,
            int repeats = 0, bool bounces = false, bool autoStart = true)
        {
            if (Graphic is Animation animation)
            {
                animation.SpriteSheet = spriteSheet;
                animation.TimeLength = timeLength;
                animation.FrameIndexes = (IReadOnlyList<int>)frames;
                animation.Repeats = repeats;
                animation.Bounces = bounces;
            }
            else
                Graphic = new Animation(timeLength, spriteSheet, frames, repeats, bounces); ;
            if (autoStart) ((Animation)Graphic).Start();
        }

        /// <summary>
        /// Sets an animation graphic for this object.
        /// </summary>
        /// <param name="path">Location of the image containing a sprite sheet.</param>
        /// <param name="spriteHeight">Height of a sprite.</param>
        /// <param name="spriteWidth">Width of a sprite.</param>
        /// <param name="spritesPerRow">Maximum amount of indexes in one row within sprite sheet.</param>
        /// <param name="timeLength">Duration of the animation.</param>
        /// <param name="frames">Frame indexes in the sprite sheet that make up the animation.</param>
        /// <param name="tilePivot">Pivot of the sprite. relative to its tile.</param>
        /// <param name="imagePivot">Pivot of the sprites relative to their image.</param>
        /// <param name="padding">Separation between sprite indexes.</param>
        /// <param name="margin">Separation between first sprite index and its image borders.</param>
        /// <param name="repeats">Amount of repetitions. Set to 0 if it should loop indefinitely.</param>
        /// <param name="bounces">Determines if the animation plays front then back once it reaches its last frame.</param>
        /// <param name="autoStart">Start the animation once this method ends?</param>
        public void SetAnimation(string path, int spriteHeight, int spriteWidth, int spritesPerRow, double timeLength,
            IEnumerable<int> frames, Vector2 tilePivot = default, Vector2 imagePivot = default,
            Vector2 padding = default, Vector2 margin = default, int repeats = 0, bool bounces = false,
            bool autoStart = true)
            => SetAnimation(
                new SpriteSheet(path, spriteHeight, spriteWidth, spritesPerRow, tilePivot, imagePivot, padding, margin),
                timeLength, frames, repeats, bounces, autoStart
            );

        /// <summary>
        /// Sets an animation graphic for this object.
        /// </summary>
        /// <param name="animation">Animation from which to copy properties</param>
        /// <param name="autoStart">Start the animation once this method ends?</param>
        public void SetAnimation(Animation animation, bool autoStart = true)
            => SetAnimation(animation.SpriteSheet, animation.TimeLength, animation.FrameIndexes, animation.Repeats,
                animation.Bounces, autoStart);
    }
}
