using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPivotController : MonoBehaviour
{
    [SerializeField] private float horizontalSensitivity;
    [SerializeField] private float verticalSensitivity;

    [SerializeField] private Transform orientation;

    private float x_rotation;
    private float y_rotation;

    private bool followMouse;

    void Start()
    {
        LockMouse();
    }

    void Update()
    {
        if (followMouse) {
            float mouse_x = Input.GetAxisRaw("Mouse X") * Time.deltaTime * horizontalSensitivity;
            float mouse_y = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * verticalSensitivity;

            y_rotation += mouse_x;
            x_rotation -= mouse_y;

            x_rotation = Mathf.Clamp(x_rotation, -90f, 90f);


            transform.rotation = Quaternion.Euler(x_rotation, y_rotation, 0);
            orientation.rotation = Quaternion.Euler(0, y_rotation, 0);
        }
    }

    public void LockMouse() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        followMouse = true;
    }

    public void UnlockMouse() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        followMouse = false;
    }
}
