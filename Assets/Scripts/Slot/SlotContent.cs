using System;
using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MUGCUP
{
    [Serializable]
    public class ContentData
    {
        public ItemType ItemType;
        public Sprite IconSprite;
        public Color BackgroundColor;
    }
    
    public class SlotContent : MonoBehaviour, IInitializable, IEquatable<SlotContent>
    {
        public bool IsInit { get; private set; }

        [field: SerializeField]
        public string ID { get; private set; }

        [field: SerializeField] public ItemType       ItemType          { get; private set; }
        [field: SerializeField] public SpriteRenderer IconSpriteRender  { get; private set; }
        [field: SerializeField] public Rectangle      TopPanel          { get; private set; }

        [SerializeField] private List<ContentData> ContentData = new List<ContentData>();
     
        [field: SerializeField] public MoveControl      MoveControl      { get; private set; }
        [field: SerializeField] public AnimationControl AnimationControl { get; private set; }
       
        public Slot SlotOwner { get; private set; }
        
 #if UNITY_EDITOR
        private void OnValidate()
        {
            if (IconSpriteRender != null)
                ID = IconSpriteRender.sprite.name;
        }
#endif
        private void Awake()
        {
            Init();
            SetRandomIcon();
        }

        public void SetSlotOwner(Slot _slot)
        {
            SlotOwner = _slot;
        }
        
        public void Init()
        {
            if(IsInit)
                return;
            IsInit = true;
            
            if (MoveControl)
                MoveControl.Init();
	        
            if(AnimationControl)
                AnimationControl.Init();

            if (IconSpriteRender != null)
                ID = IconSpriteRender.sprite.name;
        }
        
        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public void SetRandomIcon()
        {
            var _index = Random.Range(0, ContentData.Count);
            var _contentData = ContentData[_index];
            
            ItemType                = _contentData.ItemType;
            IconSpriteRender.sprite = _contentData.IconSprite;
            TopPanel.Color          = _contentData.BackgroundColor;

            ID = ItemType.ToString();
        }

        public void ChangeIconSprite(string _name)
        {
            
        }

        public IEnumerator SelfDestroyedCoroutine()
        {
            AnimationControl.PlayAnimation(AnimationName.ON_DESTROYED);
            yield return 
                new WaitUntil(() => !AnimationControl.IsAnimationPlaying);
            
            SlotOwner.SetSlotContent(null);
            Destroy(gameObject);
        }

        public bool Equals(SlotContent _other)
        {
            return ID == _other.ID;
        }
    }
}
