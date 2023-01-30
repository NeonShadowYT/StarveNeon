using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraControllerPanel : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
    public CustomCharacterController customCharacterController;

    public CinemachineVirtualCamera CVC, CVC3;
    private CinemachinePOV pov, pov3;
    private float sensitivity;

    private bool pressed = false;
    private bool isMobile = true;

    private int fingerId;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Sensitivity")) sensitivity = PlayerPrefs.GetFloat("Sensitivity"); else sensitivity = 1f;

        pov = CVC.GetCinemachineComponent<CinemachinePOV>();
        pov3 = CVC3.GetCinemachineComponent<CinemachinePOV>();

        pov.m_HorizontalAxis.m_MaxSpeed = sensitivity;
        pov.m_VerticalAxis.m_MaxSpeed = sensitivity;
        pov3.m_HorizontalAxis.m_MaxSpeed = sensitivity;
        pov3.m_VerticalAxis.m_MaxSpeed = sensitivity;

        if (customCharacterController.PC == false) isMobile = true; else isMobile = false;

        if (isMobile == true)
        {
            pov.m_HorizontalAxis.m_InputAxisName = "";
            pov.m_VerticalAxis.m_InputAxisName = "";
            pov3.m_HorizontalAxis.m_InputAxisName = "";
            pov3.m_VerticalAxis.m_InputAxisName = "";
        }
        else
        {
            pov.m_HorizontalAxis.m_InputAxisName = "Mouse X";
            pov.m_VerticalAxis.m_InputAxisName = "Mouse Y";
            pov3.m_HorizontalAxis.m_InputAxisName = "Mouse X";
            pov3.m_VerticalAxis.m_InputAxisName = "Mouse Y";
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.pointerCurrentRaycast.gameObject == gameObject)
        {
            pressed = true;
            fingerId = eventData.pointerId;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;

        pov.m_VerticalAxis.m_InputAxisValue = 0;
        pov.m_HorizontalAxis.m_InputAxisValue = 0;
        pov3.m_VerticalAxis.m_InputAxisValue = 0;
        pov3.m_HorizontalAxis.m_InputAxisValue = 0;
    }
    void Update()
    {
        if (pressed)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.fingerId == fingerId)
                {
                    if (touch.phase == TouchPhase.Moved)
                    {
                        pov.m_VerticalAxis.m_InputAxisValue = touch.deltaPosition.y;
                        pov.m_HorizontalAxis.m_InputAxisValue = touch.deltaPosition.x;
                        pov3.m_VerticalAxis.m_InputAxisValue = touch.deltaPosition.y;
                        pov3.m_HorizontalAxis.m_InputAxisValue = touch.deltaPosition.x;
                    }
                    if (touch.phase == TouchPhase.Stationary)
                    {
                        pov.m_VerticalAxis.m_InputAxisValue = 0;
                        pov.m_HorizontalAxis.m_InputAxisValue = 0;
                        pov3.m_VerticalAxis.m_InputAxisValue = 0;
                        pov3.m_HorizontalAxis.m_InputAxisValue = 0;
                    }
                }
            }
        }
    }
}
