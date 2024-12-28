using Plutono.Util;
using UnityEngine;

namespace Game
{
    public struct HairCutEvent : IEvent
    {
        public GameObject hair;
        public float CutHeight;
        public float CutRatio;
        public Vector2 EndCutPoint;
    }

    public struct GetScoreEvent : IEvent
    {
        public Vector2 EndCutPoint;
    }
    
    public struct GameFailEvent : IEvent
    {
    }

    public struct GameClearEvent : IEvent
    {
        
    }
}