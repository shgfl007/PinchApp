using UnityEngine;
using System.Collections;

public class NavigationButton : VRButton
{
    public Sprite rotationSprite;
    public Sprite movementSprite;

    public override void start(ref Cursor cursorInput)
    {
        base.start(ref cursorInput);
        if(RayCast.instance.swimMode == RayCast.SWIMMODE.MOVE)
        {
            RayCast.instance.swimMode = RayCast.SWIMMODE.ROTATE;
            targetRender.sprite = rotationSprite;
        }else
        {
            RayCast.instance.swimMode = RayCast.SWIMMODE.MOVE;
            targetRender.sprite = movementSprite;
        }
    }
}
