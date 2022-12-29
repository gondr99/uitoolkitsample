using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthUI : MonoBehaviour
{
    [SerializeField]
    private Transform _playerTrm;

    private UIDocument _document;

    private VisualElement _root;
    private VisualElement _healthBar;
    private VisualElement _bar;

    private Camera _mainCam;
    
    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        _mainCam = Camera.main;
    }

    private void OnEnable()
    {
        _root = _document.rootVisualElement;
        _healthBar = _root.Q<VisualElement>("health_bar");
        _bar = _root.Q<VisualElement>("bar");
    }

    private void LateUpdate()
    {
        Vector3 worldPos = _playerTrm.position;
        Vector3 UIpos = RuntimePanelUtils.CameraTransformWorldToPanel(_root.panel, worldPos, _mainCam);

        float half = _healthBar.layout.width * 0.5f;

        _healthBar.style.left = UIpos.x - half;
        _healthBar.style.top = UIpos.y - 100;
    }

    public void OnChangeHealth(float normalizedHealth)
    {
        _bar.style.width = new Length(normalizedHealth * 100, LengthUnit.Percent);
    }
}
