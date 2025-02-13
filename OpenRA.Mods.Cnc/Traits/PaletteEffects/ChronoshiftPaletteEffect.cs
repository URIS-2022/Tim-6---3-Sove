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

using System.Collections.Generic;
using System.Linq;
using OpenRA.Graphics;
using OpenRA.Primitives;
using OpenRA.Traits;

namespace OpenRA.Mods.Cnc.Traits
{
	[TraitLocation(SystemActors.World | SystemActors.EditorWorld)]
	[Desc("Apply palette full screen rotations during chronoshifts. Add this to the world actor.")]
	public class ChronoshiftPaletteEffectInfo : TraitInfo
	{
		[Desc("Measured in ticks.")]
		public readonly int ChronoEffectLength = 60;

		public override object Create(ActorInitializer init) { return new ChronoshiftPaletteEffect(this); }
	}

	public class ChronoshiftPaletteEffect : IPaletteModifier, ITick
	{
		readonly ChronoshiftPaletteEffectInfo info;
		int remainingFrames;

		public ChronoshiftPaletteEffect(ChronoshiftPaletteEffectInfo info)
		{
			this.info = info;
		}

		public void Enable()
		{
			remainingFrames = info.ChronoEffectLength;
		}

		void ITick.Tick(Actor self)
		{
			if (remainingFrames > 0)
				remainingFrames--;
		}

		void IPaletteModifier.AdjustPalette(IReadOnlyDictionary<string, MutablePalette> palettes)
		{
			if (remainingFrames == 0)
				return;

			var frac = (float)remainingFrames / info.ChronoEffectLength;

			foreach (var palValue in palettes.Select(palValue => palValue.Value))
			{
				for (var x = 0; x < Palette.Size; x++)
				{
					var orig = palValue.GetColor(x);
					var lum = (int)(255 * orig.GetBrightness());
					var desat = Color.FromArgb(orig.A, lum, lum, lum);
					palValue.SetColor(x, Exts.ColorLerp(frac, orig, desat));
				}
			}
		}
	}
}
