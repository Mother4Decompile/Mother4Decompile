using System;

namespace Mother4.Battle.Combos
{
	// Token: 0x020000D2 RID: 210
	internal class ComboNode : IComparable
	{
		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060004C9 RID: 1225 RVA: 0x0001E7DC File Offset: 0x0001C9DC
		public ComboType Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x0001E7E4 File Offset: 0x0001C9E4
		public uint Timestamp
		{
			get
			{
				return this.timestamp;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060004CB RID: 1227 RVA: 0x0001E7EC File Offset: 0x0001C9EC
		public uint Duration
		{
			get
			{
				return this.duration;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060004CC RID: 1228 RVA: 0x0001E7F4 File Offset: 0x0001C9F4
		public float? BPM
		{
			get
			{
				return this.bpm;
			}
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x0001E7FC File Offset: 0x0001C9FC
		public ComboNode(ComboType type, uint timestamp, uint duration)
		{
			this.Initialize(type, timestamp, duration, null);
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x0001E821 File Offset: 0x0001CA21
		public ComboNode(ComboType type, uint timestamp, uint duration, float bpm)
		{
			this.Initialize(type, timestamp, duration, new float?(bpm));
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x0001E839 File Offset: 0x0001CA39
		private void Initialize(ComboType type, uint timestamp, uint duration, float? bpm)
		{
			this.type = type;
			this.timestamp = timestamp;
			this.duration = duration;
			this.bpm = bpm;
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0001E858 File Offset: 0x0001CA58
		public int CompareTo(object obj)
		{
			if (obj is ComboNode)
			{
				ComboNode comboNode = (ComboNode)obj;
				return (int)(comboNode.timestamp - this.timestamp);
			}
			throw new ArgumentException(string.Format("Cannot compare between ComboNode and {0}", obj.GetType().Name));
		}

		// Token: 0x0400067F RID: 1663
		private ComboType type;

		// Token: 0x04000680 RID: 1664
		private uint timestamp;

		// Token: 0x04000681 RID: 1665
		private uint duration;

		// Token: 0x04000682 RID: 1666
		private float? bpm;
	}
}
