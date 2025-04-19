using Content.Shared.Actions;
using Content.Shared.Roles;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Roles;

[Serializable, NetSerializable]
public sealed class SwitchMindRoleMessage : BoundUserInterfaceMessage
{
    public EntProtoId MindRole;
    public string MindRoleTagComp;

    public SwitchMindRoleMessage(AlignmentChoiceEntry alignment)
    {
        MindRole = alignment.MindRole;
        MindRoleTagComp = alignment.MindRoleTagComponent;
    }
}

public sealed partial class ToggleSwitchMindRoleInterfaceEvent : InstantActionEvent
{

}

[Serializable, NetSerializable]
public enum SwitchMindRoleUiKey : byte
{
    Key
}
