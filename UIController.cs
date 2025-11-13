using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public AnatomyModelController modelController;
    public ARPlacement placementController;

    public Toggle skinToggle, musclesToggle, skeletonToggle, organsToggle, vesselsToggle;
    public Button placeButton, saveButton, resetButton, hideAnnotationButton;

    void Start()
    {
        if (skinToggle) skinToggle.onValueChanged.AddListener(v => modelController.ToggleLayer("Skin", v));
        if (musclesToggle) musclesToggle.onValueChanged.AddListener(v => modelController.ToggleLayer("Muscles", v));
        if (skeletonToggle) skeletonToggle.onValueChanged.AddListener(v => modelController.ToggleLayer("Skeleton", v));
        if (organsToggle) organsToggle.onValueChanged.AddListener(v => modelController.ToggleLayer("Organs", v));
        if (vesselsToggle) vesselsToggle.onValueChanged.AddListener(v => modelController.ToggleLayer("Vessels", v));

        if (placeButton) placeButton.onClick.AddListener(() =>
        {
            if (placementController != null && placementController.GetSpawnedModel() == null &&
                placementController.placementIndicator != null)
            {
                var indicator = placementController.placementIndicator.transform;
                placementController.PlaceModel(indicator.position, indicator.rotation);
            }
        });

        if (saveButton) saveButton.onClick.AddListener(() => placementController?.SaveModelTransform());
        if (resetButton) resetButton.onClick.AddListener(() =>
        {
            placementController?.RemoveModel();
            modelController.HideAnnotation();
        });

        if (hideAnnotationButton) hideAnnotationButton.onClick.AddListener(() => modelController.HideAnnotation());
    }
}
