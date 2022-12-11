using System;
using System.Collections.Generic;
using Loadson;
using LoadsonAPI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ThirdPerson
{
    public class Main : Mod
    {
        public override void OnEnable()
        {
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.buildIndex >= 2)
            {
                if (!camera && !player && !bean && !cameraShake)
                {
                    camera = GameObject.Find("Camera");
                    player = GameObject.Find("Player");
                    bean = player.GetComponent<MeshRenderer>();
                    cameraShake = GameObject.Find("Camera").transform.Find("Main Camera").GetComponent<EZCameraShake.CameraShaker>();
                }

                if (camera && bean && cameraShake)
                {
                    bean.enabled = true;
                    cameraShake.enabled = false;
                }
            }
        }

        public bool thirdPerson = false;
        private GameObject player;
        private MeshRenderer bean;
        private GameObject camera;
        private EZCameraShake.CameraShaker cameraShake;

        public override void Update(float _)
        {
            if (Input.GetKeyUp(KeyCode.M))
                thirdPerson = !thirdPerson;

            if (camera)
            {
                camera.transform.Find("Main Camera").localPosition = thirdPerson ? new Vector3(0, 1.5f, -6f) : new Vector3(0, 0, 0);
                camera.transform.Find("Main Camera/GunCam").localPosition = thirdPerson ? new Vector3(0, 1f, -10f) : new Vector3(0, 0, 0);
            }
        }
    }
}
