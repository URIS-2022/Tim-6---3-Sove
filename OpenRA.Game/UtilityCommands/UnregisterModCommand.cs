#region Copyright & License Information
/*
 * Copyright 2007-2022 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */
#endregion

using System.Linq;

namespace OpenRA.UtilityCommands
{
	class UnregisterModCommand : IUtilityCommand
	{
		string IUtilityCommand.Name => "--unregister-mod";

		bool IUtilityCommand.ValidateArguments(string[] args)
		{
			return args.Length >= 2 && new string[] { "system", "user", "both" }.Contains(args[1]);
		}

		[Desc("(system|user|both)", "Removes the mod metadata entry for the in-game mod switcher.")]
		void IUtilityCommand.Run(Utility utility, string[] args)
		{
			ModRegistrations type = 0;
			if (args[1] == "system" || args[1] == "both")
				type |= ModRegistrations.System;

			if (args[1] == "user" || args[1] == "both")
				type |= ModRegistrations.User;

			new ExternalMods().Unregister(utility.ModData.Manifest, type);
		}
	}
}
