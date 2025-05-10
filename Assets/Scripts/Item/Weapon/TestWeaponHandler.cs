using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OnGame
{
    public class TestWeaponHandler : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Transform weaponPivot;
        [SerializeField] private Camera mainCamera;

        private static readonly int IsAttack = Animator.StringToHash("IsAttack");

        private void Update()
        {
            RotateToMouse();

            if (Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger(IsAttack);
            }
        }

        private void RotateToMouse()
        {
            Vector2 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mouseWorld - (Vector2)transform.position).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            weaponPivot.rotation = Quaternion.Euler(0, 0, angle + 90);
        }
    }
}
