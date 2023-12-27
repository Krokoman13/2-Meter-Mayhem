using System;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{    
    PlayerMovement pMove = null;
    PlayerCollision pCol = null;    

    //SETTINGS
    [Header("SETTINGS")]
    public bool SETTING_carMovement = false;

    //Movement
    [HideInInspector]
    public Vector2 inputXY = Vector2.zero;

    [Header("InputNames"), SerializeField]
    protected string horizontalInputName = "Horizontal", verticalInputName = "Vertical";
    public enum InputTypes { Get, Down, Up, NONE }

    //INPUT ACTIONS
    public delegate void InputAction(InputTypes type);
    //Fire1 input
    [SerializeField] protected string fire1InputName = "Fire1";
    public InputAction fire1InputAction = null;
    //Fire2 input
    [SerializeField] protected string fire2InputName = "Fire2";
    public InputAction fire2InputAction = null;


    //Get components and such before the game starts.
    void Awake()
    {        
        pMove = GetComponent<PlayerMovement>();
        pCol = GetComponent<PlayerCollision>();
    
    }

    void Start()
    {
        string carMovementSettingName = SettingsHandler.instance.allSettings[1].name;
        SETTING_carMovement = Convert.ToBoolean(PlayerPrefs.GetInt(carMovementSettingName));
        fire1InputAction += Fire1Input;
        fire2InputAction += Fire2Input;
    }

    /// <summary>
    /// Get inputs in the main update, making the game responsive.
    /// </summary>
    void Update()
    {
        //Get inputs in Update, use them in FixedUpdate
        GetInput();
        DEBUG();
    }

    private void FixedUpdate()
    {
        ApplyInputs();
    }

    //Save inputs to variables for later use.
    void GetInput()
    {
        inputXY = new Vector2(Input.GetAxisRaw(horizontalInputName), Input.GetAxisRaw(verticalInputName)).normalized;

        CheckInputAction(fire1InputName, fire1InputAction);
        CheckInputAction(fire2InputName, fire2InputAction);
    }

    /// <summary>
    /// Check if and/or how a button is being pressed and invoke the function, notifying it's subscribers.
    /// </summary>
    /// <param name="_inputName">Name of the Button input. (Example: "Jump")</param>
    /// <param name="_inputAction">The InputAction-delegate that will be invoked based on the InputName variable.</param>
    void CheckInputAction(string _inputName, InputAction _inputAction)
    {
        //Button is not pressed by default.
        InputTypes currentInput = InputTypes.NONE;

        //Set the respective press-type
        if (Input.GetButton(_inputName))                //Get: button is being held down at this moment.
            currentInput = InputTypes.Get;
        if (Input.GetButtonDown(_inputName))            //Down: Button starts being pressed down on this frame only.
            currentInput = InputTypes.Down;
        else if (Input.GetButtonUp(_inputName))         //Up: Button stops being pressed down on this frame only.
            currentInput = InputTypes.Up;

        //Invoke the action (if it's not null) and include the press-type.
        _inputAction?.Invoke(currentInput);
    }

    //Apply saved inputs to control the player.
    void ApplyInputs()
    {
        if (SETTING_carMovement)
            pMove.CarMovement(inputXY);
        else
        {
            pMove.DoRotate(inputXY);
            pMove.DoMove(inputXY);
        }
    }

    #region Fire1 Input Methods
    void Fire1Input(InputTypes _inputType)
    {
        if (_inputType == InputTypes.NONE)
            return;

        if (_inputType == InputTypes.Get)
            INPUT_Fire1_Get();
        else if (_inputType == InputTypes.Down)
            INPUT_Fire1_Down();
        else
            INPUT_Fire1_Up();
    }
    void INPUT_Fire1_Get()
    {

    }
    void INPUT_Fire1_Down()
    {
        //pCol.InteractWithShelf();
        pCol.InteractWithShelf();
        //pCartManager.InteractWithShelf();
    }
    void INPUT_Fire1_Up()
    {
        //pCol.InteractWithShelf();
    }
    #endregion


    #region Fire2 Input Methods
    /// <summary>
    /// Receives input and passes it to the proper method within this class.
    /// </summary>
    /// <param name="_inputType"></param>
    void Fire2Input(InputTypes _inputType)
    {
        if (_inputType == InputTypes.NONE)
            return;

        if (_inputType == InputTypes.Get)
            INPUT_Fire2_Get();
        else if (_inputType == InputTypes.Down)
            INPUT_Fire2_Down();
        else
            INPUT_Fire2_Up();
    }
    void INPUT_Fire2_Get()
    {

    }
    void INPUT_Fire2_Down()
    {

    }
    void INPUT_Fire2_Up()
    {

    }
    #endregion


    void DEBUG()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            SETTING_carMovement = !SETTING_carMovement;
        }
    }
}