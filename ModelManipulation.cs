using UnityEngine;

public class ModelManipulation : MonoBehaviour
{
    public Transform modelRoot;
    public float rotateSpeed = 0.2f;
    public float pinchSpeed = 0.01f;
    public float minScale = 0.1f;
    public float maxScale = 3f;

    private float prevDistance = 0f;

    void Update()
    {
        if (modelRoot == null) return;

#if UNITY_EDITOR
        // Rotate with right mouse, scale with scroll
        if (Input.GetMouseButton(1))
        {
            float dx = Input.GetAxis("Mouse X");
            modelRoot.Rotate(Vector3.up, -dx * rotateSpeed * 100f, Space.World);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0f)
        {
            Vector3 s = modelRoot.localScale;
            s *= (1f + scroll);
            s = ClampScale(s);
            modelRoot.localScale = s;
        }
#else
        // Touch rotate and pinch zoom
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Moved)
            {
                modelRoot.Rotate(Vector3.up, -t.deltaPosition.x * rotateSpeed, Space.World);
            }
        }
        else if (Input.touchCount == 2)
        {
            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);

            float curDistance = Vector2.Distance(t1.position, t2.position);

            if (prevDistance > 0)
            {
                float delta = curDistance - prevDistance;
                Vector3 s = modelRoot.localScale;
                s *= (1f + delta * pinchSpeed * Time.deltaTime * 60f);
                s = ClampScale(s);
                modelRoot.localScale = s;
            }
            prevDistance = curDistance;
        }
        else
        {
            prevDistance = 0;
        }
#endif
    }

    private Vector3 ClampScale(Vector3 s)
    {
        s.x = Mathf.Clamp(s.x, minScale, maxScale);
        s.y = Mathf.Clamp(s.y, minScale, maxScale);
        s.z = Mathf.Clamp(s.z, minScale, maxScale);
        return s;
    }
}
