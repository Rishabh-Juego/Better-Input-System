# Better-Input-System  
Project to try and capture complete new input system  

## General info  
New input system for better key bidings in games  
Unity input system can be added by using the id: `com.unity.inputsystem`  
Corresponding setting can be foubd in **Edit** > **Project Settings** > **Player** > **Other Settings** > **Active Input Handling**. If you change this setting you must restart the Editor for it to take effect.  
Namespace to use: `UnityEngine.InputSystem;`  
<br/>
<br/>
There are [3 ways](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.17/manual/Workflows.html) of using the new input system:
- **Directly reading Device States**
	- *Useful for POC*, bad for actual work
	- Works great if you have only one way of getting the input(only keyboard and mouse, or only one type of controller on consoles)
- **Using Actions** 
	- script references Input Action Asset, 
	- useful for work with *fixed input format*
- **Using an Actions Asset and a PlayerInput component**
	- attach callbacks from actions to your scripts
	- script does not need code between input system and your methods
	- useful for work with *key rebinding format*

### Directly reading Device States
This is good for scenarios where you have only on way of getting the input and only on one platform.
We directly access the keyboard or gamepad reference and then try to fetch individual keys and axis values for calculations.
This means, to change an input key binding, we will have to manually go to the script and make changes, this is prone to errors in complex setup.
See [`DirectSyntax.cs`](Assets/NewInputSystemLearning/DirectReadingDeviceStates/Scripts/DirectSyntax.cs) for an example of this type of setup.

For input sources like GamePad, we can check the deadzone of the axis to avoid unwanted movements.
This setting is found here: **Project Settings** > **Input System Package** > **Settings** > **Default Deadzone Min/Max**.  
We can chang it by crating an asset : **Project Settings** > **Input System Package** > **Settings** > **Create settings asset**

### Using Actions
#### Using Embedded Actions
This allows a user to create key bindings in the inspector, and the script only holds reference to the result value type. This is ideal if you have a single input Handler script and want to define all input bindings in one place.
This system focuses on defining the type of input action we want to perform, and then binding multiple keys to the action for different platforms. In case of a change needed in future, the bindings can be updated without effecting the code.

This allows us to create an `InputAction` variable that can be used to read input values by binding them to the action.  
This action can be binded to multiple keys, and we can read the value of the action in a unified way.  
![jump action](Images/jumpAction.png)
- **Add Binding**: Allows us to add a key(keyboard key, gamepad button, etc) to the action, so this key will trigger this action.(`EmbeddedWorkflow.jumpAction`)  
- **Add Positive/Negative Binding**: Allows us to add keys for single axis input, like single axis movement like scrollbar.  
- **Add Up\Down\Left\Right Composite**: Allows us to add keys for 2D axis input, like movement in 2D space.(`EmbeddedWorkflow.moveAction`)
- **Add Binding with one modifier**: Allows us to add keys with modifier keys, like Ctrl+C for copy.
- **Add Binding with two modifiers**: Allows us to add keys with two modifier keys, like Ctrl+Shift+S for save as.

This `InputAction` cannot work directly, we need to enable it first by calling `action.Enable()` in the script. Similarly, we have to disable it by calling `action.Disable()` when it is no longer needed to avoid errors.  
The Action can be subscribed to by 3 events: `Started`(on key press), `Performed`(first frame where key being pressed), and `Canceled`(on key released).  
- `Started`: runs when the action starts, e.g. when a button is pressed down or when a joystick is moved.
- `Performed`: runs when the action is performed, e.g. when a button is registered as pressed or when the joystick moves.
- `Canceled`: runs when the action is canceled, e.g. when a button is released.<br/>

So, `Started` and `Performed` are played in same frame, but `Started` is called before `Performed`.  
<br/>
Each Subscribed method gets the context of the action as parameter, which can be used to get more info about the action. This is of type `InputAction.CallbackContext`.<br/>
`InputAction.CallbackContext` can be used to get the value of the action as well as the action that triggered the event, most common functions/methods of this type include:
- `ReadValueAsButton()`: Read the current value of the action as a float and return true if it is equal to or greater than the button press threshold.
- `ReadValue<TValue>()`: Read the current value of the associated action.
- `ReadValueAsObject()`: Same as `ReadValue<TValue>()` except that it is not necessary to know the type of the value at compile time.

