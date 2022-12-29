using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FlexLayout : MonoBehaviour
{
    private UIDocument _ui;
    private List<Justify> _optionList;
    private int _idx;
    private void Awake()
    {
        _ui = GetComponent<UIDocument>();
        _optionList = new List<Justify>();

        foreach (Justify value in Enum.GetValues(typeof(Justify)) ) {
            _optionList.Add(value);
        }
    }

    private void OnEnable()
    {
        Debug.Log("enable");
        VisualElement root = _ui.rootVisualElement;
        VisualElement container = root.Query<VisualElement>("Container");

        container.RegisterCallback<ClickEvent>(e =>
        {
            _idx = (_idx + 1) % _optionList.Count;
            Debug.Log(_optionList[_idx].ToString());
            container.style.justifyContent = _optionList[_idx];
        });
    }
}
