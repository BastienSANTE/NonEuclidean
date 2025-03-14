using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

// Use action set asset instead of lose InputActions directly on component.
public class SimpleController_UsingActionAsset : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed;

    private SimpleControls m_Controls;
    private bool m_Charging;
    private Vector2 m_Rotation;

    public void Awake()
    {
        m_Controls = new SimpleControls();
    }

    public void OnEnable()
    {
        m_Controls.Enable();
    }

    public void OnDisable()
    {
        m_Controls.Disable();
    }

    public void Update()
    {
        var look = m_Controls.gameplay.look.ReadValue<Vector2>();
        var move = m_Controls.gameplay.move.ReadValue<Vector2>();

        // Update orientation first, then move. Otherwise move orientation will lag
        // behind by one frame.
        Look(look);
        Move(move);
    }

    private void Move(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01)
            return;
        var scaledMoveSpeed = moveSpeed * Time.deltaTime;
        // For simplicity's sake, we just keep movement in a single plane here. Rotate
        // direction according to world Y rotation of player.
        var move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);
        transform.position += move * scaledMoveSpeed;
    }

    private void Look(Vector2 rotate)
    {
        if (rotate.sqrMagnitude < 0.01)
            return;
        var scaledRotateSpeed = rotateSpeed * Time.deltaTime;
        m_Rotation.y += rotate.x * scaledRotateSpeed;
        m_Rotation.x = Mathf.Clamp(m_Rotation.x - rotate.y * scaledRotateSpeed, -89, 89);
        transform.localEulerAngles = m_Rotation;
    }
}
