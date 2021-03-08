using EventBusSystem;
using UnityEngine;

public class NotesShredder : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var args = new NoteCollectArgs()
        {
            Accuracy = -1,
            Collectable = other.GetComponent<ICollectable>(),
        };
        EventBus.RaiseEvent<INoteCollectedHandler>(x => x.CollectNote(args));
    }
}
