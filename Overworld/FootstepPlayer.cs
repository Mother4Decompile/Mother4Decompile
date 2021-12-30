using System;
using System.Collections.Generic;
using Carbine.Audio;
using Carbine.Utility;
using Mother4.Data;

namespace Mother4.Overworld
{
	// Token: 0x020000FE RID: 254
	internal class FootstepPlayer : IDisposable
	{
		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060005D1 RID: 1489 RVA: 0x00022B7D File Offset: 0x00020D7D
		// (set) Token: 0x060005D2 RID: 1490 RVA: 0x00022B85 File Offset: 0x00020D85
		public TerrainType Terrain
		{
			get
			{
				return this.terrainType;
			}
			set
			{
				this.terrainType = value;
			}
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00022B90 File Offset: 0x00020D90
		public FootstepPlayer()
		{
			this.stepCount = 0;
			this.footstepMap = new Dictionary<TerrainType, CarbineSound[]>();
			this.terrainType = TerrainType.Tile;
			this.isPaused = false;
			this.timerIndex = -1;
			TimerManager.Instance.OnTimerEnd += this.TimerEnd;
			CarbineSound[] value = new CarbineSound[]
			{
				this.Load("stepGrass1"),
				this.Load("stepGrass2")
			};
			this.footstepMap.Add(TerrainType.Grass, value);
			this.footstepMap.Add(TerrainType.Moss, value);
			CarbineSound[] value2 = new CarbineSound[]
			{
				this.Load("stepTile1"),
				this.Load("stepTile2")
			};
			this.footstepMap.Add(TerrainType.Tile, value2);
			this.footstepMap.Add(TerrainType.Stone, value2);
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00022C60 File Offset: 0x00020E60
		~FootstepPlayer()
		{
			this.Dispose(false);
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00022C90 File Offset: 0x00020E90
		private CarbineSound Load(string name)
		{
			return AudioManager.Instance.Use(Paths.AUDIO + name + ".wav", AudioType.Sound);
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x00022CAD File Offset: 0x00020EAD
		private void TimerEnd(int timerIndex)
		{
			if (this.timerIndex == timerIndex)
			{
				this.Play();
				this.timerIndex = TimerManager.Instance.StartTimer(12);
			}
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00022CD0 File Offset: 0x00020ED0
		private void Play()
		{
			CarbineSound[] array;
			if (!this.disposed && !this.isPaused && this.footstepMap.TryGetValue(this.terrainType, out array))
			{
				this.lastSound = array[this.stepCount % array.Length];
				this.lastSound.Play();
				this.stepCount++;
			}
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00022D2D File Offset: 0x00020F2D
		public void Start()
		{
			if (!this.disposed)
			{
				if (!this.isPaused)
				{
					this.timerIndex = TimerManager.Instance.StartTimer(0);
				}
				this.isPaused = false;
			}
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00022D57 File Offset: 0x00020F57
		public void Resume()
		{
			if (!this.disposed)
			{
				this.isPaused = false;
			}
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00022D68 File Offset: 0x00020F68
		public void Pause()
		{
			if (!this.disposed)
			{
				this.isPaused = true;
				if (this.lastSound != null)
				{
					this.lastSound.Stop();
				}
			}
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x00022D8C File Offset: 0x00020F8C
		public void Stop()
		{
			if (!this.disposed && this.lastSound != null)
			{
				this.lastSound.Stop();
				this.lastSound = null;
				TimerManager.Instance.Cancel(this.timerIndex);
				this.timerIndex = -1;
			}
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x00022DC7 File Offset: 0x00020FC7
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00022DD8 File Offset: 0x00020FD8
		private void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				foreach (CarbineSound[] array in this.footstepMap.Values)
				{
					for (int i = 0; i < array.Length; i++)
					{
						AudioManager.Instance.Unuse(array[i]);
					}
				}
				TimerManager.Instance.OnTimerEnd -= this.TimerEnd;
			}
			this.disposed = true;
		}

		// Token: 0x0400079B RID: 1947
		private const string EXTENSION = ".wav";

		// Token: 0x0400079C RID: 1948
		private const int FOOTSTEP_TIMER_DURATION = 12;

		// Token: 0x0400079D RID: 1949
		private bool disposed;

		// Token: 0x0400079E RID: 1950
		private Dictionary<TerrainType, CarbineSound[]> footstepMap;

		// Token: 0x0400079F RID: 1951
		private int stepCount;

		// Token: 0x040007A0 RID: 1952
		private CarbineSound lastSound;

		// Token: 0x040007A1 RID: 1953
		private TerrainType terrainType;

		// Token: 0x040007A2 RID: 1954
		private int timerIndex;

		// Token: 0x040007A3 RID: 1955
		private bool isPaused;
	}
}
