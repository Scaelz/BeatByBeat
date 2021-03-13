using UnityEngine;
using EventBusSystem;

public class GameUIController : MonoBehaviour
{
    public void NotePressed(NoteButton noteButton)
    {
        EventBus.RaiseEvent<IUserInputHandler>(x => x.UpdatePlayerDestination(noteButton.note));
    }
}
