using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground
{
    internal static class GMC
    {
        internal static void Main()
        {
            using (var db = new SequenceNumberContext())
            {
                foreach (var entity in db.Numbers)
                {
                    Console.WriteLine("Number: {0} ({1})", entity.Name, entity.Description);
                }

                var name = "DEBITOR";
                var numbers = NextNumbers(name, 3);

                Console.WriteLine();
                foreach (var number in numbers)
                {
                    Console.WriteLine("Next '{0}' number: '{1}'", name, number);
                }
            }
            PressAnyKey();
        }

        private static string[] NextNumbers(string name, int count)
        {
            using (var context = new SequenceNumberContext())
            {
                var entity = context.Numbers.Find(name);
                if (entity == null)
                {
                    entity = new SequenceNumberEntity()
                    {
                        Name = name,
                        Seed = 0,
                        Increment = 1
                    };
                    context.Numbers.Add(entity);
                }
                var format = string.IsNullOrWhiteSpace(entity.Format) ? "#" : entity.Format;
                var numbers = new string[count];
                for (int i = 0; i < count; i++)
                {
                    entity.Seed += entity.Increment;
                    numbers[i] = entity.Seed.ToString(format, CultureInfo.InvariantCulture);
                }
                context.SaveChanges();
                return numbers;
            }
        }

        private static void PressAnyKey()
        {
            Console.WriteLine();
            Console.Write("Press any key to continue ... ");
            Console.ReadKey(true);
            Console.WriteLine();
        }
    }

    public class SequenceNumberEntity
    {
        [Key]
        public string Name
        {
            get; set;
        }

        public string Description
        {
            get; set;
        }

        public string Format
        {
            get; set;
        }

        public int Seed
        {
            get; set;
        }

        public int Increment
        {
            get; set;
        }
    }

    public class SequenceNumberContext : DbContext
    {
        public SequenceNumberContext() : base("name=GMC") { }

        public DbSet<SequenceNumberEntity> Numbers
        {
            get; set;
        }
    }
}
