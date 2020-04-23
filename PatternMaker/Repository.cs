using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatternMaker
{
	public class Repository
	{
		private PatternEntities db;
		#region Instance Management
		public Repository() { db = new PatternEntities(); }
		//public static Repository Instance
		//{
		//	get
		//	{
		//		Repository repository;
		//		if (HttpContext.Current.Session["repositoryInstance"] != null)
		//			repository = HttpContext.Current.Session["repositoryInstance"] as Repository;
		//		else
		//			HttpContext.Current.Session["repositoryInstance"] = repository = new Repository();
		//		return repository;
		//	}
		//}
		~Repository() { db.Dispose(); }
		#endregion
		#region Service methods
		public void SaveChanges() { db.SaveChanges(); }
		public void RollBack()
		{
			db.Dispose();
			db = new PatternEntities();
		}
		public void Reload(ControllerName controller)
		{
			db.Entry(controller).Reload();
		}
		#endregion
		#region Controller User Support
		public ControllerUser LookupUserByEmailAddress(string email)
		{
			email = email.Trim().ToLower();
			foreach (ControllerUser u in db.ControllerUsers)
			{
				if (u.EmailAddress.Trim().ToLower() == email)
					return u;
			}
			return null;
		}
		public void AddControllerUser(ControllerUser user)
		{
			db.ControllerUsers.Add(user);
		}
		#endregion
		#region Contoller Model Name Support
		public ControllerName LookupControllerById(int id)
		{
			foreach (ControllerName cn in db.ControllerNames)
			{
				if (cn.Id == id) return cn;
			}
			return null;
		}
		public StateTransition FindStateTransitionByID(ControllerName controller, int id)
		{
			foreach (StateTransition st in controller.StateTransitions)
			{
				if (st.Id == id) return st;
			}
			return null;
		}
		public ControllerName FindAUsersControllerByID(ControllerUser user, int id)
		{
			foreach (ControllerName controller in user.ControllerNames)
			{
				if (controller.Id == id) return controller;
			}
			return null;
		}
		public ControllerName LookupHighestControllerByUserEmail(string email)
		{
			email = email.Trim().ToLower();
			List<ControllerName> cList = new List<ControllerName>();
			foreach (ControllerName cn in db.ControllerNames)
			{
				if (cn.UserEmail.Trim().ToLower() == email)
					cList.Add(cn);
			}
			cList.Sort((cn1, cn2) => cn2.Id - cn1.Id);
			return cList[0];
		}
		public void Remove(ControllerName controller)
		{
			List<StateTransition> cns = new List<StateTransition>();
			foreach (StateTransition st in controller.StateTransitions)
			{
				cns.Add(st);
			}
			foreach (StateTransition st in cns)
			{
				db.StateTransitions.Remove(st);
			}
			db.ControllerNames.Remove(controller);
		}
		public void Insert(ControllerName cn)
		{
			db.ControllerNames.Add(cn);
		}
		#endregion
	}
}