using GSWebAPI.GSWebAPI.Services.Common;
using Xunit;

namespace GSWebAPI.Tests.Community
{
    /// <summary>Pure unit tests (no database) for the English/Spanish foul-language filter.</summary>
    public sealed class ProfanityFilterTests
    {
        private readonly ProfanityFilter _filter = new();

        [Theory]
        // English, including inflections, leetspeak, stretched and spaced-out letters.
        [InlineData("What the fuck")]
        [InlineData("You MOTHERFUCKER")]
        [InlineData("this is shit")]
        [InlineData("total bullsh1t")]
        [InlineData("$hit happens")]
        [InlineData("sh!t")]
        [InlineData("fuuuuuck")]
        [InlineData("f u c k")]
        [InlineData("f.u.c.k you")]
        [InlineData("you ass!")]
        [InlineData("asssss")]
        [InlineData("son of a b1tch")]
        // Spanish, with and without accents, ñ kept.
        [InlineData("Qué mierda")]
        [InlineData("eres un pendejo")]
        [InlineData("PENDEJOOOO")]
        [InlineData("cabrón")]
        [InlineData("hijo de puta")]
        [InlineData("hijueputa")]
        [InlineData("put@")]
        [InlineData("¡Coño!")]
        [InlineData("vete a la verga")]
        [InlineData("no seas gilipollas")]
        [InlineData("m.i.e.r.d.a")]
        [InlineData("pinche tarea")]
        public void Detects_foul_language(string text)
        {
            Assert.True(_filter.ContainsProfanity(text));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Great class, thanks for the assist!")]
        [InlineData("Our assignment is due Friday")]
        [InlineData("The passage about Scunthorpe")]
        [InlineData("Shiitake mushrooms and cocktails")]
        [InlineData("Sussex, Essex and Massachusetts")]
        [InlineData("El cálculo del vehículo es putativo")]
        [InlineData("Un cono de helado")] // "cono" (cone) is fine; "coño" is not
        [InlineData("La computadora y el disputado partido")]
        [InlineData("Me da vergüenza; es un sinvergüenza")]
        [InlineData("Las cuentas de matemáticas")]
        [InlineData("Room 101, page 3, 4 students")]
        [InlineData("I am a teacher")]
        public void Allows_clean_text(string? text)
        {
            Assert.False(_filter.ContainsProfanity(text));
        }

        [Fact]
        public void Extra_words_from_configuration_are_added()
        {
            var filter = new ProfanityFilter(new[] { "tonto", "*baboso*" });

            Assert.True(filter.ContainsProfanity("no seas tonto"));
            Assert.True(filter.ContainsProfanity("superbabosote"));
            Assert.False(filter.ContainsProfanity("tontería")); // exact entry: "tonteria" is a different word
            Assert.True(filter.ContainsProfanity("fuck")); // built-in lists still apply
        }
    }
}
