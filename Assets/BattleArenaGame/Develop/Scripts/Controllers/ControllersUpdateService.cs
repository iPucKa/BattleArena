using System;
using System.Collections.Generic;

public class ControllersUpdateService
{
	private List<ControllerToRemoveReason> _controllers = new();

	public void Add(Controller controller, Func<bool> removeReason)
	{
		_controllers.Add(new ControllerToRemoveReason(controller, removeReason));
	}

	public void Update(float deltaTime)
	{
		_controllers.RemoveAll(item => item.RemoveReason.Invoke());

		foreach (ControllerToRemoveReason item in _controllers)
			item.Controller.Update(deltaTime);
	}

	private class ControllerToRemoveReason
	{
		public ControllerToRemoveReason(Controller controller, Func<bool> removeReason)
		{
			Controller = controller;
			RemoveReason = removeReason;
		}

		public Controller Controller { get; }
		public Func<bool> RemoveReason { get; }
	}
}
