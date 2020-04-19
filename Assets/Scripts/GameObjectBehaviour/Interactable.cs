using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameObjectBehavior
{
    [System.Serializable] public class InteractableEvent : UnityEvent {}

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class Interactable : MonoBehaviour
    {
        public InteractableEvent OnInteracted;
        public bool IsHovered { get; private set; } = false;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
                OnInteracted.Invoke();
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
