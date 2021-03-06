﻿Задание:
	Макет приложения "Управление текущими задачами": Показ задач пользователя - в работе, планируемые, завершенные.
	Разбиение задач идет по статусу исполнения.
	Требуется бэкенд asp.net mvc (или webapi), достаточный для работы приложения, с набором тестов на логику.
	Сделать архитектуру так, как считаешь «красиво», чтобы посмотреть, как ты делаешь архитектуру веб-приложений.
	Продумать, какая часть будет делаться на бэкенде, а какая — на фронтенде, и как они будут взаимодействовать.
	Фронтенд сделать любой, чисто для демонстрации работы, верстки по макету не нужно, рисовать график тоже не нужно.
	С БД работать через entity framework code first.
	Реализовать на бэкенде алгоритм генерации паджинатора. Функция, отвечающая за генерацию, должна принимать три параметра: число задач на странице, общее число задач, и текущая страница.

Условия приема реализации:
  Набор простейших unit-тестов (успешно пройденных), для покрытия логики работы бизнес-модели

Реализация:
  0) Приложение и Unit-тесты работают с упрощенной БД MSSQL "LocalDb"
  1) Упрощенно проект разбит на 9 логических сборок: UI-web, Core, DAL, Domain, Migrations, Service, Test
  2) Основной бизнес-объект – задача (Task)

  3) Основной TaskApiController построен наследованием от базового generic контроллера + его расширение
  4) Основной TaskRepository построен наследованием от базового generic репозитория  + его расширение 
  5) За счет наследования стандартизованы:
		имена методов контроллера и репозитория
		принимаемые и возвращаемые значения методов
		обеспечивается простая реализация ACL
		обеспечивается реализация пагинации, фильтрации, поиска и сортировки, валидации
		обеспечивается шаблонный и унифицированный подход к проектированию ПО

  6) Взаимодействие с клиентской средой построено на трех объектах: Request, Response, CreateDocumentRequest (запрос на new документ)
  7) Request содержит в себе данные по пагинации, фильтрами, сортировке и кастомные фильтры для каждого бизнес-объекта системы
  8) Response, понятно, содержит в себе данные в виде JSON + итоговый Total записей + список ошибок если есть
  9) CreateDocumentRequest - запрос от клиента, на получение DTO по новому создаваемому документу. т.е. его первичная инициализация происходит на сервере
  10) Также есть валидатор на сохранение. Плюс еще должен был бы быть здесь отдельный валидатор на удаление/аннуляцию

  11) Используется "копипастный" сервисный слой из запасников Родины: автомаппинг бизнес-объектов в DTO, IoC-контейнер, ACL по ресурсам
  12) Схема точно работает в связке с KendoUI/AngularJS
  13) Пагинация поиск фильтрация сортировка - работает на уровне СУБД по прямо замапенным полям, и на уровне IIS по расчетным полям
  14) WebApi тестами не покрывал, но можно проверить через Advanced Rest Client
		http://localhost:41787/Api/Task/GetList (POST + Request)
		http://localhost:41787/Api/Task/GetGroupedList (POST + Request)