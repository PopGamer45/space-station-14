using Content.Client.UserInterface.Controls;
using Content.Shared.Roles;
using Robust.Client.UserInterface;
using Robust.Shared.Prototypes;
using Robust.Shared.Player;
using Robust.Shared.Utility;

namespace Content.Client.Roles;

public sealed class SwitchMindRoleBoundUserInterface : BoundUserInterface
{
    private SimpleRadialMenu? _menu;

    public SwitchMindRoleBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
        IoCManager.InjectDependencies(this);
    }

    protected override void Open()
    {
        base.Open();

        if (!EntMan.TryGetComponent<SwitchableMindRoleComponent>(Owner, out var comp))
            return;

        _menu = this.CreateWindow<SimpleRadialMenu>();
        _menu.Track(Owner);
        var models = ConvertToButtons(comp.Alignments);
        _menu.SetButtons(models);

        _menu.OpenOverMouseScreenPosition();
    }

    private IEnumerable<RadialMenuActionOption> ConvertToButtons(List<AlignmentChoiceEntry> alignments)
    {
        var models = new RadialMenuActionOption[alignments.Count];
        for (int i = 0; i < alignments.Count; i++)
        {
            var alignment = alignments[i];
            var actionOption = new RadialMenuActionOption<AlignmentChoiceEntry>(HandleRadialMenuClick, alignment)
            {
                Sprite = alignment.Icon,
                ToolTip = alignment.ToolTip
            };
            models[i] = actionOption;
        }

        return models;
    }

    private void HandleRadialMenuClick(AlignmentChoiceEntry alignment)
    {
        SendMessage(new SwitchMindRoleMessage(alignment));
    }
}
