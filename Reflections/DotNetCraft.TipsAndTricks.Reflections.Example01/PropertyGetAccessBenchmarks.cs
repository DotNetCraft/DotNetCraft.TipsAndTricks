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
    [CsvExporter, RPlotExporter, HtmlExporter]
    [MinColumn, MaxColumn]
    public class PropertyGetAccessBenchmarks
    {
        private const string ParamName = "Age";

        private readonly SimpleModel _myObject;
        private readonly PropertyInfo _propertyInfo;
        private readonly dynamic _dynamicObject;
        private readonly Func<SimpleModel, int> _getter;
        private readonly Type _objectType;
        public PropertyGetAccessBenchmarks()
        {
            _myObject = new SimpleModel { Age = 42 };
            _objectType = typeof(SimpleModel);
            _propertyInfo = _objectType.GetProperty(ParamName);
            _dynamicObject = _myObject;
            _getter = PropertyAccessor<SimpleModel>.CreateGetter<int>(ParamName);
        }

        [Benchmark(Baseline = true)]
        public int DirectAccessGet() { return _myObject.Age; }

        [Benchmark]
        public int PrecompiledExpressionGet() { return _getter(_myObject); }

        [Benchmark]
        public int DynamicGet() { return _dynamicObject.Age; }

        [Benchmark]
        public int ReflectionGet() { return (int)_propertyInfo.GetValue(_myObject); }

        [Benchmark]
        public int InvokeMemberGet() { return (int)_objectType.InvokeMember(ParamName, BindingFlags.GetProperty, null, _myObject, null); }
    }
}
