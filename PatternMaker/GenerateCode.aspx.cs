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
				string enumList = "\tpublic enum " + enumName + " { ";
				for (int i = 0; i < distinctMethods.Count(); i++)
				{
					if (i != 0) enumList += ", ";
					enumList += distinctMethods[i];
				}
				WriteLine(enumList + "  }");
			}
			WriteLine("\tpublic partial class " + ctrlName + " : Observable");
			WriteLine("\t{");
			WriteLine("\t\tprivate BaseState state;");
			if (includeAvailabilityProfile)
			{
				WriteLine("\t\tprivate bool[] ops;");
				WriteLine("\t\tpublic bool CanDo(" + enumName + " op) { return ops[(int)op]; }");
			}
			WriteLine("\t\tpublic " + ctrlName + "()");
			WriteLine("\t\t{");
			if (includeAvailabilityProfile)
			{
				WriteLine("\t\t\tops = new bool[Enum.GetValues(typeof(" + enumName + ")).Length];");
			}
			WriteLine("\t\t\tstate = new " + initialStateName + "(this);");
			WriteLine("\t\t}");
			if (includeStateTextMessages)
			{
				WriteLine("\t\tpublic string Message {get; set; } = string.Empty;");
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
				WriteLine("\t\tpublic void " + method + "(" + paramType + " " + paramName + ") { " + "state." + method + "(" + paramName + "); Notify(); }");
			}
			WriteLine("\t\t" + "private abstract class BaseState");
			WriteLine("\t\t{");
			WriteLine("\t\t\tprotected " + ctrlName + " controller;");
			WriteLine("\t\t\tprotected void Invalid() { throw new InvalidOperationException(); }");
			WriteLine("\t\t\tpublic BaseState(" + ctrlName + " controller)");
			WriteLine("\t\t\t{");
			WriteLine("\t\t\t\tthis.controller = controller;");
			if (includeAvailabilityProfile)
			{
				WriteLine("\t\t\t\tfor (int i = 0; i < controller.ops.Length; i++)");
				WriteLine("\t\t\t\t{");
				WriteLine("\t\t\t\t\tcontroller.ops[i] = false;");
				WriteLine("\t\t\t\t}");
			}
			WriteLine("\t\t\t}");

			foreach (string method in distinctMethods)
			{
				string param = (from p in controller.StateTransitions
								where p.MethodName.Trim() == method
								select p.MethodParameter).First() + " p";
				if (param.Trim().ToLower() == "void p") param = string.Empty;
				WriteLine("\t\t\tpublic virtual void " + method + "(" + param + ") { Invalid(); }");
			}
			WriteLine("\t\t}");

			foreach (string state in distinctStateNames)
			{
				var methodsInThisState = from t in controller.StateTransitions
										 where t.StateName == state
										 select t.MethodName;
				WriteLine("\t\tclass " + state + " : BaseState");
				WriteLine("\t\t{");
				WriteLine("\t\t\tpublic " + state + "(" + ctrlName + " controller) : base(controller)");
				WriteLine("\t\t\t{");
				if (includeAvailabilityProfile)
				{
					foreach (string method in methodsInThisState)
					{
						WriteLine("\t\t\t\tcontroller.ops[(int)" + enumName + "." + method + "] = true;");
					}
				}
				if (includeStateTextMessages)
				{
					string message = (from m in controller.StateTransitions
									  where m.StateName == state
									  select m.Description).First();
					WriteLine("\t\t\t\tcontroller.Message = " + "\"" + message + "\";");
				}
				WriteLine("\t\t\t}");
				foreach (string method in methodsInThisState)
				{
					string param = (from p in controller.StateTransitions
									where p.MethodName.Trim() == method
									select p.MethodParameter).First() + " p";
					if (param.Trim().ToLower() == "void p") param = string.Empty;
					WriteLine("\t\t\tpublic override void " + method + "(" + param + ")");
					WriteLine("\t\t\t{");
					var nextState = (from t in controller.StateTransitions
									 where t.StateName == state && t.MethodName == method
									 select t.NextState).FirstOrDefault();
					nextState = nextState.Trim();
					string partMeth = method + "_" + state + "(" + param + ");";
					partialMethods.Add(partMeth);
					if (param.Length > 0) WriteLine("\t\t\t\tcontroller." + method + "_" + state + "(p);");
					else WriteLine("\t\t\t\tcontroller." + method + "_" + state + "();");
					WriteLine("\t\t\t}");
				}
				WriteLine("\t\t}");
			}
			partialMethods.Sort();
			foreach (string pm in partialMethods)
			{
				WriteLine("\t\tpartial void " + pm);
			}
			WriteLine("\t}");
			WriteLine("\tpublic abstract class Observable");
			WriteLine("\t{");
			WriteLine("\t\tpublic event EventHandler StateChanged;");
			WriteLine("\t\tprotected void Notify()");
			WriteLine("\t\t{");
			WriteLine("\t\t\tif (StateChanged != null) StateChanged(this,EventArgs.Empty);");
			WriteLine("\t\t}");
			WriteLine("\t}");
			WriteLine("}");

			WriteLine("//==========================================================================");
			WriteLine("//Copy this code into a separate file named " + ctrlName + "Methods.cs" + Environment.NewLine +
				"//Complete these partial methods as needed. You should not need to edit within" + Environment.NewLine +
				"//the state pattern logic directly.");
			WriteLine("//==========================================================================" + Environment.NewLine);

			WriteLine("//using System;");
			WriteLine("//namespace " + controller.Namespace.Trim());
			WriteLine("//{");
			WriteLine("//\tpublic partial class " + ctrlName);
			WriteLine("//\t{");
			foreach (string pmeth in partialMethods)
			{
				string pstr = pmeth.Trim();
				if (pstr.EndsWith(";")) pstr = pstr.Substring(0, pmeth.Length - 1);
				WriteLine("//\t\tpartial void " + pstr);
				WriteLine("//\t\t{");
				WriteLine("//\t\t}");
			}
			WriteLine("//\t}");
			WriteLine("//}");
		}
	}
}