The `InputAction.ReadValue<TValue>()` method (`EmbeddedWorkflow.moveAction.ReadValue<Vector2>()`) will give you the current value of the action, where `TValue`(Vector2) is the type of the value you want to read. This allows us to ask for a value rather than waiting for the system to trigger the action.  
The `InputAction.CallbackContext` is only valid during the callback, so if you want to store the value for later use, you need to read it and store the value inside it in a variable, the option to read values directly from action by passes this concern if you want to get current value in something like `Update` loop.  

See [`EmbeddedWorkflow.cs`](Assets/NewInputSystemLearning/UsingActions/Scripts/EmbeddedWorkflow.cs) for an example of this type of setup.

Using Embedded Actions is good for scenarios where you have a fixed input handler and do not need to read inputs from multiple scripts.  
But if you want multiple scripts to read inputs, or want to have a more modular input system, you can use Input Action Assets.

##### Using Input Action Assets
Input Action Assets are assets that hold input actions and their bindings. The scripts can then use the reference to this asset to read input values. This is useful for modular input systems where multiple scripts need to read inputs.  
We can also create multiple action maps in the asset to group related actions together. For example, we can have a "Player" action map for player controls and a "UI" action map for UI controls. This allows us to enable/disable entire action maps based on the game state.  
This also enables us to have different control schemes for different platforms, like keyboard and mouse for PC and gamepad for consoles which we can swap between as needed.  
<br/><br/>
The 'Input Action' Asset has 3 sections:
- **Action Maps**: Group of related actions, like player controls, UI controls, etc
  - Can be enabled/disabled as a whole 
  - for example playerCar, playerNormal, playerInWater, etc
- **Actions**: Individual actions that can be performed, 
  - for example jump, move, shoot, etc
- Third Panel:
  - **Action Properties**: Properties of the selected action
    - Shows what action is selected and its properties
  - **Bindings**: Keys or buttons that trigger the actions 
    - can be multiple bindings for a single action from multiple devices or input sources.
    - for example WASD keys for move action, Space key for jump action, etc

When we select an 'action' in the Input Action Asset, we can see its properties in the *Action Properties* panel.  
In the Action Properpies panel, we can set the action type, control type(except button action type), interactions, processors, etc. which defines what types of bindings will be available to us.  
By default, we had 'Button' so we get button action type of bindings.
![img.png](Images/ActionProperties.png)

Control Schemes:  
All key bindings can belong to Control Schemes, which allows us to group bindings for different devices or input sources.  
The idea is that if a hardware is needed but not present, the input system can automatically ignore those bindings. This also helps when your UI shows the keys to press for action, like "press [E] to interact" or "press [X] to interact" depending n keyboard or Gamepad in use so Control scheme recognises the devices being used and can automatically change the key shown based on the current control scheme.

##### Using Input Action Assets in Script
To use Input Action Assets in script, we first need to create a reference to the asset in the script.  
This can be done by creating a public variable of type `InputActionAsset`
Each map, action, and binding in the asset can be accessed using their names or IDs.   
So, we can use 
- `InputActionMap` variables to store Action Map reference that are defined in the asset (`InputActionAsset.FindActionMap("<action map name>")`)  
- `InputAction` variables to store Action reference that are defined in any action map(`InputActionMap.FindAction("<action name>")`)  
Similarly, 
- we can enable/disable a complete `InputActionAsset` instead of enabling individual actions or action maps by calling `InputActionAsset.Enable()`  
- we can enable/disable a complete `InputActionMap` instead of enabling individual actions by calling `InputActionMap.Enable()`  

As the names of the action maps and actions are strings, it is prone to errors if we make a typo. To avoid this, we can use the generated C# class for the Input Action Asset.  
Select your Input Action Asset in the Project window, and in the Inspector window, check the "Generate C# Class" option and click on "Apply". This will generate a C# class that contains all the action maps and actions as properties.  
Now we can use the generated class in our script to access the action maps and actions without worrying about typos.
![img.png](Images/generateInputActionClass.png)  
We also do not need to worry about updates, as updating the Input Action Asset will automatically update the generated class.

See [`UsingInputActionAssetCode.cs`](Assets/NewInputSystemLearning/UsingInputActionAsset/Scripts/UsingInputActionAssetCode.cs) for an example of this type of setup.

