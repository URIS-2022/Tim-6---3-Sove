﻿#region Copyright & License Information
/*
 * Copyright 2007-2011 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation. For more information,
 * see COPYING.
 */
#endregion

using System.Collections.Generic;
using System.Drawing;
using OpenRA.FileFormats;
using OpenRA.Graphics;
using OpenRA.Traits;

namespace OpenRA.Mods.RA
{
	public class CombatDebugOverlayInfo : ITraitInfo
	{
		public object Create(ActorInitializer init) { return new CombatDebugOverlay(init.self); }
	}

	public class CombatDebugOverlay : IPostRender
	{
		Lazy<AttackBase> attack;
		Lazy<IBodyOrientation> coords;
		Lazy<Health> health;
		DeveloperMode devMode;

		public CombatDebugOverlay(Actor self)
		{
			attack = Lazy.New(() => self.TraitOrDefault<AttackBase>());
			coords = Lazy.New(() => self.Trait<IBodyOrientation>());
			health = Lazy.New(() => self.TraitOrDefault<Health>());

			var localPlayer = self.World.LocalPlayer;
			devMode = localPlayer != null ? localPlayer.PlayerActor.Trait<DeveloperMode>() : null;
		}

		public void RenderAfterWorld(WorldRenderer wr, Actor self)
		{
			if (devMode == null || !devMode.ShowCombatGeometry)
				return;

			if (health.Value != null)
				wr.DrawRangeCircle(self.CenterPosition, health.Value.Info.Radius, Color.Red);

			// No armaments to draw
			if (attack.Value == null)
				return;

			var wlr = Game.Renderer.WorldLineRenderer;
			var c = Color.White;

			// Fire ports on garrisonable structures
			var garrison = attack.Value as AttackGarrisoned;
			if (garrison != null)
			{
				var bodyOrientation = coords.Value.QuantizeOrientation(self, self.Orientation);
				foreach (var p in garrison.Ports)
				{
					var pos = self.CenterPosition + coords.Value.LocalToWorld(p.Offset.Rotate(bodyOrientation));
					var da = coords.Value.LocalToWorld(new WVec(224, 0, 0).Rotate(WRot.FromYaw(p.Yaw + p.Cone)).Rotate(bodyOrientation));
					var db = coords.Value.LocalToWorld(new WVec(224, 0, 0).Rotate(WRot.FromYaw(p.Yaw - p.Cone)).Rotate(bodyOrientation));

					var o = wr.ScreenPosition(pos);
					var a = wr.ScreenPosition(pos + da * 224 / da.Length);
					var b = wr.ScreenPosition(pos + db * 224 / db.Length);
					wlr.DrawLine(o, a, c, c);
					wlr.DrawLine(o, b, c, c);
				}

				return;
			}

			foreach (var a in attack.Value.Armaments)
			{
				foreach (var b in a.Barrels)
				{
					var muzzle = self.CenterPosition + a.MuzzleOffset(self, b);
					var dirOffset = new WVec(0, -224, 0).Rotate(a.MuzzleOrientation(self, b));

					var sm = wr.ScreenPosition(muzzle);
					var sd = wr.ScreenPosition(muzzle + dirOffset);
					wlr.DrawLine(sm, sd, c, c);
					wr.DrawTargetMarker(c, sm);
				}
			}
		}
	}
}
