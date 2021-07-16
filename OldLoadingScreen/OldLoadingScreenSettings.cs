﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIExpansionKit.API;
using UnityEngine;
using MelonLoader;

namespace OldLoadingScreen
{
    class OldLoadingScreenSettings
    {

        public static MelonPreferences_Entry<bool> ShowLoadingMessages;
        public static MelonPreferences_Entry<bool> WarpTunnel;
        public static MelonPreferences_Entry<bool> VrcLogo;
        public static MelonPreferences_Entry<bool> ModSounds;

        public static void RegisterSettings()
        {
            var category = MelonPreferences.CreateCategory("BetterLoadingScreen", "Better Loading Screen");

            ShowLoadingMessages = category.CreateEntry("LoadingMessages", false, "Shows the default loading messages. (Enable for LoadingScreenPictures compatibility)");
            WarpTunnel = category.CreateEntry("Warp Tunnel", true, "Toggles the warp tunnel and particle effect (good for reducing motion)");
            VrcLogo = category.CreateEntry("Vrchat Logo", true, "Toggles the spinning VRChat logo");
            ModSounds = category.CreateEntry("Mod Sounds", true, "Toggles the music and ambience from the mod");

        }
        
    }
}