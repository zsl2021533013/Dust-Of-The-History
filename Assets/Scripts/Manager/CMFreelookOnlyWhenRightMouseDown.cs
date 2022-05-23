using Cinemachine;
using UnityEngine;
public class CMFreelookOnlyWhenRightMouseDown : Singleton<CMFreelookOnlyWhenRightMouseDown>
{
    private CinemachineFreeLook cinemachineFreeLook;
    void Start()
    {
        cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
        CinemachineCore.GetInputAxis = GetAxisCustom;
    }
    public float GetAxisCustom(string axisName)
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {   
            if (cinemachineFreeLook.m_Lens.FieldOfView <= 50)
            {
                cinemachineFreeLook.m_Lens.FieldOfView += 1;
            }
            //cinemachineFreeLook.m_Lens.OrthographicSize
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (cinemachineFreeLook.m_Lens.FieldOfView >= 30)
            {
                cinemachineFreeLook.m_Lens.FieldOfView -= 1;
            }
        }

        if (axisName == "Mouse X")
        {
            if (Input.GetMouseButton(1))
            {
                return UnityEngine.Input.GetAxis("Mouse X");
            }
            else
            {
                return 0;
            }
        }
        else if (axisName == "Mouse Y")
        {
            if (Input.GetMouseButton(1))
            {
                return UnityEngine.Input.GetAxis("Mouse Y");
            }
            else
            {
                return 0;
            }
            
        }

        return UnityEngine.Input.GetAxis(axisName);
    }
}