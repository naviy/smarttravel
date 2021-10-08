Feature: Avia document processing

Background:
	Given the user has logged in as Agent
	And the user has selected menu item "Документы->Авиа документы"


Scenario: Process avia ticket
	Given the user selected "3544667874" ticket
	When the user presses "обработать"
	Then the system opens form "Обработка билета 628-3544667874"
	When the user fills out the form as follows
		| Label           | Value     |
		| Заказчик        | Конечный  |
		| Сервисный сбор  | 200       |
		| Скидка          | 50        |
	Then the system calculates "К оплате" as "1813,00"
	When the user presses "Сохранить"
	Then the system closes the form
	And the system updates the selected row as follows
		| Label     | Value                |
		| Заказчик  | Конечный покупатель  |
		| К оплате  | 1 813,00 UAH         |
