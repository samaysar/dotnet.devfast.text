﻿namespace DevFast.Net.Text.Tests.Json
{
    [TestFixture]
    public class JsonTypeTests
    {
        [Test]
        public void JsonType_Values_Are_Consistent()
        {
            Array typeValues = Enum.GetValues(typeof(JsonType));
            Multiple(() =>
            {
                That(typeValues, Is.Not.Null);
                That(typeValues, Has.Length.EqualTo(7));
                int pos = 0;
                That(typeValues.GetValue(pos++)!.ToString(), Is.EqualTo("Undefined"));
                That(typeValues.GetValue(pos++)!.ToString(), Is.EqualTo("Obj"));
                That(typeValues.GetValue(pos++)!.ToString(), Is.EqualTo("Arr"));
                That(typeValues.GetValue(pos++)!.ToString(), Is.EqualTo("Num"));
                That(typeValues.GetValue(pos++)!.ToString(), Is.EqualTo("Str"));
                That(typeValues.GetValue(pos++)!.ToString(), Is.EqualTo("Bool"));
                That(typeValues.GetValue(pos)!.ToString(), Is.EqualTo("Null"));
            });
        }
    }
}