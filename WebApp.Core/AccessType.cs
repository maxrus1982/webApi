using System;
using System.ComponentModel;

namespace WebApp.Core
{
    [Flags]
    public enum AccessType : int
    {
        [Description("Ошибка")]
        Error = 0x0001,

        /// <summary>
        /// Вход в систему
        /// </summary>
        [Description("Вход в систему")]
        Login = 0x0002,

        /// <summary>
        /// Выход из системы
        /// </summary>
        [Description("Выход из системы")]
        Logoff = 0x0004,

        /// <summary>
        /// Поиск, просмотр списка
        /// </summary>
        [Description("Поиск, просмотр списка")]
        Search = 0x0008,

        /// <summary>
        /// Просмотр
        /// </summary>
        [Description("Просмотр")]
        View = 0x0010,

        /// <summary>
        /// Редактирование
        /// </summary>
        [Description("Редактирование")]
        Modify = 0x0020,

        /// <summary>
        /// Удаление
        /// </summary>
        [Description("Удаление")]
        Remove = 0x0040,

        /// <summary>
        /// Выполнение
        /// </summary>
        [Description("Выполнение")]
        Execute = 0x0080,

        /// <summary>
        /// Оповещение
        /// </summary>
        [Description("Оповещение")]
        Notify = 0x0100,

        /// <summary>
        /// Создание
        /// </summary>
        [Description("Создание")]
        Create = 0x0200,

        /// <summary>
        /// Права только на чтение
        /// </summary>
        ReadOnly = Search | View,

        /// <summary>
        /// Права на чтение и изменение (нет создания и удаления)
        /// </summary>
        ModifyAccess = Search | View | Modify,

        /// <summary>
        /// Полные права на объект
        /// </summary>
        FullAccess = Search | View | Create | Modify | Remove | Execute,

        /// <summary>
        /// Права на создание и просмотр
        /// </summary>
        CreateAccess = Create | ReadOnly
    }
}
