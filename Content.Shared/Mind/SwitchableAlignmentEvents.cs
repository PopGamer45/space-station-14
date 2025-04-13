using Content.Shared.Actions;
using Content.Shared.Mind.Components;
using Robust.Shared.Serialization;

namespace Content.Shared.Mind;

[Serializable, NetSerializable]
public sealed class SwitchAlignmentMessage : BoundUserInterfaceMessage
{
    public AlignmentChoiceEntry Alignment;

    public SwitchAlignmentMessage(AlignmentChoiceEntry alignment)
    {
        Alignment = alignment;
    }
}

public sealed partial class ToggleSwitchAlignmentInterfaceEvent : InstantActionEvent
{

}

[Serializable, NetSerializable]
public enum SwitchAlignmentUiKey : byte
{
    Key
}
