using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class AutoScaleDownAndDestroy : MonoBehaviour {
    [FormerlySerializedAs("parentObj")] [SerializeField] private Transform parentObject;
    [SerializeField] private float totalDestroyTime = 5f;
    [SerializeField] private float scaleDownTime = 2f;
        
    private float startScale;
    
    void Start() {
        startScale = parentObject.GetChild(0).transform.localScale.x;
        StartCoroutine(ScaleDownAndDestroy());
    }

    private IEnumerator ScaleDownAndDestroy() {
        yield return new WaitForSeconds(totalDestroyTime - scaleDownTime);
            
        float startTime = Time.time;
        while (Time.time < startTime + scaleDownTime) {
            float percent = (Time.time - startTime) / scaleDownTime;

            foreach (Transform child in parentObject) {
                child.localScale = (1 - percent)*startScale*Vector3.one;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
