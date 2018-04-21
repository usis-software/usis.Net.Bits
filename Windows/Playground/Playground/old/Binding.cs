using System;
using System.Globalization;
using usis.Platform.Portable.Data;

namespace Playground
{
    internal static class BindingPlayground
    {
        internal static void Main()
        {
            var entity = new Entity() { Name = "Udo", Integer = 42 };
            var control = new Control();

            //var binding = new Binding<string, string>();
            //binding.Source = new BindingProperty<string>(entity, "Name");
            //var binding = new Binding<int, string>();
            //var source = new BindingProperty(entity, "Integer");
            //var target = new BindingProperty(control, "Value");
            //var binding = new Binding(source, target);
            //binding.Source = source;
            //binding.Target = target;
            //control.SetBindings(binding);
            //control.SetBindings(new Binding(source, target));
            control.SetBinding("Value", entity, "Integer");
            control.Bind(CultureInfo.CurrentCulture);

            Console.WriteLine(control.Value);
            ConsoleTool.PressAnyKey();
        }
    }

    internal class Control : BindingTarget
    {
        public string Value { get; set; }
    }

    internal class Entity
    {
        public string Name { get; set; }
        public int Integer { get; set; }
    }

}

namespace Hidden
{
    public interface IBindingProperty<T>
    {
        T Value { get; }
        void SetValue(T value);
    }

    public class Binding<TSource, TTarget> : IBinding
    {
        private IFormatProvider formatProvider;

        public IBindingProperty<TSource> Source { get; set; }
        public IBindingProperty<TTarget> Target { get; set; }

        protected Converter<TSource, TTarget> SourceConverter { get; private set; }
        protected Converter<TTarget, TSource> TargetConverter { get; private set; }

        public Binding(IBindingProperty<TSource> source, IBindingProperty<TTarget> target)
        {
            Source = source; Target = target;
        }

        public Binding()
        {
            SourceConverter = (source) => { return (TTarget)Convert.ChangeType(source, typeof(TTarget), formatProvider); };
            TargetConverter = (target) => { return (TSource)Convert.ChangeType(target, typeof(TSource), formatProvider); };
        }

        public void UpdateTarget(IFormatProvider provider)
        {
            formatProvider = provider;
            Target.SetValue(SourceConverter(Source.Value));
        }
    }

    public class BindingProperty<T> : IBindingProperty<T>
    {
        private object bindingObject;
        private System.Reflection.PropertyInfo propertyInfo;

        public BindingProperty(object source, string name)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            propertyInfo = source.GetType().GetProperty(name, typeof(T));
            bindingObject = source;
        }

        public T Value { get { return (T)propertyInfo.GetValue(bindingObject); } }

        public void SetValue(T value)
        {
            propertyInfo.SetValue(bindingObject, value);
        }
    }
}