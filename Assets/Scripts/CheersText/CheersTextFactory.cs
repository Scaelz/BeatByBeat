using UnityEngine;

public class CheersTextFactory : IFactory<RateCheersText>
{
    AnnounceTextData _announceTextData;

    public CheersTextFactory(AnnounceTextData announceTextData)
    {
        _announceTextData = announceTextData;
    }

    public RateCheersText Create()
    {
        var go = GameObject.Instantiate(_announceTextData.Prefab);
        var cheersText = go.AddComponent<RateCheersText>();
        cheersText.Data = _announceTextData;
        return cheersText;
    }
}