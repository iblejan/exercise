using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi1.Models;

namespace WebApi1.Tests.Controllers
{
    public static class TestData
    {
        public static List<Item> CreateTestData()
        {
            return new List<Item>
            {
                new Item {
                    Description = "Description a",
                    Price = 1,
                    Name = "Item a"
                },
                new Item {
                    Description = "Description b",
                    Price = 2,
                    Name = "Item b"
                },
            };
        }

        public static List<Item> CreateLargeTestDataSet()
        {
            return new List<Item>
            {
                new Item {
                    Description = "Description a",
                    Price = 1,
                    Name = "Item a"
                },
                new Item {
                    Description = "Description b",
                    Price = 2,
                    Name = "Item b"
                },
                new Item {
                    Description = "Description c",
                    Price = 3,
                    Name = "Item c"
                },
                new Item {
                    Description = "Description d",
                    Price = 4,
                    Name = "Item d"
                },
                new Item {
                    Description = "Description e",
                    Price = 5,
                    Name = "Item e"
                },
                new Item {
                    Description = "Description f",
                    Price = 6,
                    Name = "Item f"
                },
                new Item {
                    Description = "Description g",
                    Price = 7,
                    Name = "Item g"
                },
                new Item {
                    Description = "Description h",
                    Price = 8,
                    Name = "Item h"
                },
                new Item {
                    Description = "Description i",
                    Price = 9,
                    Name = "Item i"
                },
                new Item {
                    Description = "Description j",
                    Price = 10,
                    Name = "Item j"
                },
                new Item {
                    Description = "Description k",
                    Price = 11,
                    Name = "Item k"
                }
            };
        }
    }
}
