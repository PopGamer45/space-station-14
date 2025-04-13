using Content.Shared.Actions;
using Content.Shared.Mind;
using Content.Shared.Mind.Components;
using Content.Shared.Roles;
using Content.Server.Mind;
using Robust.Server.GameObjects;
using Robust.Shared.Player;

namespace Content.Server.Mind;

public sealed class SwitchableAlignmentSystem : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _actions = default!;
    [Dependency] private readonly SharedRoleSystem _roles = default!;
    [Dependency] private readonly MindSystem _mind = default!;
    [Dependency] private readonly UserInterfaceSystem _userInterface = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<SwitchableAlignmentComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<SwitchableAlignmentComponent, ComponentShutdown>(OnShutdown);
        SubscribeLocalEvent<SwitchableAlignmentComponent, ToggleSwitchAlignmentInterfaceEvent>(OnToggleSwitchAlignmentInterface);
        SubscribeLocalEvent<SwitchableAlignmentComponent, SwitchAlignmentMessage>(OnSwitchAlignmentMessage);
    }

    private void OnStartup(EntityUid uid, SwitchableAlignmentComponent component, ComponentStartup args)
    {
        if (!TryComp(uid, out ActionsComponent? comp))
            return;

        _actions.AddAction(uid, ref component.ActionSwitchAlignmentEntity, component.ActionSwitchAlignment, component: comp);
    }

    private void OnShutdown(EntityUid uid, SwitchableAlignmentComponent component, ComponentShutdown args)
    {
        if (!TryComp(uid, out ActionsComponent? comp))
            return;

        _actions.RemoveAction(uid, component.ActionSwitchAlignmentEntity, comp);
    }

    private void OnToggleSwitchAlignmentInterface(EntityUid uid, SwitchableAlignmentComponent component, ToggleSwitchAlignmentInterfaceEvent args)
    {
        if (args.Handled || !TryComp<ActorComponent>(uid, out var actor))
            return;
        args.Handled = true;

        _userInterface.TryToggleUi(uid, SwitchAlignmentUiKey.Key, actor.PlayerSession);
    }

    private void OnSwitchAlignmentMessage(EntityUid uid, SwitchableAlignmentComponent component, SwitchAlignmentMessage args)
    {
        if (!_mind.TryGetMind(uid, out var mindId, out _))
            return;

        if (!EntityManager.ComponentFactory.TryGetRegistration(args.Alignment.MindRole, out var role))
            return;

        if (!_roles.MindHasRole(mindId, role.Type, out _))
        {
            _roles.MindAddRole(mindId, args.Alignment.MindRole, silent: true);
            if (component.SingleUse && TryComp(uid, out ActionsComponent? comp))
            {
                _actions.RemoveAction(uid, component.ActionSwitchAlignmentEntity, comp);
            }
        }
    }
}
