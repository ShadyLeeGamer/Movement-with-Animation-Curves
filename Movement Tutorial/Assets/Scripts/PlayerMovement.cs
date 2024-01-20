using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] MotionController movementController;

    Vector2 dirInputSmooth;
    Vector2 dirInputTarget;
    Vector2 dirInputSmoothVelocity;
    [SerializeField] float dirInputSmoothTime;

    private void Update()
    {
        Vector2 dirInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"));
        bool isMoving = dirInput != Vector2.zero;
        if (isMoving)
        {
            dirInputTarget = dirInput;
        }
        dirInputSmooth = Vector2.SmoothDamp(dirInputSmooth, dirInputTarget, ref dirInputSmoothVelocity, dirInputSmoothTime);

        movementController.Update(isMoving);

        Vector3 moveDirection = Vector3.right * dirInputSmooth.x + Vector3.forward * dirInputSmooth.y;
        Vector3 moveVelocity = moveDirection * movementController.Speed;
        transform.Translate(moveVelocity * Time.deltaTime);
    }
}