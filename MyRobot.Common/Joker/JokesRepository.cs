using DuoVia.FuzzyStrings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyRobot.Joker
{
    public class JokesRepository
    {
        static List<Joke> _jokes;

        static JokesRepository()
        {
            _jokes = new List<Joke>();
            _jokes.Add(new Joke("Mouse", "What mouse was a Roman emperor ? Julius Cheeser !"));
            _jokes.Add(new Joke("TV", ",Why can't nanyone stay angry long with an actress? Because she always makes up."));
            _jokes.Add(new Joke("Computer", "This customer comes into the computer store. I'm looking for a mystery Adventure Game with lots of graphics. You know, something really challenging. Well, replied the clerk, Have you tried Windows ME?"));
        }

        public class Joke
        {
            public Joke(string category,string jokeText)
            {
                Category = category;
                JokeText = jokeText;
            }
            public string Category { get; set; }
            public string JokeText { get; set; }
        }

        public static string SelectProperCategory(string categoryToSearch)
        {
            //We will do a similarity search in order to minimize spoken differences!
            //Cool Project: https://github.com/tylerjensen/duovia-fuzzystrings
            return _jokes.Select(joke=>joke.Category).OrderByDescending(category => category.FuzzyMatch(categoryToSearch)).FirstOrDefault();
        }

        public static Joke NextJoke(string category)
        {
            try
            {
                return _jokes
                        .Where(joke => joke.Category == category)
                        .OrderBy(joke => Guid.NewGuid()) // quick and Dirtz random sort! 
                        .FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public static Joke NextJoke()
        {
            return _jokes
                    .OrderBy(joke => Guid.NewGuid()) // quick and Dirtz random sort! 
                    .FirstOrDefault();
        }
    }
}
