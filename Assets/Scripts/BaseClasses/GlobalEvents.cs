using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public delegate void IntEvent(int _);
public delegate void VoidEvent();
public delegate void FloatEvent(float _);
public delegate void Vector3Event(Vector3 _);
public delegate void BoolEvent(bool _);

public static class GlobalEvents {

    public static class UI {
        public static VoidEvent OnUILayerOpened;
        public static VoidEvent OnUILayerClosed;
    }

    public static class Player {

    }

    public static class Time {
        public delegate void TimePeriodEvent(TimePeriod _);

        public static TimePeriodEvent AdvanceTimePeriodEvent;
    }

    public static class Narrative {
        public delegate void FlagEvent(string flag, bool value);

        public static FlagEvent SetFlagValueEvent;
    }
    
    public static class Scene {
        public delegate void SceneEnumEvent(SceneEnum scene);

        public static SceneEnumEvent ChangeSceneEvent;
    }

    public static class Map {
        public static VoidEvent MapLoadBeginEvent;
        public static FloatEvent MapLoadProgressEvent;
        public static VoidEvent MapLoadCompleteEvent;
    }

    public static class Starvation {
        public static FloatEvent StarvationChangeEvent;
        public static VoidEvent PlayerStarveEvent;
    }

    public static class Minigame
    {
        public static class LaneMinigame
        {
            public delegate void LaneMinigameEvent(LaneWaveSO _, int waveNumber);
            public static IntEvent ScoreChangeEvent;
            public static IntEvent ScoreSetEvent;
            public static LaneMinigameEvent BeginLaneMinigameWaveEvent;
            public static VoidEvent EndMinigameEvent;
        }
    }

    public static class Inventory
    {
        public delegate void ItemStackEvent(ItemStack _);
        public static ItemStackEvent ItemConsumedEvent;
        public static ItemStackEvent ItemAddedToInventoryEvent;
        public static ItemStackEvent ItemDiscardedEvent;
    }

    public static class Shop
    {
        public static Inventory.ItemStackEvent ItemBoughtEvent;
    }

    public static class PotionShop
    {
        public static VoidEvent PotionShopUpgradedEvent;
        public static VoidEvent OnPotionMadeEvent;
    }

    public static class Coin
    {
        public static IntEvent OnConsumeCoin;
        public static IntEvent OnAddCoin;
        public static IntEvent OnUpdateCoin;        
    }

    public static class Input
    {
        public static class Keybinding
        {
            public delegate void InputActionRefEvent(InputAction _, int _2);

            public static InputActionRefEvent OnInputActionPathChangedEvent;
        }

        public delegate void ControlSchemeEvent(InputControlScheme _);
        public static ControlSchemeEvent OnControlSchemeChangedEvent;
    }
}
