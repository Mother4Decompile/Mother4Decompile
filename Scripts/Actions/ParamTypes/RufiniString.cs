using System;

namespace Mother4.Scripts.Actions.ParamTypes
{
	// Token: 0x0200011D RID: 285
	internal struct RufiniString
	{
		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x0002BBA4 File Offset: 0x00029DA4
		public string QualifiedName
		{
			get
			{
				return string.Join('.'.ToString(), this.nameParts);
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060006DE RID: 1758 RVA: 0x0002BBC6 File Offset: 0x00029DC6
		public string[] Names
		{
			get
			{
				return this.nameParts;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060006DF RID: 1759 RVA: 0x0002BBCE File Offset: 0x00029DCE
		public string Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0002BBD8 File Offset: 0x00029DD8
		public RufiniString(string qualifiedName, string value)
		{
			this.nameParts = qualifiedName.Split(new char[]
			{
				'.'
			});
			this.value = value;
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x0002BC05 File Offset: 0x00029E05
		public RufiniString(string[] nameParts, string value)
		{
			this.nameParts = new string[nameParts.Length];
			Array.Copy(nameParts, this.nameParts, nameParts.Length);
			this.value = value;
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0002BC2C File Offset: 0x00029E2C
		public override string ToString()
		{
			string text;
			if (this.Value != null)
			{
				text = (this.value ?? string.Empty).Replace("\n", "");
				if (text.Length > 50)
				{
					int val = text.Substring(0, 50).LastIndexOf(' ');
					int length = Math.Max(50, val);
					text = text.Substring(0, length) + "…";
				}
			}
			else
			{
				text = this.QualifiedName;
			}
			return text;
		}

		// Token: 0x040008CB RID: 2251
		public const char SEPARATOR = '.';

		// Token: 0x040008CC RID: 2252
		private const int MAX_LENGTH = 50;

		// Token: 0x040008CD RID: 2253
		private const string TRAIL = "…";

		// Token: 0x040008CE RID: 2254
		private string[] nameParts;

		// Token: 0x040008CF RID: 2255
		private string value;
	}
}
