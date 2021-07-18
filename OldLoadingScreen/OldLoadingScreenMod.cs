﻿// Pls be gentle this is my first mod, I know it's messy
// If there's better or more efficient ways of doing things please let me know :)


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using UnhollowerRuntimeLib;
using MelonLoader;
using OldLoadingScreen;
using UnityEngine;
// using UnityEngine.Audio;
// using UnityEngine.UI;
// using UnityEngine.Networking;
using Object = UnityEngine.Object;
using VRC;
using VRC.Core;
// using VRCSDK2;

[assembly: MelonInfo(typeof(OldLoadingScreenMod), "BetterLoadingScreen", "v0.8.0", "Grummus")]
[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonOptionalDependencies("UIExpansionKit")]


namespace OldLoadingScreen
{
	public class OldLoadingScreenMod : MelonMod
	{
		// private GameObject partiallyOffline;

		// private GameObject cavernDry;
		private GameObject loadScreenPrefab;
		private GameObject loginPrefab;

		private AssetBundle assets;

		// private AudioMixerGroup myUIGroup;

		public override void OnApplicationStart()
		{
			if (typeof(MelonLoader.MelonMod).GetMethod("VRChat_OnUiManagerInit") == null)
				MelonLoader.MelonCoroutines.Start(GetAssembly());
		}
		private System.Collections.IEnumerator GetAssembly()
		{
			System.Reflection.Assembly assemblyCSharp = null;
			while (true)
			{
				assemblyCSharp = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(assembly => assembly.GetName().Name == "Assembly-CSharp");
				if (assemblyCSharp == null)
					yield return null;
				else
					break;
			}

			MelonLoader.MelonCoroutines.Start(WaitForUiManagerInit(assemblyCSharp));
		}
		private System.Collections.IEnumerator WaitForUiManagerInit(System.Reflection.Assembly assemblyCSharp)
		{

			System.Type vrcUiManager = assemblyCSharp.GetType("VRCUiManager");
			System.Reflection.PropertyInfo uiManagerSingleton = vrcUiManager.GetProperties().First(pi => pi.PropertyType == vrcUiManager);

			while (uiManagerSingleton.GetValue(null) == null)
				yield return null;

			// while (ReferenceEquals(VRCUiManager.prop_VRCUiManager_0, null)) yield return null;

			// var audioManager = VRCAudioManager.field_Private_Static_VRCAudioManager_0;

			// myUIGroup = new[]
			// {
			// 	audioManager.field_Public_AudioMixerGroup_0, audioManager.field_Public_AudioMixerGroup_1,
			// 	audioManager.field_Public_AudioMixerGroup_2
			// }.Single(it => it.name == "UI");

			OnUiManagerInit();
		}

		// public override void VRChat_OnUiManagerInit()
		// {
		//     OnUiManagerInit();
		// }

		public void OnUiManagerInit()
		{

			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("OldLoadingScreen.loading.assetbundle"))
			using (var tempStream = new MemoryStream((int)stream.Length))
			{
				stream.CopyTo(tempStream);

				assets = AssetBundle.LoadFromMemory_Internal(tempStream.ToArray(), 0);
				assets.hideFlags |= HideFlags.DontUnloadUnusedAsset;
			}

			loadScreenPrefab = assets.LoadAsset_Internal("Assets/Bundle/LoadingBackground.prefab", Il2CppType.Of<GameObject>()).Cast<GameObject>();
			loadScreenPrefab.hideFlags |= HideFlags.DontUnloadUnusedAsset;

			//cavernDry = assets.LoadAsset_Internal("Assets/Bundle/CavernDry.prefab", Il2CppType.Of<GameObject>()).Cast<GameObject>();
			//cavernDry.hideFlags |= HideFlags.DontUnloadUnusedAsset;

			loginPrefab = assets.LoadAsset_Internal("Assets/Bundle/Login.prefab", Il2CppType.Of<GameObject>()).Cast<GameObject>();
			loginPrefab.hideFlags |= HideFlags.DontUnloadUnusedAsset;

			OldLoadingScreenSettings.RegisterSettings();
			CreateGameObjects();
		}

