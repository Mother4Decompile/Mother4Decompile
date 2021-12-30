using System;
using System.Collections.Generic;
using Carbine.Actors;
using Carbine.Collision;
using Carbine.Graphics;
using Carbine.Maps;
using Mother4.Actors;
using Mother4.Actors.NPCs;
using Mother4.GUI;

namespace Mother4.Scripts
{
	// Token: 0x0200016A RID: 362
	internal class ExecutionContext
	{
		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000798 RID: 1944 RVA: 0x00031970 File Offset: 0x0002FB70
		// (set) Token: 0x06000799 RID: 1945 RVA: 0x00031978 File Offset: 0x0002FB78
		public ScriptExecutor Executor { get; set; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600079A RID: 1946 RVA: 0x00031981 File Offset: 0x0002FB81
		// (set) Token: 0x0600079B RID: 1947 RVA: 0x00031989 File Offset: 0x0002FB89
		public RenderPipeline Pipeline { get; set; }

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600079C RID: 1948 RVA: 0x00031992 File Offset: 0x0002FB92
		// (set) Token: 0x0600079D RID: 1949 RVA: 0x0003199A File Offset: 0x0002FB9A
		public ActorManager ActorManager { get; set; }

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600079E RID: 1950 RVA: 0x000319A3 File Offset: 0x0002FBA3
		// (set) Token: 0x0600079F RID: 1951 RVA: 0x000319AB File Offset: 0x0002FBAB
		public CollisionManager CollisionManager { get; set; }

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060007A0 RID: 1952 RVA: 0x000319B4 File Offset: 0x0002FBB4
		// (set) Token: 0x060007A1 RID: 1953 RVA: 0x000319BC File Offset: 0x0002FBBC
		public TextBox TextBox { get; set; }

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060007A2 RID: 1954 RVA: 0x000319C5 File Offset: 0x0002FBC5
		// (set) Token: 0x060007A3 RID: 1955 RVA: 0x000319CD File Offset: 0x0002FBCD
		public Player Player { get; set; }

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060007A4 RID: 1956 RVA: 0x000319D6 File Offset: 0x0002FBD6
		// (set) Token: 0x060007A5 RID: 1957 RVA: 0x000319DE File Offset: 0x0002FBDE
		public NPC ActiveNPC { get; set; }

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060007A6 RID: 1958 RVA: 0x000319E7 File Offset: 0x0002FBE7
		// (set) Token: 0x060007A7 RID: 1959 RVA: 0x000319EF File Offset: 0x0002FBEF
		public NPC CheckedNPC { get; set; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060007A8 RID: 1960 RVA: 0x000319F8 File Offset: 0x0002FBF8
		// (set) Token: 0x060007A9 RID: 1961 RVA: 0x00031A00 File Offset: 0x0002FC00
		public ICollection<Map.Path> Paths { get; set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060007AA RID: 1962 RVA: 0x00031A09 File Offset: 0x0002FC09
		// (set) Token: 0x060007AB RID: 1963 RVA: 0x00031A11 File Offset: 0x0002FC11
		public ICollection<Map.Area> Areas { get; set; }

		// Token: 0x060007AC RID: 1964 RVA: 0x00031A1A File Offset: 0x0002FC1A
		public ExecutionContext()
		{
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x00031A24 File Offset: 0x0002FC24
		public ExecutionContext(ExecutionContext source)
		{
			this.Executor = source.Executor;
			this.Pipeline = source.Pipeline;
			this.ActorManager = source.ActorManager;
			this.CollisionManager = source.CollisionManager;
			this.TextBox = source.TextBox;
			this.Player = source.Player;
			this.ActiveNPC = source.ActiveNPC;
			this.CheckedNPC = source.CheckedNPC;
			this.Paths = source.Paths;
			this.Areas = source.Areas;
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x00031ADC File Offset: 0x0002FCDC
		public NPC GetNpcByName(string npcName)
		{
			return (NPC)this.ActorManager.Find((Actor n) => n is NPC && ((NPC)n).Name == npcName);
		}
	}
}
