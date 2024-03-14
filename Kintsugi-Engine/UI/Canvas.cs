using System.Numerics;
using Kintsugi.Core;
using Kintsugi.Input;
using SDL2;

namespace Kintsugi.UI;

public class Canvas : GameObject, IInputListener
{
    public List<CanvasObject> Objects { get; set; } = [];
    public int CurrentIndex { get; private set; }
    public CanvasObject CurrentObject => Objects[CurrentIndex];
    public bool Focused { get; set; }
    private CanvasObject? CurrentHovered { get; set; }
    public bool Visible { get; set; } = true;

    public int MoveIndex(int units)
    {
        CurrentIndex = (CurrentIndex + units) % Objects.Count;
        return CurrentIndex;
    }

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
                CanvasObject? localCurrentHovered = null;
                foreach (var canvasObject in Objects)
                {
                    Vector2 pivotOffsetPosition;
                    if (canvasObject.FollowedTileobject is not null)
                    {
                        var cam = Bootstrap.GetCameraSystem();
                        // follow mode
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
                        pivotOffsetPosition =
                            Position
                            + new Vector2(Bootstrap.GetDisplay().GetWidth(), Bootstrap.GetDisplay().GetHeight()) * canvasObject.TargetPivot
                            - (canvasObject.Graphic is null? Vector2.Zero : 
                                canvasObject.Graphic.Properties.ImagePivot * canvasObject.Graphic.Scale);
                    }

                    if (!WithinBounds(
                            new Vector2(inp.X, inp.Y),
                            pivotOffsetPosition + canvasObject.Position,
                            canvasObject.Dimensions * (canvasObject.Graphic?.Scale ?? Vector2.One)
                        )) continue;
                    localCurrentHovered = canvasObject;
                    break;
                }
                if (localCurrentHovered != CurrentHovered)
                {
                    CurrentHovered?.OnHoverEnd();
                    localCurrentHovered?.OnHoverStart();
                    CurrentHovered = localCurrentHovered;
                }

                break;
            }
            case "MouseDown" when inp.Button == SDL.SDL_BUTTON_LEFT:
                CurrentHovered?.OnClick();
                break;
        }
    }

    private static bool WithinBounds(Vector2 vector, Vector2 start, Vector2 span)
        => start.X < vector.X && 
           vector.X < start.X + span.X && 
           start.Y < vector.Y && 
           vector.Y < start.Y + span.Y;
}