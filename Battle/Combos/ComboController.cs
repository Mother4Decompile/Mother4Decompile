using System;
using Mother4.Data;

namespace Mother4.Battle.Combos
{
	// Token: 0x020000D1 RID: 209
	internal class ComboController : IDisposable
	{
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060004C1 RID: 1217 RVA: 0x0001E5F3 File Offset: 0x0001C7F3
		public ComboSet ComboSet
		{
			get
			{
				return this.combos;
			}
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x0001E5FB File Offset: 0x0001C7FB
		public ComboController(ComboSet combos, CharacterType[] party)
		{
			this.combos = combos;
			this.currentNode = combos.GetFirstCombo();
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0001E618 File Offset: 0x0001C818
		~ComboController()
		{
			this.Dispose(false);
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0001E648 File Offset: 0x0001C848
		public bool IsCombo(uint time)
		{
			if (this.currentNode.Timestamp + this.currentNode.Duration + 225U < time - 225U)
			{
				this.currentNode = this.combos.GetNextCombo(this.currentNode);
			}
			bool result = false;
			switch (this.currentNode.Type)
			{
			case ComboType.BPMRange:
				result = this.IsBPMCombo(time);
				break;
			case ComboType.Point:
				result = this.IsPointCombo(time);
				break;
			}
			return result;
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0001E6C4 File Offset: 0x0001C8C4
		private bool IsPointCombo(uint time)
		{
			return time >= this.currentNode.Timestamp - 225U && time < this.currentNode.Timestamp + this.currentNode.Duration + 225U;
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0001E70C File Offset: 0x0001C90C
		private bool IsBPMCombo(uint time)
		{
			bool flag = time >= this.currentNode.Timestamp - 225U && time < this.currentNode.Timestamp + this.currentNode.Duration + 225U;
			uint num = (uint)(60000f / this.currentNode.BPM.Value);
			uint num2 = time - this.currentNode.Timestamp;
			uint num3 = num2 / num;
			uint num4 = this.currentNode.Timestamp + num * num3;
			bool flag2 = time >= num4 - 225U && time < num4 + 225U;
			bool result = false;
			if (flag && flag2)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0001E7BA File Offset: 0x0001C9BA
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0001E7C9 File Offset: 0x0001C9C9
		protected void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				this.disposed = true;
			}
		}

		// Token: 0x0400067A RID: 1658
		public const uint MS_PER_MIN = 60000U;

		// Token: 0x0400067B RID: 1659
		public const uint TOLERANCE = 225U;

		// Token: 0x0400067C RID: 1660
		protected bool disposed;

		// Token: 0x0400067D RID: 1661
		private ComboSet combos;

		// Token: 0x0400067E RID: 1662
		private ComboNode currentNode;
	}
}
