namespace DevFast.Net.Text.Tests.Json
{
    [TestFixture]
    public class RawJsonTests
    {
        [Test]
        public void Equals_Returns_True_Only_If_Array_Ref_Are_Same_Else_False()
        {
            object third = 1;
            RawJson first = new(JsonType.Str, new byte[1]);
            That(first.Equals(third), Is.False);

            RawJson second = new(JsonType.Str, new byte[1]);
            That(first, Is.Not.EqualTo(second));

            third = second;
            That(first.Equals(third), Is.False);

            RawJson forth = new(JsonType.Str, first.Value);
            That(first, Is.EqualTo(forth));

            third = forth;
            That(first.Equals(third), Is.True);
        }

        [Test]
        public void GetHashCode_Is_Consistent_For_Equal_Instances()
        {
            RawJson first = new(JsonType.Str, new byte[1]);
            RawJson forth = new(JsonType.Str, first.Value);
            That(first, Is.EqualTo(forth));
            That(first.GetHashCode(), Is.EqualTo(forth.GetHashCode()));

            object third = forth;
            That(first.GetHashCode(), Is.EqualTo(third.GetHashCode()));
        }

        [Test]
        public void Equal_Operator_And_Not_Equal_Operator_Are_Consistent()
        {
            RawJson first = new(JsonType.Str, new byte[1]);
            RawJson second = new(JsonType.Str, new byte[1]);
            That(first != second, Is.True);
            That(first == second, Is.False);

            RawJson forth = new(JsonType.Str, first.Value);
            That(first == forth, Is.True);
            That(first != forth, Is.False);
        }
    }
}
