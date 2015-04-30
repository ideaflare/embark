using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Embark.DesignPatterns.MVVM
{
    /// <summary>
    /// Lambda expressions to get NotifyChangeBase property strings and raise properties.
    /// <para>Intended to avoid using magic strings that are not easily refactor-safe</para>
    /// </summary>
    public static class PropertyChangeBaseReflection
    {
        /// <summary>
        /// Raise the PropertyChangedEvent of a property passed in via lambda
        /// <para>example:</para>
        /// <example>
        /// <para>this.RaisePropertyChangedEvent((dog) => dog.BarkType)</para>
        /// <para>refactor-safe equivalent to this.RaisePropertyChangedEvent("BarkType")</para>
        /// </example>
        /// </summary>
        /// <typeparam name="TSource">Implementation type of NotifyChangeBase class</typeparam>
        /// <typeparam name="TProperty">Property type to raise event for</typeparam>
        /// <param name="obj">Instance of NotifyChangeBase object</param>
        /// <param name="property">Property to raise event for</param>
        public static void RaisePropertyChangedEvent<TSource, TProperty>(this TSource obj, Expression<Func<TSource, TProperty>> property) where TSource : PropertyChangeBase
        {
            var propertyName = PropertyChangeBaseReflection.GetPropertyString(obj, property);
            obj.RaisePropertyChangedEvent(propertyName);
        }

        /// <summary>
        /// Return the string name of a property to avoid using magic strings
        /// <para>example:</para>
        /// <example>
        /// <para>dog.GetPropertyString((d) => d.BarkType)</para>
        /// <para>returns string "BarkType"</para>
        /// </example>
        /// </summary>
        /// <typeparam name="TSource">Implementation type of NotifyChangeBase class</typeparam>
        /// <typeparam name="TProperty">Property type to raise event for</typeparam>
        /// <param name="obj">Instance of NotifyChangeBase object</param>       
        /// <param name="property">Property to raise event for</param> 
        /// <returns>String of property lambda</returns>
        public static string GetPropertyString<TSource, TProperty>(this TSource obj, Expression<Func<TSource, TProperty>> property) where TSource : PropertyChangeBase
        {
            var lambda = (LambdaExpression)property;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else memberExpression = (MemberExpression)lambda.Body;
            return memberExpression.Member.Name;
        }
    }
}
