using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item 
{
    public enum ItemType
    {
        Ticket,
        Balloon,
        BottleEmpty,
        BottleFizzy,
        BottleSoul,
        Can,
        CaramelApple,
        Corndog,
        CottonCandy,
        Dart,
        FishingRod,
        FunnelCake,
        HotdogPlain,
        HotdogSauced,
        Plate,
        PopcornEmpty,
        PopcornFull,
        Ring,
        RubberDuck,
        Softball,
        Soul,
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.Ticket:           return ItemAssets.Instance.ticketSprite;
            case ItemType.Balloon:          return ItemAssets.Instance.balloonSprite;
            case ItemType.BottleEmpty:      return ItemAssets.Instance.bottleEmptySprite;
            case ItemType.BottleFizzy:      return ItemAssets.Instance.bottleFizzySprite;
            case ItemType.BottleSoul:       return ItemAssets.Instance.bottleSoulSprite;
            case ItemType.Can:              return ItemAssets.Instance.canSprite;
            case ItemType.CaramelApple:     return ItemAssets.Instance.caramelAppleSprite;
            case ItemType.Corndog:          return ItemAssets.Instance.corndogSprite;
            case ItemType.CottonCandy:      return ItemAssets.Instance.cottonCandySprite;
            case ItemType.Dart:             return ItemAssets.Instance.dartSprite;
            case ItemType.FishingRod:       return ItemAssets.Instance.fishingRodSprite;
            case ItemType.FunnelCake:       return ItemAssets.Instance.funnelCakeSprite;
            case ItemType.HotdogPlain:      return ItemAssets.Instance.hotdogPlainSprite;
            case ItemType.HotdogSauced:     return ItemAssets.Instance.hotdogSaucedSprite;
            case ItemType.Plate:            return ItemAssets.Instance.plateSprite;
            case ItemType.PopcornEmpty:     return ItemAssets.Instance.popcornEmptySprite;
            case ItemType.PopcornFull:      return ItemAssets.Instance.popcornFullSprite;
            case ItemType.Ring:             return ItemAssets.Instance.ringSprite;
            case ItemType.RubberDuck:       return ItemAssets.Instance.rubberDuckSprite;
            case ItemType.Softball:         return ItemAssets.Instance.softballSprite;
            case ItemType.Soul:             return ItemAssets.Instance.soulSprite;
        }
    }

    public string GetItemName()
    {
        switch (itemType)
        {
            default:
            case ItemType.Ticket:           return "Ticket";
            case ItemType.Balloon:          return "Balloon";
            case ItemType.BottleEmpty:      return "Empty Bottle";
            case ItemType.BottleFizzy:      return "Bottle Of Fizzy";
            case ItemType.BottleSoul:       return "Soul In A Bottle";
            case ItemType.Can:              return "Can";
            case ItemType.CaramelApple:     return "Caramel Apple";
            case ItemType.Corndog:          return "Corndog";
            case ItemType.CottonCandy:      return "Cotton Candy";
            case ItemType.Dart:             return "Dart";
            case ItemType.FishingRod:       return "Fishing Rod";
            case ItemType.FunnelCake:       return "Funnel Cake";
            case ItemType.HotdogPlain:      return "Plain Hotdog";
            case ItemType.HotdogSauced:     return "Hotdog With Sauce";
            case ItemType.Plate:            return "Plate";
            case ItemType.PopcornEmpty:     return "Empty Popcorn Container";
            case ItemType.PopcornFull:      return "Popcorn";
            case ItemType.Ring:             return "Ring";
            case ItemType.RubberDuck:       return "Rubber Duck";
            case ItemType.Softball:         return "Softball";
            case ItemType.Soul:             return "Wild Soul";
        }
    }

    public bool isStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.Ticket:
            case ItemType.Balloon:
            case ItemType.BottleEmpty:
            case ItemType.BottleFizzy:
            case ItemType.Can:
            case ItemType.CaramelApple:
            case ItemType.Corndog:
            case ItemType.CottonCandy:
            case ItemType.Dart:
            case ItemType.FunnelCake:
            case ItemType.HotdogPlain:
            case ItemType.HotdogSauced:
            case ItemType.Plate:
            case ItemType.PopcornEmpty:
            case ItemType.PopcornFull:
            case ItemType.Ring:
            case ItemType.Softball:
                return true;
            case ItemType.BottleSoul:
            case ItemType.FishingRod:
            case ItemType.RubberDuck:
            case ItemType.Soul:
                return false;
        }
    }
}
