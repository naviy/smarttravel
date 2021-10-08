Feature: PaymentSystem


Background:
	Given the user has logged in as Agent
	And the user has selected menu item "Справочники->Платежные системы"


Scenario: Create a new Payment System
	When the user is going to begin to create Payment System
	And the user fills out the form as follows
		| Label         | Value                   |
		| Название      | Новая платежная система |
	And the user presses "Сохранить"
	Then the system closes the form
	And the system shows notification "Платежные системы: добавлено Новая платежная система"


Scenario: Create a new payment system without required fields
	When the user is going to begin to create Payment System
	And the user fills out the form as follows
		| Label         | Value                 |
		| Название      |                       |
	And the user presses "Сохранить"
	Then the system shows validation message "Название: Это поле обязательно для заполнения"
