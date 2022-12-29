using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CardSelector : MonoBehaviour
{
    [SerializeField]
    private List<SkillSO> _skillList;

    private UIDocument _document;

    private VisualElement _activeSlotMarker;
    private Label _nameLabel;
    private Label _descLabel;
    private Label _tierLabel;
    private Label _tierDescLabel;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {   
        //매번 Enable시마다 가져와야 해
        VisualElement root = _document.rootVisualElement;
        //UQueryBuilder<VisualElement> builder = new UQueryBuilder<VisualElement>(root);
        //List<VisualElement> tabList = builder.Class("chartab").ToList();
        

        //Query도 리스트와 가능해.
        List<VisualElement> tabList = root.Query<VisualElement>(className: "chartab").ToList();
        VisualElement container = root.Q<VisualElement>("content-container");

        int idx = 0;
        tabList.ForEach(tab =>
        {
            int myIdx = idx; //클로저안에 넣어주고
            tab.RegisterCallback<ClickEvent>(e =>
            {
                tabList.ForEach(t => t.RemoveFromClassList("selected-chartab"));
                tab.AddToClassList("selected-chartab");
                container.style.left = new Length(-myIdx * 100f, LengthUnit.Percent);
            });
            idx++;
        });

        MakeSkillTab(); //스킬탭 구성
    }

    private void MakeSkillTab()
    {
        VisualElement root = _document.rootVisualElement;
        List<VisualElement> slotList = root.Query<VisualElement>(className: "skill-slot").ToList();

        _activeSlotMarker = root.Q<VisualElement>("skill-slot-active");
        VisualElement contentRoot = root.Q<VisualElement>("skill-content");

        _nameLabel = contentRoot.Q<Label>("skill-name-label");
        _descLabel = contentRoot.Q<Label>("skill-desc-label");
        _tierLabel = contentRoot.Q<Label>("tier-label");
        _tierDescLabel = contentRoot.Q<Label>("tier-desc");

        int idx = 0;
        foreach(SkillSO so in _skillList)
        {
            VisualElement icon = slotList[idx].Q<VisualElement>("skill-icon");
            icon.style.backgroundImage = new StyleBackground(so.SkillIcon);
            //아이콘 클릭시 이동
            icon.RegisterCallback<ClickEvent>(e => ClickSkillIcon(icon, so));
            idx++;
        }

        VisualElement firstIcon = slotList[0].Q<VisualElement>("skill-icon");
        ClickSkillIcon(firstIcon, _skillList[0], false); //첫번째건 미리 셋팅
    }

    private void ClickSkillIcon(VisualElement icon, SkillSO so, bool moveIcon = true)
    {
        if(moveIcon)
        {
            Vector3 pos = ElementInRootSpace(icon, _activeSlotMarker);
            _activeSlotMarker.style.top = new Length(pos.y, LengthUnit.Pixel);
            _activeSlotMarker.style.left = new Length(pos.x - 12, LengthUnit.Pixel);
        }

        _nameLabel.text = so.SkillName;
        _descLabel.text = so.SkillDesc;
        _tierLabel.text = $"Tier {so.Tier}";
        _tierDescLabel.text = $"Deal <b>{so.Damage} Damage</b> points to enemy every <b>{so.CoolTime} seconds<b>";
    }

    //타겟 비쥬얼 엘레멘트를 다른 비쥬얼 엘레멘트의 부모 공간을 기준으로 포지션을 알아낸다.
    private Vector3 ElementInRootSpace(VisualElement targetElement, VisualElement newElement)
    {
        //부모로부터의 상대위치를 월드 좌표로 변환한다.
        Vector2 targetInWorldSpace = targetElement.parent.LocalToWorld(targetElement.layout.position);
        VisualElement newRoot = newElement.parent; //다른 엘레멘트의 부모를 가져와서
        //월드스페이스를 해당 부모를 기준으로 하는 로컬 스페이스로 변경한다.
        return newRoot.WorldToLocal(targetInWorldSpace);
    }
}
