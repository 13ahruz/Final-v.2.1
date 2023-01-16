using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;

namespace CASP.CameraManager
{

    public class CameraManager : MonoBehaviour
    {
        public static CameraManager instance;

        [SerializeField] CinemachineBrain Brain;
        // [SerializeField] List<CinemachineVirtualCamera> VCamList;
        Dictionary<string, CinemachineFreeLook> Cams = new Dictionary<string, CinemachineFreeLook>();
        [SerializeField] List<StructCam> Camslist;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }
        void Start()
        {
            foreach (Transform camTransform in transform)
            {
                if (camTransform.TryGetComponent<CinemachineFreeLook>(out CinemachineFreeLook vCam))
                {
                    // VCamList.Add(vCam);
                    AddCamera(vCam, Cams, Camslist);
                }
            }
        }

        void AddCamera(CinemachineFreeLook _vCam, Dictionary<string, CinemachineFreeLook> _camDict, List<StructCam> _camsList)
        {
            // Struct Add
            StructCam s = new StructCam();
            s.Key = _vCam.name;
            s.Value = _vCam;
            _camsList.Add(s);

            // Dictionary Add
            _camDict.Add(_vCam.name, _vCam);
        }

        public void OpenCamera(string CameraName)
        {

            Cams.FirstOrDefault(x => x.Key == CameraName).Value.Priority = 11;

            foreach (var item in Cams)
            {
                // item.Value.Priority = item.Key == CameraName ? 11 : 10;
                if (item.Key != CameraName)
                {
                    item.Value.Priority = 10;
                }
            }
        }

        public void OpenCamera(string CameraName, float Time, CameraEaseStates CameraEase)
        {
            Brain.m_DefaultBlend.m_Time = Time;
            Brain.m_DefaultBlend.m_Style = (CinemachineBlendDefinition.Style)CameraEase;

            Cams.FirstOrDefault(x => x.Key == CameraName).Value.Priority = 11;

            foreach (var item in Cams)
            {
                // item.Value.Priority = item.Key == CameraName ? 11 : 10;
                if (item.Key != CameraName)
                {
                    item.Value.Priority = 10;
                }
            }
        }

        public void SetFollow(string CameraName, Transform ObjectTransform)
        {
            Cams.FirstOrDefault(x => x.Key == CameraName).Value.Follow = ObjectTransform;
        }

        public void SetLookAt(string CameraName, Transform ObjectTransform)
        {
            Cams.FirstOrDefault(x => x.Key == CameraName).Value.LookAt = ObjectTransform;
        }
    }

    [System.Serializable]
    public struct StructCam
    {
        public string Key;
        public CinemachineFreeLook Value;
    }

    public enum CameraEaseStates
    {
        Cut = 0,
        EaseInOut = 1,
        EaseIn = 2,
        EaseOut = 3,
        HardIn = 4,
        HardOut = 5,
        Linear = 6,
        Custom = 7
    }
}
