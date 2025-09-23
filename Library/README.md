## Library

#### Структура проекта
- [Library](#library)
    - [Структура проекта](#структура-проекта)
    - [Предметная область](#предметная-область)
    - [Infrastructure](#infrastructure)
    - [Application](#application)
    - [Api](#api)
    - [Tests](#tests)
    - [AppHost](#apphost)

#### Предметная область
- **Entities** — сущности предметной области:
  - `Book` — карточка книги
  - `BorrowRecord` — запись о выдаче книги
  - `Customer` — читатель библиотеки
- **Enums** — перечисления, используемые в карточках книг:
  - `Publisher` — издательства
  - `PublishingType` — типы изданий
- **Interfaces** — абстракции для репозиториев, которые реализуются в `Infrastructure`.

#### Infrastructure
- **Repositories** — Реализации интерфейсов из `Domain`:
  - `BookRepository` — карточка книги
  - `BorrowRecordRepository` — запись о выдаче книги
  - `CustomerRepository` — читатель библиотеки
- **Persistence** — Средства для подключения и настройки БД:
  - `Migrations` — Миграции для БД
  - `AppDbContext` — Контекст БД, используемой в приложении.

#### Application
- **Services** — Сервисы с методами для работы с приложением:
  - `AnalyticsService` — Сервис с методами для получения аналитических данных о библиотеке.

#### Api
- **Controllers** — Контроллеры для работы с API:
  - `BookController` — контроллер для работы с карточками книг
  - `BorrowRecordController` — контроллер для работы записями выданных книг
  - `CustomerController` — контроллер для работы с записями о читателях
  - `AnalyticsController` — контроллер для получения аналитических данных о библиотеке
- `Program` - Конфигурация для приложения, настройка DI, регистрация сервисов.

#### Tests
- `DomainTests` — Юнит-тесты для `Domain`
- `TestDataFixture` — Вспомогательный класс для инициализации данных
- `TestDataSeeder` — Класс, содержащий подготовленные данные для тестов.

#### AppHost
- `Program` - Отвечает за запуск приложения с помощью Aspire, настройка зависимостей разных модулей и создание контейнеров.