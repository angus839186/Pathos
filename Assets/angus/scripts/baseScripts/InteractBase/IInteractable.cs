public interface IInteractable
{
    
    void Interact();
    
    string GetDescription(Item heldItem);

    string GetAnimationTrigger(Item heldItem);

    void InteractEvent(Item heldItem);
}