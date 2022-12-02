using System.Diagnostics;

namespace Aoc2022.days;

public class Day2
{
    enum GameResult { Win, Loss, Draw }
    enum RPS { Rock = 1, Paper, Scissors }

    public static void Solve()
    {
        RPS ToRps(string s) => s switch { "A" or "X" => RPS.Rock, "B" or "Y" => RPS.Paper, "C" or "Z" => RPS.Scissors };
        GameResult ToResult(string s) => s switch { "X" => GameResult.Loss, "Y" => GameResult.Draw, "Z" => GameResult.Win };

        var lines = File.ReadLines("days/Day2.txt").ToList();
        var part1 = lines.Select(s => s.Split(" ")).Select(x => (ToRps(x[0]), ToRps(x[1])));
        
        RPS Not(RPS x) => x switch
        {
            RPS.Rock => RPS.Scissors,
            RPS.Paper => RPS.Rock,
            RPS.Scissors => RPS.Paper,
        };
        
        GameResult ResultOf((RPS opponent, RPS me) round)
        {
            if (round.opponent == round.me) return GameResult.Draw;
            return Not(round.opponent) == round.me ? GameResult.Loss : GameResult.Win;
        }
        int Score((RPS opponent, RPS me) round) =>
            ResultOf(round) switch
            {
                GameResult.Loss => 0 + (int)(round.me),
                GameResult.Draw => 3 + (int)round.me,
                GameResult.Win => 6 + (int)(round.me),
            };

        Console.WriteLine($"Part 1: {part1.Sum(Score)}");
        
        var part2 = lines.Select(s => s.Split(" ")).Select(x => (opponent:ToRps(x[0]), strategy:ToResult(x[1])));
        
        RPS FixOutcome(GameResult result, RPS opponent) => result switch
        {
            GameResult.Draw => opponent,
            GameResult.Loss => Not(opponent),
            GameResult.Win => Not(Not(opponent)) 
        };
        var fixedGame = part2.Select(r => (r.opponent, FixOutcome(r.strategy, r.opponent)));
        Console.WriteLine($"Part 2: {fixedGame.Sum(Score)}");
    }
}