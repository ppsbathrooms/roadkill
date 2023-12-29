using UnityEngine;

public class TireMarkSpawner : MonoBehaviour
{
    public WheelCollider[] wheelColliders;

    public float markFadeDuration = 2.0f;
    public float markDestroyAfter = 5.0f;
    public float markYOffset = 0.1f;

    private void Update()
    {
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            WheelHit hit;
            if (wheelColliders[i].GetGroundHit(out hit))
            {
                Vector3 position = hit.point + hit.normal * markYOffset;
                Quaternion rotation = Quaternion.LookRotation(-hit.forwardDir);

                SpawnTireMark(position, rotation, hit.forwardDir);
            }
        }
    }

    private void SpawnTireMark(Vector3 position, Quaternion rotation, Vector3 wheelForward)
    {
        GameObject tireMark = Instantiate(Settings.instance.tireMark, position, rotation, Settings.instance.tireMarkContainer);
        Quaternion markRotation = Quaternion.LookRotation(wheelForward);
        tireMark.transform.rotation = markRotation;

        DestroyAfterDuration destroyScript = tireMark.AddComponent<DestroyAfterDuration>();
        destroyScript.Init(markFadeDuration, markDestroyAfter);
    }
}

public class DestroyAfterDuration : MonoBehaviour
{
    private float fadeTimer = 0f;
    private float destroyTimer = 0f;

    private Renderer rend;
    private Color initialColor;

    public void Init(float fadeDuration, float destroyAfter)
    {
        rend = GetComponent<Renderer>();
        initialColor = rend.material.color;

        destroyTimer = destroyAfter;

        FadeOut(fadeDuration);
    }

    private void Update()
    {
        destroyTimer -= Time.deltaTime;

        if (destroyTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FadeOut(float fadeDuration)
    {
        if (rend == null) return;

        fadeTimer += Time.deltaTime;

        Color newColor = rend.material.color;
        newColor.a = Mathf.Lerp(initialColor.a, 0f, fadeTimer / fadeDuration);
        rend.material.color = newColor;
    }
}
