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

using OpenRA.Primitives;

namespace OpenRA
{
	public class WorldViewportSizes : IGlobalModData
	{
		public readonly Int2 CloseWindowHeights = new Int2(480, 600);
		public readonly Int2 MediumWindowHeights = new Int2(600, 900);
		public readonly Int2 FarWindowHeights = new Int2(900, 1300);

		public readonly float MaxZoomScale = 2.0f;
		public readonly int MaxZoomWindowHeight = 240;
		public readonly bool AllowNativeZoom = true;

		public readonly Size MinEffectiveResolution = new Size(1024, 720);

		public Int2 GetSizeRange(WorldViewport distance)
		{
			if (distance == WorldViewport.Close)
				return CloseWindowHeights;
			else if (distance == WorldViewport.Medium)
				return MediumWindowHeights;
			else return FarWindowHeights; 
		}
	}
}
