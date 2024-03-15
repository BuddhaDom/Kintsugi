using System.Numerics;
using Kintsugi.Core;
using Kintsugi.Input;
using SDL2;

namespace Kintsugi.UI;

/// <summary>
/// A GameObject representing UI elements. The canvas and its <see cref="Objects"/> are rendered onto screen space.
/// </summary>
public class Canvas : GameObject, IInputListener
{
    /// <summary>
    /// Objects existing on this canvas.
    /// </summary>
    public List<CanvasObject> Objects { get; set; } = [];
    /// <summary>
    /// The currently hovered object. 
    /// </summary>
    private CanvasObject? Hovered { get; set; }
    /// <summary>
    /// <c>true</c> if this canvas should be rendered.
    /// </summary>
    public bool Visible { get; set; } = true;

    public override void Update()
    {
        Bootstrap.GetDisplay().DrawCanvas(this);
    }
    
    public void HandleInput(InputEvent inp, string eventType)
    {
        switch (eventType)
        {
            case "MouseMotion":
            {
                CanvasObject? localFocused = null;
                foreach (var canvasObject in Objects)
                {
                    Vector2 pivotOffsetPosition;
                    if (canvasObject.FollowedTileobject is not null)
                    {
                        // Following an object
                        var cam = Bootstrap.GetCameraSystem();
                        pivotOffsetPosition = 
                            cam.WorldToScreenSpace(
                                canvasObject.FollowedTileobject.Easing.CurrentPosition + 
                                (canvasObject.FollowedTileobject.Graphic is null ? Vector2.Zero : 
                                    canvasObject.TargetPivot * canvasObject.FollowedTileobject.Graphic.Properties.Dimensions)
                            )
                            - (canvasObject.Graphic is null? Vector2.Zero : 
                                canvasObject.Graphic.Properties.ImagePivot * canvasObject.Graphic.Scale);
                    }
                    else
                    {
                        // Static on screen
                        pivotOffsetPosition =
                            Position
                            + new Vector2(Bootstrap.GetDisplay().GetWidth(), Bootstrap.GetDisplay().GetHeight()) * canvasObject.TargetPivot
                            - (canvasObject.Graphic is null? Vector2.Zero : 
                                canvasObject.Graphic.Properties.ImagePivot * canvasObject.Graphic.Scale);
                    }

                    if (!WithinBounds(new Vector2(inp.X, inp.Y),
                            pivotOffsetPosition + canvasObject.Position,
                            canvasObject.Dimensions * (canvasObject.Graphic?.Scale ?? Vector2.One)
                        )) continue;
                    localFocused = canvasObject;
                    break;
                }
                if (localFocused != Hovered)
                {
                    Hovered?.OnHoverEnd();
                    localFocused?.OnHoverStart();
                    Hovered = localFocused;
                }
                return;
            }
            case "MouseDown" when inp.Button == SDL.SDL_BUTTON_LEFT:
                Hovered?.OnClick();
                return;
        }
    }

    private static bool WithinBounds(Vector2 vector, Vector2 start, Vector2 span)
        => start.X < vector.X && 
           vector.X < start.X + span.X && 
           start.Y < vector.Y &&    
           vector.Y < start.Y + span.Y;
}