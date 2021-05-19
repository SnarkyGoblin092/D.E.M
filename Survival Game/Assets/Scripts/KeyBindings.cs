using UnityEngine;

[CreateAssetMenu(fileName = "KeyBindings", menuName = "KeyBindings")]
public class KeyBindings : ScriptableObject
{
    public KeyCode moveForward;
    public KeyCode moveBackward;
    public KeyCode moveRight;
    public KeyCode moveLeft;
    public KeyCode jump;
    public KeyCode run;
    public KeyCode inventory;
    public KeyCode interact;

    public KeyCode GetKey(string key)
    {
        switch (key)
        {
            case "forward":
                return moveForward;
            case "backward":
                return moveBackward;
            case "right":
                return moveRight;
            case "left":
                return moveLeft;
            case "jump":
                return jump;
            case "run":
                return run;
            case "inventory":
                return inventory;
            case "interact":
                return interact;
            default:
                return KeyCode.None;
        }
    }
}
