using System;
using System.Collections;
using System.Runtime.CompilerServices;

using Ext;
using Ext.data;

using LxnBase;
using LxnBase.Data;
using LxnBase.UI;
using LxnBase.UI.AutoControls;

using Action = Ext.Action;
using Record = Ext.data.Record;


namespace Luxena.Travel
{

	public class SemanticEntity
	{
		public IEditForm EditForm;
		public IGridForm GridForm;


		public SemanticDomain _domain;

		public string _className;
		public string _name;
		public string _title;
		public string _titles;

		public bool _isAbstract;
		public Func<SemanticDomain, SemanticEntity[]> _getDerivedEntities;

		public string _idFieldName = "Id";
		public string _nameFieldName = "Name";
		public SemanticMember _nameMember;

		public Action<Dictionary> _prepareNewData;


		protected static SemanticMember Member { get { return new SemanticMember(); } }


		public virtual void Initialize()
		{
			// ReSharper disable once SuspiciousTypeConversion.Global
			if (Script.IsValue(_nameFieldName))
				_nameMember = (SemanticMember)((Dictionary)(object)this)[_nameFieldName];
		}



		public string getName(object data)
		{
			return Script.IsValue(_nameMember) ? _nameMember.GetString(data) : _title ?? _className;
		}

		public Reference getReference(object data, string className)
		{
			return Reference.Create(
				className ?? _className,
				(string)((Dictionary)data)[_nameFieldName],
				((Dictionary)data)[_idFieldName]
			);
		}



		protected ToEditorFormMemberAction ToGridEditor(SemanticMember semantic, IGridControl grid)
		{
			return delegate
			{
				FormMember member = new FormMember(EditForm, semantic, null);
				member.OnLoadValue += delegate
				{
					Dictionary data = EditForm.Data;
					if (Script.IsValue(data))
						grid.LoadData((object[])data[semantic._name]);
				};

				member.OnSaveValue += delegate
				{
					EditForm.SetValue(semantic._name, grid.GetData());
				};

				member.OnIsModified += delegate { return grid.IsModified(); };

				EditForm.Members.Add(member);

				return grid.Widget;
			};
		}

		public SemanticMember GridMember(string name, IGridControl grid)
		{
			SemanticMember sm = new SemanticMember();
			sm._name = name;

			sm.ToEditor = ToGridEditor(sm, grid);

			return sm;
		}



		protected static void BoldLabel(FormMember m)
		{
			m.BoldLabel();
		}
		protected static void HideLabel(FormMember m)
		{
			m.HideLabel();
		}


		public Action ToListAction()
		{
			OperationPermissions permissions = (OperationPermissions)AppManager.AllowedActions[_className];
			OperationStatus status = permissions != null ? permissions.CanList : null;

			if (status == null || (!status.Visible && string.IsNullOrEmpty(status.DisableInfo)))
				return null;

			Action action = new Action(new ActionConfig()
				.text(_titles ?? _title ?? _className)
				.handler(new AnonymousDelegate(delegate { FormsRegistry.ListObjects(_className); }))
				.ToDictionary()
			);

			if (Script.IsValue(status.DisableInfo))
				action.disable();

			return action;
		}

		public Action ToViewAction(object id)
		{
			OperationPermissions permissions = (OperationPermissions)AppManager.AllowedActions[_className];
			OperationStatus status = permissions != null ? permissions.CanList : null;

			if (status == null || (!status.Visible && string.IsNullOrEmpty(status.DisableInfo)))
				return null;

			Action action = new Action(new ActionConfig()
				.text(_title ?? _titles ?? _className)
				.handler(new AnonymousDelegate(delegate { FormsRegistry.ViewObject(_className, id); }))
				.ToDictionary()
			);

			if (Script.IsValue(status.DisableInfo))
				action.disable();

			return action;
		}

		[AlternateSignature]
		public static extern BoxComponent TextComponent_(string html);

		public static BoxComponent TextComponent_(string html, int width)
		{
			BoxComponentConfig config = new BoxComponentConfig()
				.autoEl(new Dictionary("tag", "div", "html", html))
				.cls("x-form-item float-left box-label");

			if (Script.IsValue(width))
			{
				if (width == -4)
				{
					width = 122;
					config.style("text-align: right;");
				}
				config.width(width);
			}

			return new BoxComponent(config.ToDictionary());
		}

		[AlternateSignature]
		public extern BoxComponent TextComponent(string html);

		public BoxComponent TextComponent(string html, int width)
		{
			return TextComponent_(html, width);
		}

		public static Panel RowPanel_(object[] items)
		{
			for (int i = 0; i < items.Length; i++)
			{
				SemanticMember semantic = items[i] as SemanticMember;
				if (semantic != null)
					items[i] = semantic.ToEditor();
			}

			return new Panel(new PanelConfig()
				.layout("form")
				.itemCls("float-left")
				.items(items)
				.ToDictionary()
			);
		}

		public Panel RowPanel(object[] items)
		{
			return RowPanel_(items);
		}


