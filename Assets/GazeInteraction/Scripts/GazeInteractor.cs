#region Includes
using UnityEngine;
#endregion

namespace TS.GazeInteraction
{
    /// <summary>
    /// Manages gaze-based interactions with interactables.
    /// </summary>
    public class GazeInteractor : MonoBehaviour
    {
        #region Variables

        [Header("Configuration")]

        [Tooltip("The maximum distance for gaze detection.")]
        [SerializeField] private float _maxDetectionDistance;

        [Tooltip("The minimum distance for gaze detection.")]
        [SerializeField] private float _minDetectionDistance;

        [Tooltip("Time required for activation.")]
        [SerializeField] private float _timeToActivate = 1.0f;

        [Tooltip("Layer mask for raycasting.")]
        [SerializeField] private LayerMask _layerMask;

        [Tooltip("Type of reticle (visual feedback) to display.")]
        [SerializeField] private ReticleType _reticleType;

        private Ray _ray;
        private RaycastHit _hit;

        private GazeReticle _reticle;
        private GazeInteractable _interactable;

        private float _enterStartTime;

        #endregion

        /// <summary>
        /// Initializes the GazeInteractor by instantiating a reticle (visual indicator) based on a prefab.
        /// </summary>
        private void Start()
        {
            var instance = ResourcesManager.GetPrefab(ResourcesManager.FILE_PREFAB_RETICLE);
            if (!instance.TryGetComponent<GazeReticle>(out var reticle))
            {
                Debug.LogError("Missing GazeReticle");
            }

            _reticle = Instantiate(reticle);
            _reticle.SetType(_reticleType);
            _reticle.SetInteractor(this);
        }

        /// <summary>
        /// Handles raycasting to detect gaze interactions. If an object is within the specified distance,
        /// the reticle is enabled and interactions are processed.
        /// </summary>
        private void Update()
        {
            _ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(_ray, out _hit, _maxDetectionDistance, _layerMask))
            {
                var distance = Vector3.Distance(transform.position, _hit.transform.position);
                if (distance < _minDetectionDistance)
                {
                    _reticle.Enable(false);
                    Reset();
                    return;
                }

                _reticle.SetTarget(_hit);
                _reticle.Enable(true);

                var interactable = _hit.collider.transform.GetComponent<GazeInteractable>();
                if (interactable == null)
                {
                    Reset();
                    return;
                }

                if (interactable != _interactable)
                {
                    Reset();

                    _enterStartTime = Time.time;

                    _interactable = interactable;
                    _interactable.GazeEnter(this, _hit.point);
                }

                _interactable.GazeStay(this, _hit.point);

                if (_interactable.IsActivable && !_interactable.IsActivated)
                {
                    var timeToActivate = (_enterStartTime + _timeToActivate) - Time.time;
                    var progress = 1 - (timeToActivate / _timeToActivate);
                    progress = Mathf.Clamp(progress, 0, 1);

                    _reticle.SetProgress(progress);

                    if (progress == 1)
                    {
                        _reticle.Enable(false);
                        _interactable.Activate();
                    }
                }

                return;
            }

            _reticle.Enable(false);
            Reset();
        }

        /// <summary>
        /// Resets the interaction state.
        /// </summary>
        private void Reset()
        {
            _reticle.SetProgress(0);

            if (_interactable == null) { return; }
            _interactable.GazeExit(this);
            _interactable = null;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, transform.forward * _maxDetectionDistance);
        }
#endif
    }
}