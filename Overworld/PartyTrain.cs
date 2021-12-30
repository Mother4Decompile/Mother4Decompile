using System;
using System.Collections.Generic;
using Carbine.Utility;
using Mother4.Actors;
using Mother4.Data;
using SFML.System;

namespace Mother4.Overworld
{
	// Token: 0x02000174 RID: 372
	internal class PartyTrain
	{
		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060007CF RID: 1999 RVA: 0x00032761 File Offset: 0x00030961
		// (set) Token: 0x060007D0 RID: 2000 RVA: 0x00032769 File Offset: 0x00030969
		public bool Running
		{
			get
			{
				return this.running;
			}
			set
			{
				this.running = value;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x00032772 File Offset: 0x00030972
		// (set) Token: 0x060007D2 RID: 2002 RVA: 0x0003277A File Offset: 0x0003097A
		public bool Crouching
		{
			get
			{
				return this.crouching;
			}
			set
			{
				this.crouching = value;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060007D3 RID: 2003 RVA: 0x00032783 File Offset: 0x00030983
		// (set) Token: 0x060007D4 RID: 2004 RVA: 0x0003278B File Offset: 0x0003098B
		public bool MovementLocked
		{
			get
			{
				return this.movementLocked;
			}
			set
			{
				this.movementLocked = value;
			}
		}

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x060007D5 RID: 2005 RVA: 0x00032794 File Offset: 0x00030994
		// (remove) Token: 0x060007D6 RID: 2006 RVA: 0x000327CC File Offset: 0x000309CC
		public event PartyTrain.OnResetHandler OnReset;

		// Token: 0x060007D7 RID: 2007 RVA: 0x00032801 File Offset: 0x00030A01
		public PartyTrain(Vector2f initialPosition, int direction, TerrainType initialTerrain, bool extend)
		{
			this.followers = new List<PartyFollower>();
			this.recordPoints = new PartyTrain.RecordPoint[1];
			this.Reset(initialPosition, direction, initialTerrain, extend);
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x0003282B File Offset: 0x00030A2B
		public void Reset(Vector2f position, int direction, TerrainType terrain)
		{
			this.Reset(position, direction, terrain, false);
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x00032838 File Offset: 0x00030A38
		public void Reset(Vector2f position, int direction, TerrainType terrain, bool extend)
		{
			Vector2f v = VectorMath.DirectionToVector((direction + 4) % 8);
			for (int i = 0; i < this.recordPoints.Length; i++)
			{
				this.recordPoints[i].position = ((!extend) ? position : (position + (float)(this.recordPoints.Length - i) * v));
				this.recordPoints[i].velocity = VectorMath.DirectionToVector(direction);
				this.recordPoints[i].terrain = terrain;
			}
			if (this.OnReset != null)
			{
				this.OnReset(position, direction);
			}
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x000328D0 File Offset: 0x00030AD0
		private int FindNextPlace()
		{
			int num = 0;
			for (int i = 0; i < this.followers.Count; i++)
			{
				PartyFollower partyFollower = this.followers[i];
				num += (int)partyFollower.Width + 2;
			}
			return num;
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x00032910 File Offset: 0x00030B10
		private int GetRecordPointIndexFromPlace(int place)
		{
			int i;
			for (i = this.pos - place; i < 0; i += this.recordPoints.Length)
			{
			}
			return i;
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x00032938 File Offset: 0x00030B38
		private void ResizeRecording()
		{
			int num = this.recordPoints.Length;
			int num2 = 0;
			foreach (PartyFollower partyFollower in this.followers)
			{
				num2 += (int)partyFollower.Width + 2;
			}
			num2++;
			if (num2 != num)
			{
				PartyTrain.RecordPoint[] array = new PartyTrain.RecordPoint[num2];
				int num3 = (this.pos + 1) % this.recordPoints.Length;
				for (int i = 0; i < array.Length; i++)
				{
					if (i < this.recordPoints.Length)
					{
						int num4 = (this.pos + 1 + i) % this.recordPoints.Length;
						array[i] = this.recordPoints[num4];
					}
					else
					{
						array[i] = this.recordPoints[num3];
					}
				}
				if (num2 > num)
				{
					this.pos = this.recordPoints.Length - 1;
				}
				this.recordPoints = array;
			}
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x00032A54 File Offset: 0x00030C54
		public void Add(PartyFollower follower)
		{
			this.followers.Add(follower);
			this.ResizeRecording();
			follower.Place = this.FindNextPlace();
			float width = follower.Width;
			int num = (this.pos + 1) % this.recordPoints.Length;
			int num2 = this.recordPoints.Length - ((this.followers.Count == 1) ? 0 : 1);
			Vector2f position = this.recordPoints[num].position;
			Vector2f position2 = follower.Position;
			int direction = VectorMath.VectorToDirection(position - position2);
			Vector2f velocity = VectorMath.DirectionToVector(direction);
			for (int i = num; i <= num2; i++)
			{
				float num3 = (float)(i - num) / (float)(num2 - num);
				int num4 = i % this.recordPoints.Length;
				this.recordPoints[num4].position.X = (float)((int)(position2.X + (position.X - position2.X) * num3));
				this.recordPoints[num4].position.Y = (float)((int)(position2.Y + (position.Y - position2.Y) * num3));
				this.recordPoints[num4].velocity = velocity;
			}
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x00032B90 File Offset: 0x00030D90
		public PartyFollower Remove(PartyFollower follower)
		{
			float width = follower.Width;
			int place = follower.Place;
			int num = this.followers.IndexOf(follower);
			this.followers.Remove(follower);
			follower.Place = -1;
			int num2 = (num - 1 < 0) ? 0 : this.followers[num - 1].Place;
			for (int i = num; i < this.followers.Count; i++)
			{
				int num3 = (int)this.followers[i].Width + 2;
				int place2 = this.followers[i].Place;
				int num4 = (i - 1 < 0) ? 0 : this.followers[i - 1].Place;
				int num5 = num4 + num3;
				this.followers[i].Place = num5;
				for (int j = num4; j <= num5; j++)
				{
					int recordPointIndexFromPlace = this.GetRecordPointIndexFromPlace(j);
					float num6 = (float)(j - num4) / (float)(num5 - num4);
					int place3 = num2 + (int)((float)(place2 - num2) * num6);
					int recordPointIndexFromPlace2 = this.GetRecordPointIndexFromPlace(place3);
					this.recordPoints[recordPointIndexFromPlace] = this.recordPoints[recordPointIndexFromPlace2];
				}
				num2 = place2;
			}
			this.ResizeRecording();
			return follower;
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x00032CD4 File Offset: 0x00030ED4
		public PartyFollower Remove(int index)
		{
			PartyFollower result = null;
			if (index >= 0 && index < this.followers.Count)
			{
				result = this.Remove(this.followers[index]);
			}
			return result;
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x00032D24 File Offset: 0x00030F24
		public IList<PartyFollower> Remove(CharacterType character)
		{
			IList<PartyFollower> list = this.followers.FindAll((PartyFollower x) => x.Character == character);
			foreach (PartyFollower follower in list)
			{
				this.Remove(follower);
			}
			return list;
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x00032D94 File Offset: 0x00030F94
		public void Record(Vector2f position, Vector2f velocity, TerrainType terrain)
		{
			this.pos = (this.pos + 1) % this.recordPoints.Length;
			this.recordPoints[this.pos].position = position;
			this.recordPoints[this.pos].velocity = velocity;
			this.recordPoints[this.pos].terrain = terrain;
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x00032E00 File Offset: 0x00031000
		public void Update()
		{
			for (int i = 0; i < this.followers.Count; i++)
			{
				int place = this.followers[i].Place;
				this.followers[i].Update(this.GetPosition(place), this.GetVelocity(place), this.GetTerrain(place));
			}
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x00032E78 File Offset: 0x00031078
		public ICollection<PartyFollower> GetFollowers(CharacterType character)
		{
			return this.followers.FindAll((PartyFollower x) => x.Character == character);
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x00032EAC File Offset: 0x000310AC
		public Vector2f GetPosition(int place)
		{
			int i;
			for (i = this.pos - place; i < 0; i += this.recordPoints.Length)
			{
			}
			return this.recordPoints[i].position;
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x00032EE4 File Offset: 0x000310E4
		public Vector2f GetVelocity(int place)
		{
			int i;
			for (i = this.pos - place; i < 0; i += this.recordPoints.Length)
			{
			}
			return this.recordPoints[i].velocity;
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x00032F1C File Offset: 0x0003111C
		public TerrainType GetTerrain(int place)
		{
			int i;
			for (i = this.pos - place; i < 0; i += this.recordPoints.Length)
			{
			}
			return this.recordPoints[i].terrain;
		}

		// Token: 0x04000980 RID: 2432
		private const int INITIAL_LENGTH = 1;

		// Token: 0x04000981 RID: 2433
		private PartyTrain.RecordPoint[] recordPoints;

		// Token: 0x04000982 RID: 2434
		private int pos;

		// Token: 0x04000983 RID: 2435
		private bool running;

		// Token: 0x04000984 RID: 2436
		private bool crouching;

		// Token: 0x04000985 RID: 2437
		private bool movementLocked;

		// Token: 0x04000986 RID: 2438
		private List<PartyFollower> followers;

		// Token: 0x02000175 RID: 373
		private struct RecordPoint
		{
			// Token: 0x04000988 RID: 2440
			public Vector2f position;

			// Token: 0x04000989 RID: 2441
			public Vector2f velocity;

			// Token: 0x0400098A RID: 2442
			public TerrainType terrain;
		}

		// Token: 0x02000176 RID: 374
		// (Invoke) Token: 0x060007E8 RID: 2024
		public delegate void OnResetHandler(Vector2f position, int direction);
	}
}
