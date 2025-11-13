using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AnatomyModelController : MonoBehaviour
{
    [Header("Layers (assign GameObjects)")]
    public GameObject layerSkin;
    public GameObject layerMuscles;
    public GameObject layerSkeleton;
    public GameObject layerOrgans;
    public GameObject layerVessels;

    [Header("Annotation UI")]
    public GameObject annotationPanel;
    public Text annotationTitle;
    public Text annotationBody;
    public Image annotationImage;
    public GameObject annotationRoot; // root canvas

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        HideAnnotation();
    }

    void Update()
    {
        // Mouse click (Editor)
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                AnatomyPart part = hit.collider.GetComponentInParent<AnatomyPart>();
                if (part != null)
                {
                    ShowAnnotation(part);
                }
            }
        }

        // Touch (Mobile)
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began && !IsPointerOverUI())
            {
                Ray ray = mainCamera.ScreenPointToRay(t.position);
                if (Physics.Raycast(ray, out RaycastHit hit, 100f))
                {
                    AnatomyPart part = hit.collider.GetComponentInParent<AnatomyPart>();
                    if (part != null) ShowAnnotation(part);
                }
            }
        }
    }

    public void ToggleLayer(string layerName, bool isOn)
    {
        switch (layerName)
        {
            case "Skin": if (layerSkin) layerSkin.SetActive(isOn); break;
            case "Muscles": if (layerMuscles) layerMuscles.SetActive(isOn); break;
            case "Skeleton": if (layerSkeleton) layerSkeleton.SetActive(isOn); break;
            case "Organs": if (layerOrgans) layerOrgans.SetActive(isOn); break;
            case "Vessels": if (layerVessels) layerVessels.SetActive(isOn); break;
        }
    }

    public void ShowAnnotation(AnatomyPart part)
    {
        if (annotationPanel == null) return;
        annotationTitle.text = part.partName;
        annotationBody.text = part.description;

        if (part.image != null)
        {
            annotationImage.sprite = part.image;
            annotationImage.gameObject.SetActive(true);
        }
        else annotationImage.gameObject.SetActive(false);

        annotationPanel.SetActive(true);
    }

    public void HideAnnotation()
    {
        if (annotationPanel) annotationPanel.SetActive(false);
    }

    bool IsPointerOverUI()
    {
        if (EventSystem.current == null) return false;
#if UNITY_EDITOR
        return EventSystem.current.IsPointerOverGameObject();
#else
        if (Input.touchCount > 0)
            return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        return false;
#endif
    }
}
