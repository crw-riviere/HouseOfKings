namespace HouseOfKings.Web.Migrations
{
    using HouseOfKings.Web.Models;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<HouseOfKings.Web.DAL.HouseOfKingsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "HouseOfKings.Web.DAL.HouseOfKingsContext";
        }

        protected override void Seed(HouseOfKings.Web.DAL.HouseOfKingsContext context)
        {
            context.Rule.AddOrUpdate(new Rule { Number = 1, Title = "Waterfall", Description = "Everyone must keep drinking until the person who picked the card stops." });
            context.Rule.AddOrUpdate(new Rule { Number = 2, Title = "Choose", Description = "You can choose someone to drink." });
            context.Rule.AddOrUpdate(new Rule { Number = 3, Title = "Me", Description = "You must drink." });
            context.Rule.AddOrUpdate(new Rule { Number = 4, Title = "Floor", Description = "Last one to touch the floor must drink." });
            context.Rule.AddOrUpdate(new Rule { Number = 5, Title = "Thumb Master", Description = "When you put your thumb on the table everyone must follow and whomever is last must drink." });
            context.Rule.AddOrUpdate(new Rule { Number = 6, Title = "Dance Off", Description = "Everyone in the group must perform a dance move." });
            context.Rule.AddOrUpdate(new Rule { Number = 7, Title = "Heaven", Description = "Last one to point upwards must drink." });
            context.Rule.AddOrUpdate(new Rule { Number = 8, Title = "Straight", Description = "Person opposite you must drink." });
            context.Rule.AddOrUpdate(new Rule { Number = 9, Title = "Bust a Rhyme", Description = "Pick a word such a dog and the person next to you must rhyme with dog, like log, and it goes to the next person and the next, in a circle, until someone messes up and he or she will have to drink." });
            context.Rule.AddOrUpdate(new Rule { Number = 10, Title = "Categories", Description = " Pick a category such a football and you go in a circle and everyone has to say a word that fits with football such as: touchdown, field goal, USC. Whoever messes up, drinks." });
            context.Rule.AddOrUpdate(new Rule { Number = 11, Title = "Make a Rule", Description = "You can make up any rule that everyone has to follow, such as you can only drink with your right hand. Everyone (including you) must follow this rule for the whole entire game and if you disobey you must drink." });
            context.Rule.AddOrUpdate(new Rule { Number = 12, Title = "Questions", Description = "Go around in a circle and you have to keep asking questions to each other. Doesn’t matter what the question is, as long as its a question. Whoever messes up and does not say a question, drinks." });
            context.Rule.AddOrUpdate(new Rule { Number = 13, Title = "Pour", Description = "You must pour a little of your drink into the cup that is in the middle of the table. Whomever picks up the LAST king must drink the whole cup, which could be filled with different drinks, so who knows how bad it could taste!" });
        }
    }
}