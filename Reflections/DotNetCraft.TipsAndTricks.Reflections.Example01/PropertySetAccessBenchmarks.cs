using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System;
using System.Reflection;

namespace DotNetCraft.TipsAndTricks.Reflections.Example01
{
    [SimpleJob(RuntimeMoniker.Net50, baseline: true)]
    [SimpleJob(RuntimeMoniker.Net60)]
    [SimpleJob(RuntimeMoniker.Net70)]
    [SimpleJob(RuntimeMoniker.Net80)]
    [CsvExporter, HtmlExporter, RPlotExporter]
    [MinColumn, MaxColumn]
    public class PropertySetAccessBenchmarks
    {
        private const string ParamName = "Age";

        private readonly SimpleModel _myObject;
        private readonly PropertyInfo _propertyInfo;
        private readonly dynamic _dynamicObject;
        private readonly Action<SimpleModel, int> _setter;
        private readonly Type _objectType;
        
        public PropertySetAccessBenchmarks()
        {
            _myObject = new SimpleModel { Age = 42 };
            _objectType = typeof(SimpleModel);
            _propertyInfo = _objectType.GetProperty(ParamName);
            _dynamicObject = _myObject;
            _setter = PropertyAccessor<SimpleModel>.CreateSetter<int>(ParamName);
        }

        [Benchmark(Baseline = true)]
        public SimpleModel DirectSet()
        {
            _myObject.Age = 123;
            return _myObject;
        }

        [Benchmark]
        public SimpleModel ReflectionSet()
        {
            _propertyInfo.SetValue(_myObject, 123);
            return _myObject;
        }

        [Benchmark]
        public SimpleModel DynamicSet()
        {
            _dynamicObject.Age = 123;
            return _myObject;
        }

        [Benchmark]
        public SimpleModel InvokeMemberSet()
        {
            _objectType.InvokeMember(ParamName, BindingFlags.SetProperty, null, _myObject, new object[] { 123 });
            return _myObject;
        }

        [Benchmark]
        public SimpleModel PrecompiledExpressionSet()
        {
            _setter(_myObject, 123);
            return _myObject;
        }
    }
}
