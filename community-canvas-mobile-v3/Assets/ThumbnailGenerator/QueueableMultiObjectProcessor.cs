using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThumbGen
{
    public class QueueableMultiObjectProcessor : MonoBehaviour
    {
        private List<IGameObjectActivity> _activities;

        private IGameObjectActivity _curActivity;

        [SerializeField]
        private GameObject[] modelPrefabs;

        [SerializeField]
        private string folderName;

        void Start()
        {

        }

        void Awake()
        {
            _curActivity = null;
            _activities = new List<IGameObjectActivity>();

            ThumbnailGenerator2 thumbGen = GetComponent<ThumbnailGenerator2>();

            foreach (GameObject go in modelPrefabs)
            {
                var thumbActivity = new ThumbnailActivity(thumbGen, $"{folderName}/{go.name}");
                QueueActivity(thumbActivity);
            }
        }

        public void QueueActivity(IGameObjectActivity activity)
        {
            StartCoroutine(DoQueueActivity(activity));
        }

        IEnumerator DoQueueActivity(IGameObjectActivity activity)
        {
            yield return new WaitUntil(() => _curActivity == null);

            _activities.Add(activity);
        }

        void Update()
        {
            if (_curActivity == null)
            {
                if (_activities.Count > 0)
                {
                    _curActivity = _activities[0];

                    _curActivity.Setup();

                    StartCoroutine("DoProcess");

                    _activities.RemoveAt(0);
                }
            }
        }

        IEnumerator DoProcess()
        {
            yield return new WaitForEndOfFrame();

            if (_curActivity == null)
                yield return null;

            if (!_curActivity.CanProcess())
                yield return null;

            _curActivity.Process();

            StartCoroutine(DoCleanup());
        }

        IEnumerator DoCleanup()
        {
            yield return new WaitForEndOfFrame();

            if (_curActivity != null)
            {
                _curActivity.Cleanup();
                _curActivity = null;
            }
        }
    }
}
