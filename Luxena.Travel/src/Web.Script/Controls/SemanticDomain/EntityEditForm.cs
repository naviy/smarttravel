using System;
using System.Collections;

using Ext;

using LxnBase;
using LxnBase.Data;
using LxnBase.UI;
using LxnBase.UI.Controls;

using Luxena.Travel.Services;


namespace Luxena.Travel
{

	public delegate bool WindowBeforeCloseDelegate(Panel panel);


	public abstract class EntityEditForm : BaseEditForm2
	{

		protected override void PreInitialize()
		{
			base.PreInitialize();

			dsm = new SemanticDomain(this);
			Window.cls += " editform";
			Window.width = -3;

			Window.addListener("beforeclose", new WindowBeforeCloseDelegate(OnWindowBeforeClose));
		}

		protected SemanticDomain dsm;
		protected SemanticEntity Entity;

		protected virtual string ClassName { get { return Entity._className; } }


		protected override string GetNameBy(object data)
		{
			return Entity.getName(data);
		}

		protected override void PostInitialize()
		{
			if (!Script.IsValue(Entity)) 
				throw new Exception(GetType().Name + ": not assigned ViewEntity Entity");

			base.PostInitialize();
		}

		public override void Open()
		{
			if (_args.Mode == LoadMode.Local)
				OnLoad(_args.FieldValues);
			else
				DomainService.Get(ClassName, _args.IdToLoad, OnLoad, null);
		}

		protected override void OnSave()
		{
			if (_args.Mode == LoadMode.Local)
				CompleteSave(ItemResponse.Create(GetData()));
			else
				DomainService.Update(ClassName, GetData(), _args.RangeRequest, CompleteSave, FailSave);
		}


		protected override void CompleteSave(object result)
		{
			if (Script.IsValue(result))
			{
				Dictionary r = (Dictionary)((ItemResponse)result).Item;

				r[ObjectPropertyNames.Reference] = GetNameBy(r);
				r[ObjectPropertyNames.ObjectClass] = ClassName;

				//Log.Add("CompleteSave." + ObjectPropertyNames.Reference, r[ObjectPropertyNames.Reference]);
				//Log.Add("CompleteSave.r", r);
			}

			base.CompleteSave(result);
		}


		protected override void ShowSaveMessage(Dictionary r)
		{
			MessageRegister.Info(
				_itemConfig.ListCaption,
				string.Format("{0} {1}",
					_args.IsNew ? BaseRes.Created : BaseRes.Updated,
					ObjectLink.Render(r["Id"], GetNameBy(r), ClassName)
				)
			);
		}


		private bool OnWindowBeforeClose(Panel panel)
		{
			if (IsSaved || IsCanceled)
				return true;

			if (IsModified())
			{
				MessageBoxWrap.Show(new Dictionary(
					"buttons", MessageBox.YESNOCANCEL,
					"title", BaseRes.Confirmation,
					"icon", MessageBox.QUESTION,
					"msg", BaseRes.SaveChenges_Msg,
					"fn", new GenericOneArgDelegate(
						delegate(object btn)
						{
							string button = (string)btn;

							if (button == "yes")
								Save();
							else if (button == "no")
								Cancel();
						})));

				return false;
			}

			return true;
		}


	}


	public class Entity3EditForm : EntityEditForm
	{

		protected override void CreateControls()
		{
			Entity3Semantic se = (Entity3Semantic)Entity;

			Form.add(MainDataPanel(new object[]
			{
				se.Name.ToField(-3),
			}));
		}

	}


	public class Entity3DEditForm : EntityEditForm
	{

		protected override void CreateControls()
		{
			Entity3DSemantic se = (Entity3DSemantic)Entity;

			Form.add(MainDataPanel(new object[]
			{
				se.Name.ToField(-3),
				se.Description.ToField(-3),
			}));
		}

	}

}