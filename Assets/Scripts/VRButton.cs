using UnityEngine.UI;
using UnityEngine;

public class VRButton : VRObject
{
    public Color normalColor    = Color.white;
    public Color hoverColor     = new Color32(156, 244, 167, 255);
    public Image targetImage;
    public SpriteRenderer targetRender;
    public bool onlyLeftCursorCanClick;

    public override void enterHover(Cursor c)
    {
        base.enterHover(c);
        if(onlyLeftCursorCanClick)
        {
            if(c == RightCursor.instance)
            {
                return;
            }
        }
        if (targetImage)
        {
            targetImage.color = hoverColor;
        }
        if (targetRender)
        {
            targetRender.color = hoverColor;
        }
    }

    public override void exitHover(Cursor c)
    {
        if (onlyLeftCursorCanClick)
        {
            if (c == RightCursor.instance)
            {
                return;
            }
        }
        if (targetImage)
        {
            targetImage.color = normalColor;
        }
        if (targetRender)
        {
            targetRender.color = normalColor;
        }
        base.exitHover(c);
    }
}
