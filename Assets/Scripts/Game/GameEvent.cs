﻿using Plutono.Util;
using UnityEngine;

namespace Game
{
    public struct HairCutEvent : IEvent
    {
        public Vector2 cutPosition;
        public GameObject hair;
    }

    public struct GameFailEvent : IEvent
    {
    }

    public struct GameClearEvent : IEvent
    {
        
    }
}