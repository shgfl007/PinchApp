using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Clickable : MonoBehaviour {
    public delegate void Action(Clickable c, Cursor cursor);
    public event Action onClicked;
    public delegate void CursorReleased(Clickable c);
    public event CursorReleased onReleased;
    public Material hoverMaterial;
    public Material normalMaterial;
    public SpriteRenderer uiSprite;
    public SpriteRenderer buttonImage;
    public Color uiNormalColor  = Color.white;
    public Color uiHoverColor   = Color.green;
    public Color uiSelectedColor    = Color.grey;
    public BoxCollider box;
    bool mHover;
    bool mSelected;
    Cursor mHoverCursor;
    public Renderer render;

    public bool IsHovering()
    {
        return mHover;
    }

    void HandleClicked(GameObject obj, Cursor c)
    {
        mSelected = true;
        mHover = false;
        AfterClicked(c);
        if (onClicked != null)
        {
            onClicked(this, c);
        }
        if (TrackerScript.instance)
        {
            if (TrackerScript.instance != null)
            {
                c.cursoHold += Released;
            }
        }
        if(box)
        {
            box.enabled = false;
        }
        IgnoreClick();
    }

    public virtual void Deselected()
    {
        //Debug.Log("Deselected");
        mSelected = false;
        mHover = false;
        IgnoreClick();
        if (onReleased != null)
        {
            onReleased(this);
        }
        if (box)
        {
            box.enabled = true;
        }
    }

    public virtual void AfterClicked(Cursor c) {}

    public virtual void Released(Cursor c)
    {
        //Debug.Log("Released");
        if (TrackerScript.instance)
        {
           c.cursoHold -= Released;
        }
        if(onReleased!=null)
        {
            onReleased(this);
        }
        if (box)
        {
            box.enabled = true;
        }
        IgnoreClick();
    }

    public void Hover(Cursor c)
    {
        if (mSelected)
        {
            return;
        }
        if (hoverMaterial & render)
        {
            render.material = hoverMaterial;
        }
        if(uiSprite)
        {
            uiSprite.color = uiHoverColor;
        }
        if(buttonImage)
        {
            buttonImage.color = uiHoverColor;
        }
        mHover = true;
        ListenToClick(c);
    }

    public virtual void AfterUpdate() { }

    public void UnHover(Cursor c)
    {
        mHover = false;
        mHoverCursor = null;
        if (buttonImage)
        {
            buttonImage.color = uiNormalColor;
        }
        if (onReleased != null)
        {
            onReleased(this);
        }
        if (mHoverCursor == c)
        {
            IgnoreClick();
        }
    }

    void ListenToClick(Cursor c)
    {
        if (mHoverCursor == null)
        {
            if (box)
            {
                box.enabled = true;
            }
            mHoverCursor = c;
            c.rayHitObj += HandleClicked;
        }
    }

    public void ListenToClick()
    {
        if (mHoverCursor != null)
        {
            if (box)
            {
                box.enabled = true;
            }
            mHoverCursor.rayHitObj += HandleClicked;
        }
    }

    public void IgnoreClick()
    {
        //Debug.Log("IgnoreClick");
        if (mHoverCursor != null)
        {
            mHoverCursor.rayHitObj -= HandleClicked;
        }
    }

    void Update()
    {
        if (mSelected)
        {
            Selected();
            mHover = false;
        }
        else if (mHover)
        {
            Hovered();
        }
        else
        {
            Normal();
        }
        
        AfterUpdate();
        mHover = false;
    }

    public virtual void Selected(){}
    public virtual void Hovered() {}
    public virtual void Normal()
    {
        if (normalMaterial & render)
        {
            render.material = normalMaterial;
        }
        if (uiSprite)
        {
            uiSprite.color = uiNormalColor;
        }
    }
}
