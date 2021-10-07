using System.Collections;

using Ext.form;

using LxnBase;


namespace Luxena.Travel.Controls
{
	public class MoneyControlCalculator
	{
		public MoneyControlCalculator Add(Field control, int multiplier)
		{
			if (control != null)
			{
				_controls.Add(control);
				_multipliers.Add(multiplier);
			}

			return this;
		}

		public void Recalculate(Field field)
		{
			MoneyControl control = (MoneyControl)field;

			if (!Has(control) && !MoneyControl.CanCalculate((MoneyControl[]) _controls))
				return;

			decimal sum = 0;
			int multiplier = 1;

			for (int i = 0; i < _controls.Count; i++)
			{
				if (_controls[i] == control)
				{
					multiplier = (int) _multipliers[i];
					continue;
				}

				sum += ((MoneyControl) _controls[i]).Amount * (int)_multipliers[i];
			}

			sum /= -multiplier;

			control.Amount = sum;
		}

		public bool Has(MoneyControl control)
		{
			foreach (object t in _controls) {
				if (control == t)
					return true;
			}

			return false;
		}

		private readonly ArrayList _controls = new ArrayList();
		private readonly ArrayList _multipliers = new ArrayList();
	}
}