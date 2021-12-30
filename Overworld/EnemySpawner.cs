using System;
using System.Collections.Generic;
using Carbine;
using Carbine.Collision;
using Carbine.Graphics;
using Carbine.Maps;
using Mother4.Actors.NPCs;
using Mother4.Data;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Overworld
{
	// Token: 0x02000101 RID: 257
	internal class EnemySpawner
	{
		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060005F2 RID: 1522 RVA: 0x0002322D File Offset: 0x0002142D
		public FloatRect Bounds
		{
			get
			{
				return this.rectangle;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060005F3 RID: 1523 RVA: 0x00023235 File Offset: 0x00021435
		// (set) Token: 0x060005F4 RID: 1524 RVA: 0x0002323D File Offset: 0x0002143D
		public bool SpawnFlag
		{
			get
			{
				return this.spawnFlag;
			}
			set
			{
				this.spawnFlag = value;
			}
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00023246 File Offset: 0x00021446
		public EnemySpawner(FloatRect rectangle, List<Map.Enemy> enemies)
		{
			this.rectangle = rectangle;
			this.chances = enemies;
			this.spawnFlag = true;
			this.spawnedOnce = false;
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x0002326C File Offset: 0x0002146C
		public List<EnemyNPC> GenerateEnemies(RenderPipeline pipeline, CollisionManager collision)
		{
			List<EnemyNPC> list = null;
			if (this.spawnFlag && !this.spawnedOnce)
			{
				foreach (Map.Enemy enemy in this.chances)
				{
					if (Engine.Random.Next(100) < enemy.Chance)
					{
						Vector2f position = new Vector2f(this.rectangle.Left + (float)Engine.Random.Next((int)this.rectangle.Width), this.rectangle.Top + (float)Engine.Random.Next((int)this.rectangle.Height));
						EnemyNPC item = new EnemyNPC(pipeline, collision, (EnemyType)enemy.ID, position, this.rectangle);
						if (list == null)
						{
							list = new List<EnemyNPC>();
						}
						list.Add(item);
						this.spawnedOnce = true;
						break;
					}
				}
				this.spawnFlag = false;
			}
			return list;
		}

		// Token: 0x040007B3 RID: 1971
		private const int MAX_CHANCE = 100;

		// Token: 0x040007B4 RID: 1972
		private FloatRect rectangle;

		// Token: 0x040007B5 RID: 1973
		private List<Map.Enemy> chances;

		// Token: 0x040007B6 RID: 1974
		private bool spawnFlag;

		// Token: 0x040007B7 RID: 1975
		private bool spawnedOnce;
	}
}
