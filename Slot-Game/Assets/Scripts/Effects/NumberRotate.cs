using UnityEngine;
using System.Collections;

public class NumberRotate : MonoBehaviour
{
    [Header("”š‚Ìƒ‚ƒfƒ‹")]
    [SerializeField] private MeshFilter meshFilter;

    [Header("0`9‚ÌƒƒbƒVƒ…")]
    [SerializeField] private Mesh[] numberMeshes = new Mesh[10];

    [Header("‰ñ“]İ’è")]
    [SerializeField, Min(0.01f)]
    private float rotateTime = 0.3f;

    [SerializeField, Min(1)]
    private int rotationCount = 2;

    [SerializeField, Min(1)]
    private int numberLoopCount = 1;

    private int currentNumber;
    private bool isRotating;
    private Quaternion baseRotation;

    public bool IsRotating => isRotating;

    void Awake()
    {
        baseRotation = transform.localRotation;
        CenterMesh();
    }

    /// <summary>
    /// ‰‰o‚È‚µ‚Å”š‚ğİ’è
    /// </summary>
    public void SetNumber(int number)
    {
        currentNumber = Mathf.Clamp(number, 0, 9);

        SetMesh(currentNumber);
    }

    /// <summary>
    /// w’è‚³‚ê‚½”š‚Ü‚Å‰ñ“]
    /// </summary>
    public void RotateTo(int number)
    {
        number = Mathf.Clamp(number, 0, 9);

        if (isRotating)
            return;

        // “¯‚¶”š‚È‚ç‰ñ“]‚µ‚È‚¢
        if (currentNumber == number)
            return;

        StartCoroutine(RotateNumber(number));
    }

    private IEnumerator RotateNumber(int targetNumber)
    {
        isRotating = true;

        float elapsed = 0f;

        while (elapsed < rotateTime)
        {
            float t = Mathf.Clamp01(
                elapsed / rotateTime);

            // 0`9‚ğ‚‘¬‚ÅØ‚è‘Ö‚¦‚é
            int displayNumber =
                Mathf.FloorToInt(
                    t * 10 * numberLoopCount) % 10;

            SetMesh(displayNumber);

            // Y²‚Å‰¡‰ñ“]
            float angle =
                360f * rotationCount * t;

            transform.localRotation =
                baseRotation *
                Quaternion.Euler(0f, angle, 0f);

            elapsed += Time.deltaTime;

            yield return null;
        }

        // ÅI“I‚È”š‚ğ•\¦
        currentNumber = targetNumber;

        SetMesh(currentNumber);

        // ³–Ê‚É–ß‚·
        transform.localRotation = baseRotation;

        isRotating = false;
    }

    /// <summary>
    /// ƒƒbƒVƒ…‚ÌØ‚è‘Ö‚¦
    /// </summary>
    private void SetMesh(int number)
    {
        if (meshFilter == null)
            return;

        if (number < 0 || number >= numberMeshes.Length)
            return;

        if (numberMeshes[number] == null)
            return;

        if (meshFilter.sharedMesh == numberMeshes[number])
            return;

        meshFilter.sharedMesh = numberMeshes[number];

        CenterMesh();
    }

    /// <summary>
    /// ƒƒbƒVƒ…‚Ì’†S‚ğ‰ñ“]²‚É‡‚í‚¹‚é
    /// </summary>
    private void CenterMesh()
    {
        if (meshFilter == null ||
            meshFilter.sharedMesh == null)
            return;

        Vector3 meshCenter =
            meshFilter.sharedMesh.bounds.center;

        Vector3 worldCenter =
            meshFilter.transform.TransformPoint(meshCenter);

        Vector3 offset =
            transform.position - worldCenter;

        meshFilter.transform.position += offset;
    }
}