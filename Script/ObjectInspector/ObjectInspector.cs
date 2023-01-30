using Cinemachine;
using UnityEngine;
public class ObjectInspector : MonoBehaviour
{
    private GameObject _inspectableObject;
    public CustomCharacterController customCharacterController;
    [SerializeField] private CinemachineVirtualCamera _cvc1, _cvc3;
    private CinemachinePOV pov, pov3;
    [Header("Кнопка включения")]
    [SerializeField] private KeyCode _openInspectionButton = KeyCode.F;
    [SerializeField] private KeyCode _closeInspectionButton = KeyCode.Mouse1;
    [Header("Pick up Settings")]
    [SerializeField] private Camera _maincamera;
    [SerializeField] private float _reachDistance = 1.5f;
    [SerializeField] private InspectionCamera _inspectionCamera;
    [SerializeField] private GameObject _inspectionCanvas, _mainCanvas;
    public void Start()
    {
        pov = _cvc1.GetCinemachineComponent<CinemachinePOV>();
        pov3 = _cvc3.GetCinemachineComponent<CinemachinePOV>();
    }
    public void OnF()
    {
        if (_inspectableObject == null)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Ray ray = _maincamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, _reachDistance))
                {
                    if (hit.collider.CompareTag("Item")) // если нет то
                    {
                        if (hit.collider.gameObject.GetComponent<Rigidbody>() != null)
                        {
                            _inspectableObject = Instantiate(hit.collider.gameObject, _inspectionCamera.transform.GetChild(0));
                            _inspectableObject.GetComponent<Rigidbody>().isKinematic = true;
                            InspectableObject inspectableObject = _inspectableObject.GetComponent<InspectableObject>();
                            inspectableObject.transform.localPosition = Vector3.zero + inspectableObject.spawnPositionOffset;
                            inspectableObject.transform.localRotation = Quaternion.Euler(Vector3.zero + inspectableObject.spawnRotationOffset);
                            _inspectionCanvas.SetActive(true);
                            _inspectionCamera.inspectableObject = inspectableObject;
                            _inspectionCamera.gameObject.SetActive(true);
                            _mainCanvas.SetActive(false);
                            TurnOffCameraMovement();
                        }
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Destroy(_inspectableObject);
            _inspectionCanvas.SetActive(false);
            _inspectionCamera.gameObject.SetActive(false);
            _mainCanvas.SetActive(true);
            TurnOnCameraMovement();
        }
    }
    private void TurnOnCameraMovement()
    {
        pov.m_HorizontalAxis.m_InputAxisName = "Mouse X";
        pov.m_VerticalAxis.m_InputAxisName = "Mouse Y";
        pov3.m_HorizontalAxis.m_InputAxisName = "Mouse X";
        pov3.m_VerticalAxis.m_InputAxisName = "Mouse Y";
        customCharacterController.enabled = true;
    }
    private void TurnOffCameraMovement()
    {
        pov.m_HorizontalAxis.m_InputAxisName = "";
        pov.m_VerticalAxis.m_InputAxisName = "";
        pov.m_HorizontalAxis.m_InputAxisValue = 0;
        pov.m_VerticalAxis.m_InputAxisValue = 0;
        pov3.m_HorizontalAxis.m_InputAxisName = "";
        pov3.m_VerticalAxis.m_InputAxisName = "";
        pov3.m_HorizontalAxis.m_InputAxisValue = 0;
        pov3.m_VerticalAxis.m_InputAxisValue = 0;
        customCharacterController.enabled = false;
    }
}
