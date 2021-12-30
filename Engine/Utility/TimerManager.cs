using System;
using System.Collections.Generic;

namespace Carbine.Utility
{
	// Token: 0x02000065 RID: 101
	public class TimerManager
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060002BE RID: 702 RVA: 0x0000ECCC File Offset: 0x0000CECC
		public static TimerManager Instance
		{
			get
			{
				if (TimerManager.instance == null)
				{
					TimerManager.instance = new TimerManager();
				}
				return TimerManager.instance;
			}
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060002BF RID: 703 RVA: 0x0000ECE4 File Offset: 0x0000CEE4
		// (remove) Token: 0x060002C0 RID: 704 RVA: 0x0000ED1C File Offset: 0x0000CF1C
		public event TimerManager.OnTimerEndHandler OnTimerEnd;

		// Token: 0x060002C1 RID: 705 RVA: 0x0000ED51 File Offset: 0x0000CF51
		private TimerManager()
		{
			this.timers = new List<TimerManager.Timer>();
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000ED64 File Offset: 0x0000CF64
		public int StartTimer(int duration)
		{
			long frame = Engine.Frame;
			TimerManager.Timer item = new TimerManager.Timer
			{
				End = frame + (long)duration,
				Index = ++this.timerCounter
			};
			this.timers.Add(item);
			return this.timerCounter;
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000EDB8 File Offset: 0x0000CFB8
		public void Cancel(int timerIndex)
		{
			for (int i = 0; i < this.timers.Count; i++)
			{
				if (this.timers[i].Index == timerIndex)
				{
					this.timers.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000EDFC File Offset: 0x0000CFFC
		public void Update()
		{
			for (int i = 0; i < this.timers.Count; i++)
			{
				if (this.timers[i].End < Engine.Frame)
				{
					if (this.OnTimerEnd != null)
					{
						this.OnTimerEnd(this.timers[i].Index);
					}
					this.timers.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x04000209 RID: 521
		private static TimerManager instance;

		// Token: 0x0400020A RID: 522
		private List<TimerManager.Timer> timers;

		// Token: 0x0400020B RID: 523
		private int timerCounter;

		// Token: 0x02000066 RID: 102
		private struct Timer
		{
			// Token: 0x0400020D RID: 525
			public long End;

			// Token: 0x0400020E RID: 526
			public int Index;
		}

		// Token: 0x02000067 RID: 103
		// (Invoke) Token: 0x060002C6 RID: 710
		public delegate void OnTimerEndHandler(int timerIndex);
	}
}
