namespace FirstApplication.Migrations.DataContext
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FirstApplication.Models.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations.DataContext";
        }

        protected override void Seed(FirstApplication.Models.DataContext context)
        {
            List<string> dgl = new List<string>();
            dgl.Add("Fantasy");
            dgl.Add("Puzzle");
            dgl.Add("Platformer");
            dgl.Add("MOBA");

            foreach(string s in dgl)
            {
                Genre model = new Genre();
                model.Name = s;

                Genre check = context.Genres.SingleOrDefault(x => x.Name == model.Name);

                if (check == null)
                {
                    context.Genres.Add(model);
                }
            }

            context.SaveChanges();
        }
    }
}
