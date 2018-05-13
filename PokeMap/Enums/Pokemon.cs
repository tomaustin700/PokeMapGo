using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PokeMap.Enums
{
    public enum Pokemon
    {
        [Description("Bulbasaur")]
        Bulbasaur,

        [Description("Ivysaur")]
        Ivysaur,

        [Description("Venusaur")]
        Venusaur,

        [Description("Charmander")]
        Charmander,

        [Description("Charmeleon")]
        Charmeleon,

        [Description("Charizard")]
        Charizard,

        [Description("Squirtle")]
        Squirtle,

        [Description("Wartortle")]
        Wartortle,

        [Description("Blastoise")]
        Blastoise,

        [Description("Caterpie")]
        Caterpie,

        [Description("Metapod")]
        Metapod,

        [Description("Butterfree")]
        Butterfree,

        [Description("Weedle")]
        Weedle,

        [Description("Kakuna")]
        Kakuna,

        [Description("Beedrill")]
        Beedrill,

        [Description("Pidgey")]
        Pidgey,

        [Description("Pidgeotto")]
        Pidgeotto,

        [Description("Pidgeot")]
        Pidgeot,

        [Description("Rattata")]
        Rattata,

        [Description("Raticate")]
        Raticate,

        [Description("Spearow")]
        Spearow,

        [Description("Fearow")]
        Fearow,

        [Description("Ekans")]
        Ekans,

        [Description("Arbok")]
        Arbok,

        [Description("Pikachu")]
        Pikachu,

        [Description("Raichu")]
        Raichu,

        [Description("Sandshrew")]
        Sandshrew,

        [Description("Sandslash")]
        Sandslash,

        [Description("Nidoran(F)")]
        NidoranF,

        [Description("Nidorina")]
        Nidorina,

        [Description("Nidoqueen")]
        Nidoqueen,

        [Description("Nidoran(M)")]
        NidoranM,

        [Description("Nidorino")]
        Nidorino,

        [Description("Nidoking")]
        Nidoking,

        [Description("Clefairy")]
        Clefairy,

        [Description("Clefable")]
        Clefable,

        [Description("Vulpix")]
        Vulpix,

        [Description("Ninetales")]
        Ninetales,

        [Description("Jigglypuff")]
        Jigglypuff,

        [Description("Wigglytuff")]
        Wigglytuff,

        [Description("Zubat")]
        Zubat,

        [Description("Golbat")]
        Golbat,

        [Description("Oddish")]
        Oddish,

        [Description("Gloom")]
        Gloom,

        [Description("Vileplume")]
        Vileplume,

        [Description("Paras")]
        Paras,

        [Description("Parasect")]
        Parasect,

        [Description("Venonat")]
        Venonat,

        [Description("Venomoth")]
        Venomoth,

        [Description("Diglett")]
        Diglett,

        [Description("Dugtrio")]
        Dugtrio,

        [Description("Meowth")]
        Meowth,

        [Description("Persian")]
        Persian,

        [Description("Psyduck")]
        Psyduck,

        [Description("Golduck")]
        Golduck,

        [Description("Mankey")]
        Mankey,

        [Description("Primeape")]
        Primeape,

        [Description("Growlithe")]
        Growlithe,

        [Description("Arcanine")]
        Arcanine,

        [Description("Poliwag")]
        Poliwag,

        [Description("Poliwhirl")]
        Poliwhirl,

        [Description("Poliwrath")]
        Poliwrath,

        [Description("Abra")]
        Abra,

        [Description("Kadabra")]
        Kadabra,

        [Description("Alakazam")]
        Alakazam,

        [Description("Machop")]
        Machop,

        [Description("Machoke")]
        Machoke,

        [Description("Machamp")]
        Machamp,

        [Description("Bellsprout")]
        Bellsprout,

        [Description("Weepinbell")]
        Weepinbell,

        [Description("Victreebel")]
        Victreebel,

        [Description("Tentacool")]
        Tentacool,

        [Description("Tentacruel")]
        Tentacruel,

        [Description("Geodude")]
        Geodude,

        [Description("Graveler")]
        Graveler,

        [Description("Golem")]
        Golem,

        [Description("Ponyta")]
        Ponyta,

        [Description("Rapidash")]
        Rapidash,

        [Description("Slowpoke")]
        Slowpoke,

        [Description("Slowbro")]
        Slowbro,

        [Description("Magnemite")]
        Magnemite,

        [Description("Magneton")]
        Magneton,

        [Description("Farfetchd")]
        Farfetchd,

        [Description("Doduo")]
        Doduo,

        [Description("Dodrio")]
        Dodrio,

        [Description("Seel")]
        Seel,

        [Description("Dewgong")]
        Dewgong,

        [Description("Grimer")]
        Grimer,

        [Description("Muk")]
        Muk,

        [Description("Shellder")]
        Shellder,

        [Description("Cloyster")]
        Cloyster,

        [Description("Gastly")]
        Gastly,

        [Description("Haunter")]
        Haunter,

        [Description("Gengar")]
        Gengar,

        [Description("Onix")]
        Onix,

        [Description("Drowzee")]
        Drowzee,

        [Description("Hypno")]
        Hypno,

        [Description("Krabby")]
        Krabby,

        [Description("Kingler")]
        Kingler,

        [Description("Voltorb")]
        Voltorb,

        [Description("Electrode")]
        Electrode,

        [Description("Exeggcute")]
        Exeggcute,

        [Description("Exeggutor")]
        Exeggutor,

        [Description("Cubone")]
        Cubone,

        [Description("Marowak")]
        Marowak,

        [Description("Hitmonlee")]
        Hitmonlee,

        [Description("Hitmonchan")]
        Hitmonchan,

        [Description("Lickitung")]
        Lickitung,

        [Description("Koffing")]
        Koffing,

        [Description("Weezing")]
        Weezing,

        [Description("Rhyhorn")]
        Rhyhorn,

        [Description("Rhydon")]
        Rhydon,

        [Description("Chansey")]
        Chansey,

        [Description("Tangela")]
        Tangela,

        [Description("Kangaskhan")]
        Kangaskhan,

        [Description("Horsea")]
        Horsea,

        [Description("Seadra")]
        Seadra,

        [Description("Goldeen")]
        Goldeen,

        [Description("Seaking")]
        Seaking,

        [Description("Staryu")]
        Staryu,

        [Description("Starmie")]
        Starmie,

        [Description("Mr.Mime")]
        MrMime,

        [Description("Scyther")]
        Scyther,

        [Description("Jynx")]
        Jynx,

        [Description("Electabuzz")]
        Electabuzz,

        [Description("Magmar")]
        Magmar,

        [Description("Pinsir")]
        Pinsir,

        [Description("Tauros")]
        Tauros,

        [Description("Magikarp")]
        Magikarp,

        [Description("Gyrados")]
        Gyrados,

        [Description("Lapras")]
        Lapras,

        [Description("Ditto")]
        Ditto,

        [Description("Eevee")]
        Eevee,

        [Description("Vaporeon")]
        Vaporeon,

        [Description("Jolteon")]
        Jolteon,

        [Description("Flareon")]
        Flareon,

        [Description("Porygon")]
        Porygon,

        [Description("Omanyte")]
        Omanyte,

        [Description("Omastar")]
        Omastar,

        [Description("Kabuto")]
        Kabuto,

        [Description("Kabutops")]
        Kabutops,

        [Description("Aerodactyl")]
        Aerodactyl,

        [Description("Snorlax")]
        Snorlax,

        [Description("Articuno")]
        Articuno,

        [Description("Zapdos")]
        Zapdos,

        [Description("Moltres")]
        Moltres,

        [Description("Dratini")]
        Dratini,

        [Description("Dragonair")]
        Dragonair,

        [Description("Dragonite")]
        Dragonite,

        [Description("Mewtwo")]
        Mewtwo,

        [Description("Mew")]
        Mew
    }
}