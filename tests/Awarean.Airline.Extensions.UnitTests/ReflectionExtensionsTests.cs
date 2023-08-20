using FluentAssertions;

namespace Awarean.Airline.Extensions.UnitTests;

public class ReflectionExtensionsTests
{
    [Fact]
    public void Multiple_Convertion_Operators_Should_Throw()
    {
        var instance = new ClassWithoutOperatorWithMultipleConvertions();
        var action = () => instance.GetType().GetConvertionMethod();

        action.Should().ThrowExactly<InvalidOperationException>()
        .And.Message.Should().Contain("Found more than one static operator methods that converts to primitive types");
    }

    [Fact]
    public void Marked_Convertion_Method_Should_Have_Priority()
    {
        var instance = new ClassWithOperatorAndMultipleConvertions();

        var convertionMethod = instance.GetType().GetConvertionMethod();

        convertionMethod.ReturnType.IsPrimitiveOrConvertible().Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(ObjectsGenerator))]
    public void Should_Tell_If_Type_Is_Primitive(object instance, bool expected)
    {
        var isPrimitive = instance.GetType().IsPrimitiveOrConvertible();
        isPrimitive.Should().Be(expected);
    }

    public static IEnumerable<object[]> ObjectsGenerator()
    {
        yield return new object[] { new ClassWithoutOperatorWithMultipleConvertions(), false };
        yield return new object[] { new object(), false };
        yield return new object[] { 1, true };
        yield return new object[] { 1.45, true };
        yield return new object[] { "my test string", true };
    }

    private class ClassWithoutOperatorWithMultipleConvertions
    {
        public static implicit operator string(ClassWithoutOperatorWithMultipleConvertions _) => "Implicit operator called";

        public static implicit operator ClassWithOperatorAndMultipleConvertions(ClassWithoutOperatorWithMultipleConvertions _) => null;
    }

    private class ClassWithOperatorAndMultipleConvertions
    {
        [PersistenceConvertion]
        public static implicit operator string(ClassWithOperatorAndMultipleConvertions _) => "Implicit operator called for another class";

        public static implicit operator ClassWithOperatorAndMultipleConvertions(int _) => null;
    }

    private class ClassWithInstanceConvertionMethodMarkedWithAttribute
    {
        public const string INSTANCE_CONVERTION_METHOD_STRING = "marked method attribute was called";
        private static int instantiations;

        public ClassWithInstanceConvertionMethodMarkedWithAttribute()
        {
            instantiations++;
        }

        [PersistenceConvertion]
        public string InstanceConvertionMethod() => $"{INSTANCE_CONVERTION_METHOD_STRING} {instantiations} times";

        public static implicit operator int(ClassWithInstanceConvertionMethodMarkedWithAttribute _) => 100;
    }
}