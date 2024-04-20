#region Includes
using System.Collections;

using UnityEngine;
using UnityEngine.Events;
#endregion

namespace TS.GazeInteraction
{
    /// <summary>
    /// Represents an interactable object that responds to gaze-based interactions.
    /// </summary>
    public class GazeInteractable : MonoBehaviour
    {
        #region Variables

        private const string WAIT_TO_EXIT_COROUTINE = "WaitToExit_Coroutine";

        /// <summary>
        /// Delegate for handling the gaze enter event.
        /// </summary>
        /// <param name="interactable">The interactable object.</param>
        /// <param name="interactor">The gaze interactor.</param>
        /// <param name="point">The point where the gaze entered.</param>
        public delegate void OnEnter(GazeInteractable interactable, GazeInteractor interactor, Vector3 point);

        /// <summary>
        /// Event triggered when the gaze enters the interactable.
        /// </summary>
        public event OnEnter Enter;

        /// <summary>
        /// Delegate for handling the gaze stay event.
        /// </summary>
        /// <param name="interactable">The interactable object.</param>
        /// <param name="interactor">The gaze interactor.</param>
        /// <param name="point">The current gaze point.</param>
        public delegate void OnStay(GazeInteractable interactable, GazeInteractor interactor, Vector3 point);

        /// <summary>
        /// Event triggered while the gaze remains on the interactable.
        /// </summary>
        public event OnStay Stay;

        /// <summary>
        /// Delegate for handling the gaze exit event.
        /// </summary>
        /// <param name="interactable">The interactable object.</param>
        /// <param name="interactor">The gaze interactor.</param>
        public delegate void OnExit(GazeInteractable interactable, GazeInteractor interactor);

        /// <summary>
        /// Event triggered when the gaze exits the interactable.
        /// </summary>
        public event OnExit Exit;

        /// <summary>
        /// Delegate for handling the activation event.
        /// </summary>
        /// <param name="interactable">The interactable object.</param>
        public delegate void OnActivated(GazeInteractable interactable);

        /// <summary>
        /// Event triggered when the interactable is activated.
        /// </summary>
        public event OnActivated Activated;

        [Header("Configuration")]

        [Tooltip("Determines whether this interactable can be activated.")]
        [SerializeField] private bool _isActivable;

        [Tooltip("Time delay before triggering the exit event after gaze leaves the interactable.")]
        [SerializeField] private float _exitDelay;

        [Header("Events")]

        [Tooltip("Unity event triggered when the gaze enters the interactable.")]
        public UnityEvent OnGazeEnter;

        [Tooltip("Unity event triggered while the gaze remains on the interactable.")]
        public UnityEvent OnGazeStay;

        [Tooltip("Unity event triggered when the gaze exits the interactable.")]
        public UnityEvent OnGazeExit;

        [Tooltip("Unity event triggered when the interactable is activated.")]
        public UnityEvent OnGazeActivated;

        [Tooltip("Unity event triggered when the gaze toggle state changes.")]
        public UnityEvent<bool> OnGazeToggle;

        /// <summary>
        /// Indicates whether the interactable is enabled.
        /// </summary>
        public bool IsEnabled
        {
            get { return _collider.enabled; }
            set { _collider.enabled = value; }
        }

        /// <summary>
        /// Indicates whether the interactable is activable.
        /// </summary>
        public bool IsActivable
        {
            get { return _isActivable; }
        }

        /// <summary>
        /// Indicates whether the interactable is currenlty activated.
        /// </summary>
        public bool IsActivated { get; private set; }

        private Collider _collider;

        #endregion

        private void Awake()
        {
            if (!TryGetComponent<Collider>(out _collider))
            {
                Debug.LogError("Missing Collider");
            }
        }
        private void Start()
        {
            enabled = false;
        }

        /// <summary>
        /// Enables or disables the interactable game object.
        /// </summary>
        /// <param name="enable">True to enable, false to disable.</param>
        public void Enable(bool enable)
        {
            gameObject.SetActive(enable);
        }

        /// <summary>
        /// Activates the interactable and invokes the activated event.
        /// </summary>
        public void Activate()
        {
            IsActivated = true;

            Activated?.Invoke(this);
            OnGazeActivated?.Invoke();
        }

        /// <summary>
        /// Invokes the gaze enter event.
        /// </summary>
        public void GazeEnter(GazeInteractor interactor, Vector3 point)
        {
            StopCoroutine(WAIT_TO_EXIT_COROUTINE);

            Enter?.Invoke(this, interactor, point);

            OnGazeEnter?.Invoke();
            OnGazeToggle?.Invoke(true);
        }

        /// <summary>
        /// Invokes the gaze stay event.
        /// </summary>
        public void GazeStay(GazeInteractor interactor, Vector3 point)
        {
            Stay?.Invoke(this, interactor, point);

            OnGazeStay?.Invoke();
        }

        /// <summary>
        /// Invokes the gaze exit event after the _exitDelay duration.
        /// </summary>
        public void GazeExit(GazeInteractor interactor)
        {
            if (gameObject.activeInHierarchy)
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