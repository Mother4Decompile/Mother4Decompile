using System;

namespace Carbine.Scenes
{
	// Token: 0x02000051 RID: 81
	internal class EmptySceneStackException : Exception
	{
		// Token: 0x06000249 RID: 585 RVA: 0x0000D1E4 File Offset: 0x0000B3E4
		public EmptySceneStackException() : base("The scene stack is empty.")
		{
		}
	}
}
