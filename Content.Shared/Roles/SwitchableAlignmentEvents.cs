using Content.Shared.Actions;
using Content.Shared.Roles;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Roles;

[Serializable, NetSerializable]
public sealed class SwitchAlignmentMessage : BoundUserInterfaceMessage
{
    public EntProtoId MindRole;
    public string MindRoleTagComp;

    public SwitchAlignmentMessage(AlignmentChoiceEntry alignment)
    {
        MindRole = alignment.MindRole;
        MindRoleTagComp = alignment.MindRoleTagComponent;
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
