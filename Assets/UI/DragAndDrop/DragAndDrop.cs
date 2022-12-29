using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DragAndDrop : MonoBehaviour
{
    private UIDocument _document;
    private Camera _mainCam;
    private VisualElement _potion;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        
        _mainCam = Camera.main;
    }

    private void OnEnable()
    {
        VisualElement root = _document.rootVisualElement;

        _potion = root.Q<VisualElement>("potion");
        _potion.AddManipulator(new Dragger(PotionDrop));
    }

    private void Update()
    {
        
    }
    private void PotionDrop(Vector2 startPos, Vector2 endPos)
    {
        //Vector2 worldEndPos = _potion.parent.LocalToWorld(endPos);
        Vector2 endScreenPoint = new Vector2(endPos.x, Screen.height - endPos.y);//y축 반전이라 이렇게 처리
        
        Ray ray = _mainCam.ScreenPointToRay(endScreenPoint);
        RaycastHit hit;
        int playerLayer = LayerMask.NameToLayer("Player");

        bool isRayHit = Physics.Raycast(ray, out hit, _mainCam.farClipPlane, 1 << playerLayer);

        if(isRayHit)
        {
            _potion.parent.Remove(_potion);
            Player p = hit.collider.GetComponent<Player>();
            if(p != null)
            {
                p.ChangeHealth(20); //20만큼 증가
            }
        }else
        {
            _potion.style.left = startPos.x;
            _potion.style.top = startPos.y;
        }
    }
}
