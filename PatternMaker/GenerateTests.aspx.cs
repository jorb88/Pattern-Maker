using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace PatternMaker
{
	public partial class GenerateTests : System.Web.UI.Page
	{
		private ControllerName ctrlr;
		protected void Page_Load(object sender, EventArgs e)
		{
			ctrlr = (ControllerName)Session["controller"];
			if (ctrlr == null) Response.Redirect("Default.aspx");
			if (!IsPostBack) ShowTests();
		}
		private void ShowTests()
		{
			string ctrlName = ctrlr.Name;
			string enumName = ctrlName + "Op";
			if (ctrlName.EndsWith("Controller") && !ctrlName.StartsWith("Controller"))
				enumName = ctrlName.Replace("Controller", string.Empty) + "Op";
			var distinctMethodNames = (from mn in ctrlr.StateTransitions
									   orderby mn.Id select mn.MethodName).Distinct().ToList();
			var distinctStateNames = (from sn in ctrlr.StateTransitions
									  orderby sn.Id select sn.StateName).Distinct().ToList();
			txtCode.Text = string.Empty;
			WriteLine("using System;");
			WriteLine("using Microsoft.VisualStudio.TestTools.UnitTesting;");
			WriteLine("using " + ctrlr.Namespace + ";");
			WriteLine("namespace " + ctrlr.Namespace.Trim() + ".Tests");
			WriteLine("{");
			WriteLine("\t[TestClass]");
			WriteLine("\tpublic class " + ctrlName + "Tests");
			WriteLine("\t{");
			WriteLine("\t\tprivate " + ctrlName + " controller;");
			WriteLine("\t\t[TestInitialize]");
			WriteLine("\t\tpublic void Init() { controller = new " + ctrlName + "(); }");

			foreach (string methodName in distinctMethodNames.OrderBy(mn => mn))
			{
				WriteLine(string.Empty);
				foreach (string stateName in distinctStateNames.OrderBy(dn => dn))
				{
					var state = (from st in ctrlr.StateTransitions
								 where 	((st.StateName == stateName) &&
										(st.MethodName == methodName))
								 select st).FirstOrDefault();
					WriteLine("\t\t[TestMethod]");
					if (state == null) WriteLine("\t\t[ExpectedException(typeof(InvalidOperationException))]");
					WriteLine("\t\tpublic void " + methodName + "_" + stateName + "_Test()");
					WriteLine("\t\t{");
					WriteLine("\t\t\tStepThroughTo_" + stateName + "State();");
					if (state != null) WriteLine("\t\t\tAssert.IsTrue(controller.CanDo(" + enumName + "." + methodName + "));");
					else WriteLine("\t\t\tAssert.IsFalse(controller.CanDo(" + enumName + "." + methodName + "));");
					var type = (from st in ctrlr.StateTransitions
								where (st.MethodName == methodName)
								select st).First().MethodParameter;
					string p = "p";
					if (type == "void") p = string.Empty;
					else
					{
						string argDef = type + " p = null;";
						if (type == "DateTime") argDef = type + " p = DateTime.Now;";
						else if (type == "char") argDef = type + " p = '0'";
						else
						{
							string[] valueTypes = new string[] { "byte", "short", "ushort", "int", "uint", "long", "ulong", "float", "double", "decimal" };
							foreach (string valueType in valueTypes)
							{
								if (type == valueType) { argDef = type + " p = 0;"; }
							}
						}
						WriteLine("\t\t\t" + argDef);
					}
					WriteLine("\t\t\tcontroller." + methodName + "(" + p + ");");
					if (state != null)
					{
						var validMethodsInThisNewState = (from st in ctrlr.StateTransitions
														 where st.StateName == state.NextState
														 select st.MethodName).Distinct().ToList();
						foreach (var vMeth in validMethodsInThisNewState)
						{
							WriteLine("\t\t\tAssert.IsTrue(controller.CanDo(" + enumName + "." + vMeth + "));");
						}
						var methodsNotValidInThisNewState = distinctMethodNames.Except(validMethodsInThisNewState);
						foreach (var vMeth in methodsNotValidInThisNewState)
						{
							WriteLine("\t\t\tAssert.IsFalse(controller.CanDo(" + enumName + "." + vMeth + "));");
						}
						WriteLine("\t\t\tAssert.Inconclusive(\"Test not fully implemented\");");
					}
					WriteLine("\t\t}");
				}
			}
			foreach (string stateName in distinctStateNames.OrderBy(dn => dn))
			{
				WriteLine("\t\tprivate void StepThroughTo_" + stateName + "State()");
				WriteLine("\t\t{");
				WriteLine("\t\t\t// Add steps here to move controller to appropriate state");
				WriteLine("\t\t\t// throw new Exception(\"Not implemented\");");
				WriteLine("\t\t}");
			}
			WriteLine("\t}");
			WriteLine("}");

		}
		private void WriteLine(string v)
		{
			txtCode.Text += v + Environment.NewLine;
		}
	}
}