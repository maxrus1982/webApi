using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.DAL
{
    /// <summary>
    /// Аттрибут для ProjectionHelper-а, указывает путь до свойства вложенных объектов через точку    
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ProjectionAttribute : Attribute
    {
        private readonly string _propertyPath;

        /// <summary>
        /// путь до свойства вложенного объекта через точку, например DocumentState.Code
        /// </summary>
        public string PropertyPath { get { return _propertyPath; } }

        /// <summary>
        /// Аттрибут для ProjectionHelper-а, указывает путь до свойства вложенных объектов через точку    
        /// </summary>
        /// <param name="propertyPath">путь до свойства вложенного объекта через точку, например DocumentState.Code</param>
        public ProjectionAttribute(String propertyPath)
        {
            _propertyPath = propertyPath;
        }
    }

    /// <summary>
    /// Аттрибут для ProjectionHelper-а, указывает что надо пропустить это свойства
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ProjectionIgnore : Attribute
    {

    }
}
