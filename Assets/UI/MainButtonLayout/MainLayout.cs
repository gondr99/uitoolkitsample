using UnityEngine;
using UnityEngine.UIElements;

public class MainLayout : MonoBehaviour
{
    private VisualElement _root;
    private void OnEnable()
    {
        UIDocument ui = GetComponent<UIDocument>();
        _root = ui.rootVisualElement;

        Button btn = _root.Q<Button>("myBtn");
        btn.RegisterCallback<ClickEvent>(e =>
        {
            btn.style.backgroundColor = Color.yellow;
        });
    }
}
