using Content.Shared.Actions;
using Content.Shared.Mind;
using Content.Shared.Mind.Components;
using Content.Shared.Roles;
using Robust.Server.GameObjects;
using Robust.Shared.Player;

namespace Content.Server.Roles;

public sealed class SwitchableMindRoleSystem : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _actions = default!;
    [Dependency] private readonly SharedRoleSystem _roles = default!;
    [Dependency] private readonly SharedMindSystem _mind = default!;
    [Dependency] private readonly UserInterfaceSystem _userInterface = default!;
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly IComponentFactory _componentFactory = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<SwitchableMindRoleComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<SwitchableMindRoleComponent, ComponentShutdown>(OnShutdown);
        SubscribeLocalEvent<SwitchableMindRoleComponent, ToggleSwitchMindRoleInterfaceEvent>(OnToggleSwitchMindRoleInterface);
        SubscribeLocalEvent<SwitchableMindRoleComponent, SwitchMindRoleMessage>(OnSwitchMindRoleMessage);
    }

    private void OnStartup(EntityUid uid, SwitchableMindRoleComponent component, ComponentStartup args)
    {
        if (!TryComp(uid, out ActionsComponent? comp))
            return;

        _actions.AddAction(uid, ref component.ActionSwitchMindRoleEntity, component.ActionSwitchMindRole, component: comp);
    }

    private void OnShutdown(EntityUid uid, SwitchableMindRoleComponent component, ComponentShutdown args)
    {
        if (!TryComp(uid, out ActionsComponent? comp))
            return;

        BaseActionComponent? action = null;
        if (_actions.ResolveActionData(component.ActionSwitchMindRoleEntity, ref action) && action.AttachedEntity == null)
            return;

        _actions.RemoveAction(uid, component.ActionSwitchMindRoleEntity, comp);
    }

    private void OnToggleSwitchMindRoleInterface(EntityUid uid, SwitchableMindRoleComponent component, ToggleSwitchMindRoleInterfaceEvent args)
    {
        if (args.Handled || !TryComp<ActorComponent>(uid, out var actor))
            return;
        args.Handled = true;

        _userInterface.TryToggleUi(uid, SwitchMindRoleUiKey.Key, actor.PlayerSession);
    }

    private void OnSwitchMindRoleMessage(EntityUid uid, SwitchableMindRoleComponent component, SwitchMindRoleMessage args)
    {
        if (!_mind.TryGetMind(uid, out var mindId, out var mindComp))
            return;

        if (_roles.MindHasRole(mindId, _componentFactory.GetRegistration(args.MindRoleTagComp).Type, out var role))
            _entityManager.DeleteEntity(role);

        _roles.MindAddRole(mindId, args.MindRole, silent: true);
        if (component.SingleUse && TryComp(uid, out ActionsComponent? comp))
        {
            _actions.RemoveAction(uid, component.ActionSwitchMindRoleEntity, comp);
        }
    }
}
