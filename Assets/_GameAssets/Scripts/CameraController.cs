using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float edgeSize = 20f; // kaç pixel kenar hassasiyeti

    [Header("Map Limits")]
    public float minX, maxX;
    public float minZ, maxZ;

    private Vector2 moveInput;

    private InputSystem_Actions inputActions;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    void LateUpdate()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        HandleMouseEdge(ref moveDirection);

        MoveCamera(moveDirection);
        ClampPosition();
    }

    void HandleMouseEdge(ref Vector3 dir)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        if (mousePos.x >= Screen.width - edgeSize)
            dir.x = 1;

        if (mousePos.x <= edgeSize)
            dir.x = -1;

        if (mousePos.y >= Screen.height - edgeSize)
            dir.z = 1;

        if (mousePos.y <= edgeSize)
            dir.z = -1;
    }

    void MoveCamera(Vector3 dir)
    {
        transform.position += dir.normalized * moveSpeed * Time.deltaTime;
    }

    void ClampPosition()
    {
        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);

        transform.position = pos;
    }
}
