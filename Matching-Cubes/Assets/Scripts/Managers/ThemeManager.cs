using UnityEngine;

namespace Emir
{
    public class ThemeManager : Singleton<ThemeManager>
    {
        /// <summary>
        /// This function helper for initialize theme to the world.
        /// </summary>
        /// <param name="theme"></param>
        public void Initialize(Theme theme)
        {
            RenderSettings.skybox = theme.SkyBox;
            CubeColorsAdded(theme);
        }

        private void CubeColorsAdded(Theme theme)
        {
            if (LevelComponent.Instance.GetCubes().Count < 1) return;

            foreach (CubeComponent Cube in LevelComponent.Instance.GetCubes())
            {
                switch (Cube.GetColor())
                {
                    case EColorType.COLOR1:
                        Cube.ColorInitialize(theme.CubeColors[0]);
                        break;
                    case EColorType.COLOR2:
                        Cube.ColorInitialize(theme.CubeColors[1]);
                        break;
                    case EColorType.COLOR3:
                        Cube.ColorInitialize(theme.CubeColors[2]);
                        break;
                    default:
                        break;
                }


            }
        }
    }
}