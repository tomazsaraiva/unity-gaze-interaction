#region Includes
using UnityEngine;
#endregion

namespace TS.GazeInteraction
{
    public class GazeInteractor : MonoBehaviour
    {
        #region Variables

        [Header("Configuration")]
        [SerializeField] private float _maxDetectionDistance;
        [SerializeField] private float _minDetectionDistance;
        [SerializeField] private float _timeToActivate = 1.0f;
        [SerializeField] private LayerMask _layerMask;

        private Ray _ray;
        private RaycastHit _hit;

        private GazeReticle _reticle;
        private GazeInteractable _interactable;

        private float _enterStartTime;
        private bool _activated;

        #endregion

        private void Start()
        {
            var instance = ResourcesManager.GetPrefab(ResourcesManager.FILE_PREFAB_RETICLE);
            var reticle = instance.GetComponent<GazeReticle>();

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if(reticle == null) { throw new System.Exception("Missing GazeReticle"); }
#endif

            _reticle = Instantiate(reticle);
            _reticle.SetInteractor(this);
        }
        private void Update()
        {
            _ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(_ray, out _hit, _maxDetectionDistance, _layerMask))
            {
                var distance = Vector3.Distance(transform.position, _hit.transform.position);
                if (distance < _minDetectionDistance)
                {
                    Reset();
                    return;
                }

                _reticle.SetTarget(_hit);

                var interactable = _hit.collider.transform.GetComponent<GazeInteractable>();
                if(interactable == null)
                {
                    Reset();
                    _reticle.Enable(true);
                    return;
                }

                if (interactable != _interactable)
                {
                    Reset();
                    _reticle.Enable(true);

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

                    if (progress == 1 && !_activated)
                    {
                        _activated = true;

                        _reticle.Enable(false);
                        _interactable.Activate();
                    }
                }

                return;
            }

            Reset();
        }

        private void Reset()
        {
            _reticle.Enable(false);
            _reticle.SetProgress(0);
            _activated = false;

            if(_interactable == null) { return; }
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