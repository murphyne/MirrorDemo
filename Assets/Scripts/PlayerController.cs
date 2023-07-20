using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform playerRoot;
    [SerializeField] private Transform playerCamera;

    [SerializeField] private float moveSpeed = 1;

    [SerializeField] private float lookSpeedX = 1;
    [SerializeField] private float lookSpeedY = 1;

    private void Update()
    {
        var inputV = Input.GetAxis("Vertical");
        var inputH = Input.GetAxis("Horizontal");
        var moveDirUnclamped = new Vector3(inputH, 0, inputV);
        var moveDirClamped = Vector3.ClampMagnitude(moveDirUnclamped, 1f);
        var moveOffset = moveDirClamped * (moveSpeed * Time.deltaTime);
        playerRoot.Translate(moveOffset);

        var inputX = Input.GetAxis("Mouse X");
        var inputY = Input.GetAxis("Mouse Y");
        var lookUp = Vector3.up * (inputX * lookSpeedX * Time.deltaTime);
        var lookRight = Vector3.right * (inputY * lookSpeedY * Time.deltaTime);
        playerRoot.transform.localEulerAngles += lookUp;
        playerCamera.transform.localEulerAngles += -lookRight;
    }
}
