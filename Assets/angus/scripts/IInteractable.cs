public interface IInteractable
{
    
    void Interact(Item heldItem);
    
    string GetDescription();

    string GetAnimationTrigger(Item heldItem);
}