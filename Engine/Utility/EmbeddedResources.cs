using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Carbine.Utility
{
	// Token: 0x02000060 RID: 96
	public static class EmbeddedResources
	{
		// Token: 0x060002AE RID: 686 RVA: 0x0000EABC File Offset: 0x0000CCBC
		public static Stream GetStream(string resource)
		{
			if (EmbeddedResources.assembly == null)
			{
				EmbeddedResources.assembly = Assembly.GetExecutingAssembly();
			}
			if (EmbeddedResources.streamDict == null)
			{
				EmbeddedResources.streamDict = new Dictionary<string, Stream>();
			}
			Stream manifestResourceStream;
			if (!EmbeddedResources.streamDict.TryGetValue(resource, out manifestResourceStream))
			{
				manifestResourceStream = EmbeddedResources.assembly.GetManifestResourceStream(resource);
			}
			return manifestResourceStream;
		}

		// Token: 0x04000201 RID: 513
		private static Assembly assembly;

		// Token: 0x04000202 RID: 514
		private static IDictionary<string, Stream> streamDict;
	}
}
