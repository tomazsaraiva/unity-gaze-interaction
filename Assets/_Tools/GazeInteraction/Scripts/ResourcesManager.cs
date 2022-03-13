#region Includes
using UnityEngine;
#endregion

namespace TS.GazeInteraction
{
    public class ResourcesManager : MonoBehaviour
    {
        #region Variables

        private const string DIRECTORY_PREFABS_FORMAT = "Prefabs/{0}";

        public const string FILE_PREFAB_RETICLE = "gaze_reticle";

        #endregion

        public static GameObject GetPrefab(string file)
        {
            return Resources.Load<GameObject>(string.Format(DIRECTORY_PREFABS_FORMAT, file));
        }
    }
}