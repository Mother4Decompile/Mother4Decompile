using System;

namespace Mother4.Scripts.Actions.ParamTypes
{
	// Token: 0x0200011C RID: 284
	internal struct RufiniOption
	{
		// Token: 0x060006DC RID: 1756 RVA: 0x0002BB8A File Offset: 0x00029D8A
		public override string ToString()
		{
			return string.Format("{0}", this.Option);
		}

		// Token: 0x040008CA RID: 2250
		public int Option;
	}
}
