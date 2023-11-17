using UHC_API.HelperClasses;
using UHC_API.TeamGeneration.Interfaces;

namespace UHC_API.TeamGeneration.Implementations;

public class ThreadedTeamGeneratorIterator : ITeamGeneratorIterator
{
    public List<Team> Iterate(ITeamGenerator teamGenerator, int iterations)
    {
        var threads = new List<Thread>();
        var results = new List<List<Team>>();
        var iterator = new StandardTeamGeneratorIterator();
        const int amountOfThreads = 16;
        for (var i = 0; i < amountOfThreads; i++)
        {
            var thread = new Thread(() =>
            {
                results.Add(iterator.Iterate(teamGenerator, iterations/amountOfThreads));
            });
            threads.Add(thread);
        }
        ThreadExecutor(threads);
        while (results.Count > 1)
        {
            results = ThreadResultCombiner(results);
        }

        return results[0];
    }

    private static List<List<Team>> ThreadResultCombiner(IReadOnlyList<List<Team>> teamsList)
    {
        var results = new List<List<Team>>();
        for (var i = 0; i < teamsList.Count/2; i++)
        {
            results.Add(TeamWeigher.Weigh(teamsList[i], teamsList[teamsList.Count-1-i]));
        }

        return results;
    }

    private static void ThreadExecutor(List<Thread> threads)
    {
        foreach (var thread in threads)
        {
            thread.Start();
        }
        foreach (var thread in threads)
        {
            thread.Join();
        }
    }
}