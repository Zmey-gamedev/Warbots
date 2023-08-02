using UnityEngine;

public class TestScript : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float rotationSpeed = 5f;
    public float smoothTime = 0.2f;

    int rotationStep = 0;
    private Rigidbody rb;
    private Vector3 lastMovementDirection = Vector3.forward;

    public Transform gun;
    private Quaternion targetRotation;
    private Quaternion initialRotation;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialRotation = gun.rotation;
        targetRotation = initialRotation;

        // �������� Event Handlers ��� ��������� �� ���� �� ������������
        //InputManager.OnKeyPress += HandleKeyPress;
       // InputManager.OnKeyRelease += HandleKeyRelease;
    }

    private void OnDestroy()
    {
        // ���������� Event Handlers ����� ������������ �� ������
       // InputManager.OnKeyPress -= HandleKeyPress;
       // InputManager.OnKeyRelease -= HandleKeyRelease;
    }

    // ��������� �� ������� ��� ��������� �� ������
    private void HandleKeyPress(KeyCode key)
    {
        if (key == KeyCode.Keypad9)
        {
            rotationStep -= 1;
            if (rotationStep < 0)
                rotationStep = 7;
        }
        else if (key == KeyCode.Keypad7)
        {
            rotationStep += 1;
            rotationStep %= 8;
        }
    }

    // ��������� �� ������� ��� ��������� �� ������
    private void HandleKeyRelease(KeyCode key)
    {
        // ������ �����, ��� ���� ������ ������������ �� �������� ��� ��������� �� ������
    }

    // ������ Update(), ���������� FixedUpdate(), �� �� �� ������, �� PlayerMovement() �� ������� ������� � ��������
    private void FixedUpdate()
    {
        PlayerMovement();
        RotatePlayerTowardsMovementDirection();
    }

    public void PlayerMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical) * movementSpeed;
        rb.velocity = movement;

        if (rb.velocity != Vector3.zero)
        {
            lastMovementDirection = rb.velocity.normalized;
        }
    }

    public void RotatePlayerTowardsMovementDirection()
    {
        Vector3 movementDirection = rb.velocity.normalized;
        Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
        float t = rotationSpeed * Time.deltaTime / smoothTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, t);
    }

    public void GunRotation()
    {
        Vector3[] directions = {
            new Vector3(transform.position.x, transform.position.y, int.MaxValue),
            new Vector3(int.MinValue, transform.position.y, int.MaxValue),
            new Vector3(int.MinValue, transform.position.y, transform.position.z),
            new Vector3(int.MinValue, transform.position.y, int.MinValue),
            new Vector3(transform.position.x, transform.position.y, int.MinValue),
            new Vector3(int.MaxValue, transform.position.y, int.MinValue),
            new Vector3(int.MaxValue, transform.position.y, transform.position.z),
            new Vector3(int.MaxValue, transform.position.y, int.MaxValue)
        };

        targetRotation = Quaternion.LookRotation(directions[rotationStep] - gun.position);
        float t = rotationSpeed * Time.deltaTime / smoothTime;
        gun.rotation = Quaternion.Slerp(gun.rotation, targetRotation, t);
    }
}
