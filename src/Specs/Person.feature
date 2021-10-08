Feature: Person


Background:
	Given the user has logged in as Agent
	And the user has selected menu item "Справочники->Персоны"


Scenario: Create a new person
	When the user is going to begin to create Person
	And the user fills out the form as follows
		| Label             | Value                 |
		| Наименование      | Новый пользователь    |
		| ФИО               | Иванов Иван Иванович  |
		| Дата рождения     | 28.03.2012            |
		| Юридический адрес | Юридический адрес     |
		| Фактический адрес | Фактический адрес     |
		| Телефоны          | 444-44-44             | 
		| E-mail адреса     | test@google.com       | 
		| Факс              | 555-55-55             | 
		| Веб адрес         | www.test.com.ua       | 
		| Примечание        | Тестовый пользователь |
	And the user presses "создать" to create mileage card
	Then the system opens form "Мильная карточка (создание)"

	When the user fills out the form as follows
		| Label             | Value                 |
		| Номер				| 123                   |
	And the user presses "Сохранить"
	Then the system closes the form

	When the user presses "создать" to create passport
	Then the system opens form "Паспорт (создание)"
	
	When the user fills out the form as follows
		| Label             | Value                 |
		| Номер             | passport 111          |
		| Имя               | Иван                  |
		| Отчество          | Иванович              |
		| Фамилия           | Иванов                |
		| Дата рождения     | 02.11.1971            |
		| Действителен до   | 02.11.2020            |
		| Примечание        | К службе в ВС годен   |
	And the user presses "Сохранить"
	Then the system closes the form

	When the user presses "Сохранить"
	Then the system closes the form
	And the system shows notification "Персоны: добавлено Новый пользователь"

	When the user presses "изменить"
	Then the system opens form "Персона (редактирование)"


Scenario: Create a new person without required fields
	When the user is going to begin to create Person
	And the user fills out the form as follows
		| Label             | Value                 |
		| Наименование      |                       |
	And the user presses "Сохранить"
	Then the system shows validation message "Наименование: Это поле обязательно для заполнения"


Scenario: Delete a person
	When the user selects "Петров Петр Петрович" in the list
	And the user presses "удалить"
	Then the system shows confirmation dialog

	When the user presses at confirmation dialog "Да"
	Then the system shows notification "Персоны: удалено Пользователь для удаления"

