using System;
using Plutono.Util;
using Transition;
using UnityEngine;

namespace Game
{
    public class Game : Singleton<Game>
    {
        public int score = 0;
        public int gameStatus = 0;
    }
}