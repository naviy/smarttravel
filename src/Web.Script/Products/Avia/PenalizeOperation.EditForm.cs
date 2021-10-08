using System;
using System.Collections;

using Ext;
using Ext.data;
using Ext.form;

using LxnBase;
using LxnBase.UI;

using Field = Ext.form.Field;
using Record = Ext.data.Record;


namespace Luxena.Travel
{
	public class PenalizeOperationEditForm : BaseEditForm
	{
		public PenalizeOperationEditForm(PenalizeOperationDto[] operations, bool allowAfterDeparturePenalties)
		{
			_operations = operations ?? new PenalizeOperationDto[]{};
			_allowAfterDeparturePenalties = allowAfterDeparturePenalties;

			CreateFormItems();

			SetPenalizeOperations();

			Window.addClass("penalize-operation");
			Window.setTitle(Res.PenalizeOperationEditForm_Caption);
		}

		public void Open()
		{
			Window.show();
		}

		protected override void OnSave()
		{
			CompleteSave(GetPenalizeOperations());
		}

		private void CreateFormItems()
		{
			Panel beforePanel = new Panel(new PanelConfig()
				.layout("form")
				.items(new object[]
				{
					new BoxComponent(new BoxComponentConfig()
						.autoEl(new Dictionary("tag", "div", "html", Res.AviaTicket_BeforeDeparture))
						.cls("penalties-caption")
						.ToDictionary()),
					PenaltyRow(Res.AviaTicket_ChangesPenalty, PenalizeOperationType.ChangesBeforeDeparture),
					PenaltyRow(Res.AviaTicket_RefundPenalty, PenalizeOperationType.RefundBeforeDeparture),
					PenaltyRow(Res.AviaTicket_NoShowPenalty_Short, PenalizeOperationType.NoShowBeforeDeparture),
				}).ToDictionary());

			Form.add(beforePanel);

			if (_allowAfterDeparturePenalties)
			{
				Panel afterPanel = new Panel(new PanelConfig()
					.layout("form")
					.cls("afterDeparture-panel")
					.items(new object[]
					{
						new BoxComponent(new BoxComponentConfig()
							.autoEl(new Dictionary("tag", "div", "html", Res.AviaTicket_AfterDeparture))
							.cls("penalties-caption")
							.ToDictionary()),
						PenaltyRow(Res.AviaTicket_ChangesPenalty, PenalizeOperationType.ChangesAfterDeparture),
						PenaltyRow(Res.AviaTicket_RefundPenalty, PenalizeOperationType.RefundAfterDeparture),
						PenaltyRow(Res.AviaTicket_NoShowPenalty_Short, PenalizeOperationType.NoShowAfterDeparture),
					}).ToDictionary());

				Form.add(afterPanel);
			}

			_saveButton = Form.addButton(BaseRes.Save, new AnonymousDelegate(Save));
			_cancelButton = Form.addButton(BaseRes.Cancel, new AnonymousDelegate(Cancel));

			ArrayList fields = new ArrayList();
			fields.AddRange(new object[]
			{
				_changesBeforeCombobox, _changesBeforeTextField,
				_refundBeforeCombobox, _refundBeforeTextField,
				_noshowBeforeCombobox, _noshowBeforeTextField
			});

			if (_allowAfterDeparturePenalties)
				fields.AddRange(new object[]
				{
					_changesAfterCombobox, _changesAfterTextField,
					_refundAfterCombobox, _refundAfterTextField,
					_noshowAfterCombobox, _noshowAfterTextField
				});

			Fields = (Field[]) fields;

			ArrayList components = new ArrayList();
			components.AddRange((object[]) fields);
			components.Add(_saveButton);
			components.Add(_cancelButton);

			ComponentSequence = (Component[])components;
		}

		private Panel PenaltyRow(string fieldText, PenalizeOperationType type)
		{
			ComboBox comboBox = GetPenaltyStatusCombo(type);
			TextField field = GetPenaltyDescriptionField(type);

			ArrayList items = new ArrayList();

			items.Add(new object[] { PenalizeOperationStatus.NotAllowed, DomainRes.PenalizeOperationStatus_NotAllowed });
			items.Add(new object[] { PenalizeOperationStatus.NotChargeable, DomainRes.PenalizeOperationStatus_NotChargeable });
			items.Add(new object[] { PenalizeOperationStatus.Chargeable, DomainRes.PenalizeOperationStatus_Chargeable });

			ArrayStore store = new ArrayStore(new ArrayStoreConfig()
				.fields(new string[] { "Id", "Name" })
				.data((Array)items)
				.ToDictionary());

			comboBox.store = store;

			comboBox.fieldLabel = fieldText.ToLowerCase();
			comboBox.typeAhead = true;
			comboBox.mode = "local";
			comboBox.forceSelection = true;
			comboBox.displayField = "Name";
			comboBox.valueField = "Id";
			comboBox.triggerAction = "all";
			comboBox.selectOnFocus = true;
			comboBox.width = 100;

			field.hideLabel = true;
			field.labelSeparator = string.Empty;
			field.addClass("description");
			field.setDisabled(true);
			field.selectOnFocus = true;
			field.width = 170;

			GenericOneArgDelegate changeField =
				delegate(object status)
				{
					if ((PenalizeOperationStatus)status != PenalizeOperationStatus.Chargeable)
					{
						field.setValue(string.Empty);
						field.setDisabled(true);
					}
					else
						field.setDisabled(false);
				};

			comboBox.on("select", new ComboBoxSelectDelegate(
				delegate(ComboBox combo, Record record, double index)
				{
					changeField(record.get("Id"));
				}));

			comboBox.on("change", new ComboBoxChangeDelegate(
				delegate(Field objthis, object newValue, object oldValue)
				{
					changeField(newValue);
				}));

			return new Panel(new FormPanelConfig()
				.layout("form")
				.itemCls("float-left")
				.labelAlign("left")
				.labelWidth(80)
				.items(new object[] { comboBox, field })
				.ToDictionary());
		}

