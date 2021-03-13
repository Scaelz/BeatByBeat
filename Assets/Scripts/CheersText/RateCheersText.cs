using UnityEngine;
using DG.Tweening;

public class RateCheersText : MonoBehaviour, IActivatable, IRevertable
{
    public AnnounceTextData Data { get; set; }

    public void Activate()
    {
        gameObject.SetActive(true);
        Sequence s = DOTween.Sequence();
        s.Append(transform.DOLocalMoveX(Data.InPositionX, Data.InTime).SetEase(Data.InEase))
        .Append(transform.DOLocalMoveX(Data.OutPositionX, Data.OutTime).SetEase(Data.OutEase));
    }

    public void Revert()
    {
        transform.localPosition = new Vector3(Data.DefaultX,
                                              transform.localPosition.y,
                                              transform.localPosition.z);
        gameObject.SetActive(false);
    }
}
