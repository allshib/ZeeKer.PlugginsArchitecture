# ZeeKer Orion Plugin Architecture

Этот репозиторий демонстрирует, как можно построить модульное веб-приложение на .NET 9 с поддержкой горячего подключения плагинов. Основное приложение (`ZeeKer.Orion.API`) загружает внешние библиотеки из каталога `plugins`, валидирует их совместимость и автоматически публикует API-эндпоинты, предоставленные каждым плагином.

## Структура решения

- **ZeeKer.Orion.Abstractions** – набор контрактов для плагинов (общий интерфейс `IPlugin`, расширения `IApiPlugin` и `IServicePlugin`, а также описание дескриптора и контекста загрузки). Контракты определяют версионирование плагинов и минимально поддерживаемую версию хоста.【F:ZeeKer.Orion.Abstractions/IPlugin.cs†L1-L16】【F:ZeeKer.Orion.Abstractions/IApiPlugin.cs†L1-L13】【F:ZeeKer.Orion.Abstractions/IServicePlugin.cs†L1-L9】【F:ZeeKer.Orion.Abstractions/PluginDescriptor.cs†L1-L8】【F:ZeeKer.Orion.Abstractions/IsolatedPluginLoadContext.cs†L1-L25】
- **ZeeKer.Orion** – инфраструктура загрузки плагинов. Класс `PluginLoader` находит DLL-файлы, создаёт для каждого свой `AssemblyLoadContext`, проверяет версию контракта и позволяет плагинам зарегистрировать свои сервисы в DI контейнере.【F:ZeeKer.Orion/PluginLoader.cs†L1-L66】
- **ZeeKer.Orion.Infrastructure** – вспомогательные сервисы для API-хоста (в частности, адаптеры для работы с минимальными API).【F:ZeeKer.Orion.API/Services/PluginMapper.cs†L1-L22】
- **ZeeKer.Orion.API** – минимальное веб-приложение, которое инициализирует загрузчик плагинов, подключает Swagger и публикует маршруты плагинов по шаблону `/plugins/{id}/v{major}`.【F:ZeeKer.Orion.API/Program.cs†L1-L37】【F:ZeeKer.Orion.API/Services/PluginMapper.cs†L8-L20】
- **ZeeKer.Orion.HelloPluggin** – пример плагина, публикующий маршрут `GET /hello` и требующий минимальную версию хоста `1.0.0`. DLL-версия 2.0.0 копируется в каталог `plugins` для демонстрации нескольких выпусков.【F:ZeeKer.Orion.HelloPluggin/HelloPlugin.cs†L1-L22】【F:ZeeKer.Orion.API/ZeeKer.Orion.API.csproj†L18-L26】

## Быстрый старт

1. Убедитесь, что установлен .NET SDK 9.0.
2. Соберите решение:
   ```bash
   dotnet build ZeeKer.Orion.sln
   ```
3. Запустите хост:
   ```bash
   dotnet run --project ZeeKer.Orion.API
   ```
4. После запуска откройте Swagger UI по адресу `https://localhost:5001/swagger` (или `http://localhost:5000/swagger` при запуске без HTTPS). Вы увидите эндпоинт хоста (`GET /`) и маршруты, добавленные плагинами, например `GET /plugins/HelloPlugin/v2/hello`.

## Как работает система плагинов

1. При запуске хост создаёт контрактную версию (`1.0.0`) и вызывает `PluginLoader.LoadPlugins`, передавая сервисный контейнер, конфигурацию и путь к каталогу `plugins` внутри выходной папки приложения.【F:ZeeKer.Orion.API/Program.cs†L5-L22】
2. `PluginLoader` находит все DLL-файлы, создаёт для каждого отдельный `IsolatedPluginLoadContext`, загружает типы, реализующие `IPlugin`, и проверяет совместимость версий (свойство `MinHostVersion`). Если плагин реализует `IServicePlugin`, он может зарегистрировать свои зависимости в DI контейнере хоста.【F:ZeeKer.Orion/PluginLoader.cs†L10-L55】
3. После загрузки `PluginMapper` обходит дескрипторы и вызывает метод `MapEndpoints` у плагинов, реализующих `IApiPlugin`. Эндпоинты группируются под префиксом `/plugins/{id}/v{major}`, что позволяет размещать несколько версий плагина параллельно.【F:ZeeKer.Orion.API/Services/PluginMapper.cs†L8-L20】

## Создание собственного плагина

1. Создайте новый проект типа `classlib` и добавьте ссылку на `ZeeKer.Orion.Abstractions`.
2. Реализуйте `IPlugin`. Для HTTP-расширений используйте `IApiPlugin` и определите метод `MapEndpoints`, получающий адаптер минимальных API.
3. Укажите уникальные `Id`, `Name`, `Version` и требуемую `MinHostVersion`. Значение `Version.Major` будет использоваться в URL.
4. При необходимости реализуйте `IServicePlugin`, чтобы зарегистрировать свои сервисы через `ConfigureServices`.
5. Соберите библиотеку под `net9.0` и скопируйте DLL в каталог `ZeeKer.Orion.API/plugins`. При следующем запуске хост обнаружит новую сборку и добавит её маршруты автоматически.

## Дополнительные идеи для развития

- Добавить hot-reload плагинов без перезапуска хоста.
- Расширить систему контрактов (например, поддержка фоновых заданий или health-check эндпоинтов).
- Внедрить цифровую подпись плагинов и централизованное управление версиями.

Проект создан как учебная площадка для изучения архитектуры плагинов в .NET и может служить основой для собственных решений.
