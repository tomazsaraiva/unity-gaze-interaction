#region Includes
using UnityEngine;
#endregion

namespace TS.GazeInteraction
{
    /// <summary>
    /// Loads assets from the Resources directory.
    /// </summary>
    public class ResourcesManager : MonoBehaviour
    {
        #region Variables

        private const string DIRECTORY_PREFABS_FORMAT = "Prefabs/{0}";

        public const string FILE_PREFAB_RETICLE = "gaze_reticle";

        #endregion

        /// <summary>
        /// Loads and returns the asset specified in file from the Resources directory.
        /// </summary>
        /// <param name="file">Name of the asset to load. Use one of the constants.</param>
        /// <returns>Returns the loaded GameObject.</returns>
        public static GameObject GetPrefab(string file)
        {
            return Resources.Load<GameObject>(string.Format(DIRECTORY_PREFABS_FORMAT, file));
        }
    }
}