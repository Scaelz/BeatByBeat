using System.ComponentModel.Design.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventBusSystem;

public class NoteCollectArgs
{
    public Color Color;
    public ICollectable Collectable;
    public float Accuracy;
    public float CollectionTime;

}

public interface INoteCollectedHandler : IGlobalSubscriber
{
    void CollectNote(NoteCollectArgs args);
}
