using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChatDialog : MonoBehaviour
{
    private UIDocument _uiDocument;
    private List<VisualElement> _chatList;
    private int _currentIdx = 0;
    private bool _isClear = false;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        VisualElement root = _uiDocument.rootVisualElement;

        _chatList = root.Query<VisualElement>(className: "chat").ToList();

    }

    private void Update()
    {
        if(Input.GetButtonDown("Jump") && _isClear == false)
        {
            _chatList[_currentIdx].AddToClassList("on");

            _currentIdx = (_currentIdx + 1) % _chatList.Count;
            if(_currentIdx == 0)
            {
                _isClear = true;
            }
        }else if(Input.GetButtonDown("Jump") && _isClear)
        {
            ClearChat();
        }
    }

    private void ClearChat()
    {
        foreach(VisualElement c in _chatList)
        {
            c.RemoveFromClassList("on");
        }
        _isClear = false;
    }
}
