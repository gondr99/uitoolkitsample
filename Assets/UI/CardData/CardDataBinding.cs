using CardBinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CardDataBinding : MonoBehaviour
{
    private UIDocument _document;
    private TextField _inputName;
    private TextField _inputDesc;

    [SerializeField]
    private VisualTreeAsset _cardTemplate;
    [SerializeField]
    private List<CharacterSO> _charDatas;

    private Character _currentCharacter = null;

    private List<Card> _cardList = new List<Card>();

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        VisualElement root = _document.rootVisualElement;

        _inputName = root.Q<TextField>("InputName");
        _inputDesc = root.Q<TextField>("InputDesc");

        Button addBtn = root.Q<Button>("AddButton");
        addBtn.RegisterCallback<ClickEvent>(OnAddCardClicked);

        _inputName.RegisterCallback<ChangeEvent<string>>(OnNameChanged);
        _inputDesc.RegisterCallback<ChangeEvent<string>>(OnDescChanged);
    }

    private void OnAddCardClicked(ClickEvent e)
    {
        VisualElement root = _document.rootVisualElement;
        VisualElement cardContainer = root.Q<VisualElement>("CardContainer"); //ī�� ���� ��

        cardContainer.Clear(); //��� �ڽ� ����
        _cardList.Clear();
        _charDatas.ForEach(data =>
        {
            Character character = new Character(data.Name, data.Desc, data.Image);
            VisualElement cardXML = _cardTemplate.Instantiate();
            cardContainer.Add(cardXML);

            Card card = new Card(cardXML, character);

            _cardList.Add(card); //����Ʈ�� �־��ش�.
            cardXML.RegisterCallback<ClickEvent>(e =>
            {
                _currentCharacter = character;
                _inputName.SetValueWithoutNotify(character.Name);
                _inputDesc.SetValueWithoutNotify(character.Description);
            });
        });

        StartCoroutine(OpacityOn());
    }

    IEnumerator OpacityOn()
    {
        foreach(Card c in _cardList)
        {
            yield return new WaitForSeconds(0.2f);
            c.AddOn();
        }
    }

    //private void OnCardClicked(ClickEvent e)
    //{
    //    //������ ������� �� ��� �̷��� �ȴ�.
    //    //Label nameLabel = _card.Q<Label>("NameLabel");
    //    //Label infoLabel = _card.Q<Label>("InfoLabel");

    //    //_inputName.SetValueWithoutNotify(nameLabel.text);
    //    //_inputDesc.SetValueWithoutNotify(infoLabel.text);
    //}

    private void OnNameChanged(ChangeEvent<string> e)
    {
        if (_currentCharacter == null) return;
        _currentCharacter.Name = e.newValue;
        //������ ������� �Ұ�� �̷��� �ȴ�.
        //Label nameLabel = _card.Q<Label>("NameLabel");
        //nameLabel.text = e.newValue;
        //_testChar.Name = e.newValue;
    }

    private void OnDescChanged(ChangeEvent<string> e)
    {
        if (_currentCharacter == null) return;
        _currentCharacter.Description = e.newValue;
        //������ ������� �Ұ�� �̷��� �ȴ�.
        //Label infoLabel = _card.Q<Label>("InfoLabel");
        //infoLabel.text = e.newValue;
        //_testChar.Description = e.newValue;
    }
}
