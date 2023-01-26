#region Includes
using System.Collections;

using UnityEngine;
using UnityEngine.Events;
#endregion

namespace TS.GazeInteraction
{
    /// <summary>
    /// Component applied to GameObjects that can be interacted with using the gaze.
    /// </summary>
    public class GazeInteractable : MonoBehaviour
    {
        #region Variables

        private const string WAIT_TO_EXIT_COROUTINE = "WaitToExit_Coroutine";

        public delegate void OnEnter(GazeInteractable interactable, GazeInteractor interactor, Vector3 point);
        public event OnEnter Enter;

        public delegate void OnStay(GazeInteractable interactable, GazeInteractor interactor, Vector3 point);
        public event OnStay Stay;

        public delegate void OnExit(GazeInteractable interactable, GazeInteractor interactor);
        public event OnExit Exit;

        public delegate void OnActivated(GazeInteractable interactable);
        public event OnActivated Activated;

        [Header("Configuration")]
        [SerializeField] private bool _isActivable;
        [SerializeField] private float _exitDelay;

        [Header("Events")]
        public UnityEvent OnGazeEnter;
        public UnityEvent OnGazeStay;
        public UnityEvent OnGazeExit;
        public UnityEvent OnGazeActivated;
        public UnityEvent<bool> OnGazeToggle;

        public bool IsEnabled
        {
            get { return _collider.enabled; }
            set { _collider.enabled = value; }
        }
        public bool IsActivable
        {
            get { return _isActivable; }
        }
        public bool IsActivated { get; private set; }

        private Collider _collider;

        #endregion

        private void Awake()
        {
            _collider = GetComponent<Collider>();

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if(_collider == null) { throw new System.Exception("Missing Collider"); }
#endif
        }
        private void Start()
        {
            enabled = false;
        }

        /// <summary>
        ///  Toggles the GameObject.
        /// </summary>
        /// <param name="enable"></param>
        public void Enable(bool enable)
        {
            gameObject.SetActive(enable);
        }

        /// <summary>
        /// Invokes the Activated events.
        /// </summary>
        public void Activate()
        {
            IsActivated = true;

            Activated?.Invoke(this);
            OnGazeActivated?.Invoke();
        }

        /// <summary>
        /// Called by the GazeInteractor when the gaze enters this Interactable.
        /// Invokes the Enter events.
        /// </summary>
        /// <param name="interactor"></param>
        /// <param name="point"></param>
        public void GazeEnter(GazeInteractor interactor, Vector3 point)
        {
            StopCoroutine(WAIT_TO_EXIT_COROUTINE);

            Enter?.Invoke(this, interactor, point);

            OnGazeEnter?.Invoke();
            OnGazeToggle?.Invoke(true);
        }
        /// <summary>
        /// Called by the GazeInteractor while the gaze stays on top of this Interactable.
        /// Invokes the Stay events.
        /// </summary>
        /// <param name="interactor"></param>
        /// <param name="point"></param>
        public void GazeStay(GazeInteractor interactor, Vector3 point)
        {
            Stay?.Invoke(this, interactor, point);

            OnGazeStay?.Invoke();
        }
        /// <summary>
        /// Called by the GazeInteractor when the gaze exits this Interactable.
        /// Invokes the Exit events.
        /// </summary>
        /// <param name="interactor"></param>
        public void GazeExit(GazeInteractor interactor)
        {
            if(gameObject.activeInHierarchy)
            {
                StartCoroutine(WAIT_TO_EXIT_COROUTINE, interactor);
            }
            else
            {
                InvokeExit(interactor);
            }
        }

        private IEnumerator WaitToExit_Coroutine(GazeInteractor interactor)
        {
            yield return new WaitForSeconds(_exitDelay);

            InvokeExit(interactor);
        }

        private void InvokeExit(GazeInteractor interactor)
        {
            Exit?.Invoke(this, interactor);

            OnGazeExit?.Invoke();
            OnGazeToggle?.Invoke(false);

            IsActivated = false;
        }
    }
}