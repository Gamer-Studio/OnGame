using System.Collections;
using System.Collections.Generic;
using OnGame.Utils;
using UnityEngine;

namespace OnGame.Prefabs.Entities
{
    public class Character : Entity
    {
        private static readonly int Angle = Animator.StringToHash("Angle");

        private Animator animator;
        private SpriteRenderer characterRenderer;

        protected override void Awake()
        {
            base.Awake();
            animator = Helper.GetComponentInChildren_Helper<Animator>(gameObject, true);
        }

        protected override void Rotate(Vector2 direction)
        {
            float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            animator.SetFloat(Angle, rotZ);
        }
    }
}
