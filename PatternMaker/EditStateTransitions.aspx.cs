using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace PatternMaker
{
	public partial class EditStateTransitions : System.Web.UI.Page
	{
		private ControllerName controller;
		private Repository repository;
		protected void Page_Load(object sender, EventArgs e)
		{
			controller = Session["controller"] as ControllerName;
			if (controller == null) Response.Redirect("Default.aspx");
			repository = Session["repository"] as Repository;
			if (!IsPostBack)
			{
				txtControlClassName.Text = controller.Name;
				txtNamespace.Text = controller.Namespace;
				cbIncludeMethodAvailability.Checked = controller.IncludeAvailabilityProfile;
				cbIncludeMessages.Checked = controller.IncludeMessageText;
				SetHintText();
				lblMessage.Text = string.Empty;
			}
		}
		private void SetHintText()
		{
			lblHints.Text = "<br />Helpful Hints...";
			lblHints.Text += "<br />* All columns must have values in them.";
			lblHints.Text += "<br />* If the method has no parameters enter void.";
			lblHints.Text += "<br />* Watch the characters and their case - this is C#";
			lblHints.Text += "<br />* The top row will be used as the initial controller state.";
			lblHints.Text += "<br />* If there is no next state enter the same state.";
			lblHints.Text += "<br />* Repeat user instruction messages if there is more than one operation in the same state.";
		}
		protected void EntityDataSource1_QueryCreated(object sender, QueryCreatedEventArgs e)
		{
			var transitions = e.Query.Cast<StateTransition>();
			e.Query = from st in transitions
					  where st.ControllerID == controller.Id
					  orderby st.Id
					  select st;
		}
		protected void btnSave_Click(object sender, EventArgs e)
		{
			lblMessage.Text = string.Empty;
			if (txtNamespace.Text.Trim().Length <= 0)
			{
				lblMessage.Text = "Namespace is required";
			}
			else if (txtControlClassName.Text.Trim().Length <= 0)
			{
				lblMessage.Text = "Control class name is required";
			}
			else if (!txtControlClassName.Text.Trim().EndsWith("Controller"))
			{
				lblMessage.Text = "Control class name must end with 'Controller'";
			}
			else if (txtControlClassName.Text.Trim().StartsWith("Controller"))
			{
				lblMessage.Text = "Control class name cannot start with or be called 'Controller'";
			}
			else
			{
				controller.Name = txtControlClassName.Text;
				controller.Namespace = txtNamespace.Text;
				controller.IncludeMessageText = cbIncludeMessages.Checked;
				controller.IncludeAvailabilityProfile = cbIncludeMethodAvailability.Checked;
				repository.SaveChanges();
				lblMessage.Text = "Data Saved";
			}
		}
		protected void btnAddNewRow_Click1(object sender, EventArgs e)
		{
			StateTransition st = new StateTransition()
			{
				Description = "",
				MethodName = "",
				MethodParameter = "void",
				NextState = "",
				StateName = "",
				ControllerID = controller.Id
			};
			controller.StateTransitions.Add(st);
			repository.SaveChanges();
			GridView1.DataBind();
		}
		protected void btnGenerate_Click(object sender, EventArgs e)
		{
			LoadControllerFromGrid();
			Response.Redirect("GenerateCode.aspx");
		}
		protected void btnGenerateTestCode_Click(object sender, EventArgs e)
		{
			LoadControllerFromGrid();
			Response.Redirect("GenerateTests.aspx");
		}
		private void LoadControllerFromGrid()
		{
			for (int i = 0; i < GridView1.Rows.Count; i++)
			{
				int stid = int.Parse(GridView1.Rows[i].Cells[1].Text);
				StateTransition st = repository.FindStateTransitionByID(controller, stid);
				int col = 2;
				st.StateName = GridView1.Rows[i].Cells[col + 0].Text;
				st.Description = GridView1.Rows[i].Cells[col + 1].Text;
				st.MethodName = GridView1.Rows[i].Cells[col + 2].Text;
				st.MethodParameter = GridView1.Rows[i].Cells[col + 3].Text;
				st.NextState = GridView1.Rows[i].Cells[col + 4].Text;
			}
			repository.SaveChanges();
			Session["controller"] = controller;
		}
	}
}