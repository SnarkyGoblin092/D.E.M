using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] GameObject player;

    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    [SerializeField] Transform cam;
    [SerializeField] Transform orientation;

    float mouseX;
    float mouseY;

    float multiplier = 0.1f;

    float xRotation;
    float yRotation;

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if(player.GetComponent<PlayerMovement>().canMove)
        {
            MyInput();

            cam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
            orientation.transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
        }
    }

    void MyInput()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation += mouseX * sensX * multiplier;
        xRotation -= mouseY * sensY * multiplier;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }
}
