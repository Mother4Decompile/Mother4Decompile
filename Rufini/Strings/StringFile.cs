using System;
using System.IO;
using fNbt;
using Mother4.Data;
using Mother4.Scripts.Actions.ParamTypes;

namespace Rufini.Strings
{
	// Token: 0x02000170 RID: 368
	internal class StringFile
	{
		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060007C0 RID: 1984 RVA: 0x000320BE File Offset: 0x000302BE
		public static StringFile Instance
		{
			get
			{
				if (StringFile.instance == null)
				{
					StringFile.instance = new StringFile();
				}
				return StringFile.instance;
			}
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x000320D6 File Offset: 0x000302D6
		private StringFile()
		{
			this.filename = Path.Combine(Paths.TEXT, Settings.Locale, Settings.Locale + ".dat");
			this.Reload();
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x00032108 File Offset: 0x00030308
		public void Reload()
		{
			if (File.Exists(this.filename))
			{
				this.file = new NbtFile(this.filename);
				return;
			}
			Console.WriteLine("No strings file was found.");
			NbtCompound rootTag = new NbtCompound("strings");
			this.file = new NbtFile(rootTag);
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x00032158 File Offset: 0x00030358
		public RufiniString Get(string[] nameParts)
		{
			if (nameParts == null)
			{
				throw new ArgumentNullException("Cannot get a string with a null qualified name.");
			}
			string value = null;
			NbtCompound rootTag = this.file.RootTag;
			if (rootTag == null)
			{
				throw new InvalidOperationException("String file root tag is null.");
			}
			for (int i = 0; i < nameParts.Length - 1; i++)
			{
				rootTag.TryGet<NbtCompound>(nameParts[i], out rootTag);
				if (rootTag == null)
				{
					break;
				}
			}
			if (rootTag != null)
			{
				NbtTag nbtTag = null;
				rootTag.TryGet(nameParts[nameParts.Length - 1], out nbtTag);
				if (nbtTag is NbtString)
				{
					value = nbtTag.StringValue;
				}
			}
			return new RufiniString(nameParts, value);
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x000321DC File Offset: 0x000303DC
		public RufiniString Get(string qualifiedName)
		{
			if (qualifiedName == null)
			{
				throw new ArgumentNullException("Cannot get a string with a null qualified name.");
			}
			return this.Get(qualifiedName.Split(new char[]
			{
				'.'
			}));
		}

		// Token: 0x0400097C RID: 2428
		private static StringFile instance;

		// Token: 0x0400097D RID: 2429
		private NbtFile file;

		// Token: 0x0400097E RID: 2430
		private string filename;
	}
}
