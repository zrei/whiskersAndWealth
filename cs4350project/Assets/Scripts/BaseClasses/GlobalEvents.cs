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

        public static TimePeriodEvent OnAdvanceTimePeriod;
    }

    public static class Narrative {
        public delegate void FlagEvent(string flag, bool value);

        public static FlagEvent OnSetFlagValue;
    }

    public static class Map {
        public static VoidEvent OnBeginMapLoad;
        public static FloatEvent OnMapLoadProgress;
        public static VoidEvent OnEndMapLoad;
    }
}