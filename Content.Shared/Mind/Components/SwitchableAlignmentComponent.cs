using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;
using Robust.Shared.Serialization;

namespace Content.Shared.Mind.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class SwitchableAlignmentComponent : Component
{
    [DataField]
    public EntProtoId ActionSwitchAlignment = "ActionSwitchAlignment";

    [DataField]
    public EntityUid? ActionSwitchAlignmentEntity;

    [DataField(required: true)]
    public List<AlignmentChoiceEntry> Alignments;

    [DataField]
    public bool SingleUse = true;
}

[Serializable, NetSerializable]
public sealed partial class AlignmentChoiceEntry
{
    [DataField(required: true)]
    public EntProtoId MindRole;

    [DataField]
    public SpriteSpecifier? Icon;

    [DataField(required: true)]
    public string? ToolTip;
}
