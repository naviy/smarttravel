Feature: Department


Background:
	Given the user has logged in as Agent
	And the user has selected menu item "Справочники->Подразделения"
	And the user is going to begin to create Department


Scenario: Create a new Department
	When the user fills out the form as follows
		| Label                | Value                  |
		| Организация          | Новая организация      |
		| Наименование         | Новое подразделение    |
		| Официальное название | New department         |
		| Юридический адрес    | Юридический адрес      |
		| Фактический адрес    | Фактический адрес      |
		| Телефоны             | 444-44-44              |
		| E-mail адреса        | test@google.com        |
		| Факс                 | 555-55-55              |
		| Подчиняется          | Новая организация      |
		| Примечание           | Тестовое подразделение |
	And the user presses "Сохранить"
	Then the system closes the form
	And the system shows notification "Подразделения: добавлено Новое подразделение"