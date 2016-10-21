using UnityEngine;
using UnityEngine.UI;

public class CatalogButton : VRButton
{
    public string itemName = "Chair";
    public Sprite sprite;
    public GameObject prefab;
    public SpriteRenderer spriteRender;

    void Start()
    {
        spriteRender.sprite = sprite;
    }
}
