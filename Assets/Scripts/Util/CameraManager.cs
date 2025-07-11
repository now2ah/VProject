using System;
using UnityEngine;

namespace VProject.Utils
{
    public class CameraManager : MonoBehaviour
    {
        private const float PADDING_VALUE = 0f;

        [SerializeField] private Camera _mainCamera;

        private static CameraManager _instance;
        public static CameraManager Instance 
        {
            get
            {
                if (_instance == null)
                {
                    var instances = FindObjectsByType<CameraManager>(FindObjectsSortMode.None);

                    if (instances.Length > 0)
                    {
                        _instance = instances[0];
                    }
                    else
                    {
                        throw new Exception("There's no Camera Instance");
                    }
                }

                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void FocusOn(Vector3 center, float size)
        {
            _mainCamera.transform.position = new Vector3(center.x, center.y, -10f);

            _mainCamera.orthographicSize = size / 2 + PADDING_VALUE;
        }
    }
}