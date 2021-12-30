using System;
using System.Collections.Generic;

namespace Mother4.Scripts.Actions
{
	// Token: 0x0200011B RID: 283
	internal abstract class RufiniAction
	{
		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x0002B9C5 File Offset: 0x00029BC5
		public List<ActionParam> Params
		{
			get
			{
				return this.paramList;
			}
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0002B9CD File Offset: 0x00029BCD
		public RufiniAction()
		{
			this.paramValues = new Dictionary<string, object>();
		}

		// Token: 0x060006D7 RID: 1751
		public abstract ActionReturnContext Execute(ExecutionContext context);

		// Token: 0x060006D8 RID: 1752 RVA: 0x0002B9FC File Offset: 0x00029BFC
		public void SetValue<T>(string param, T value)
		{
			ActionParam actionParam = this.paramList.Find((ActionParam x) => x.Name == param);
			if (actionParam == null)
			{
				throw new KeyNotFoundException("The key \"" + param + "\" was not found in the parameter list");
			}
			if (!(actionParam.Type == typeof(T)))
			{
				throw new InvalidOperationException(string.Concat(new object[]
				{
					"Tried to set incorrect type for \"",
					param,
					"\", got ",
					typeof(T),
					" but expected ",
					actionParam.Type
				}));
			}
			if (this.paramValues.ContainsKey(param))
			{
				this.paramValues[param] = value;
				return;
			}
			this.paramValues.Add(param, value);
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0002BAF4 File Offset: 0x00029CF4
		public T GetValue<T>(string param)
		{
			T result = default(T);
			if (this.paramValues.ContainsKey(param))
			{
				object obj = this.paramValues[param];
				if (obj is T)
				{
					result = (T)((object)obj);
				}
			}
			return result;
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0002BB34 File Offset: 0x00029D34
		public bool TryGetValue<T>(string param, out T value)
		{
			bool result = false;
			value = default(T);
			if (this.paramValues.ContainsKey(param))
			{
				object obj = this.paramValues[param];
				if (obj is T)
				{
					result = true;
					value = (T)((object)obj);
				}
			}
			return result;
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x0002BB7C File Offset: 0x00029D7C
		public bool HasValue(string param)
		{
			return this.paramValues.ContainsKey(param);
		}

		// Token: 0x040008C8 RID: 2248
		protected List<ActionParam> paramList;

		// Token: 0x040008C9 RID: 2249
		private Dictionary<string, object> paramValues;
	}
}
