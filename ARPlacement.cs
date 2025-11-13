using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ARPlacement : MonoBehaviour
{
    public GameObject anatomyPrefab; // prefab to place
    public GameObject placementIndicator; // optional visual indicator
    public bool simulateInEditor = true; // use mouse fallback in editor
    public float editorPlaneY = 0f; // plane height for editor mode

    private ARRaycastManager arRaycastManager;
    private GameObject spawnedModel;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        // don't place when pointer over UI
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            if (placementIndicator) placementIndicator.SetActive(false);
            return;
        }

#if UNITY_EDITOR
        if (simulateInEditor)
        {
            UpdateEditorPlacement();
            return;
        }
#endif

        UpdateARPlacement();
    }

    void UpdateARPlacement()
    {
        if (arRaycastManager == null) return;

        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

        if (arRaycastManager.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes))
        {
            var hitPose = hits[0].pose;

            if (placementIndicator)
            {
                placementIndicator.SetActive(true);
                placementIndicator.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
            }

            if (spawnedModel == null && Input.touchCount > 0 && Input.GetTouch(0).phase == UnityEngine.TouchPhase.Began)
            {
                PlaceModel(hitPose.position, hitPose.rotation);
            }
        }
        else
        {
            if (placementIndicator) placementIndicator.SetActive(false);
        }
    }

    void UpdateEditorPlacement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.up * editorPlaneY);

        if (plane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);

            if (placementIndicator)
            {
                placementIndicator.SetActive(true);
                placementIndicator.transform.position = hitPoint;
                placementIndicator.transform.rotation = Quaternion.identity;
            }

            if (spawnedModel == null && Input.GetMouseButtonDown(0))
            {
                PlaceModel(hitPoint, Quaternion.identity);
            }
        }
        else
        {
            if (placementIndicator) placementIndicator.SetActive(false);
        }
    }

    public void PlaceModel(Vector3 position, Quaternion rotation)
    {
        if (anatomyPrefab == null) return;
        spawnedModel = Instantiate(anatomyPrefab, position, rotation);
    }

    public GameObject GetSpawnedModel() => spawnedModel;

    public void RemoveModel()
    {
        if (spawnedModel != null)
        {
            Destroy(spawnedModel);
            spawnedModel = null;
        }
    }

    // Save/restore helpers
    public void SaveModelTransform()
    {
        if (spawnedModel == null) return;

        PlayerPrefs.SetFloat("ModelPosX", spawnedModel.transform.position.x);
        PlayerPrefs.SetFloat("ModelPosY", spawnedModel.transform.position.y);
        PlayerPrefs.SetFloat("ModelPosZ", spawnedModel.transform.position.z);

        PlayerPrefs.SetFloat("ModelRotX", spawnedModel.transform.rotation.x);
        PlayerPrefs.SetFloat("ModelRotY", spawnedModel.transform.rotation.y);
        PlayerPrefs.SetFloat("ModelRotZ", spawnedModel.transform.rotation.z);
        PlayerPrefs.SetFloat("ModelRotW", spawnedModel.transform.rotation.w);

        PlayerPrefs.Save();
    }

    public void LoadModelTransform()
    {
        if (spawnedModel == null) return;
        if (!PlayerPrefs.HasKey("ModelPosX")) return;

        spawnedModel.transform.position = new Vector3(
            PlayerPrefs.GetFloat("ModelPosX"),
            PlayerPrefs.GetFloat("ModelPosY"),
            PlayerPrefs.GetFloat("ModelPosZ")
        );

        spawnedModel.transform.rotation = new Quaternion(
            PlayerPrefs.GetFloat("ModelRotX"),
            PlayerPrefs.GetFloat("ModelRotY"),
            PlayerPrefs.GetFloat("ModelRotZ"),
            PlayerPrefs.GetFloat("ModelRotW")
        );
    }
}
