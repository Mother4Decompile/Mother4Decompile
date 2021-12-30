using System;
using System.IO;

namespace Mother4.Data
{
	// Token: 0x02000088 RID: 136
	internal static class Paths
	{
		// Token: 0x04000424 RID: 1060
		public static readonly string RESOURCES = "Resources" + Path.DirectorySeparatorChar;

		// Token: 0x04000425 RID: 1061
		public static readonly string AUDIO = Path.Combine(Paths.RESOURCES, "Audio", "") + Path.DirectorySeparatorChar;

		// Token: 0x04000426 RID: 1062
		public static readonly string GRAPHICS = Path.Combine(Paths.RESOURCES, "Graphics", "") + Path.DirectorySeparatorChar;

		// Token: 0x04000427 RID: 1063
		public static readonly string PSI_GRAPHICS = Path.Combine(Paths.GRAPHICS, "PSI", "") + Path.DirectorySeparatorChar;

		// Token: 0x04000428 RID: 1064
		public static readonly string MAPS = Path.Combine(Paths.RESOURCES, "Maps", "") + Path.DirectorySeparatorChar;

		// Token: 0x04000429 RID: 1065
		public static readonly string DATA = Path.Combine(Paths.RESOURCES, "Data", "") + Path.DirectorySeparatorChar;

		// Token: 0x0400042A RID: 1066
		public static readonly string TEXT = Path.Combine(Paths.RESOURCES, "Text", "") + Path.DirectorySeparatorChar;

		// Token: 0x0400042B RID: 1067
		public static readonly string BATTLE_SWIRL = Path.Combine(Paths.GRAPHICS, "swirl", "") + Path.DirectorySeparatorChar;
	}
}
