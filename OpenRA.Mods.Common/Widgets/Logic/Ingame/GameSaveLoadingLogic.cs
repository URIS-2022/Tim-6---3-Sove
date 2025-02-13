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

using OpenRA.Widgets;
using System.Security.Cryptography.X509Certificates;

namespace OpenRA.Mods.Common.Widgets.Logic
{
	public class GameSaveLoadingLogic : ChromeLogic
	{
		private bool HideCursor;
		[ObjectCreator.UseCtor]
		public GameSaveLoadingLogic(Widget widget, ModData modData, World world,bool HC=true)
		{
		  
			widget.Get<ProgressBarWidget>("PROGRESS").GetPercentage = () => world.GameSaveLoadingPercentage;

			var versionLabel = widget.GetOrNull<LabelWidget>("VERSION_LABEL");
			if (versionLabel != null)
				versionLabel.Text = modData.Manifest.Metadata.Version;

			var keyhandler = widget.Get<LogicKeyListenerWidget>("CANCEL_HANDLER");
			keyhandler.AddHandler(e =>
			{
				if (e.Event == KeyInputEvent.Down && e.Key == Keycode.ESCAPE)
				{
					Game.Disconnect();
					Ui.ResetAll();
					Game.LoadShellMap();
					return true;
				}

				return false;
			});

			Game.HideCursor = HC;
		}

		protected override void Dispose(bool disposing)
		{
			Game.HideCursor = false;
		}
	}
}
