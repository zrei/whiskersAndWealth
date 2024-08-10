using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void IntEvent(int _);
public delegate void VoidEvent();
public delegate void FloatEvent(float _);
public delegate void Vector3Event(Vector3 _);

public static class GlobalEvents {

    public static class UI {

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
}