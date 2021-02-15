using System;
using System.Collections.Generic;
using TextUI.Interfaces;
using TextUI.Layouts;
using TextUI.Rendering;

namespace TextUI.Demo
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            var data = GetData();

            var screen = new Screen();

            var t1 = new Table<Match>(data,
                new Column<Match>("defence", r => r.Red.Defence.Name()),
                new Column<Match>("offence", r => r.Red.Offence.Name()),
                new Column<Match>("", r => r.Red.Score.ToString(), TextAlign.Right),
                new Column<Match>("", r => r.Blue.Score.ToString(), TextAlign.Right),
                new Column<Match>("offence", r => r.Blue.Offence.Name()),
                new Column<Match>("defence", r => r.Blue.Defence.Name())
            );

            var ui = VSplit(Line("File", "Edit", "View"), Box(HSplit(Table(t1), VSplit(Line("Here is the same table again"), Table(t1), BorderType.Single), BorderType.Double), BorderType.Double));
            screen.Root = ui;
            screen.Render();

            Console.ReadLine();

            ui = VSplit(Line("File", "Edit", "View"), Box(VSplit(HSplit(Line("X"), Table(t1), BorderType.Double), Table(t1), BorderType.Double), BorderType.Single));
            screen.Root = ui;
            screen.Render();

            Console.ReadLine();
        }

        private static Match[] GetData()
        {
            var names = "Glenn;Jean Paul;Andy;Wojtech;Tomas;Meg;Randy;Bill;James;Richard;Kenny;Ivan;Andre;Jamal;Sandy;Sarah;Anna;July;Jessica;Kate;Oliver;Hans;Peter;Chris;John;Yves;Helen;Bruce;Ben".Split(";");
            var surnames = "McLane;De Biers;Thatcher;Kracher;Muller;Swartz;Moreau;De Gaule;Platini;Mitteran;Shreder;Johnson;Hayek;Kenobi;Macron;Cameron;Trump;Jackson;Oakenfold;Oldfield;Guetta".Split(";");

            var random = new Random();
            var players = new List<Player>();

            for (var i = 0; i < 100; ++i)
            {
                players.Add(new Player
                {
                    Id = Guid.NewGuid().ToString("n"),
                    FirstName = Pick(random, names),
                    LastName = Pick(random, surnames),
                });
            }

            var data = new List<Match>();

            for (var i = 0; i < 100; ++i)
            {
                data.Add(new Match
                {
                    Id = Guid.NewGuid().ToString("n"),
                    Red = new TeamResult
                    {
                        Defence = Pick(random, players),
                        Offence = Pick(random, players),
                        Score = random.Next(5)
                    },
                    Blue = new TeamResult
                    {
                        Defence = Pick(random, players),
                        Offence = Pick(random, players),
                        Score = random.Next(5)
                    },
                    Start = DateTimeOffset.Now,
                    End = DateTimeOffset.Now
                });
            }

            return data.ToArray();
        }

        private static T Pick<T>(Random r, IList<T> items)
        {
            return items[r.Next(items.Count)];
        }

        private static IRender VSplit(IRender top, IRender bottom, BorderType? border = null)
        {
            return new SplitTopBottom(top, bottom, border);
        }

        private static IRender Line(params string[] names)
        {
            return new Line(names);
        }

        private static IRender HSplit(IRender left, IRender right, BorderType? border = null)
        {
            return new SplitLeftRight(left, right, border);
        }

        private static IRender Box(IRender content, BorderType borderType)
        {
            return new Frame(content, borderType);
        }

        private static IRender Table<TRow>(Table<TRow> table)
        {
            return new TableView(table);
        }
    }
}