# 🔌 PluginSystem

**PluginSystem** — это расширяемая модульная система плагинов и команд, предназначенная для построения гибкой архитектуры в .NET-проектах.

Проект активно развивается и фокусируется на **чистом API**, **тестируемости** и интеграции с **MVVM (Prism)**.

---

## ✨ Возможности

- 🔄 Поддержка **загрузки и выгрузки плагинов во время выполнения**
- ⚙️ Гибкая система **команд**, расширяемая плагинами
- 🧱 Разделение на независимые слои: `Core`, `Runtime`, `Hosting`, `UI`
- 🧩 Интеграция с **Dependency Injection** и конфигурацией
- ✅ **Тестируемость**: юнит-тесты и моделирование критических ситуаций
- 🎨 Поддержка **MVVM и Prism** для UI-части

---

## 🧭 Архитектура

| Проект                  | Назначение                                                  |
|------------------------|-------------------------------------------------------------|
| `PluginSystem.Core`    | Базовые интерфейсы и типы                                   |
| `PluginSystem.Runtime` | Реализация логики работы плагинов и команд                  |
| `PluginSystem.Hosting` | Слой хостинга, отвечающий за загрузку и управление плагинами |
| `PluginSystem.UI`      | Визуальная часть (MVVM + Prism)                             |

---

## 🔧 Пример использования

```csharp
var manager = new PluginManager(pluginLoader);
manager.LoadAll();
