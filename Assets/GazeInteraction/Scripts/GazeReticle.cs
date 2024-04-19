#if UNITY_EDITOR
using UnityEditor;
#endif

#region Includes
using UnityEngine;
using UnityEngine.UI;
#endregion

namespace TS.GazeInteraction
{
    /// <summary>
    /// Visual representation of the point of interaction.
    /// </summary>
    public class GazeReticle : MonoBehaviour
    {
        #region Variables

        [Header("Inner")]

        [Tooltip("The canvas used for rendering the reticle.")]
        [SerializeField] private Canvas _canvas;

        [Tooltip("The image representing the progress or state of interaction.")]
        [SerializeField] private Image _imageProgress;

        [Header("Configuration")]

        [Tooltip("Scale factor for adjusting the reticle size.")]
        [SerializeField] private float _scale = 0.0015f;

        [Tooltip("Distance offset from the hit point for positioning the reticle.")]
        [SerializeField] private float _offsetFromHit = 0.1f;

        private GazeInteractor _interactor;
        private ReticleType _type;

        #endregion

        // <summary>
        /// Initializes the GazeReticle by setting the initial scale.
        /// </summary>
        private void Start()
        {
            _canvas.transform.localScale = Vector3.one * _scale;
        }

        /// <summary>
        /// Updates the reticle's scale based on the distance from the GazeInteractor.
        /// </summary>
        private void Update()
        {
            if (_interactor == null) { return; }

            var distance = Vector3.Distance(_interactor.transform.position, transform.position);
            var scale = distance * _scale;
            scale = Mathf.Clamp(scale, _scale, scale);
            _canvas.transform.localScale = Vector3.one * scale;
        }

        /// <summary>
        /// Assigns the GazeInteractor using this reticle.
        /// </summary>
        /// <param name="interactor">The GazeInteractor to associate with this reticle.</param>
        public void SetInteractor(GazeInteractor interactor)
        {
            _interactor = interactor;
            transform.SetParent(_interactor.transform);

            enabled = true;
        }

        /// <summary>
        /// Sets the type of reticle (visible or invisible).
        /// </summary>
        /// <param name="type">The desired reticle type.</param>
        public void SetType(ReticleType type)
        {
            _type = type;

            if (_type == ReticleType.Invisible)
            {
                Enable(false);
            }
        }

        /// <summary>
        /// Toggles the visibility of the reticle GameObject.
        /// </summary>
        /// <param name="enable">Whether to enable or disable the reticle.</param>
        public void Enable(bool enable)
        {
            if (_type == ReticleType.Invisible && enable ||
                _type == ReticleType.Visible && !enable) return;

            gameObject.SetActive(enable);
        }

        /// <summary>
        /// Assigns the current hit point to adjust the reticle position and rotation.
        /// </summary>
        /// <param name="hit">The RaycastHit containing information about the hit point.</param>
        public void SetTarget(RaycastHit hit)
        {
            var direction = _interactor.transform.position - hit.point;
            var rotation = Quaternion.FromToRotation(Vector3.forward, direction);
            var targetPosition = hit.point + transform.forward * _offsetFromHit;

            transform.SetPositionAndRotation(targetPosition, rotation);
        }

        /// <summary>
        /// Updates the progress visual indicator.
        /// </summary>
        /// <param name="progress">The progress value (0 to 1) to display.</param>
        public void SetProgress(float progress)
        {
            _imageProgress.fillAmount = progress;
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(GazeReticle))]
        public class GazeReticleInspector : Editor
        {
            private GazeReticle _target;

            void OnEnable()
            {
                _target = (GazeReticle)target;
            }

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Editor");
            }
        }
#endif
    }
}