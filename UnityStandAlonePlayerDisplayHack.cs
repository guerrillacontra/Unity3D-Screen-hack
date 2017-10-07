using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.DTM.Fleetman
{
    ///<summary>
    /// Unity handles it's Display/Screen command line stuff oddly.
    /// 
    /// First of all, just before the very first "Start" is called on your first Scene, Unity will save display info (width, height, adapter...)
    /// to PlayerPrefs.
    /// 
    /// Which on Windows10 is saved to the registry on v5.4 of Unity.
    /// 
    /// To make matters worse, Unity will always use the saved values over new values passed in.
    /// 
    /// This means you must delete the registry keys under Reg delete "HKEY_CURRENT_USER\Software\[MyProductCompanyName]", to be able to allow unity to save new ones.
    /// 
    /// A pain as remember it saves it before the first Start? Well that means each instance has to wait for the next instance to reach that point or risk using the wrong
    /// reg settings!
    /// 
    /// You can now forget about that as below is some code that will override the persistent settings *take that unity* so each instance can be ran as expected.
    ///
    /// </summary>
    class UnityStandAlonePlayerDisplayHack : MonoBehaviour
    {
#if UNITY_STANDALONE
        void Start()
        {
            string[] args = System.Environment.GetCommandLineArgs();

            int adapter = 0;
            int width = Display.main.systemWidth;
            int height = Display.main.systemHeight;
            bool isFullScreen = Screen.fullScreen;

            //The following commands are standard params found in all StandAlonePlayers ref https://docs.unity3d.com/Manual/CommandLineArguments.html
            //We are simply overriding the persisted values to enable each instance its own configuration.

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-adapter")
                {
                    adapter = int.Parse(args[i + 1]);
                    i++;
                    continue;
                }

                if (args[i] == "-screen-width")
                {
                    width = int.Parse(args[i + 1]);
                    i++;
                    continue;
                }

                if (args[i] == "-screen-height")
                {
                    height = int.Parse(args[i + 1]);
                    i++;
                    continue;
                }

                if (args[i] == "-screen-fullscreen")
                {
                    isFullScreen = args[i + 1] == "1";
                    i++;
                    continue;
                }
            }

            Debug.LogFormat("Adapter {0} width {1} height {2} fullscreen? {3}", adapter, width, height, isFullScreen);

            if(isFullScreen)
            StartCoroutine(ConfigureFullScreenHack(width, height, adapter));
        }

        private IEnumerator ConfigureFullScreenHack(int width, int height, int adapter)
        {
            //So this is a hack but I can't seem to find another way round.

            // Set the target display and a low resolution.
            PlayerPrefs.SetInt("UnitySelectMonitor", adapter);
            PlayerPrefs.SetInt("Screenmanager Resolution Width", width);
            PlayerPrefs.SetInt("Screenmanager Resolution Height", height);
            PlayerPrefs.SetInt("Screenmanager Is Fullscreen mode", 1);
            PlayerPrefs.Save();

            //Intentionally set a "spicy" resolution to force unity to invalidate in confusion by its prefs.

            Screen.SetResolution(2, 2, true);

            yield return null;
            yield return null;
            yield return null;

            //Ensure the prefs are still what we want incase the previous set was overwritten

            PlayerPrefs.SetInt("UnitySelectMonitor", adapter);
            PlayerPrefs.SetInt("Screenmanager Resolution Width", width);
            PlayerPrefs.SetInt("Screenmanager Resolution Height", height);
            PlayerPrefs.SetInt("Screenmanager Is Fullscreen mode", 1);
            PlayerPrefs.Save();

            yield return null;
            yield return null;
            yield return null;
            
            //One more time, sometimes it does not work otherwise...

            PlayerPrefs.SetInt("UnitySelectMonitor", adapter);
            PlayerPrefs.SetInt("Screenmanager Resolution Width", width);
            PlayerPrefs.SetInt("Screenmanager Resolution Height", height);
            PlayerPrefs.SetInt("Screenmanager Is Fullscreen mode", 1);
            PlayerPrefs.Save();

            //This is what we are after, but we had to trick the persistent layer to make it happen...

            Screen.SetResolution(width, height, true);
        }
    }
#endif
}
