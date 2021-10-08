Feature: Order

Background:
	Given the user has logged in as Agent

Scenario: Create an empty order
	When the user has selected menu item "Документы->Новый заказ"
	Then the system opens form "Заказ (создание)"
	When the user fills out the form as follows
		| Label      | Value             |
		| Заказчик   | ТОВ               |
		| Примечание | Билет №3544667902 |
	And the user presses "Сохранить"
	Then the system closes the form
	And the system shows notification "Заказы: добавлено"
	And the system opens tab with the new Order
