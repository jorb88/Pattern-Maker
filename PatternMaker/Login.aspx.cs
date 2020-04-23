using System;

namespace PatternMaker
{
	public partial class Login : System.Web.UI.Page
	{
		private Repository repository;
		protected void Page_Load(object sender, EventArgs e)
		{
			repository = Session["repository"] as Repository;
			if (repository == null)
			{
				repository = new Repository();
				Session["repository"] = repository;
			}
			if (!IsPostBack) lblMessage.Text = string.Empty;
		}
		protected void btnLogin_Click1(object sender, EventArgs e)
		{
			var user = repository.LookupUserByEmailAddress(txtUserName.Text);
			if (user == null)
			{
				lblMessage.Text = "User not found - try again or create new account.";
				return;
			}
			Session["user"] = user;
			Response.Redirect("Default.aspx");
		}
		protected void btnCreateNew_Click(object sender, EventArgs e)
		{
			var user = repository.LookupUserByEmailAddress(txtUserName.Text);
			if (user != null)
			{
				lblMessage.Text = "User already exists - new account not created";
				return;
			}
			user = new ControllerUser()
			{
				EmailAddress = txtUserName.Text.Trim().ToLower(),
				Password = txtPassowrd.Text.Trim().ToLower()
			};
			ControllerName cn = new ControllerName()
			{
				Name = "SampleController",
				Namespace = "Sample",
				IncludeAvailabilityProfile = true,
				IncludeMessageText = false
			};
			user.ControllerNames.Add(cn);
			StateTransition st = new StateTransition()
			{
				StateName = "Start",
				Description = "To finish do operation 1",
				MethodName = "SampleMethod1",
				MethodParameter = "void",
				NextState = "Finish"
			};
			cn.StateTransitions.Add(st);
			st = new StateTransition()
			{
				StateName = "Finish",
				Description = "To go back to the start do operation 2",
				MethodName = "SampleMethod2",
				MethodParameter = "void",
				NextState = "Start"
			};
			cn.StateTransitions.Add(st);
			repository.AddControllerUser(user);
			repository.SaveChanges();
			Session["user"] = user;
			Response.Redirect("Default.aspx");
		}
	}
}