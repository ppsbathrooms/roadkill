using UnityEngine;

public class combineAttachment : MonoBehaviour
{
    [SerializeField, Range(0f, 360f)]
    private float rollerRotationSpeed = 180f;

    [Space][SerializeField] private GameObject rollerFront;
    [SerializeField] private GameObject rollerBack;

    void Update()
    {
        if (rollerFront != null)
        {
            rollerFront.transform.Rotate(Vector3.right, rollerRotationSpeed * Time.deltaTime);
        }
        if (rollerBack != null)
        {
            rollerBack.transform.Rotate(Vector3.right, -rollerRotationSpeed * Time.deltaTime);
        }
    }
}
