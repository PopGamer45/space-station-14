using Content.Client.UserInterface.Controls;
using Content.Shared.Mind;
using Content.Shared.Mind.Components;
using Robust.Client.UserInterface;
using Robust.Shared.Player;
using Robust.Shared.Utility;

namespace Content.Client.Mind;

public sealed class SwitchAlignmentBoundUserInterface : BoundUserInterface
{
    private SimpleRadialMenu? _menu;

    public SwitchAlignmentBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
        IoCManager.InjectDependencies(this);
    }

    protected override void Open()
    {
        base.Open();

        if (!EntMan.TryGetComponent<SwitchableAlignmentComponent>(Owner, out var comp))
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
            models[i] = new RadialMenuActionOption<AlignmentChoiceEntry>(HandleRadialMenuClick, alignment);
        }

        return models;
    }

    private void HandleRadialMenuClick(AlignmentChoiceEntry alignment)
    {
        SendMessage(new SwitchAlignmentMessage(alignment));
    }
}
