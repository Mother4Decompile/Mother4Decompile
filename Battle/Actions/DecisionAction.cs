using System;

namespace Mother4.Battle.Actions
{
	// Token: 0x020000B1 RID: 177
	internal abstract class DecisionAction : BattleAction
	{
		// Token: 0x060003DE RID: 990 RVA: 0x00018638 File Offset: 0x00016838
		public DecisionAction(ActionParams aparams) : base(aparams)
		{
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00018641 File Offset: 0x00016841
		protected override void UpdateAction()
		{
			base.UpdateAction();
		}

		// Token: 0x0400058A RID: 1418
		private const int DECIDER_INDEX = 0;
	}
}
