using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using Carbine.Utility;
using SFML.Graphics;

namespace Carbine.Graphics
{
	// Token: 0x02000028 RID: 40
	public class IndexedTexture : ICarbineTexture, IDisposable
	{
		// Token: 0x0600015D RID: 349
		[SuppressUnmanagedCodeSecurity]
		[DllImport("csfml-graphics-2", CallingConvention = CallingConvention.Cdecl)]
		private unsafe static extern void sfTexture_updateFromPixels(IntPtr texture, byte* pixels, uint width, uint height, uint x, uint y);

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600015E RID: 350 RVA: 0x00006DAC File Offset: 0x00004FAC
		public Texture Image
		{
			get
			{
				return this.imageTex;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00006DB4 File Offset: 0x00004FB4
		public Texture Palette
		{
			get
			{
				return this.paletteTex;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00006DBC File Offset: 0x00004FBC
		// (set) Token: 0x06000161 RID: 353 RVA: 0x00006DC4 File Offset: 0x00004FC4
		public uint CurrentPalette
		{
			get
			{
				return this.currentPal;
			}
			set
			{
				this.currentPal = Math.Min(this.totalPals, value);
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00006DD8 File Offset: 0x00004FD8
		public float CurrentPaletteFloat
		{
			get
			{
				return this.currentPal / this.totalPals;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00006DEB File Offset: 0x00004FEB
		public uint PaletteCount
		{
			get
			{
				return this.totalPals;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00006DF3 File Offset: 0x00004FF3
		public uint PaletteSize
		{
			get
			{
				return this.palSize;
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00006DFC File Offset: 0x00004FFC
		public unsafe IndexedTexture(uint width, int[][] palettes, byte[] image, Dictionary<int, SpriteDefinition> definitions, SpriteDefinition defaultDefinition)
		{
			this.totalPals = (uint)palettes.Length;
			this.palSize = (uint)palettes[0].Length;
			uint num = (uint)(image.Length / (int)width);
			Color[] array = new Color[this.palSize * this.totalPals];
			for (uint num2 = 0U; num2 < this.totalPals; num2 += 1U)
			{
				uint num3 = 0U;
				while ((ulong)num3 < (ulong)((long)palettes[(int)((UIntPtr)num2)].Length))
				{
					array[(int)((UIntPtr)(num2 * this.palSize + num3))] = ColorHelper.FromInt(palettes[(int)((UIntPtr)num2)][(int)((UIntPtr)num3)]);
					num3 += 1U;
				}
			}
			Color[] array2 = new Color[width * num];
			uint num4 = 0U;
			while ((ulong)num4 < (ulong)((long)image.Length))
			{
				array2[(int)((UIntPtr)num4)].A = byte.MaxValue;
				array2[(int)((UIntPtr)num4)].R = image[(int)((UIntPtr)num4)];
				array2[(int)((UIntPtr)num4)].G = image[(int)((UIntPtr)num4)];
				array2[(int)((UIntPtr)num4)].B = image[(int)((UIntPtr)num4)];
				num4 += 1U;
			}
			this.paletteTex = new Texture(this.palSize, this.totalPals);
			this.imageTex = new Texture(width, num);
			fixed (Color* ptr = array)
			{
				byte* pixels = (byte*)ptr;
				IndexedTexture.sfTexture_updateFromPixels(this.paletteTex.CPointer, pixels, this.palSize, this.totalPals, 0U, 0U);
			}
			fixed (Color* ptr2 = array2)
			{
				byte* pixels2 = (byte*)ptr2;
				IndexedTexture.sfTexture_updateFromPixels(this.imageTex.CPointer, pixels2, width, num, 0U, 0U);
			}
			this.definitions = definitions;
			this.defaultDefinition = defaultDefinition;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00006FA8 File Offset: 0x000051A8
		~IndexedTexture()
		{
			this.Dispose(false);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00006FD8 File Offset: 0x000051D8
		public SpriteDefinition GetSpriteDefinition(string name)
		{
			int hash = Hash.Get(name);
			return this.GetSpriteDefinition(hash);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00006FF4 File Offset: 0x000051F4
		public SpriteDefinition GetSpriteDefinition(int hash)
		{
			SpriteDefinition result;
			if (!this.definitions.TryGetValue(hash, out result))
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00007014 File Offset: 0x00005214
		public ICollection<SpriteDefinition> GetSpriteDefinitions()
		{
			return this.definitions.Values;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00007021 File Offset: 0x00005221
		public SpriteDefinition GetDefaultSpriteDefinition()
		{
			return this.defaultDefinition;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000702C File Offset: 0x0000522C
		public FullColorTexture ToFullColorTexture()
		{
			uint x = this.imageTex.Size.X;
			uint y = this.imageTex.Size.Y;
			Image image = new Image(x, y);
			Image image2 = this.imageTex.CopyToImage();
			Image image3 = this.paletteTex.CopyToImage();
			for (uint num = 0U; num < y; num += 1U)
			{
				for (uint num2 = 0U; num2 < x; num2 += 1U)
				{
					uint x2 = (uint)((double)image2.GetPixel(num2, num).R / 255.0 * this.palSize);
					Color pixel = image3.GetPixel(x2, this.currentPal);
					image.SetPixel(num2, num, pixel);
				}
			}
			image.SaveToFile("img.png");
			image2.SaveToFile("indImg.png");
			image3.SaveToFile("palImg.png");
			return new FullColorTexture(image);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000710E File Offset: 0x0000530E
		public virtual void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000711D File Offset: 0x0000531D
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				this.imageTex.Dispose();
				this.paletteTex.Dispose();
			}
			this.disposed = true;
		}

		// Token: 0x040000C6 RID: 198
		private SpriteDefinition defaultDefinition;

		// Token: 0x040000C7 RID: 199
		private Dictionary<int, SpriteDefinition> definitions;

		// Token: 0x040000C8 RID: 200
		private Texture paletteTex;

		// Token: 0x040000C9 RID: 201
		private Texture imageTex;

		// Token: 0x040000CA RID: 202
		private uint currentPal;

		// Token: 0x040000CB RID: 203
		private uint totalPals;

		// Token: 0x040000CC RID: 204
		private uint palSize;

		// Token: 0x040000CD RID: 205
		private bool disposed;
	}
}
