using UnityEngine;
using EventBusSystem;
using System.Collections;

public interface ICollectable : IComponent
{
    void Collect(NoteCollectArgs args);
}

public class Block : MonoBehaviour, IBlock, ICollectable
{
    public Vector3 Destination { get; set; }
    public float Speed { get; set; }
    GameObject _shine;
    Material _shineMaterial;


    [SerializeField]
    MeshRenderer _renderer;

    private void Awake()
    {
        if (_renderer == null)
        {
            _renderer = GetComponent<MeshRenderer>();
            _shine = transform.GetChild(0).gameObject;
            _shineMaterial = _shine.GetComponent<MeshRenderer>().material;
        }
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        _shine.transform.localScale = new Vector3(
            _shine.transform.localScale.x,
            0,
            _shine.transform.localScale.z);
    }

    public void Move()
    {
        transform.position += Destination * Speed * Time.deltaTime;
    }

    public void Revert()
    {
        if (gameObject != null)
            gameObject.SetActive(false);
    }

    public void SetMaterial(Material material)
    {
        _renderer.material = material;
        _shineMaterial.SetFloat("Intensity", 1);
        _shineMaterial.SetColor("EColor", _renderer.material.GetColor("EColor"));
    }

    private void OnTriggerEnter(Collider other)
    {
        var block = other.GetComponent<IPlayer>();
        NoteCollectArgs args = new NoteCollectArgs() { Collectable = this, };
        if (block == null)
        {
            args.Accuracy = -1;
        }
        else
        {
            var accuracy = Mathf.Abs(other.transform.position.z - transform.position.z);
            args.Color = _renderer.material.GetColor("EColor");
            args.Accuracy = accuracy;
        }
        EventBus.RaiseEvent<INoteCollectedHandler>(x => x.CollectNote(args));
    }

    public void Collect(NoteCollectArgs args)
    {
        _shine.SetActive(true);
        _shineMaterial.SetColor("EColor", args.Color);
        //_shineMaterial.SetFloat("Intensity", 1);
        StartCoroutine(ShineDispose(args.CollectionTime));
    }

    IEnumerator ShineDispose(float time)
    {
        var start = 0;
        var end = 1;
        var currentColor = _renderer.material.GetColor("EColor");
        float t = 0;
        while (t < 1)
        {
            var intensity = Mathf.Lerp(start, end, t);
            //var colorIntensity = intensity * 3;
            //var colorIntensity = 0;
            //Color newColor = new Color(currentColor.r * colorIntensity, currentColor.g * colorIntensity, currentColor.b * colorIntensity);
            //_renderer.material.SetColor("EColor", newColor);
            _shineMaterial.SetFloat("Intensity", intensity);
            _shine.transform.localScale = new Vector3(
                _shine.transform.localScale.x,
                intensity,
                _shine.transform.localScale.z);
            if (t < 1)
            {
                t += Time.deltaTime / time;
            }
            yield return null;
        }
    }
}
