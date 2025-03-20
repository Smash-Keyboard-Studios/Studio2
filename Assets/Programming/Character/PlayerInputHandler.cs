using UnityEngine;
using UnityEngine.InputSystem;

//Script by Aaron Wing

//HOW TO USE: In order to make the player use any action make sure you have both the Player and PlayerInputHandler prefabs in the scene. The scripts should already be attached to the individual game objects.
//This script should be attached to the PlayerInputHandler Game Object.
public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset PlayerControls;

    [Header("Action Map Name References")]
    [SerializeField] private string ActionMapName = "Player";

    [Header("Action Name References")]
    [SerializeField] private string Move = "Move";
    [SerializeField] private string Sprint = "Sprint";
    [SerializeField] private string Interact = "Interaction";
    [SerializeField] private string Block = "Block";

    private InputAction MoveAction;
    private InputAction SprintAction;
    private InputAction InteractAction;
    private InputAction BlockAction;

    public Vector2 MoveInput { get; private set; }
    public float SprintValue { get; private set; }
    public bool InteractionTriggered { get; private set; }
    public bool BlockTriggered { get; private set; }

    public static PlayerInputHandler Instance { get; private set; }

    private void Awake()
    {
        //if instance is null it creates a instance in the scene otherwise it destroys that gameobject instance.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        MoveAction = PlayerControls.FindActionMap(ActionMapName).FindAction(Move);
        SprintAction = PlayerControls.FindActionMap(ActionMapName).FindAction(Sprint);
        InteractAction = PlayerControls.FindActionMap(ActionMapName).FindAction(Interact);
        BlockAction = PlayerControls.FindActionMap(ActionMapName).FindAction(Block);
        RegisterInputActions();
    }

    void RegisterInputActions()
    {
        MoveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        MoveAction.canceled += context => MoveInput = Vector2.zero;

        SprintAction.performed += context => SprintValue = context.ReadValue<float>();
        SprintAction.canceled += context => SprintValue = 0f;

        InteractAction.performed += context => InteractionTriggered = true;
        InteractAction.canceled += context => InteractionTriggered = false;

        BlockAction.performed += context => BlockTriggered = true;
        BlockAction.canceled += context => BlockTriggered = false;
    }

    private void OnEnable()
    {
        MoveAction.Enable();
        SprintAction.Enable();
        InteractAction.Enable();
        BlockAction.Enable();
    }

    private void OnDisable()
    {
        MoveAction.Disable();
        SprintAction.Disable();
        InteractAction.Disable();
        BlockAction.Disable();
    }

}
