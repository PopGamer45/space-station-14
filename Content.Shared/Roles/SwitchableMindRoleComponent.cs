using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;
using Robust.Shared.Serialization;

namespace Content.Shared.Roles;

[RegisterComponent, NetworkedComponent]
public sealed partial class SwitchableMindRoleComponent : Component
{
    [DataField]
    public EntProtoId ActionSwitchMindRole = "ActionSwitchMindRole";

    [DataField]
    public EntityUid? ActionSwitchMindRoleEntity;

    [DataField(required: true)]
    public List<AlignmentChoiceEntry> Alignments;

    [DataField]
    public bool SingleUse = true;
}

[DataDefinition, Serializable, NetSerializable]
public sealed partial class AlignmentChoiceEntry
{
    [DataField(required: true)]
    public EntProtoId MindRole;

    [DataField(required: true)]
    public string MindRoleTagComponent = String.Empty;

    [DataField]
    public SpriteSpecifier? Icon;

    [DataField(required: true)]
    public string? ToolTip;
}
