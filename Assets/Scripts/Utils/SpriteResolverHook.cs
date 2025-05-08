using UnityEngine;
using UnityEngine.U2D.Animation;

namespace OnGame.Utils
{
  /// <summary>
  /// SpriteLibrary, Sprite Resolver, Animator 이용해서 애니메이션 구현할 떄 사용하는 훅
  /// </summary>
  public class SpriteResolverHook : MonoBehaviour
  {
    [SerializeField] private SpriteResolver resolver;

    public string SpriteLabel
    {
      get => resolver.GetLabel();
      set => resolver.SetCategoryAndLabel(resolver.GetCategory(), value);
    }
  }
}