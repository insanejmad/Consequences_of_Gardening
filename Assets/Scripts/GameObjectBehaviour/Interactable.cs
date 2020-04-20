using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameObjectBehavior
{
    [System.Serializable] public class InteractableEvent : UnityEvent {}

    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class Interactable : MonoBehaviour
    {
        public InteractableEvent OnInteracted;
        public bool ShowSpriteInsteadOfChangingMaterial = false;
        public bool IsHovered { get; private set; } = false;
        public Material HoverMaterial;

        Material OriginalMaterial;
        SpriteRenderer Sprite;

        void Awake()
        {
            Sprite = GetComponent<SpriteRenderer>();

            if (ShowSpriteInsteadOfChangingMaterial)
                Sprite.enabled = false;
        }

        void Update()
        {
            if (IsHovered && Input.GetMouseButtonDown(0))
                OnInteracted.Invoke();

            if (ShowSpriteInsteadOfChangingMaterial)
                UpdateSprite();
            else
                UpdateMaterial();
        }

        void UpdateSprite()
        {
            Sprite.enabled = IsHovered;
        }

        void UpdateMaterial()
        {
            if (IsHovered && null == OriginalMaterial)
            {
                OriginalMaterial = Sprite.material;
                Sprite.material = HoverMaterial;
            }
            else if (!IsHovered && null != OriginalMaterial)
            {
                Sprite.material = OriginalMaterial;
                OriginalMaterial = null;
            }
        }

        void OnMouseEnter()
        {
            IsHovered = true;
        }

        void OnMouseExit()
        {
            IsHovered = false;
        }
    }   
}
