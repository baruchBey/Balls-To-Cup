using UnityEngine;

namespace Baruch
{
    [DisallowMultipleComponent]
    public class TutorialManager : Singleton<TutorialManager>, IInit
    {
        [Save]public static byte TutorialStage;

        public void Init()
        {

        }

    }
}