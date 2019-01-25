using UnityEngine.EventSystems;

public interface IHitMessageTarget : IEventSystemHandler
{
    void Hit();
}