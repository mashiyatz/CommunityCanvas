using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class CommentCreator : MonoBehaviour
{
    public TMP_InputField commentInput;
    public TMP_InputField authorInput;

    [SerializeField]
    private StateManager stateManager;

    private string comment;
    private string author;

    [SerializeField]
    private GameObject stickyNotePrefab;

    public void SetComment()
    {
        comment = commentInput.text;
        author = authorInput.text;
    }

    public void CreateStickyNote(Vector3 loc)
    {
        Instantiate(stickyNotePrefab, loc, Quaternion.identity, stateManager.origin.trackablesParent.transform);
    }
}
