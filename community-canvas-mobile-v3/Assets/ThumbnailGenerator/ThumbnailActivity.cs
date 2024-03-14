using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThumbGen
{
    /// <summary>
    /// This class is exactly like ObjectThumbnailActivity.cs, but 
    /// this now implements the IGameObjectActivity interface.
    /// </summary>
    public class ThumbnailActivity : IGameObjectActivity
    {
        private GameObject _object;
        private ThumbnailGenerator2 _thumbGen;

        private string _resourceName;

        public ThumbnailActivity(ThumbnailGenerator2 thumbGen, string resourceName)
        {
            _thumbGen = thumbGen;
            _resourceName = resourceName;
        }

        private void FitBoundsInCamera(Camera camera)
        {
            float cameraDistance = 1.0f; // Constant factor
            Bounds bounds = _object.GetComponent<MeshRenderer>().bounds;

            Vector3 objectSizes = bounds.max - bounds.min;
            float objectSize = Mathf.Max(objectSizes.x, objectSizes.y, objectSizes.z);
            float cameraView = 2.0f * Mathf.Tan(0.5f * Mathf.Deg2Rad * camera.fieldOfView); // Visible height 1 meter in front
            float distance = cameraDistance * objectSize / cameraView; // Combined wanted distance from the object
            distance += 0.5f * objectSize; // Estimated offset from the center to the outside of the object
            camera.transform.position = bounds.center - distance * camera.transform.forward;
        }

        public bool Setup()
        {
            if (string.IsNullOrWhiteSpace(_resourceName))
                return false;

            GameObject prefab = Resources.Load<GameObject>(_resourceName);

            GameObject stage = GameObject.Find("Stage");

            _object = GameObject.Instantiate(prefab, stage?.transform);
            // get camera to look at _object

            Camera cam = _thumbGen.ThumbnailCamera.GetComponent<Camera>();
            cam.transform.position = _thumbGen.thumbnailCameraStartPos;
       
            // cam.transform.LookAt(stage.transform);
            cam.transform.LookAt(_object.transform);
            FitBoundsInCamera(cam);

            return true;
        }

        public bool CanProcess()
        {
            return true;
        }

        public void Process()
        {
            if (_thumbGen == null)
                return;

            string filename = string.Format("render_objectloader_{0}", _resourceName);
            _thumbGen.Render(filename);
        }

        public void Cleanup()
        {
            if (_object != null)
            {
                GameObject.Destroy(_object);

                _object = null;
            }
        }
    }
}
