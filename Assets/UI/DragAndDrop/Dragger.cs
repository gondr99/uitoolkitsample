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
        if (CanStartManipulation(e)) //�׼������� ������ �����ϴ��� �˻�
        {
            _startPos = e.localMousePosition; //���� ��ǥ�ý��ۿ��� ���콺�� ��ǥ�� ��ȯ�Ѵ�.
            _originalPos = new Vector2(target.style.left.value.value , target.style.top.value.value);
            
            _isDragging = true;

            target.CaptureMouse(); //�ش� Ÿ���� ���콺�� ��°�

            e.StopPropagation(); //�̺�Ʈ ��������
        }
    }

    protected void OnMouseMove(MouseMoveEvent e)
    {
        if (_isDragging && target.HasMouseCapture())
        {
            Vector2 diff = e.localMousePosition - _startPos;

            //layout�� �θ�κ����� ������� ��ġ�� ���Ѵ�. �⺻�� �ȼ��̴ϱ� �� �־ �Ǳ��� new Length ����
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
        target.ReleaseMouse(); //��ġ��� ������ ������
        //Debug.Log($"{target.name} : ���� {e.localMousePosition}, ���� : {e.mousePosition}, ������ : {e.originalMousePosition}" );
        DropCallback?.Invoke(_originalPos, e.mousePosition);
    }
}
