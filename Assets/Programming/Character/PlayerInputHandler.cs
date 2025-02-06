using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset PlayerControls;

    [Header("Action Map Name References")]
    [SerializeField] private string actionMapName = "Player";

    [Header("Action Name References")]
    [SerializeField] private string move = "Move";
    [SerializeField] private string sprint = "Sprint";
    [SerializeField] private string interact = "Interaction";

    private InputAction moveAction;
    private InputAction sprintAction;
    private InputAction interactAction;

    public Vector2 MoveInput { get; private set; }
    public float SprintValue { get; private set; }
    public bool InteractionTriggered { get; private set; }

    public static PlayerInputHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        moveAction = PlayerControls.FindActionMap(actionMapName).FindAction(move);
        sprintAction = PlayerControls.FindActionMap(actionMapName).FindAction(sprint);
        interactAction = PlayerControls.FindActionMap(actionMapName).FindAction(interact);
        RegisterInputActions();
    }

    void RegisterInputActions()
    {
        moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => MoveInput = Vector2.zero;

        sprintAction.performed += context => SprintValue = context.ReadValue<float>();
        sprintAction.canceled += contect => SprintValue = 0f;

        interactAction.performed += context => InteractionTriggered = true;
        interactAction.canceled += context => InteractionTriggered = false;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        sprintAction.Enable();
        interactAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        sprintAction.Disable();
        interactAction.Disable();
    }

}
