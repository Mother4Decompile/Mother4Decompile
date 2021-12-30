using System;
using System.Collections.Generic;

namespace Carbine.Actors
{
	// Token: 0x02000003 RID: 3
	public class ActorManager
	{
		// Token: 0x0600000C RID: 12 RVA: 0x000020F1 File Offset: 0x000002F1
		public ActorManager()
		{
			this.actors = new Actor[this.MAX_ACTORS];
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002118 File Offset: 0x00000318
		public void Add(Actor actor)
		{
			if (actor == null)
			{
				throw new ArgumentNullException("Cannot add null to the actor manager.");
			}
			if (this.actorCount >= 0 && this.actorCount < this.actors.Length)
			{
				for (int i = 0; i < this.actors.Length; i++)
				{
					if (this.actors[i] == null)
					{
						this.actors[i] = actor;
						this.actorCount++;
						return;
					}
				}
				return;
			}
			throw new InvalidOperationException("Tried to add an actor out of the bounds of the actor buffer.");
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000218C File Offset: 0x0000038C
		public void AddAll<T>(IEnumerable<T> addActors) where T : Actor
		{
			foreach (T t in addActors)
			{
				Actor actor = t;
				this.Add(actor);
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000021DC File Offset: 0x000003DC
		private void ShiftActors(int startIndex, int amount)
		{
			for (int i = startIndex + amount; i < this.actors.Length; i++)
			{
				this.actors[i - amount] = this.actors[i];
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002210 File Offset: 0x00000410
		public void Remove(Actor actor)
		{
			this.Remove(actor, true);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000221C File Offset: 0x0000041C
		public void Remove(Actor actor, bool dispose)
		{
			if (actor == null)
			{
				throw new ArgumentNullException("Cannot remove null from the actor manager.");
			}
			for (int i = 0; i < this.actors.Length; i++)
			{
				if (this.actors[i] == actor)
				{
					if (dispose)
					{
						this.actors[i].Dispose();
					}
					this.ShiftActors(i, 1);
					if (this.isIterating)
					{
						this.iterator--;
					}
					this.actorCount--;
					return;
				}
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002294 File Offset: 0x00000494
		public Actor Find(Func<Actor, bool> predicate)
		{
			Actor result = null;
			for (int i = 0; i < this.actors.Length; i++)
			{
				if (this.actors[i] != null && predicate(this.actors[i]))
				{
					result = this.actors[i];
					break;
				}
			}
			return result;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000022DC File Offset: 0x000004DC
		public void Step()
		{
			this.isIterating = true;
			this.iterator = 0;
			while (this.iterator < this.actors.Length)
			{
				if (this.actors[this.iterator] != null)
				{
					this.actors[this.iterator].Input();
				}
				if (this.actors[this.iterator] != null)
				{
					this.actors[this.iterator].Update();
				}
				this.iterator++;
			}
			this.isIterating = false;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002360 File Offset: 0x00000560
		public void Clear()
		{
			this.Clear(true);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000236C File Offset: 0x0000056C
		public void Clear(bool dispose)
		{
			for (int i = 0; i < this.actors.Length; i++)
			{
				if (this.actors[i] != null)
				{
					if (dispose)
					{
						this.actors[i].Dispose();
					}
					this.actors[i] = null;
				}
			}
			this.actorCount = 0;
			if (this.isIterating)
			{
				this.iterator = 0;
			}
		}

		// Token: 0x04000005 RID: 5
		private int MAX_ACTORS = 256;

		// Token: 0x04000006 RID: 6
		private Actor[] actors;

		// Token: 0x04000007 RID: 7
		private int actorCount;

		// Token: 0x04000008 RID: 8
		private bool isIterating;

		// Token: 0x04000009 RID: 9
		private int iterator;
	}
}
