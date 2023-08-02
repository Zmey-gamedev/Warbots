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

        // Добавяме Event Handlers към събитията за вход от клавиатурата
        //InputManager.OnKeyPress += HandleKeyPress;
       // InputManager.OnKeyRelease += HandleKeyRelease;
    }

    private void OnDestroy()
    {
        // Премахваме Event Handlers преди унищожението на обекта
       // InputManager.OnKeyPress -= HandleKeyPress;
       // InputManager.OnKeyRelease -= HandleKeyRelease;
    }

    // Обработка на събития при натискане на клавиш
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

    // Обработка на събития при отпускане на клавиш
    private void HandleKeyRelease(KeyCode key)
    {
        // Празен метод, тъй като нямаме необходимост от действия при отпускане на клавиш
    }

    // Вместо Update(), използваме FixedUpdate(), за да се уверим, че PlayerMovement() се извиква редовно и правилно
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