		public static Panel RowPanel2_(SemanticMember view1, InitFormMemberAction initMember1, SemanticMember view2, InitFormMemberAction initMember2)
		{
			return RowPanel_(new Component[]
			{
				view1.ToField(-1, delegate(FormMember m)
				{
					m.Label(string.Format("{0} / {1}", view1._title, view2._title));
					if (Script.IsValue(initMember1))
						initMember1(m);
				}),

				TextComponent_("/"),

				view2.ToField(-1, delegate(FormMember m)
				{
					m.HideLabel();
					if (Script.IsValue(initMember2))
						initMember2(m);
				})
			});
		}

		public Panel RowPanel2(SemanticMember view1, InitFormMemberAction initMember1, SemanticMember view2, InitFormMemberAction initMember2)
		{
			return RowPanel2_(view1, initMember1, view2, initMember2);
		}


		public static Panel RowPanel2c_(string title, SemanticMember view1, InitFormMemberAction initMember1, SemanticMember view2, InitFormMemberAction initMember2)
		{
			return RowPanel_(new Component[]
			{
				view1.ToField(-1, delegate(FormMember m)
				{
					m.Label(title);
					m.EmptyText(view1._title);
					if (Script.IsValue(initMember1))
						initMember1(m);
				}),

				TextComponent_(","),

				view2.ToField(-1, delegate(FormMember m)
				{
					m.HideLabel();
					m.EmptyText(view2._title);
					if (Script.IsValue(initMember2))
						initMember2(m);
				})
			});
		}

		public Panel RowPanel2c(string title, SemanticMember view1, InitFormMemberAction initMember1, SemanticMember view2, InitFormMemberAction initMember2)
		{
			return RowPanel2c_(title, view1, initMember1, view2, initMember2);
		}


		public static Panel RowPanel2v_(SemanticMember view1, InitFormMemberAction initMember1, SemanticMember view2, InitFormMemberAction initMember2)
		{
			return RowPanel_(new object[]
			{
				view1.ToField(-2, initMember1),
				TextComponent_(view2._title + ": ", -4),
				view2.ToField(-2, delegate(FormMember m)
				{
					m.HideLabel();
					if (Script.IsValue(initMember2))
						initMember2(m);
				}),
			});
		}

		public Panel RowPanel2v(SemanticMember view1, InitFormMemberAction initMember1, SemanticMember view2, InitFormMemberAction initMember2)
		{
			return RowPanel2v_(view1, initMember1, view2, initMember2);
		}


		public static Panel RowPanel3c_(
			string title,
			SemanticMember view1, InitFormMemberAction initMember1,
			SemanticMember view2, InitFormMemberAction initMember2,
			SemanticMember view3, InitFormMemberAction initMember3
		)
		{
			return RowPanel_(new Component[]
			{
				view1.ToField(-2, delegate(FormMember m)
				{
					m.Label(title);
					if (Script.IsValue(initMember1))
						initMember1(m);
				}),

				TextComponent_("/"),

				view2.ToField(-2, delegate(FormMember m)
				{
					m.HideLabel();
					if (Script.IsValue(initMember2))
						initMember2(m);
				}),

				TextComponent_("/"),

				view3.ToField(-2, delegate(FormMember m)
				{
					m.HideLabel();
					if (Script.IsValue(initMember3))
						initMember3(m);
				})
			});
		}

		public Panel RowPanel3c(
			string title,
			SemanticMember view1, InitFormMemberAction initMember1,
			SemanticMember view2, InitFormMemberAction initMember2,
			SemanticMember view3, InitFormMemberAction initMember3
		)
		{
			return RowPanel3c_(title, view1, initMember1, view2, initMember2, view3, initMember3);
		}


		public static Panel RowPanel3_(
			SemanticMember view1, InitFormMemberAction initMember1,
			SemanticMember view2, InitFormMemberAction initMember2,
			SemanticMember view3, InitFormMemberAction initMember3
		)
		{
			return RowPanel3c_(
				string.Format("{0} / {1} / {2}", view1._title, view2._title, view3._title),
				view1, initMember1,
				view2, initMember2,
				view3, initMember3
			);
		}

		public Panel RowPanel3(
			SemanticMember view1, InitFormMemberAction initMember1,
			SemanticMember view2, InitFormMemberAction initMember2,
			SemanticMember view3, InitFormMemberAction initMember3
		)
		{
			return RowPanel3_(view1, initMember1, view2, initMember2, view3, initMember3);
		}

		[AlternateSignature]
		public extern GridRenderDelegate GetNameRenderer();

		public GridRenderDelegate GetNameRenderer(string className)
		{
			if (!Script.IsValue(className))
				className = _className;

			return delegate(object value, object metadata, Record record, int index, int colIndex, Store store)
			{
				return ControlFactory
					.CreateRefrenceRenderer(className)
					.Invoke(value, metadata, record, index, colIndex, store);
			};
		}


		[AlternateSignature]
		public extern GridRenderDelegate GetDateRenderer();

		public virtual GridRenderDelegate GetDateRenderer(string className)
		{
			return delegate(object value, object metadata, Record record, int index, int colIndex, Store store)
			{
				if (!Script.IsValue(value)) return "";

				if (!Script.IsValue(className))
					className = _className;

				return ControlFactory
					.CreateRefrenceRenderer(className)
					.Invoke(((Date)value).Format("d.m.Y"), metadata, record, index, colIndex, store);
			};
		}


	}

}