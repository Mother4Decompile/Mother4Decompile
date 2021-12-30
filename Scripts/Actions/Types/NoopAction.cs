using System;

namespace Mother4.Scripts.Actions.Types
{
	// Token: 0x02000123 RID: 291
	internal class NoopAction : RufiniAction
	{
		// Token: 0x060006EE RID: 1774 RVA: 0x0002C3F8 File Offset: 0x0002A5F8
		public NoopAction()
		{
			this.code = null;
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x0002C407 File Offset: 0x0002A607
		public NoopAction(string code)
		{
			this.code = code;
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0002C418 File Offset: 0x0002A618
		public override ActionReturnContext Execute(ExecutionContext context)
		{
			if (this.code != null)
			{
				ConsoleColor foregroundColor = Console.ForegroundColor;
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Unknown command \"{0}\"", this.code);
				Console.ForegroundColor = foregroundColor;
			}
			else
			{
				Console.WriteLine("Noop");
			}
			return default(ActionReturnContext);
		}

		// Token: 0x0400091C RID: 2332
		private string code;
	}
}
