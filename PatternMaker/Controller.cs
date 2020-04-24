using System;
namespace StockControl.Logic
{
	public enum ShopAndGoOp { CardTapped, ProductScanned, LeaveStore, ResetForNextCustomer, CartBroken }
	public partial class ShopAndGoController : Observable
	{
		private BaseState state;
		private bool[] ops;
		public bool CanDo(ShopAndGoOp op) { return ops[(int)op]; }
		public ShopAndGoController()
		{
			ops = new bool[Enum.GetValues(typeof(ShopAndGoOp)).Length];
			state = new IdleReady(this);
		}
		public string Message { get; set; } = string.Empty;
		public void CardTapped(string p) { state.CardTapped(p); Notify(); }
		public void ProductScanned(string p) { state.ProductScanned(p); Notify(); }
		public void LeaveStore() { state.LeaveStore(); Notify(); }
		public void ResetForNextCustomer() { state.ResetForNextCustomer(); Notify(); }
		public void CartBroken() { state.CartBroken(); Notify(); }
		private abstract class BaseState
		{
			protected ShopAndGoController controller;
			protected void Invalid() { throw new InvalidOperationException(); }
			public BaseState(ShopAndGoController controller)
			{
				this.controller = controller;
				for (int i = 0; i < controller.ops.Length; i++)
				{
					controller.ops[i] = false;
				}
			}
			public virtual void CardTapped(string p) { Invalid(); }
			public virtual void ProductScanned(string p) { Invalid(); }
			public virtual void LeaveStore() { Invalid(); }
			public virtual void ResetForNextCustomer() { Invalid(); }
			public virtual void CartBroken() { Invalid(); }
		}
		class IdleReady : BaseState
		{
			public IdleReady(ShopAndGoController controller) : base(controller)
			{
				controller.ops[(int)ShopAndGoOp.CardTapped] = true;
				controller.ops[(int)ShopAndGoOp.ResetForNextCustomer] = true;
				controller.ops[(int)ShopAndGoOp.CartBroken] = true;
				controller.Message = "To start shopping please tap your S and G card";
			}
			public override void CardTapped(string p)
			{
				controller.CardTapped_IdleReady(p);
			}
			public override void ResetForNextCustomer()
			{
				controller.ResetForNextCustomer_IdleReady();
			}
			public override void CartBroken()
			{
				controller.CartBroken_IdleReady();
			}
		}
		class CartInUseEmpty : BaseState
		{
			public CartInUseEmpty(ShopAndGoController controller) : base(controller)
			{
				controller.ops[(int)ShopAndGoOp.ProductScanned] = true;
				controller.ops[(int)ShopAndGoOp.ResetForNextCustomer] = true;
				controller.ops[(int)ShopAndGoOp.CartBroken] = true;
				controller.Message = "You can now add items to your cart";
			}
			public override void ProductScanned(string p)
			{
				controller.ProductScanned_CartInUseEmpty(p);
			}
			public override void ResetForNextCustomer()
			{
				controller.ResetForNextCustomer_CartInUseEmpty();
			}
			public override void CartBroken()
			{
				controller.CartBroken_CartInUseEmpty();
			}
		}
		class CartInUseHasItems : BaseState
		{
			public CartInUseHasItems(ShopAndGoController controller) : base(controller)
			{
				controller.ops[(int)ShopAndGoOp.LeaveStore] = true;
				controller.ops[(int)ShopAndGoOp.ProductScanned] = true;
				controller.ops[(int)ShopAndGoOp.ResetForNextCustomer] = true;
				controller.ops[(int)ShopAndGoOp.CartBroken] = true;
				controller.Message = "When you finish shopping you are free to leave the store";
			}
			public override void LeaveStore()
			{
				controller.LeaveStore_CartInUseHasItems();
			}
			public override void ProductScanned(string p)
			{
				controller.ProductScanned_CartInUseHasItems(p);
			}
			public override void ResetForNextCustomer()
			{
				controller.ResetForNextCustomer_CartInUseHasItems();
			}
			public override void CartBroken()
			{
				controller.CartBroken_CartInUseHasItems();
			}
		}
		class CartOutside : BaseState
		{
			public CartOutside(ShopAndGoController controller) : base(controller)
			{
				controller.ops[(int)ShopAndGoOp.ResetForNextCustomer] = true;
				controller.ops[(int)ShopAndGoOp.CartBroken] = true;
				controller.Message = "Please leave the cart in the holding area.";
			}
			public override void ResetForNextCustomer()
			{
				controller.ResetForNextCustomer_CartOutside();
			}
			public override void CartBroken()
			{
				controller.CartBroken_CartOutside();
			}
		}
		class OutOfService : BaseState
		{
			public OutOfService(ShopAndGoController controller) : base(controller)
			{
				controller.ops[(int)ShopAndGoOp.ResetForNextCustomer] = true;
				controller.ops[(int)ShopAndGoOp.CartBroken] = true;
				controller.Message = "Please select another cart this one is not working";
			}
			public override void ResetForNextCustomer()
			{
				controller.ResetForNextCustomer_OutOfService();
			}
			public override void CartBroken()
			{
				controller.CartBroken_OutOfService();
			}
		}
		partial void CardTapped_IdleReady(string p);
		partial void CartBroken_CartInUseEmpty();
		partial void CartBroken_CartInUseHasItems();
		partial void CartBroken_CartOutside();
		partial void CartBroken_IdleReady();
		partial void CartBroken_OutOfService();
		partial void LeaveStore_CartInUseHasItems();
		partial void ProductScanned_CartInUseEmpty(string p);
		partial void ProductScanned_CartInUseHasItems(string p);
		partial void ResetForNextCustomer_CartInUseEmpty();
		partial void ResetForNextCustomer_CartInUseHasItems();
		partial void ResetForNextCustomer_CartOutside();
		partial void ResetForNextCustomer_IdleReady();
		partial void ResetForNextCustomer_OutOfService();
	}
	public abstract class Observable
	{
		public event EventHandler StateChanged;
		protected void Notify()
		{
			if (StateChanged != null) StateChanged(this, EventArgs.Empty);
		}
	}
}
