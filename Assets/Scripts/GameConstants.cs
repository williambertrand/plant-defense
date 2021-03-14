using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConstants
{

    public struct Player
    {
        public static int StartSeeds = 5;
    }

    public struct Tags
    {
        public static string Player = "Player";
        public static string Seed = "Seed";
    }


    /* NOTE: Make sure these values line up with how the scenes are ordered in 
     * File => Build Setting => Scenes In Build
     */
    public struct Scenes
    {
        public static int Menu = 0;
        public static int Game = 1;
        public static int GameOver = 2;
    }
}