		public override void OnPreferencesSaved()
		{
			MelonLogger.Msg("Applying Preferences");

			loadScreenPrefab = GameObject.Find("UserInterface/MenuContent/Popups/LoadingPopup/LoadingBackground(Clone)");
			var music = loadScreenPrefab.transform.Find("MenuMusic");
			var spaceSound = loadScreenPrefab.transform.Find("SpaceSound");
			var cube = loadScreenPrefab.transform.Find("SkyCube");
			var particles = loadScreenPrefab.transform.Find("Stars");
			var warpTunnel = loadScreenPrefab.transform.Find("Tunnel");
			var logo = loadScreenPrefab.transform.Find("VRCLogo");
			var InfoPanel = GameObject.Find("UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingInfoPanel");
			var originalLoadingAudio = GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/LoadingSound");
			var aprfools = loadScreenPrefab.transform.Find("meme");

			if (OldLoadingScreenSettings.ModSounds.Value)
			{
				music.gameObject.SetActive(true);
				spaceSound.gameObject.SetActive(true);

				originalLoadingAudio.SetActive(false);
			} else
			{
				music.gameObject.SetActive(false);
				spaceSound.gameObject.SetActive(false);

				originalLoadingAudio.SetActive(true);
			}

			if (OldLoadingScreenSettings.WarpTunnel.Value)
			{
				warpTunnel.gameObject.SetActive(true);
			} else
			{
				warpTunnel.gameObject.SetActive(false);
			}

			if (OldLoadingScreenSettings.VrcLogo.Value)
			{
				logo.gameObject.SetActive(true);
			} else
			{
				logo.gameObject.SetActive(false);
			}

			if (OldLoadingScreenSettings.ShowLoadingMessages.Value)
			{
				InfoPanel.SetActive(true);
			} else
			{
				InfoPanel.SetActive(false);
			}

			if (DateTime.Today.Month == 4 && DateTime.Now.Day == 1)
            {
				logo.gameObject.SetActive(false);
				aprfools.gameObject.SetActive(true);
            }


		}


		private void CreateGameObjects()
		{
			MelonLogger.Msg("Finding original GameObjects");
			var UIRoot = GameObject.Find("UserInterface/MenuContent/Popups/LoadingPopup");
			var InfoPanel = GameObject.Find("UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingInfoPanel");
			var SkyCube = GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingBackground_TealGradient/SkyCube_Baked");
			var bubbles = GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/3DElements/LoadingBackground_TealGradient/_FX_ParticleBubbles");
			var loginBubbles = GameObject.Find("/UserInterface/LoadingBackground_TealGradient_Music/_FX_ParticleBubbles");
			var StartScreen = GameObject.Find("/UserInterface/LoadingBackground_TealGradient_Music/");
			var originalStartScreenAudio = GameObject.Find("/UserInterface/LoadingBackground_TealGradient_Music/LoadingSound");
			var originalStartScreenSkyCube = GameObject.Find("/UserInterface/LoadingBackground_TealGradient_Music/SkyCube_Baked");
			var originalLoadingAudio = GameObject.Find("/UserInterface/MenuContent/Popups/LoadingPopup/LoadingSound");

			MelonLogger.Msg("Creating new GameObjects");
			loadScreenPrefab = CreateGameObject(loadScreenPrefab, new Vector3(400, 400, 400), "UserInterface/MenuContent/Popups/", "LoadingPopup");
			loginPrefab = CreateGameObject(loginPrefab, new Vector3(0.5f, 0.5f, 0.5f), "UserInterface/", "LoadingBackground_TealGradient_Music");
			// newCube = CreateGameObject(newCube, new Vector3(0.5f, 0.5f, 0.5f), "UserInterface/", "LoadingBackground_TealGradient_Music");

			MelonLogger.Msg("Disabling original GameObjects");

			// Disable original objects from loading screen
			SkyCube.active = false;
			bubbles.active = false;
			originalLoadingAudio.active = false;

			// Disable original objects from login screen
			originalStartScreenAudio.active = false;
			originalStartScreenSkyCube.active = false;
			loginBubbles.active = false;

			// Apply any preferences (yes ik this is lazy)
			OnPreferencesSaved();

		}

		private GameObject CreateGameObject(GameObject obj, Vector3 scale, String rootDest, String parent)
		{
			MelonLogger.Msg("Creating " + obj.name);
			var UIRoot = GameObject.Find(rootDest);
			var requestedParent = UIRoot.transform.Find(parent);
			var newObject = Object.Instantiate(obj, requestedParent, false).Cast<GameObject>();
			newObject.transform.parent = requestedParent;
			newObject.transform.localScale = scale;
			return newObject;
		}
	}
}
