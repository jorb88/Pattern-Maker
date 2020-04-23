using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace PatternMaker
{
	public partial class GenerateCode : System.Web.UI.Page
	{
		private ControllerName controller;
		private Repository repository;
		protected void Page_Load(object sender, EventArgs e)
		{
			controller = (ControllerName)Session["controller"];
			if (controller == null) Response.Redirect("Default.aspx");
			repository = Session["repository"] as Repository;
			if (!IsPostBack) ShowCode();
		}
		private void WriteLine(string v)
		{
			txtCode.Text += v + Environment.NewLine;
		}
		private void ShowCode()
		{
			string ctrlName = controller.Name;
			bool includeAvailabilityProfile = controller.IncludeAvailabilityProfile;
			bool includeStateTextMessages = controller.IncludeMessageText;
			var distinctMethods = (from t in controller.StateTransitions
								   orderby t.Id
								   select t.MethodName).Distinct().ToList();
			var distinctParameters = (from t in controller.StateTransitions
									  orderby t.Id
									  select t.MethodParameter).Distinct().ToList();
			var distinctStates = (from s in controller.StateTransitions
								  orderby s.Id
								  select s).Distinct().ToList();
			var distinctStateNames = (from s in controller.StateTransitions
									  orderby s.Id
									  select s.StateName).Distinct().ToList();
			string initialStateName = (from s in controller.StateTransitions
									   orderby s.Id
									   select s).Distinct().First().StateName;
			txtCode.Text = string.Empty;
			List<string> partialMethods = new List<string>();

			WriteLine("using System;");
			WriteLine("namespace " + controller.Namespace.Trim());
			WriteLine("{");

			string enumName = ctrlName + "Op";
			if (ctrlName.EndsWith("Controller") && !ctrlName.StartsWith("Controller"))
				enumName = ctrlName.Replace("Controller", string.Empty) + "Op";
			if (includeAvailabilityProfile)
			{
				string enumList = "  public enum " + enumName + " { ";
				for (int i = 0; i < distinctMethods.Count(); i++)
				{
					if (i != 0) enumList += ", ";
					enumList += distinctMethods[i];
				}
				WriteLine(enumList + "  }");
			}
			WriteLine("  public partial class " + ctrlName + " : Observable");
			WriteLine("  {");
			string encapsulation = "public ";
			if (controller.UseInnerClasses) encapsulation = "private ";
			WriteLine("    " + encapsulation + "BaseState state;");
			if (includeAvailabilityProfile)
			{
				WriteLine("    " + encapsulation + "bool[] ops;");
				WriteLine("    public bool CanDo(" + enumName + " op) { return ops[(int)op]; }");
			}
			WriteLine("    public " + ctrlName + "()");
			WriteLine("    {");
			if (includeAvailabilityProfile)
			{
				WriteLine("        ops = new bool[Enum.GetValues(typeof(" + enumName + ")).Length];");
			}
			WriteLine("        state = new " + initialStateName + "(this);");
			WriteLine("    }");
			if (includeStateTextMessages)
			{
				WriteLine("    public string Message {get; set; } ");
			}
			foreach (string method in distinctMethods)
			{
				string paramType = (from p in controller.StateTransitions
									where p.MethodName.Trim() == method
									select p.MethodParameter).First();
				string paramName = "p";
				if (paramType.Trim().ToLower() == "void")
				{
					paramName = string.Empty;
					paramType = string.Empty;
				}
				WriteLine("    public void " + method + "(" + paramType + " " + paramName + ") { " + "state." + method + "(" + paramName + "); Notify(); }");
			}
			if (!controller.UseInnerClasses) WriteLine("    }");
			WriteLine("    " + encapsulation + "abstract class BaseState");
			WriteLine("    {");
			WriteLine("        protected " + ctrlName + " controller;");
			WriteLine("        protected void Invalid() { throw new InvalidOperationException(); }");
			WriteLine("        public BaseState(" + ctrlName + " controller)");
			WriteLine("        {");
			WriteLine("           this.controller = controller;");
			if (includeAvailabilityProfile)
			{
				WriteLine("           for (int i = 0; i < controller.ops.Length; i++)");
				WriteLine("               { controller.ops[i] = false; }");
			}
			WriteLine("        }");

			foreach (string method in distinctMethods)
			{
				string param = (from p in controller.StateTransitions
								where p.MethodName.Trim() == method
								select p.MethodParameter).First() + " p";
				if (param.Trim().ToLower() == "void p") param = string.Empty;
				WriteLine("        public virtual void " + method + "(" + param + ") { Invalid(); }");
			}
			WriteLine("    }");

			foreach (string state in distinctStateNames)
			{
				var methodsInThisState = from t in controller.StateTransitions
										 where t.StateName == state
										 select t.MethodName;
				WriteLine("    class " + state + " : BaseState");
				WriteLine("    {");
				WriteLine("        public " + state + "(" + ctrlName + " controller) : base(controller)");
				WriteLine("        {");
				if (includeAvailabilityProfile)
				{
					foreach (string method in methodsInThisState)
					{
						WriteLine("            controller.ops[(int)" + enumName + "." + method + "] = true;");
					}
				}
				if (includeStateTextMessages)
				{
					string message = (from m in controller.StateTransitions
									  where m.StateName == state
									  select m.Description).First();
					WriteLine("            controller.Message = " + "\"" + message + "\";");
				}
				WriteLine("        }");
				foreach (string method in methodsInThisState)
				{
					string param = (from p in controller.StateTransitions
									where p.MethodName.Trim() == method
									select p.MethodParameter).First() + " p";
					if (param.Trim().ToLower() == "void p") param = string.Empty;
					WriteLine("        public override void " + method + "(" + param + ")");
					WriteLine("        {");
					var nextState = (from t in controller.StateTransitions
									 where t.StateName == state && t.MethodName == method
									 select t.NextState).FirstOrDefault();
					nextState = nextState.Trim();
					string partMeth = method + "_" + state + "(" + param + ");";
					partialMethods.Add(partMeth);
					if (param.Length > 0) WriteLine("             controller." + method + "_" + state + "(p);");
					else WriteLine("             controller." + method + "_" + state + "();");
					WriteLine("        }");
				}
				WriteLine("    }");
			}
			foreach (string pm in partialMethods)
			{
				WriteLine("    partial void " + pm);
			}
			if (controller.UseInnerClasses) WriteLine("  }");
			WriteLine("  public abstract class Observable");
			WriteLine("  {");
			WriteLine("      public event EventHandler StateChanged;");
			WriteLine("      protected void Notify()");
			WriteLine("      {");
			WriteLine("          if (StateChanged != null) StateChanged(this,EventArgs.Empty);");
			WriteLine("      }");
			WriteLine("  }");
			WriteLine("}");
		}
	}
}