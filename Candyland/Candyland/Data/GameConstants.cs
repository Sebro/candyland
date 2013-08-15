﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Candyland
{
    public class GameConstants
    {
        /// <summary>
        /// Y Coordinate for Lower Boundary of the Game World
        /// </summary>
        public const float endOfWorld_Y = -8.0f;

        /// <summary>
        /// Gravity of all dynamic Objects
        /// </summary>
        public const float gravity = -0.004f;

        /// <summary>
        /// speed of moving obstacles
        /// </summary>
        public const float slippingSpeed = 0.03f;

        /// <summary>
        /// time a timed switch stays activated
        /// </summary>
        public const double switchActiveTime = 4;

        /// <summary>
        /// time a breakable plattform takes for breaking
        /// </summary>
        public const double breakTime = 4;

        // material params
        public static Vector4 ambient = new Vector4(0.95f, 0.95f, 0.95f, 1.0f);
        public static Vector4 diffuse = new Vector4(0.3f, 0.3f, 0.3f, 1.0f);

        /// <summary>
        /// state a switch is in
        /// </summary>
        public enum TouchedState
        {
            notTouched = 0,
            touched = 1,
            stillTouched = 2
        }

        /// <summary>
        /// distance a movable obstacle will move when pushed
        /// </summary>
        public const float obstacleMoveDistance = 0.1f;

        public enum SubActionType
        {
            appear = 0,
            movement = 1,
            dialog = 2,
            disappear = 3
        }

        /// <summary>
        /// bounding box rendering on/off
        /// </summary>
        public const bool boundingBoxRendering = false;
        public const bool singlestepperOFF = true;
        public const int framerate = 1;

        public const int inputManagerMode = InputManager.KEYBOARDMOUSE;

        // data regarding the scene
        public const string sceneFile = @"Content\Scenes\HoehlenTest.xml";
        public const string eventFile = @"Content\Scenes\Events\EventTest.xml";
        public const string actionsFile = @"Content\Scenes\Actions\ActionTest.xml";
        public const string startAreaID = "6";
        public const string startLevelID = "6.0";


        /// <summary>
        /// Strings
        /// </summary>
        /// 
        // Dialogue
        public const string tradesmanGreeting = "Ah, ein Kunde. Guten Tag, Reisender! Womit kann ich behilflich sein? Kann ich dich für meine Waren begeistern oder suchst du eine Transportmöglichkeit? Oder möchtest du ein paar Neuigkeiten über Candyland hören? Ich höre viel und rede gern :) Hast du z.B. schon von der neuen Bedrohung durch die größenwahnsinnige Lakritze gehört?";

        public static String getFairyMessage(String levelID)
        {
            switch(levelID)
            {
                case "7.0": return "Ich gebe dir hilfreiche Tipps.";
            }

            return "";
        }
    }
}
