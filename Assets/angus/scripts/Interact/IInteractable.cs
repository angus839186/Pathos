public interface IInteractable
{
    
    void Interact();
    
    string GetDescription();

    string GetAnimationTrigger(Item heldItem);

    void InteractEvent(Item heldItem);
}