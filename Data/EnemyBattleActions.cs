using System;
using System.Collections.Generic;
using Mother4.Battle.Actions;
using Mother4.Data.Enemies;

namespace Mother4.Data
{
	// Token: 0x02000081 RID: 129
	internal static class EnemyBattleActions
	{
		// Token: 0x060002AB RID: 683 RVA: 0x00010C10 File Offset: 0x0000EE10
		public static List<ActionParams> GetBattleActionParams(EnemyType enemy)
		{
			List<ActionParams> param = new List<ActionParams>();

				param.Add(new ActionParams
				{
					actionType = typeof(EnemyBashAction),
					data = new object[]
						{
							2f
						}
				});
				param.Add(new ActionParams
				{
					actionType = typeof(EnemyProjectileAction),
					data = new object[]
						{
							"a comet",
							850
						}
				});
				
			
			return param;

		}

		// Token: 0x0400041C RID: 1052
		private static Dictionary<EnemyType, List<ActionParams>> battleActionTypes = new Dictionary<EnemyType, List<ActionParams>>
		{
			{
				EnemyType.Dummy,
								new List<ActionParams>
				{
					new ActionParams
					{
						actionType = typeof(EnemyBashAction),
						data = new object[]
						{
							2f
						}
					}
				}
			},
			{
				EnemyType.MagicSnail,
				new List<ActionParams>
				{
					new ActionParams
					{
						actionType = typeof(EnemyBashAction),
						data = new object[]
						{
							2f
						}
					}
				}
			},
			{
				EnemyType.Stickat,
				new List<ActionParams>
				{
					new ActionParams
					{
						actionType = typeof(EnemyTurnWasteAction),
						data = new object[]
						{
							"The Stickat started sweating generously!",
							false
						}
					}
				}
			},
			{
				EnemyType.Rat,
				new List<ActionParams>
				{
					new ActionParams
					{
						actionType = typeof(EnemyBashAction),
						data = new object[]
						{
							2f,
							"{0}{1} bit {2}!"
						}
					}
				}
			},
			{
				EnemyType.HermitCan,
				new List<ActionParams>
				{
					new ActionParams
					{
						actionType = typeof(EnemyTurnWasteAction),
						data = new object[]
						{
							"The Hermit Can is just sort of hanging out, I guess.",
							false
						}
					}
				}
			},
			{
				EnemyType.Flamingo,
				new List<ActionParams>
				{
					new ActionParams
					{
						actionType = typeof(EnemyTurnWasteAction),
						data = new object[]
						{
							"Mr. Flamingo is smirking.",
							false
						}
					}
				}
			},
			{
				EnemyType.AtomicPowerRobo,
				new List<ActionParams>
				{
					new ActionParams
					{
						actionType = typeof(EnemyTurnWasteAction),
						data = new object[]
						{
							"The Atomic Power Robo is emitting a slight hum.",
							false
						}
					}
				}
			},
			{
				EnemyType.CarbonPup,
				new List<ActionParams>
				{
					new ActionParams
					{
						actionType = typeof(EnemyTurnWasteAction),
						data = new object[]
						{
							"The Carbon Pup is wagging its tail nervously.",
							false
						}
					}
				}
			},
			{
				EnemyType.MeltyRobot,
				new List<ActionParams>
				{
					new ActionParams
					{
						actionType = typeof(EnemyTurnWasteAction),
						data = new object[]
						{
							"The Melty Robot is slowly melting.",
							false
						}
					}
				}
			},
			{
				EnemyType.ModernMind,
				new List<ActionParams>
				{

				}
			},
			{
				EnemyType.NotSoDeer,
				new List<ActionParams>
				{
					new ActionParams
					{
						actionType = typeof(EnemyTurnWasteAction),
						data = new object[]
						{
							"The Not-So-Deer is staring blankly.",
							false
						}
					}
				}
			},
			{
				EnemyType.PunkAssassin,
				new List<ActionParams>
				{
					new ActionParams
					{
						actionType = typeof(EnemyBashAction),
						data = new object[]
						{
							2f,
							"{0}{1} brandished a knife!"
						}
					}
				}
			},
			{
				EnemyType.PunkEnforcer,
				new List<ActionParams>
				{
					new ActionParams
					{
						actionType = typeof(EnemyBashAction),
						data = new object[]
						{
							2f,
							"{0}{1} swung at {2}'s head!"
						}
					}
				}
			},
			{
				EnemyType.RatDispenser,
				new List<ActionParams>
				{
					new ActionParams
					{
						actionType = typeof(EnemyTurnWasteAction),
						data = new object[]
						{
							"The Rat Dispenser gave a gentle grin."
						}
					}
				}
			}
		};
	}
}
