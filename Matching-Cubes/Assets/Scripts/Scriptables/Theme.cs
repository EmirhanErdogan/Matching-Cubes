using UnityEngine;

namespace Emir
{
    [CreateAssetMenu(menuName = "Emir/Default/Theme", fileName = "Theme", order = 3)]
    public class Theme : ScriptableObject
    {
        [Header("Materials")] 
        public Material SkyBox;
    }
}