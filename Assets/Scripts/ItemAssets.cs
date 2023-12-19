using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Transform pfItemWorld;

    public Sprite ticketSprite;
    public Sprite balloonSprite;
    public Sprite bottleEmptySprite;
    public Sprite bottleFizzySprite;
    public Sprite bottleSoulSprite;
    public Sprite canSprite;
    public Sprite caramelAppleSprite;
    public Sprite corndogSprite;
    public Sprite cottonCandySprite;
    public Sprite dartSprite;
    public Sprite fishingRodSprite;
    public Sprite funnelCakeSprite;
    public Sprite hotdogPlainSprite;
    public Sprite hotdogSaucedSprite;
    public Sprite plateSprite;
    public Sprite popcornEmptySprite;
    public Sprite popcornFullSprite;
    public Sprite ringSprite;
    public Sprite rubberDuckSprite;
    public Sprite softballSprite;
    public Sprite soulSprite;
}
