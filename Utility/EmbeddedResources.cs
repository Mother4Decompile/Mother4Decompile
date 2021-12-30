using System;
using System.IO;
using System.Reflection;

namespace Mother4.Utility
{
	// Token: 0x02000173 RID: 371
	internal static class EmbeddedResources
	{
		// Token: 0x060007CE RID: 1998 RVA: 0x00032730 File Offset: 0x00030930
		public static Stream GetStream(string resource)
		{
			if (EmbeddedResources.assembly == null)
			{
				EmbeddedResources.assembly = Assembly.GetExecutingAssembly();
			}
			return EmbeddedResources.assembly.GetManifestResourceStream(resource);
		}

		// Token: 0x0400097F RID: 2431
		private static Assembly assembly;
	}
}
