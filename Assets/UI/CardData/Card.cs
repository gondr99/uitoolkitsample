using UnityEngine;
using UnityEngine.UIElements;

namespace CardBinding
{
    public class Card
    {
        private Character _myCharacter;
        private VisualElement _cardRoot;

        public VisualElement Root => _cardRoot;

        private Label _nameLabel;
        private Label _descLabel;
        private VisualElement _avatarImage;

        public Card(VisualElement cardRoot, Character character)
        {
            _myCharacter = character;
            _cardRoot = cardRoot;

            _nameLabel = _cardRoot.Q<Label>("NameLabel");
            _descLabel = _cardRoot.Q<Label>("InfoLabel");
            _avatarImage = _cardRoot.Q<VisualElement>("imgElemnt");

            _myCharacter.OnChanged += UpdateInfo;
            UpdateInfo();
        }

        private void UpdateInfo()
        {
            _nameLabel.text = _myCharacter.Name;
            _descLabel.text = _myCharacter.Description;
            _avatarImage.style.backgroundImage = new StyleBackground(_myCharacter.Sprite);
        }

        public void AddOn()
        {
            _cardRoot.Q<VisualElement>("CardBorder").AddToClassList("on");
        }
    }
}
