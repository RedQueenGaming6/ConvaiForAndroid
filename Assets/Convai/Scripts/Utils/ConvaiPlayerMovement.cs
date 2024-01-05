using UnityEngine;
using UnityEngine.EventSystems;

namespace Convai.Scripts.Utils
{
    /// <summary>
    ///     Class for handling player movement including walking, running, jumping, and looking around.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    [DisallowMultipleComponent]
    [AddComponentMenu("Convai/Player Movement")]
    [HelpURL("https://docs.convai.com/api-docs/plugins-and-integrations/unity-plugin/scripts-overview")]
    public class ConvaiPlayerMovement : MonoBehaviour
    {
        [Header("Movement Parameters")]
        [SerializeField]
        [Tooltip("The speed at which the player walks.")]
        [Range(1, 10)]
        private float walkingSpeed = 7.5f;

        [SerializeField] [Tooltip("The speed at which the player runs.")] [Range(1, 10)]
        private float runningSpeed = 11.5f;

        [SerializeField] [Tooltip("The speed at which the player jumps.")] [Range(1, 10)]
        private float jumpSpeed = 8.0f;

        [Header("Gravity & Grounding")] [SerializeField] [Tooltip("The gravity applied to the player.")] [Range(1, 10)]
        private float gravity = 9.8f;

        [Header("Camera Parameters")] [SerializeField] [Tooltip("The main camera the player uses.")]
        private Camera playerCamera;

        [SerializeField] [Tooltip("Speed at which the player can look around.")] [Range(0, 10)]
        private float lookSpeed = 2.0f;

        [SerializeField] [Tooltip("Limit of upwards and downwards look angles.")] [Range(1, 90)]
        private float lookXLimit = 45.0f;

        [HideInInspector] public bool canMove = true;

        private CharacterController _characterController;
        private Vector3 _moveDirection = Vector3.zero;
        private float _rotationX;

        public float speed;
        public VariableJoystick variableJoystick;
        public Rigidbody rb;
        public float cameraFollowSpeed = 5f;

        //Singleton Instance
        public static ConvaiPlayerMovement Instance { get; private set; }

        private void Awake()
        {
            // Singleton pattern to ensure only one instance exists
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            LockCursor();
        }

        private void Update()
        {
            // Handle cursor locking/unlocking
            HandleCursorLocking();

            // Check for running state and move the player
            MovePlayer();

            // Handle the player and camera rotation
            //RotatePlayerAndCamera();
        }

        /// <summary>
        ///     Unlock the cursor when the ESC key is pressed, Re-lock the cursor when the left mouse button is pressed
        /// </summary>
        private void HandleCursorLocking()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) LockCursor();
        }

        private static void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void MovePlayer()
        {
            Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;

            // Check if the joystick is in the neutral position (not moved)
            if (direction.magnitude > 0)
            {
                rb.AddForce(direction.normalized * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
            }
            else
            {
                // Stop the player when the joystick is in the neutral position
                rb.velocity = Vector3.zero;
            }
        }


    }
}