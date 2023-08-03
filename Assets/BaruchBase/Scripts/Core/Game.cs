using Baruch.Extension;
using UnityEngine;
using static Baruch.Util.BaruchUtil;

namespace Baruch.Core
{
    public static class Game
    {
        public static bool IsInitialized;

        public static void Initialize()
        {
            IsInitialized = false;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.targetFrameRate = 60;

            Save.Load();

            FindInterfacesOfType<IInit>(true).ForEach(iinit => iinit.Init());


            LevelManager.Instance.Build();

            IsInitialized = true;

        }
        

       
    }
}
