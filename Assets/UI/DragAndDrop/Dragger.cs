using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Dragger : MouseManipulator
{
    private Vector2 _startPos;
    private bool _isDragging = false;
    private Vector2 _originalPos;

    private Action<Vector2, Vector2> DropCallback;

    public Dragger(Action<Vector2, Vector2> DropCallback = null)
    {
        _isDragging = false;
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        this.DropCallback = DropCallback;
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        target.RegisterCallback<MouseUpEvent>(OnMouseUp);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
        target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
    }

    protected void OnMouseDown(MouseDownEvent e)
    {
        if (CanStartManipulation(e)) //액션필터의 조건을 충족하는지 검사
        {
            _startPos = e.localMousePosition; //현재 좌표시스템에서 마우스의 좌표를 반환한다.
            _originalPos = new Vector2(target.style.left.value.value , target.style.top.value.value);
            
            _isDragging = true;

            target.CaptureMouse(); //해당 타겟이 마우스를 잡는거

            e.StopPropagation(); //이벤트 전파중지
        }
    }

    protected void OnMouseMove(MouseMoveEvent e)
    {
        if (_isDragging && target.HasMouseCapture())
        {
            Vector2 diff = e.localMousePosition - _startPos;

            //layout은 부모로부터의 상대적인 위치를 말한다. 기본은 픽셀이니까 걍 넣어도 되긴해 new Length 없이
            target.style.top = new Length(target.layout.y + diff.y, LengthUnit.Pixel);
            target.style.left = new Length(target.layout.x + diff.x, LengthUnit.Pixel);
        }   
    }

    protected void OnMouseUp(MouseUpEvent e)
    {
        if(!_isDragging || !target.HasMouseCapture())
        {
            return;
        }

        _isDragging = false;
        target.ReleaseMouse(); //터치라면 릴리즈 포인터
        //Debug.Log($"{target.name} : 로컬 {e.localMousePosition}, 월드 : {e.mousePosition}, 오리진 : {e.originalMousePosition}" );
        DropCallback?.Invoke(_originalPos, e.mousePosition);
    }
}
