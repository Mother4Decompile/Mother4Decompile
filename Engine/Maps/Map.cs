using System;
using System.Collections.Generic;
using Carbine.Collision;
using Carbine.Tiles;
using SFML.Graphics;
using SFML.System;

namespace Carbine.Maps
{
	// Token: 0x0200003E RID: 62
	public class Map
	{
		// Token: 0x06000235 RID: 565 RVA: 0x0000BA80 File Offset: 0x00009C80
		public Map()
		{
			this.Head = default(Map.Header);
			this.Music = new List<Map.BGM>();
			this.SoundEffects = new List<Map.SFX>();
			this.Portals = new List<Map.Portal>();
			this.Triggers = new List<Map.Trigger>();
			this.NPCs = new List<Map.NPC>();
			this.Paths = new List<Map.Path>();
			this.Areas = new List<Map.Area>();
			this.Crowds = new List<Map.Crowd>();
			this.Spawns = new List<Map.EnemySpawn>();
			this.Mesh = new List<Mesh>();
			this.Groups = new List<Map.Group>();
			this.TileAnimationProperties = new Dictionary<int, Map.TileAnimation>();
			this.Parallaxes = new List<Map.Parallax>();
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000BB30 File Offset: 0x00009D30
		public IList<TileGroup> MakeTileGroups(string graphicDirectory, uint palette)
		{
			string arg = "default";
			if (this.Head.Tilesets.Count > 0)
			{
				arg = this.Head.Tilesets[0].Name;
			}
			string resource = string.Format("{0}{1}.dat", graphicDirectory, arg);
			IList<TileGroup> list = new List<TileGroup>(this.Groups.Count);
			long ticks = DateTime.Now.Ticks;
			for (int i = 0; i < this.Groups.Count; i++)
			{
				Map.Group group = this.Groups[i];
				IList<Tile> list2 = new List<Tile>(group.Tiles.Length / 2);
				int j = 0;
				int num = 0;
				while (j < group.Tiles.Length)
				{
					int num2 = (int)(group.Tiles[j] - 1);
					if (num2 >= 0)
					{
						ushort num3;
						if (j + 1 < group.Tiles.Length)
						{
							num3 = group.Tiles[j + 1];
						}
						else
						{
							num3 = 0;
						}
						int num4 = group.Width * 8;
						Vector2f position = new Vector2f((float)((long)num * 8L % (long)num4), (float)((long)num * 8L / (long)num4 * 8L));
						bool flipHoriz = (num3 & 1) > 0;
						bool flipVert = (num3 & 2) > 0;
						bool flipDiag = (num3 & 4) > 0;
						ushort animId = (ushort)(num3 >> 3);
						Tile item = new Tile((uint)num2, position, flipHoriz, flipVert, flipDiag, animId);
						list2.Add(item);
					}
					j += 2;
					num++;
				}
				TileGroup item2 = new TileGroup(list2, resource, group.Depth, new Vector2f((float)group.X, (float)group.Y), palette);
				list.Add(item2);
			}
			Console.WriteLine("Created tile groups in {0}ms", (DateTime.Now.Ticks - ticks) / 10000L);
			return list;
		}

		// Token: 0x04000165 RID: 357
		public Map.Header Head;

		// Token: 0x04000166 RID: 358
		public IList<Map.BGM> Music;

		// Token: 0x04000167 RID: 359
		public IList<Map.SFX> SoundEffects;

		// Token: 0x04000168 RID: 360
		public IList<Map.Portal> Portals;

		// Token: 0x04000169 RID: 361
		public IList<Map.Trigger> Triggers;

		// Token: 0x0400016A RID: 362
		public IList<Map.NPC> NPCs;

		// Token: 0x0400016B RID: 363
		public IList<Map.Path> Paths;

		// Token: 0x0400016C RID: 364
		public IList<Map.Area> Areas;

		// Token: 0x0400016D RID: 365
		public IList<Map.Crowd> Crowds;

		// Token: 0x0400016E RID: 366
		public IList<Map.EnemySpawn> Spawns;

		// Token: 0x0400016F RID: 367
		public IList<Mesh> Mesh;

		// Token: 0x04000170 RID: 368
		public IList<Map.Group> Groups;

		// Token: 0x04000171 RID: 369
		public IList<Map.Parallax> Parallaxes;

		// Token: 0x04000172 RID: 370
		public IDictionary<int, Map.TileAnimation> TileAnimationProperties;

		// Token: 0x0200003F RID: 63
		public struct Header
		{
			// Token: 0x04000173 RID: 371
			public Color PrimaryColor;

			// Token: 0x04000174 RID: 372
			public Color SecondaryColor;

			// Token: 0x04000175 RID: 373
			public string Title;

			// Token: 0x04000176 RID: 374
			public string Subtitle;

			// Token: 0x04000177 RID: 375
			public int Width;

			// Token: 0x04000178 RID: 376
			public int Height;

			// Token: 0x04000179 RID: 377
			public List<Map.Tileset> Tilesets;

			// Token: 0x0400017A RID: 378
			public string Script;

			// Token: 0x0400017B RID: 379
			public string BBG;

			// Token: 0x0400017C RID: 380
			public bool Shadows;

			// Token: 0x0400017D RID: 381
			public bool Ocean;
		}

		// Token: 0x02000040 RID: 64
		public struct Tileset
		{
			// Token: 0x0400017E RID: 382
			public string Name;

			// Token: 0x0400017F RID: 383
			public int FirstId;
		}

		// Token: 0x02000041 RID: 65
		public struct BGM
		{
			// Token: 0x04000180 RID: 384
			public int X;

			// Token: 0x04000181 RID: 385
			public int Y;

			// Token: 0x04000182 RID: 386
			public int Width;

			// Token: 0x04000183 RID: 387
			public int Height;

			// Token: 0x04000184 RID: 388
			public string Name;

			// Token: 0x04000185 RID: 389
			public short Flag;

			// Token: 0x04000186 RID: 390
			public bool Loop;
		}

		// Token: 0x02000042 RID: 66
		public struct SFX
		{
			// Token: 0x04000187 RID: 391
			public int X;

			// Token: 0x04000188 RID: 392
			public int Y;

			// Token: 0x04000189 RID: 393
			public int Width;

			// Token: 0x0400018A RID: 394
			public int Height;

			// Token: 0x0400018B RID: 395
			public string Name;

			// Token: 0x0400018C RID: 396
			public short Flag;

			// Token: 0x0400018D RID: 397
			public short Interval;

			// Token: 0x0400018E RID: 398
			public bool Loop;
		}

		// Token: 0x02000043 RID: 67
		public struct Portal
		{
			// Token: 0x0400018F RID: 399
			public int X;

			// Token: 0x04000190 RID: 400
			public int Y;

			// Token: 0x04000191 RID: 401
			public int Width;

			// Token: 0x04000192 RID: 402
			public int Height;

			// Token: 0x04000193 RID: 403
			public int Xto;

			// Token: 0x04000194 RID: 404
			public int Yto;

			// Token: 0x04000195 RID: 405
			public int DirectionTo;

			// Token: 0x04000196 RID: 406
			public string Map;

			// Token: 0x04000197 RID: 407
			public int Flag;

			// Token: 0x04000198 RID: 408
			public int SFX;
		}

		// Token: 0x02000044 RID: 68
		public struct Trigger
		{
			// Token: 0x04000199 RID: 409
			public Vector2f Position;

			// Token: 0x0400019A RID: 410
			public List<Vector2f> Points;

			// Token: 0x0400019B RID: 411
			public string Script;

			// Token: 0x0400019C RID: 412
			public int Flag;
		}

		// Token: 0x02000045 RID: 69
		public struct NPCtext
		{
			// Token: 0x0400019D RID: 413
			public string ID;

			// Token: 0x0400019E RID: 414
			public int Flag;
		}

		// Token: 0x02000046 RID: 70
		public struct NPC
		{
			// Token: 0x0400019F RID: 415
			public int X;

			// Token: 0x040001A0 RID: 416
			public int Y;

			// Token: 0x040001A1 RID: 417
			public int Width;

			// Token: 0x040001A2 RID: 418
			public int Height;

			// Token: 0x040001A3 RID: 419
			public string Name;

			// Token: 0x040001A4 RID: 420
			public string Sprite;

			// Token: 0x040001A5 RID: 421
			public short Direction;

			// Token: 0x040001A6 RID: 422
			public short Mode;

			// Token: 0x040001A7 RID: 423
			public float Speed;

			// Token: 0x040001A8 RID: 424
			public short Delay;

			// Token: 0x040001A9 RID: 425
			public short Distance;

			// Token: 0x040001AA RID: 426
			public string Constraint;

			// Token: 0x040001AB RID: 427
			public short Flag;

			// Token: 0x040001AC RID: 428
			public bool Shadow;

			// Token: 0x040001AD RID: 429
			public bool Solid;

			// Token: 0x040001AE RID: 430
			public bool Sticky;

			// Token: 0x040001AF RID: 431
			public int DepthOverride;

			// Token: 0x040001B0 RID: 432
			public bool Enabled;

			// Token: 0x040001B1 RID: 433
			public List<Map.NPCtext> Text;

			// Token: 0x040001B2 RID: 434
			public List<Map.NPCtext> TeleText;
		}

		// Token: 0x02000047 RID: 71
		public struct Path
		{
			// Token: 0x040001B3 RID: 435
			public string Name;

			// Token: 0x040001B4 RID: 436
			public List<Vector2f> Points;
		}

		// Token: 0x02000048 RID: 72
		public struct Area
		{
			// Token: 0x040001B5 RID: 437
			public string Name;

			// Token: 0x040001B6 RID: 438
			public IntRect Rectangle;
		}

		// Token: 0x02000049 RID: 73
		public struct Crowd
		{
			// Token: 0x040001B7 RID: 439
			public bool Loop;

			// Token: 0x040001B8 RID: 440
			public List<int> Sprites;

			// Token: 0x040001B9 RID: 441
			public List<Vector2f> Points;
		}

		// Token: 0x0200004A RID: 74
		public struct Enemy
		{
			// Token: 0x040001BA RID: 442
			public int ID;

			// Token: 0x040001BB RID: 443
			public int Chance;
		}

		// Token: 0x0200004B RID: 75
		public struct EnemySpawn
		{
			// Token: 0x040001BC RID: 444
			public int X;

			// Token: 0x040001BD RID: 445
			public int Y;

			// Token: 0x040001BE RID: 446
			public int Width;

			// Token: 0x040001BF RID: 447
			public int Height;

			// Token: 0x040001C0 RID: 448
			public List<Map.Enemy> Enemies;
		}

		// Token: 0x0200004C RID: 76
		public struct TileModifier
		{
			// Token: 0x040001C1 RID: 449
			public bool FlipHorizontal;

			// Token: 0x040001C2 RID: 450
			public bool FlipVertical;

			// Token: 0x040001C3 RID: 451
			public bool FlipDiagonal;
		}

		// Token: 0x0200004D RID: 77
		public struct Group
		{
			// Token: 0x040001C4 RID: 452
			public ushort[] Tiles;

			// Token: 0x040001C5 RID: 453
			public int Depth;

			// Token: 0x040001C6 RID: 454
			public int X;

			// Token: 0x040001C7 RID: 455
			public int Y;

			// Token: 0x040001C8 RID: 456
			public int Width;

			// Token: 0x040001C9 RID: 457
			public int Height;
		}

		// Token: 0x0200004E RID: 78
		public struct TileAnimation
		{
			// Token: 0x040001CA RID: 458
			public int Id;

			// Token: 0x040001CB RID: 459
			public int Frames;

			// Token: 0x040001CC RID: 460
			public int SkipVert;

			// Token: 0x040001CD RID: 461
			public int SkipHoriz;

			// Token: 0x040001CE RID: 462
			public float Speed;
		}

		// Token: 0x0200004F RID: 79
		public struct Parallax
		{
			// Token: 0x040001CF RID: 463
			public string Sprite;

			// Token: 0x040001D0 RID: 464
			public Vector2f Vector;

			// Token: 0x040001D1 RID: 465
			public IntRect Area;

			// Token: 0x040001D2 RID: 466
			public int Depth;
		}
	}
}
