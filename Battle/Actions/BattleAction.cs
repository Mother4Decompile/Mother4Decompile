using System;
using Mother4.Battle.Combatants;

namespace Mother4.Battle.Actions
{
	// Token: 0x02000012 RID: 18
	internal abstract class BattleAction : IComparable
	{
		// Token: 0x0600001D RID: 29 RVA: 0x00003664 File Offset: 0x00001864
		public static BattleAction GetInstance(ActionParams aparams)
		{
			return (BattleAction)Activator.CreateInstance(aparams.actionType, new object[]
			{
				aparams
			});
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600001E RID: 30 RVA: 0x00003698 File Offset: 0x00001898
		// (remove) Token: 0x0600001F RID: 31 RVA: 0x000036D0 File Offset: 0x000018D0
		public event BattleAction.ActionCompleteHandler OnActionComplete;

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000020 RID: 32 RVA: 0x00003705 File Offset: 0x00001905
		public int Priority
		{
			get
			{
				return this.priority;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000021 RID: 33 RVA: 0x0000370D File Offset: 0x0000190D
		public bool Complete
		{
			get
			{
				return this.complete;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000022 RID: 34 RVA: 0x00003715 File Offset: 0x00001915
		public Combatant Sender
		{
			get
			{
				return this.sender;
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000371D File Offset: 0x0000191D
		public BattleAction(ActionParams aparams)
		{
			this.controller = aparams.controller;
			this.priority = aparams.priority;
			this.sender = aparams.sender;
			this.targets = aparams.targets;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00003759 File Offset: 0x00001959
		public void Update()
		{
			this.UpdateAction();
			if (this.complete && !this.onCompleteFired && this.OnActionComplete != null)
			{
				this.OnActionComplete(this);
				this.onCompleteFired = true;
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000378C File Offset: 0x0000198C
		protected virtual void UpdateAction()
		{
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00003790 File Offset: 0x00001990
		public int CompareTo(object obj)
		{
			if (obj is BattleAction)
			{
				BattleAction battleAction = (BattleAction)obj;
				return battleAction.priority - this.priority;
			}
			throw new ArgumentException("Cannot compare BattleAction to object not of type BattleAction.");
		}

		// Token: 0x040000D4 RID: 212
		protected BattleController controller;

		// Token: 0x040000D5 RID: 213
		protected int priority;

		// Token: 0x040000D6 RID: 214
		protected bool complete;

		// Token: 0x040000D7 RID: 215
		protected Combatant sender;

		// Token: 0x040000D8 RID: 216
		protected Combatant[] targets;

		// Token: 0x040000D9 RID: 217
		private bool onCompleteFired;

		// Token: 0x02000013 RID: 19
		// (Invoke) Token: 0x06000028 RID: 40
		public delegate void ActionCompleteHandler(BattleAction action);
	}
}
