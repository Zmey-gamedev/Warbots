using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float rotationSpeed = 5f;
    public float smoothTime = 0.2f;

    int rotationStep = 0;
    private Rigidbody rb;
    private Vector3 lastMovementDirection = Vector3.forward; // Запази последната посока на движение

    public Transform gun;
    private Quaternion targetRotation;
    private Quaternion initialRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Определяме първоначалната ротация на gun
        initialRotation = gun.rotation;
        targetRotation = initialRotation;
    }

    private void Update()
    {
        GunRotation();
        PlayerMovement();
        RotatePlayerTowardsMovementDirection();
    }

    public void PlayerMovement()
    {
        // Движение по оста X и Y
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical) * movementSpeed;
        rb.velocity = movement;

        // Запазваме последната посока на движение
        if (rb.velocity != Vector3.zero)
        {
            lastMovementDirection = rb.velocity.normalized;
        }
    }

    public void RotatePlayerTowardsMovementDirection()
    {
        // Проверка дали играчът се движи
        if (rb.velocity != Vector3.zero)
        {
            Vector3 movementDirection = rb.velocity.normalized;

            // Завъртаме играча към посоката на движение с плавност
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            float t = rotationSpeed * Time.deltaTime / smoothTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, t);
        }
        else
        {
            // Завъртаме играча към последната посока на движение, когато е спрял
            Quaternion targetRotation = Quaternion.LookRotation(lastMovementDirection);
            float t = rotationSpeed * Time.deltaTime / smoothTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, t);
        }
    }

    public void GunRotation()
    {
        Vector3 upDirection = new Vector3(transform.position.x, transform.position.y, int.MaxValue);
        Vector3 upRightDirection = new Vector3(int.MinValue, transform.position.y, int.MaxValue);
        Vector3 rightDirection = new Vector3(int.MinValue, transform.position.y, transform.position.z);
        Vector3 downRightDirection = new Vector3(int.MinValue, transform.position.y, int.MinValue);
        Vector3 downDirection = new Vector3(transform.position.x, transform.position.y, int.MinValue);
        Vector3 downLeftDirection = new Vector3(int.MaxValue, transform.position.y, int.MinValue);
        Vector3 leftDirection = new Vector3(int.MaxValue, transform.position.y, transform.position.z);
        Vector3 upLeftDirection = new Vector3(int.MaxValue, transform.position.y, int.MaxValue);

        Vector3[] directions = { upDirection, upRightDirection, rightDirection, downRightDirection, downDirection, downLeftDirection, leftDirection, upLeftDirection };

        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            rotationStep -= 1;

            if (rotationStep < 0)
                rotationStep = directions.Length - 1;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            rotationStep += 1;
            rotationStep %= directions.Length;
        }
        
        // Определяме целевата ротация на gun
        targetRotation = Quaternion.LookRotation(directions[rotationStep] - gun.position);

        // Интерполация на ротацията с плавност
        float t = rotationSpeed * Time.deltaTime / smoothTime;
        gun.rotation = Quaternion.Slerp(gun.rotation, targetRotation, t);
    }
}
