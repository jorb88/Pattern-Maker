using System;

using System.Web.UI;
using System.Web.UI.WebControls;

namespace PatternMaker
{
	public partial class _Default : Page
	{
		private Repository repository;
		private ControllerUser user;
		protected void Page_Load(object sender, EventArgs e)
		{
			user = (ControllerUser)Session["user"];
			if (user == null) Response.Redirect("Login.aspx");
			repository = Session["repository"] as Repository;
			if (!IsPostBack) LoadDropDownList();
		}
		private void LoadDropDownList()
		{
			ddlSelectContoller.Items.Clear();
			foreach (ControllerName controller in user.ControllerNames)
			{
				ListItem li = new ListItem(controller.Name, controller.Id.ToString());
				ddlSelectContoller.Items.Add(li);
			}
		}
		protected void btnSelect_Click(object sender, EventArgs e)
		{
			int id = int.Parse(ddlSelectContoller.SelectedValue);
			ControllerName controller = repository.FindAUsersControllerByID(user, id);
			Session["controller"] = controller;
			Response.Redirect("EditStateTransitions.aspx");
		}
		protected void txtDelete_Click(object sender, EventArgs e)
		{
			lblAreYouSure.Visible = true;
			btnYes.Visible = true;
			btnNo.Visible = true;
		}
		protected void btnCreateNew_Click(object sender, EventArgs e)
		{
			ControllerName controller = new ControllerName()
			{
				Name = "[EDIT]",
				Namespace = "[EDIT]",
				IncludeAvailabilityProfile = true,
				IncludeMessageText = true,
				UseInnerClasses = true
			};
			StateTransition st = new StateTransition()
			{
				StateName = "[EDIT]",
				Description = "[EDIT]",
				MethodName = "[EDIT]",
				MethodParameter = "void",
				NextState = "[EDIT]"
			};
			controller.StateTransitions.Add(st);
			user.ControllerNames.Add(controller);
			repository.SaveChanges();
			Session["controller"] = controller;
			Response.Redirect("EditStateTransitions.aspx");
		}
		protected void btnYes_Click(object sender, EventArgs e)
		{
			int id = int.Parse(ddlSelectContoller.SelectedValue);
			ControllerName controller = repository.FindAUsersControllerByID(user, id);
			repository.Remove(controller);
			repository.SaveChanges();
			Session.Remove("Controller");
			lblAreYouSure.Text = "Controller State Table Deleted";
			LoadDropDownList();
			btnYes.Visible = false;
			btnNo.Visible = false;
		}
		protected void btnNo_Click(object sender, EventArgs e)
		{
			lblAreYouSure.Visible = false;
			btnYes.Visible = false;
			btnNo.Visible = false;
		}
	}
}