#if UNITY_EDITOR
using UnityEditor;
#endif

#region Includes
using UnityEngine;
using UnityEngine.UI;
#endregion

namespace TS.GazeInteraction
{
    public class GazeReticle : MonoBehaviour
    {
        #region Variables

        [Header("Inner")]
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Image _imageProgress;

        [Header("Configuration")]
        [SerializeField] private float _scale = 0.0015f;
        [SerializeField] private float _offsetFromHit = 0.1f;

        private GazeInteractor _interactor;

        #endregion

        private void Start()
        {
            _canvas.transform.localScale = Vector3.one * _scale;
        }
        private void Update()
        {
            if(_interactor == null) { return; }

            var distance = Vector3.Distance(_interactor.transform.position, transform.position);
            var scale = distance * _scale;
            scale = Mathf.Clamp(scale, _scale, scale);
            _canvas.transform.localScale = Vector3.one * scale;
        }

        public void SetInteractor(GazeInteractor interactor)
        {
            _interactor = interactor;
            enabled = true;
        }
        public void Enable(bool enable)
        {
            gameObject.SetActive(enable);
        }

        public void SetTarget(RaycastHit hit)
        {
            var direction = _interactor.transform.position - hit.point;
            var rotation = Quaternion.FromToRotation(Vector3.forward, direction);
            var position = hit.point + transform.forward * _offsetFromHit;

            transform.SetPositionAndRotation(position, rotation);
        }
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