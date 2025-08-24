using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Статический класс, реализующий паттерн Service Locator.
/// Предоставляет глобальный доступ к зарегистрированным сервисам, избегая прямых зависимостей.
/// </summary>
public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
    
    /// <summary>
    /// Регистрирует экземпляр сервиса. Перезаписывает существующий сервис того же типа.
    /// </summary>
    public static void Register<T>(T service)
    {
        Type type = typeof(T);
        if (_services.ContainsKey(type))
        {
            _services[type] = service;
            Debug.LogWarning($"Сервис типа {type.Name} был перезаписан.");
        }
        else
        {
            _services.Add(type, service);
        }
    }

    /// <summary>
    /// Возвращает зарегистрированный сервис указанного типа.
    /// </summary>
    /// <returns>Экземпляр сервиса или default(T), если сервис не найден.</returns>
    public static T Get<T>()
    {
        Type type = typeof(T);
        if (_services.TryGetValue(type, out object service))
        {
            return (T)service;
        }
        
        Debug.LogError($"Сервис типа {type.Name} не зарегистрирован.");
        return default;
    }

    /// <summary>
    /// Очищает все зарегистрированные сервисы.
    /// </summary>
    public static void Clear()
    {
        _services.Clear();
    }
}