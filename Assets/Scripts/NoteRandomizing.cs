using System;
using Random = UnityEngine.Random;

public class NoteRandomizing : INoteProvider
{
    int lastSpawnedIndex = -1;
    public Note GetNote()
    {
        var values = Enum.GetValues(typeof(Note));
        var nextIndex = 0;
        do
        {
            nextIndex = Random.Range(0, values.Length);
        } while (nextIndex == lastSpawnedIndex);
        lastSpawnedIndex = nextIndex;
        return (Note)values.GetValue(nextIndex);
        //return Note.Red;
    }
}