		private void SetPenalizeOperations()
		{
			SetPenalizeOperation(PenalizeOperationType.ChangesBeforeDeparture);
			SetPenalizeOperation(PenalizeOperationType.ChangesAfterDeparture);
			SetPenalizeOperation(PenalizeOperationType.RefundBeforeDeparture);
			SetPenalizeOperation(PenalizeOperationType.RefundAfterDeparture);
			SetPenalizeOperation(PenalizeOperationType.NoShowBeforeDeparture);
			SetPenalizeOperation(PenalizeOperationType.NoShowAfterDeparture);
		}

		private void SetPenalizeOperation(PenalizeOperationType type)
		{
			ComboBox comboBox = GetPenaltyStatusCombo(type);
			TextField textField = GetPenaltyDescriptionField(type);

			PenalizeOperationDto operation = FindOperation(type);

			if (operation == null)
				return;

			comboBox.setValue(operation.Status);

			if (operation.Status == PenalizeOperationStatus.Chargeable)
			{
				textField.setValue(operation.Description);
				textField.setDisabled(false);
			}
			else
				textField.setDisabled(true);
		}

		private PenalizeOperationDto[] GetPenalizeOperations()
		{
			ArrayList list = new ArrayList();

			TryAddPenalizeOperation(list, PenalizeOperationType.ChangesBeforeDeparture);
			TryAddPenalizeOperation(list, PenalizeOperationType.ChangesAfterDeparture);
			TryAddPenalizeOperation(list, PenalizeOperationType.RefundBeforeDeparture);
			TryAddPenalizeOperation(list, PenalizeOperationType.RefundAfterDeparture);
			TryAddPenalizeOperation(list, PenalizeOperationType.NoShowBeforeDeparture);
			TryAddPenalizeOperation(list, PenalizeOperationType.NoShowAfterDeparture);

			return (PenalizeOperationDto[]) list;
		}

		private void TryAddPenalizeOperation(ArrayList list, PenalizeOperationType operationType)
		{
			ComboBox comboBox = GetPenaltyStatusCombo(operationType);
			TextField field = GetPenaltyDescriptionField(operationType);

			object value = comboBox.getValue();

			if (Script.IsNullOrUndefined(value) || value == (object) "") // don't remove cast to object, need to produce value === ''
				return;

			PenalizeOperationDto operation = new PenalizeOperationDto();
			operation.Type = operationType;
			operation.Status = (PenalizeOperationStatus) value;

			if (operation.Status == PenalizeOperationStatus.Chargeable)
				operation.Description = (string) field.getValue();

			list.Add(operation);
		}

		private PenalizeOperationDto FindOperation(PenalizeOperationType type)
		{
			foreach (PenalizeOperationDto dto in _operations)
				if (dto.Type == type)
					return dto;

			return null;
		}

		private ComboBox GetPenaltyStatusCombo(PenalizeOperationType operationType)
		{
			switch (operationType)
			{
				case PenalizeOperationType.ChangesBeforeDeparture:
					return _changesBeforeCombobox;
				case PenalizeOperationType.ChangesAfterDeparture:
					return _changesAfterCombobox;
				case PenalizeOperationType.RefundBeforeDeparture:
					return _refundBeforeCombobox;
				case PenalizeOperationType.RefundAfterDeparture:
					return _refundAfterCombobox;
				case PenalizeOperationType.NoShowBeforeDeparture:
					return _noshowBeforeCombobox;
				case PenalizeOperationType.NoShowAfterDeparture:
					return _noshowAfterCombobox;
			}

			return null;
		}

		private TextField GetPenaltyDescriptionField(PenalizeOperationType operationType)
		{
			switch (operationType)
			{
				case PenalizeOperationType.ChangesBeforeDeparture:
					return _changesBeforeTextField;
				case PenalizeOperationType.ChangesAfterDeparture:
					return _changesAfterTextField;
				case PenalizeOperationType.RefundBeforeDeparture:
					return _refundBeforeTextField;
				case PenalizeOperationType.RefundAfterDeparture:
					return _refundAfterTextField;
				case PenalizeOperationType.NoShowBeforeDeparture:
					return _noshowBeforeTextField;
				case PenalizeOperationType.NoShowAfterDeparture:
					return _noshowAfterTextField;
			}

			return null;
		}

		private readonly PenalizeOperationDto[] _operations;
		private readonly bool _allowAfterDeparturePenalties;

		private readonly ComboBox _changesBeforeCombobox = new ComboBox();
		private readonly TextField _changesBeforeTextField = new TextField();

		private readonly ComboBox _refundBeforeCombobox = new ComboBox();
		private readonly TextField _refundBeforeTextField = new TextField();

		private readonly ComboBox _noshowBeforeCombobox = new ComboBox();
		private readonly TextField _noshowBeforeTextField = new TextField();

		private readonly ComboBox _changesAfterCombobox = new ComboBox();
		private readonly TextField _changesAfterTextField = new TextField();

		private readonly ComboBox _refundAfterCombobox = new ComboBox();
		private readonly TextField _refundAfterTextField = new TextField();

		private readonly ComboBox _noshowAfterCombobox = new ComboBox();
		private readonly TextField _noshowAfterTextField = new TextField();

		private Button _saveButton;
		private Button _cancelButton;
	}
}